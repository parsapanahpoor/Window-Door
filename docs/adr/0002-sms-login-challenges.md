# ADR 0002 — SMS sign-in challenge and account provisioning

## Status

Accepted for local implementation.

## Context

The site needs password and SMS sign-in through the existing account and cookie model. The Users table requires a password value. Recovery and activation share a legacy code field, so SMS sign-in needs a separate purpose and lifecycle.

## Decision

Store one SQL Server challenge per canonical Iranian mobile number. Reserve an issuance before calling the SMS provider. Store the provider-accepted code using ASP.NET Core Data Protection and bind its identifier to the requesting browser session. Charge guesses with a row update inside the verification transaction. Consume the code and save account changes together.

A new account is created only when the mobile number is proven. An account without a verified password stores a marker outside the legacy hash format. An unverified account's prior password is invalidated when the mobile owner claims it. The ordinary recovery flow can establish a password.

Index a computed canonical key for user mobile numbers. Preserve existing values and permit distinct non-phone legacy records. Normalized duplicates cause the migration to fail. Resolve ownership before altering conflicting data. Include deleted accounts in uniqueness to prevent accidental account recreation under an old identifier.

Use the same cookie claims and authorized account identity after proof. The owner selected SMS sign-in for all roles, including administrators and sellers. Recheck deleted and blocked status for SMS sessions. Rate limits combine per-mobile SQL counters and per-IP ASP.NET Core limits.

## Operations

Run the additive migration before switching IIS binaries. Review normalized duplicates and database compatibility against a copy. In case of application rollback, retain the additive schema so the previous binaries can continue to run. The short-lived browser session uses the existing in-process session store. An interrupted browser session can start the flow again after the cooldown.

The approved Register template and the existing Kavenegar host allowlist are required. Test with an isolated SQL database and a fake provider.
