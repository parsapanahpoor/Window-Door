# 0014 — Seller SMS notifications

Status: Ready for implementation (design complete; no code or migration in this change)

## Goal

Send four independent VerifyLookup template messages:

1. New seller registered → admins.
2. Seller submitted information for review → admins.
3. Subscription expiry approaching → seller.
4. Subscription expired / renewal window → seller.

Templates are owner-provided and disabled until configured. No fallback to free text or simple SMS.

## Confirmed starting points (verify again before coding)

- Outbound template messages use `ISMSService.SendLookupSMS(receptor, token, token2, token3, template)`.
- `SMSService` calls Kavenegar `verify/lookup.json`; the existing OTP flows use the `Register` template.
- Store creation flows through `MarketService.AddMarket` / `AddMarketEntities`.
- SMS registration, password registration and post-session-loss resume are separate paths.
- Current coverage end is `MarketChargeInfo.EndDate`; renewal history exists.
- `WaitingForConfirmPersonalInformations` is set from store profile, introduction video and received-cheque settings (and any further paths found during implementation).
- `GetLoginPanelRoleAsync` prefers Seller over Admin for multi-role accounts and must not be used to choose admin recipients.

## Architecture: transactional outbox + two dispatch paths

### Outbox table (migration required)

`SmsNotificationOutbox`

| Column | Notes |
| --- | --- |
| Id | bigint identity |
| EventKey | nvarchar(200), unique with RecipientUserId |
| Kind | enum: SellerRegistered, SellerReviewRequested, SubscriptionExpiring, SubscriptionExpired |
| RecipientUserId | FK Users, nullable? no — resolved recipients only |
| TemplateKey | configuration key, e.g. `SellerRegisteredAdmin` |
| Token1/Token2/Token3 | nvarchar, template contract values |
| Status | Pending, Sending, Sent, Failed, Skipped, Disabled |
| Attempts | int |
| NextAttemptAtUtc | datetimeoffset |
| LeaseOwner / LeaseUntilUtc | dispatcher claim |
| ProviderMessageId / ProviderStatus | acceptance record |
| LastError | sanitized, no phone/API key |
| CreatedAtUtc / SentAtUtc | datetimeoffset |

- Unique index `(EventKey, RecipientUserId)` makes enqueue idempotent per recipient.
- Rows are written in the **same transaction** as the business change (store creation / review transition). Recipients are resolved at enqueue time, but status is re-checked before sending.
- External send happens only after commit, so a provider failure can never fail registration or profile save.

### Event keys

- `SellerRegistered:{marketId}` — exactly once per store, never for historical stores (only enqueued on the creation transaction after this release flag exists).
- `SellerReviewRequested:{marketId}:{reviewCycleId}` — a new `reviewCycleId` (GUID or monotonic value) is stored on the market when it enters `WaitingForConfirmPersonalInformations`; repeated edits inside one review cycle reuse it, admin decision + resubmission creates a new one.
- `SubscriptionExpiring:{marketId}:{tehranDate}` and `SubscriptionExpired:{marketId}:{tehranDate}` — at most one reminder per store per Tehran date.

### Dispatch

- Immediate dispatcher: short-delay queued/background pump (or a hosted worker) drains `Pending` rows for registration/review events.
- Daily dispatcher: 08:00 Asia/Tehran run evaluates reminder eligibility, enqueues rows, then drains them.
- Both use the same claim: `UPDATE ... SET LeaseOwner=@owner, LeaseUntilUtc=@now+2min WHERE Status=Pending AND NextAttemptAtUtc<=@now` with `READPAST`/row locks; expired leases are reclaimable.
- Retry with bounded exponential backoff (e.g. 1m, 5m, 15m, 1h, max 5 attempts). Permanent configuration errors (missing template/feature disabled) → `Disabled`, no endless retry.
- A provider timeout after acceptance is ambiguous; the row is retried with the documented duplicate risk. No exactly-once claim for external delivery.
- One recipient failing does not affect other rows.

### Scheduler on Plesk

- Hosted `BackgroundService` alone is not reliable (App Pool recycle).
- Preferred: standalone runner project (console) that calls the same Application dispatcher and reminder logic; Plesk Scheduled Task runs it daily at 08:00 Tehran and the task is idempotent.
- Alternative: an authenticated, rate-limited management endpoint guarded by a secret header from private configuration (never in the URL).
- Catch-up: on startup or the next run, if today's 08:00 passed and today's run has not completed, run it once for the current Tehran date; never replay older missed days.
- Deployment steps and Plesk prerequisites are a release-stage action, documented but not performed here.

