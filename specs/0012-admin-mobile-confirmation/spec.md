# 0012 — Admin mobile confirmation

- User management operators with the existing ManageUsers permission and Admin area access may confirm the saved mobile from Edit User using POST and antiforgery.
- The action rejects a changed, invalid, nonunique, excluded, deleted or blocked account target. Confirmation consumes pending activation state without setting a password or sending a message. A change to a different mobile resets confirmation.
- Record actor and target IDs through structured application logging. Never log the activation code or the number.
- Verify eligibility and repeat behavior with SmsLoginChecks on disposable local SQL Server. Deployment requires no schema change for this feature.
