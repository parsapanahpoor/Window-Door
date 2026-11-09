# 0007 Seller store profile progress

Status: Verified with isolated browser fixtures

- AC-1. A seller dashboard shows a dedicated progress notice to the owner while the store profile is incomplete. The three existing setup steps have equal weight. A saved profile, a nondeleted link with a title and value, and a nondeleted work sample with a title and image yield 0%, 33%, 67% or 100% according to the number saved. The setup action takes the owner to the next step or the edit page for submitted profiles.
- AC-2. The store management shortcut appears first on the dashboard with a verification badge. A completed profile and support approval are separate states. The status comes from the associated market.
- AC-3. The dashboard resolves the signed-in owner and associated market on the server. Employee dashboards do not query owner-only progress. The notice participates in the existing severity order and bell.

The progress query reads the existing records and does not require a migration. The dashboard uses the same notice styles and navigation rules.

## Evidence

The seller browser runner passed with incomplete profiles at 0%, 33% and 67%, including the link to the next step. It verified that completed profiles hide the progress notice while pending review, approved and rejected states render distinct labels. The employee fixture fails if its dashboard calls the owner-only progress method. Browser checks used a loopback host and isolated service fixtures.
