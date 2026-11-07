# 0002 — Password recovery SMS

Status: Verified (offline acceptance checks; production rollout pending)

## Problem and evidence

The recovery service persists an OTP before starting an unobserved Kavenegar request and always reports success. The reset view sends retries to the account-activation page. A read-only Kavenegar account request from the workstation returned 416 (source IP mismatch). The hosting egress IP and production provider acceptance still need verification.

## Scope

First recovery request and retries from the reset form. Keep the existing Register template, registration and account activation behavior, password validation rules, and schema.

## Acceptance criteria

- AC-1: A known, unblocked account proceeds to the reset page only after Kavenegar accepts the SMS and the application saves its new code and timestamp.
- AC-2: Missing configuration, transport or malformed response, and provider rejection fail visibly and leave the previous code and timestamp intact. Operational logs contain status codes only, without provider payloads, credentials, mobile numbers, or OTPs.
- AC-3: Missing, deleted and blocked accounts do not send an SMS or persist a code.
- AC-4: The reset form retries through the same anti-forgery-protected recovery POST. Loading the reset page does not send an SMS.

## Design and tasks

1. Reuse ISMSService and the recovery controller's failure branch. Bind the key from deployment configuration and call the documented VerifyLookup endpoint over HTTPS with a bounded timeout.
2. Interpret provider acceptance before saving. The latest saved accepted request replaces its predecessor. A database write failure propagates instead of reporting success. SMS submission and the database write cannot be atomic.
3. Post retries through the recovery form handler. Exercise success and failure with a fake provider and isolated persistence.
4. Record rollout prerequisites. Production requires the configured key, an approved Register template, and an allowed hosting egress IP. No migration is needed.

## Verification evidence

Release Verify passed. Run dotnet run --project tests/PasswordRecoveryChecks -c Release to exercise AC-1 through AC-4 with a fake provider and persistence, including the rendered retry form and anti-forgery validation over loopback HTTP. No real SMS or database is used.
