# 0003 — Account screens and SMS sign-in

Status: Verified in the isolated development environment

## Requirements and scope

- Give sign-in, registration and both recovery steps one responsive, Persian right-to-left design with window and door illustrations. Remove placeholder social buttons, links and copy. Preserve the existing password sign-in, registration and recovery routes.
- Add SMS sign-in with a six-digit code. Start the same verification flow for registered and new mobile numbers. A new account is created after successful verification of mobile ownership. The pending challenge represents registration before verification.
- Preserve the required password column. Store a marker outside the legacy password hash format for SMS-only accounts. A verified password recovery can set a real password.

## Acceptance criteria

1. Password sign-in and manual registration retain their fields, server validation and real links. Recovery retains a functional resend and reset form. The identity screens render and remain navigable on mobile and desktop.
2. A valid Iranian mobile number receives one six-digit SMS through the configured provider. Persian and Arabic digits and standard local and international prefixes are normalized. No account or session is created when the provider rejects the request.
3. A valid, unused code authenticates the eligible existing user or provisions one verified account with no usable password. An unverified account's previously chosen password is invalidated when claimed by the phone owner. An existing verified password is preserved. All active account roles, including administrators and sellers, can use the same flow as requested by the owner. Blocked and deleted accounts receive no SMS login cookie.
4. Codes expire after two minutes, bind to a browser challenge, and are consumed atomically. The server enforces resend cooldown, sending and guess limits, and rejects replay. A resend preserves failed-attempt counts within the current rate window.
5. POST actions require antiforgery validation. The normal cookie claims and local return URL rules apply. Test with an isolated database and a fake SMS sender.

## Design

Use a dedicated SQL Server challenge per canonical mobile. Persist an encrypted code, issuance limits, a random challenge identifier, and a row version. An anonymous browser keeps the challenge identifier in its server-side session. Reserve a send before calling the provider and only enable verification after provider acceptance. Consume the challenge and create or update the account in one database commit. Apply an IP rate limiter at the HTTP boundary. Recheck account eligibility for SMS sessions. Password recovery also enforces the existing two-minute window and invalidates its code when the password changes.

The only required data changes are the dedicated challenge table and a uniqueness safeguard for account creation. Review the migration against the isolated SQL Server database before any shared deployment. Reuse the approved Register template and existing server-side SMS integration. The owner confirmed live recovery delivery after the previous host allowlist change.

## Verification

The SQL and loopback HTTP checks passed using a disposable local database and an offline SMS sender. They exercised the additive migration, existing legacy phone data, normal and administrator SMS login, blocked and deleted accounts, code expiry and replay, concurrent sends and verification, resend and guess limits, account creation, password setup after SMS registration, CSRF, safe redirects, and rendered forms and SVG assets. The existing password recovery regression checks also passed. Shared deployment and live delivery for the new login flow are pending.
