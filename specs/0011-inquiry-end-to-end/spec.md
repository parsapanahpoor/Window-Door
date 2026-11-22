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
