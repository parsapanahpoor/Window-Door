# 0011 — Inquiry end-to-end correction

Status: Implementing (phase 1 partly implemented; phases 2–4 pending)

## Problem

Source review of `develop@0291af0` found the following confirmed defects.

1. **Ownership is the server's IP address.** `InquiryProductController` builds the inquiry key with
   `Dns.GetHostEntry(Dns.GetHostName()).AddressList[1]` (Step2, Step3, Step4, profile, score, delete).
   Every visitor of the deployment therefore reads and mutates one shared basket. The key also leaks
   the deployment's internal address into URLs.
2. **Starting an inquiry destroys the previous one.** `LogInquiryForUserPart1` marks the row that
   matches the key as deleted and removes its details and result rows. Two tabs, or two concurrent
   requests, overwrite each other.
3. **Mutating GETs.** `RequestForContract`, `AddCommentFromUser` and `DeleteUserLasteInquiryDetail`
   change data through GET links without antiforgery protection.
4. **Null-reference risk.** `CheckLogResultUserInquiry` dereferences `inquiry.Id` when no inquiry
   exists.
5. **Non-atomic basket limit.** Step3 reads the current count and then inserts; concurrent requests can
   exceed five items.
6. **Missing sample existence check.** Step3 POST compares `sample.MaxHeight` before checking
   `sample == null`.
7. **API has no ownership check.** `InquiryAPIController` accepts `userMacAddress` from the URL and
   serves any basket with that string. Whether an active consumer exists is unconfirmed (phase 3).
8. **Pricing, traffic and reports are unreviewed.** `InquryService` is ~4.5k lines; item pricing,
   seller × brand loops, tariff events and admin date filters are phase 2 and 4 work.

## Implemented in this change (phase 1 subset)

- Every inquiry now receives a server-generated token (`Guid.NewGuid().ToString("N")`). The token is
  stored in the visitor's session and replaces the IP address as the inquiry key.
- Added `LogInquiryForUser.OwnerUserId` (nullable) and the `AddInquiryOwnerUserId` migration. Existing
  rows keep `OwnerUserId = null`; their `UserMAcAddress` contains historical IPs, so no session can
  claim them. They remain readable to nobody except admin reports; ownership is never guessed.
- New inquiry rows are created per start. The previous inquiry is no longer deleted when its token is
  different, so another tab or user keeps a working basket.
- `IInquiryService` gains `ClaimInquiry`, `InquiryBelongsToUser` and `GetLatestInquiryTokenForUser`.
  A guest inquiry is claimed by the signed-in user on the first signed-in inquiry request. Guests may
  only use the token stored in their own session; a token from another session is rejected.
- Invalid, foreign or missing tokens redirect to Step1 with a Persian message instead of serving data.
- `RequestForContract`, `AddCommentFromUser` and `DeleteUserLasteInquiryDetail` now require POST and
  antiforgery. The views submit forms inside the existing inquiry form where nesting would be invalid.
- `CheckLogResultUserInquiry` returns 0 when no inquiry exists; Step3 checks the sample for null; the
  five-item guard uses `>=`.
- API paths are untouched in this phase; they keep their existing token semantics and still lack
  ownership checks.

## Not implemented (next phases)

- Phase 2: price validation (blank/negative/zero), deleted product or foreign-store rates, equivalent
  rate selection, per-item coverage labels, round-trip consistency, reference formula tests, tariff
  event separation and idempotency, seller × brand × item query measurement.
- Phase 3: identical create/edit validation, unique field ids and Persian digits, late city/brand
  responses, result page coverage states, API consumer audit and contract/version decision.
- Phase 4: seller price-entry unit/current-rate feedback, sample create/edit integrity and image
  replacement safety, Jalali range validation (single day, midnight, month change), report event
  definitions.
- Automated tests: no database-backed or browser test was added yet; only the solution build was run.
  A disposable LocalDB HTTP test (two cookie containers, claim-on-login, foreign-token rejection,
  POST-only mutations) is the first test to add.

## Acceptance criteria

- AC-1: Two guests in separate browsers see independent baskets; neither can read, add, edit or delete
  the other's items or results.
- AC-2: A guest basket survives sign-in and is owned by that user afterwards; a token from another
  session or user is rejected without side effects.
- AC-3: Starting an inquiry in one tab does not delete or mutate the inquiry used by another tab or
  another visitor.
- AC-4: All mutating inquiry actions use POST with antiforgery; GET, refresh and result sorting leave
  data unchanged.
- AC-5: Seller/admin filters stop the action before execution; seller actions additionally check store
  or product ownership.
- AC-6 (phase 2): Prices come from the current basket version; missing/invalid rates are labelled;
  deleted products and foreign-store rates are never used; rounding follows the reference examples.
- AC-7 (phase 3): Validation is identical for create and edit; the result page states basket status
  and coverage; the API contract is decided and tested.