## Reminder eligibility (Asia/Tehran)

Use `TimeZoneInfo.FindSystemTimeZoneById` with `Asia/Tehran` and fall back to `Iran Standard Time` on Windows. Inject `TimeProvider`; never use server local time or DayOfYear/365.

| Condition (Tehran date) | Message |
| --- | --- |
| 2 days before effective end | Expiring, 2 days |
| 1 day before | Expiring, 1 day |
| End day, end still in the future | Expiring, today |
| End day, end passed | Expired, today |
| Day 1, 2, 3 after end | Expired/renewal, N days |
| 3+ days before, day 4+ after | not eligible |

- Effective end = latest valid coverage end (including early renewals/future periods). Old `CurrentAccountCharge` records must not trigger reminders.
- Evaluate expired (inactive) stores until the reminder window closes; exclude deleted stores, owners who are deleted/banned/invalid, and stores without a valid subscription record.
- Re-read coverage and owner status immediately before send; skip if renewed or no longer eligible.
- Reminders never change market/charge state.

## Recipients

Admins = users with an active `Admin` role **or** `IsAdmin`, including multi-role admin/seller accounts, not deleted, not banned, confirmed mobile, valid normalized mobile; deduplicate by normalized mobile. Re-check at send time. No eligible admin → event rows are marked `Skipped` and logged; registration/persistence succeed.

## Templates (disabled until the owner supplies names)

| Key | Purpose | Tokens (proposal) |
| --- | --- | --- |
| `SellerRegisteredAdmin` | new store | token = store id, token2 = store name (if the provider token policy allows multi-word text; otherwise use id + owner mobile masked) |
| `SellerReviewRequestedAdmin` | review request | token = store id, token2 = store name |
| `SubscriptionExpiringSeller` | expiring | token = store id, token2 = days remaining, token3 = end date |
| `SubscriptionExpiredSeller` | expired | token = store id, token2 = days past, token3 = end date |

- Owner must confirm the exact template names, token count and whether multi-word store names are accepted. Until then every feature is disabled by configuration and rows are `Disabled`.
- Never use the `Register` template. Never fall back to `SendSimpleSMS`.
- Configuration is private (existing configuration mechanism), no phone numbers or keys in Git.

## Acceptance criteria

- AC-1: Registration notification fires exactly once per store for SMS registration, password registration and resumed registration, in both orderings of store creation and mobile confirmation; refresh/retry/re-login produce no duplicates; historical stores produce none.
- AC-2: A successful transition to `WaitingForConfirmPersonalInformations` from every real path enqueues one admin notification per review cycle, after commit; failed/validation/no-change operations do not; a new cycle after admin decision notifies again.
- AC-3: Reminder runs only at 08:00 Tehran and for the exact eligibility window; edge dates (year boundary, leap year, midnight, pre/post 08:00, missing and missed days) behave as specified; renewal before/during dispatch suppresses the message.
- AC-4: Outbox rows are idempotent by event key; concurrent dispatchers do not duplicate; partial recipient failure is isolated; provider/configuration failure never breaks the business transaction.
- AC-5: No message is sent while its template/feature is unset; missing template does not fail registration or profile save.
- AC-6: Logs are structured with event ids and contain no full phone/API key/template secrets.

## Test plan

- Unit: event keys, review-cycle identity, Tehran conversion, reminder window table, effective-end selection, recipient filtering/dedup.
- Isolated LocalDB + fake SMS: registration (three flows), review paths, retry, rollback, duplicate suppression, concurrent dispatch with two contexts, lease expiry/restart, missed-day catch-up, renewal race, partial recipient failure, disabled templates, provider timeout.
- All assertions must fail the process; console messages with exit 0 are not acceptable.
- Migration script generated for review; not applied to shared databases.

## Open questions for the owner

1. Exact Kavenegar template names and their token contracts, and whether store names may be sent as tokens.
2. Plesk Scheduled Task availability and the preferred invocation (standalone runner vs authenticated endpoint).
3. Confirm `MarketChargeInfo.EndDate` kind (local vs UTC) and the authoritative "effective current coverage" rule.
4. Approval for the added market review-cycle column/outbox table.

## Verification

Design only. No build, migration, code or tests were run for this spec commit.
