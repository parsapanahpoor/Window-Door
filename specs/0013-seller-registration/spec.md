# 0013 — Customer and seller registration

Status: Implementing (account-type step, password registration, activation sign-in and seller redirect implemented; seller SMS store step and browser checks pending)

## Confirmed findings on develop@9ed77d0

1. **SMS sign-in has no registration step.** `IdentityController.SmsLogin` normalizes the mobile and
   immediately calls `SmsLoginService.SendAsync`. That method only creates/updates a challenge and sends
   the code; the `User` row is created later in `VerifyAsync` with `Password = "!sms-only"`. There is no
   account-type choice and no store name before the first SMS.
2. **Password registration has no role or store.** `RegisterUserViewModel` contains only
   `Password`, `RePassword` and `Mobile`. `Register` creates the account with no role selection, then
   redirects to `/Active-mobile/{Mobile}`. `UserService.RegisterUserAsync` sends the activation SMS
   inline and contains a hardcoded Kavenegar API key and template URL. The key must be removed and
   rotated; SMS must go through `ISMSService`.
3. **Store creation lives in `MarketService.AddMarket(userId, titleName)`.** It creates `Market`
   (`WaitingForCompleteInfoFromSeller`, activation tariff from `SiteSettings.ChargeOfNewMarkets`),
   `MarketUsers` with `RoleId = 4`, and `UserRole` with `RoleId = 4`. `MarketController.AddMarket`
   wraps it. The logic is reusable but currently saves three times without an explicit transaction.
4. **Post-login destination ignores store completeness.**
   `IdentityController.GetPostLoginDestinationAsync` sends every seller role to `/Seller` and only
   honours `returnUrl` for non-sellers. A newly registered seller without store data lands on the
   dashboard instead of the store-information page.
5. **Mobile activation** (`/Active-mobile/{Mobile}`) uses a hardcoded two-minute timer, sends the code
   on GET when `Resend=true`, and redirects to `/Login` after success without signing the user in.
   The reset/activation links are reachable with only a mobile number; the code itself is the proof.
6. **Existing SMS controls** (`SendAsync`, `VerifyAsync`): 4 sends per 15-minute window, 2-minute code
   lifetime, 5 failed attempts, one-time consumption, constant-time comparison and duplicate-mobile
   exclusion are present and should be preserved.

## Design

### Shared account/store creation

Add one application service method used by both registration paths, e.g.
`IRegistrationService.RegisterAsync(RegistrationRequest)` in `Window.Application`, implemented with the
scoped `WindowDbContext` so it can wrap the user, role, market and market-user rows in one transaction.
It reuses the same store rules as `IMarketService.AddMarket` by extracting the unsaved entity creation
into a shared helper (`MarketFactory` or an internal method on `MarketService`) rather than copying the
controller logic. `MarketController.AddMarket` keeps its route and delegates to the same helper.

- Customer: user row, role 3 (existing customer role id used by the project), no market.
- Seller: user row, role 4 appended (existing roles are kept), market + market user in
  `WaitingForCompleteInfoFromSeller`, activation tariff from site settings.
- Mobile is normalized with `MobileNumber.Normalize` and the duplicate check uses `MobileLoginKey`.
- A unique-index violation is translated to `MobileExist`; the transaction rolls back.
- SMS is sent after the commit, never inside it.

### SMS registration (new number)

- `POST /login/sms` checks the account first. Existing accounts keep the current flow unchanged
  (no role question, no store, no password/permission change).
- For a number without an account, the response renders a role step (`Customer` / `Seller`) without
  sending an SMS. The chosen role and normalized mobile are stored in the session registration state
  (`Window.Registration`), not trusted from later form posts.
- Seller selection renders a store-name step. The seller account is created only after a valid store
  name; the shared registration service is called in one transaction; the SMS is sent afterwards.
- Customer selection creates the account immediately (same transaction helper, no market) and sends
  the SMS.
- The session state holds the mobile, user id, role and step; every subsequent POST re-reads it and
  re-validates ownership. Browser-supplied user ids are ignored.
- Repeated or concurrent posts are idempotent: an existing unconfirmed account with the same
  normalized mobile and the same chosen role continues the flow; a different role is rejected. A
  confirmed number is never pushed into registration.
- `VerifyAsync` must not create a second user for a number that already has the pending account. It
  marks the number confirmed and signs the user in; `NoPassword` stays as the unusable password
  sentinel.

### Password registration

- `RegisterUserViewModel` gains `AccountType` (required enum) and `MarketName` (required only for
  sellers, validated server-side with the same rules as the existing store route).
- The form shows three existing fields plus the role choice and the conditional store name; on a
  validation error the mobile, role and store name are preserved while passwords follow the existing
  secure redisplay rules.
- The POST creates the account and (for sellers) the store in one transaction, then sends the
  activation SMS through `ISMSService`, then redirects both account types to
  `/Active-mobile/{Mobile}`.

### Activation and post-login redirect

- `/Active-mobile/{Mobile}` gets the same layout/typography as the new login and registration pages:
  masked mobile, code field, errors, countdown and resend, usable at 360px. A correct code confirms
  the mobile **and** signs the user in. Activation is still gated by the code, not by knowing the
  number.
- `GetPostLoginDestinationAsync` becomes the single redirect policy for password, SMS and activation:
  - new seller with no market or with `WaitingForCompleteInfoFromSeller` →
    `/Seller/SellerPersonalInfo/PageOfManageSellerInformation?welcome=1` (after sign-in), never
    overridden by an external `returnUrl`;
  - existing seller with submitted/approved data keeps the current destination;
  - customer/admin flows keep `returnUrl` and current behaviour.