- AC-8 (phase 4): Seller/admin forms and reports meet the acceptance list in the request.

## Verification

- `dotnet build Window.sln --no-restore -m:2`: 0 errors (Debug).
- Migration scaffolded as `20261121084957_AddInquiryOwnerUserId`; not applied to any shared database.
- Pending: isolated-database HTTP tests, browser flow, pricing/API/report work.

## Phase 1 review corrections

- Issue a reserved web token per draft and store separate guest grants per session. Carry the selected draft through the brand and result links. An explicit token from an authenticated request must already belong to that account.
- Claim session-granted drafts during sign-in with a database update conditional on the owner being null. Guest access checks that the record is still unowned. Logout clears inquiry-specific session keys.
- Keep the legacy API contract for its original clients and reject reserved web tokens at its boundary. The legacy result API still requires a separate contract decision.
- Require POST and antiforgery to create a draft or select a brand. Existing contract, comment and delete POST protections remain. The admin and seller role filters short-circuit execution.
- Serialize web basket additions on the parent inquiry row and count items inside the transaction. Result reads are scoped to the active inquiry and do not increment the seller tariff. Defining and recording a billable event remains phase 2.
- Scope the ownership migration to the nullable column. Existing roles and product-category seed timestamps are preserved in the migration model. For an isolated database that ran the original scaffold, validate a fresh pre-upgrade fixture before relying on migration evidence.
- Verified: solution build passed. InquiryOwnershipChecks passed on a unique disposable local SQL Server catalog. It covers competing claims, owner retry, claimed drafts rejecting the guest check, independent drafts and generated migration scope. Local HTTP returned 405 for GET and 400 for POST without antiforgery for draft creation, brand selection and item removal.
- Still required: full browser and authenticated HTTP coverage for two guest sessions and tabs, login transfer, logout, role rejection, and concurrent basket insertion. Pricing recalculation, billable event policy, the legacy result API and broader panel UX and reporting changes remain in later phases.

## Implementation and local acceptance — 2026-11-23

- Ownership: the isolated HTTP suite covers independent sessions and simultaneous tabs, guest-to-account transfer at password sign-in, rejection of a foreign draft, post-logout rejection, panel-role denial, and antiforgery. Web mutations serialize on the draft row. A form request key prevents a duplicate insertion, including a retry after the item was soft-deleted.
- Pricing: add, edit, delete and brand changes rebuild the whole basket atomically. The calculator preserves existing formula arithmetic and publishes only positive finite totals covering all items and necessary rates. Active seller/brand rates and sample components are loaded in batches. The latest active rate by ID breaks ties. The SQL fixture covers a reference 800 IRR quote, quantity changes, missing parts, deleted products, the five-item limit, and invalid or concurrent seller rate writes.
- Billing decision: one tariff per seller and completed web inquiry, independent of the number of brands, result refreshes and concurrent completions. The composite-key exposure ledger and market balance update share the completion transaction. GET reads do not bill. The latest explicit completion outcome is recorded as with or without a complete quote. Historical start logs are still reported as starts.
- Public web: browser tests at 390 and 1366 pixels cover the dependent city list, Persian digits, a rejected dimension with retained value and completion to the reference result. No-result state and the admin inclusive Jalali day filter have isolated coverage. Seller results appearances are shown separately from profile visits.
- Admin samples: the sample and component changes commit in one save. A staged replacement is only referenced on successful persistence. The old file is retained so a failed edit cannot break an existing image. The isolated constraint-failure test verifies rollback of sample fields and components.
- API decision: the owner confirmed no consumers of the old inquiry workflow. Legacy stateful steps, profile and rating routes return HTTP 410 with the web entry point. Public lookup routes remain available. The experimental MVC controller is no longer routable.
- Installed web app: the manifest remains same-origin and its declared icons resolve. The worker registers with a new cache identifier and the minimal online strategy. The inquiry controller explicitly prevents storing its pages. Edge tested the active worker and checked manifest icon responses. Physical installed devices and the transition from an already-active production worker are part of release smoke testing.
- Upgrade fixture: the test reconstructs the affected pre-upgrade schema in a uniquely named local SQL catalog and runs the generated migration batches on one connection. Migrations add ownership, exposure ledger, completion status and request keys and leave unrelated seed rows intact. No production migration has run.
- Re-run: set INQUIRY_CHECK_BROWSER=msedge then run dotnet run --project tests/InquiryOwnershipChecks from the repository root. Seller dashboard regression uses SELLER_CHECK_BROWSER_CHANNEL=msedge.
- Release sequence: review the migration SQL and backup, apply the additive schema before replacing binaries, verify a freshly opened and an already-installed PWA online, and verify read-only production status. If application rollback is needed, retain forward-compatible tables and columns so recorded financial events survive. The reference suite checks representative formulas rather than every production catalog combination.