- The store-management page renders a short guidance notice when `welcome=1`:
  «برای فعال‌شدن فروشگاه، اطلاعات و مدارک موردنیاز را تکمیل کنید.»

## Acceptance criteria

- AC-1: A new number is asked for account type before any SMS; no SMS is sent on lookup or on showing
  the role step, or for a seller before a valid store name.
- AC-2: Customer and seller SMS registrations create exactly one account with the normalized mobile,
  unconfirmed; the seller path creates exactly one market owned by that account with the current
  rules and role 4 appended, not replacing other roles.
- AC-3: The account is not signed in before code verification; a correct code confirms the mobile,
  signs in and lands on the role-appropriate destination.
- AC-4: Password registration for both roles works with server-side validation independent of
  JavaScript; invalid input preserves mobile/role/store name; the account+store creation is atomic.
- AC-5: A new seller without store information is redirected to the store-information page with the
  guidance notice after either flow; `returnUrl` cannot bypass it; other roles keep current redirects.
- AC-6: Duplicate normalized numbers, Persian/Latin digit forms, unknown roles, empty/invalid store
  names, expired/used/wrong codes, resend limits and CSRF are rejected without side effects.
- AC-7: Abandoned registration can be resumed only from the server-side session state (or after
  proving mobile ownership); crafting a user id or changing another account's role is rejected.
- AC-8: SMS provider failure does not duplicate the account/store; a retry reuses the same pending
  record.

## Test plan

- Unit/service tests against the isolated LocalDB: shared registration helper (customer/seller,
  duplicate, rollback), role append, store defaults, idempotency.
- HTTP tests with two cookie containers and a fake `ISMSService`: role step before send, no send for
  seller before store name, account state before/after verify, password registration for both roles,
  activation sign-in, redirect policy, foreign session/CSRF/returnUrl.
- Browser check at 360/768/1440 for the role step, store step, register page and activation page.

## Notes and risks

- `UserService.RegisterUserAsync` contains a hardcoded Kavenegar API key and template URL in source.
  It must be removed, moved to configuration, and the exposed key rotated. This is a security defect
  independent of the feature.
- No migration is expected: `User`, `Market`, `MarketUsers`, `UserRole` and `MobileLoginKey` already
  cover the flow. If a registration-state table is needed instead of session state, it must be a
  separate, reviewed migration; the session approach is preferred for this scope.
- The site setting for the resend timer is currently hardcoded to 2 minutes; the request asks for 120
  seconds, which matches. Keep server-side enforcement.

## Verification

- `dotnet build Window.sln --no-restore -m:2`: 0 errors.
- `dotnet run --project tests/SmsLoginChecks -c Release -- "(localdb)\MSSQLLocalDB"`: PASS, including the new
  account-type step (no SMS before a role is chosen), the customer SMS registration path, password sign-in
  redirect, and all pre-existing SMS challenge, replay, cooldown and admin checks.
- Implemented: server-generated pending account with the chosen role recorded on the account,
  shared `AddMarketEntities`, role selection in the SMS flow, customer and seller SMS registration,
  password registration with role + store and an atomic transaction, activation auto sign-in,
  seller redirect (no store → `/Market/AddMarket?welcome=1`, incomplete store → store information with
  guidance), and removal of the hardcoded SMS key.
- Verified end to end with the isolated LocalDB and the offline SMS sender: new seller through SMS
  (role step, store step, one SMS only after the store, verification, store-information redirect,
  confirmed account, incomplete market, role 4 and owner link), seller password registration (account
  and store committed before activation, unconfirmed before the code, activation signs in and redirects),
  customer password registration (no store and no seller role, activation opens `/UserPanel`), and the
  existing customer SMS, replay, cooldown, duplicate, CSRF and external-returnUrl checks.
- Session loss: a signed-in seller with no store is sent to `/register/store`, an authenticated form
  that creates the store and continues to the store-information page. The HTTP check signs the seller
  in, completes the store and verifies the resulting market.
- Concurrency: the check runs two independent `DbContext`/connection registrations for one mobile and
  two store creations for one owner. Exactly one account/store remains, one call succeeds and the loser
  receives a controlled result; deadlock/unique violations are caught (including EF transient wrappers)
  and the transaction is rolled back without a server error.
- Root causes found while adding the edge checks:
  - Concurrent registration lost the race on the unique index `IX_Users_MobileLoginKey`; the
    `DbUpdateException` escaped because the handler did not walk the exception chain. The service now
    detects SQL 2601/2627/1205 anywhere in the chain and rolls back safely. No database constraint was
    removed.
  - The `302` on `/register` was correct application behaviour: the shared test browser was already
    signed in and `[RedirectIfLoggedInActionFilter]` sent it to `/`. The test now uses a fresh browser
    and asserts status/Location instead of calling `GetStringAsync`.
  - A real duplicate-role defect was found: the SMS seller account received role 4 at creation and
    `AddMarketEntities` added it again. The helper now adds the owner role only when it is absent.
- Verified with the isolated LocalDB and the offline SMS sender: forged POSTs for `/register` and
  activation return 400 and change nothing; unknown SMS role creates no account; space-only and
  101-character store names are rejected without sending an SMS; a validation error preserves the
  mobile, selected role and store value; a forced provider failure creates exactly one account/store/role
  and resend continues with one additional code; an unconfirmed account is sent to activation and has no
  panel session, while direct panel access redirects to login.
- Not verified: `/register/store` antiforgery and the password-registration invalid-role/store variants
  (they need a fixture path that the current host does not provide without manual inserts), and the
  360/768/1440 browser review. No Playwright browser build is installed and no runner was wired to Edge
  in this session.
