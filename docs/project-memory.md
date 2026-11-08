# Durable project memory

## Confirmed context
- Owner wants incremental Spec-Driven Development with an agent; primary conversation language is Persian.
- Product name/path: Panjerehyab; repository: parsapanahpoor/Window-Door. See [Git Flow](git-flow.md) for integration on develop and production releases from master.
- Baseline examined: 96a4bca3b3e1816062e9bd53e965f8c972b0958c.
- Five .NET 8 projects; mixed legacy services and MediatR CQRS; SQL Server persistence.
- Previous onboarding installed SDK 8.0.427, restored NuGet dependencies, and built Domain + Infra.Data. Full solution had 15 CS0234/CS0246 errors.
- Missing namespace: Window.Domain.ViewModels.Admin.Log. Missing types: FilterLogForInquiryViewModel, LogForBrandsViewModel, FilterLogVisitSellerProfileViewModel. References exist in ISellerService, SellerService and ContractService. Original definitions were not found in the checkout.
- No dedicated test project found. Application startup/database behavior has not been verified.

## Source findings to investigate
- CommandGenericRepository constructor invokes SaveChangesAsync without awaiting it.
- ShopOrderQueryHandler assigns `orderDetail.Count++` back to Count for zero SalesRatio; investigate with a regression test before fixing.
- Program.cs constructs the data-protection directory with Windows backslashes.
These observations are not an exhaustive audit. Preserve business semantics until specified.

## Next work
1. Validate the updated master baseline (2e842cb); the original missing view-model files have now been tracked upstream.
2. Configure an isolated SQL Server database and validate migrations/startup after a successful full build.
3. Add meaningful automated coverage to the first changed use case; select test tooling in that spec.
4. Review dependency vulnerabilities separately from behavioral changes.

## Maintenance
After each task append a concise outcome with commit/evidence, tests executed, unresolved blockers and next action. Never claim unexecuted checks passed. Keep secrets, customer data and transient logs out of this file.

## Current session validation — 2026-11-04 (owner timezone)
- Restored published SDK in new cloud instance; full build again failed with 15 missing-type errors (37 warnings in the incremental run).
- Ran scripts/check-development.sh: restore completed, build failed, process correctly returned exit code 1.
- Bash syntax, documentation relative links and git diff whitespace checks passed.
- Added documentation and scripts only; application source was not changed.
- Updated start_skill in the environment draft; publication is still a UI action and has not been performed by the agent.
- Owner subsequently reported that the Windows checkout builds successfully. Presence and definitions of the missing classes are not yet verified; compare tracked/untracked files, Git revisions and build configuration before attributing the discrepancy to a specific cause.

## Upstream reconciliation — 2026-11-04
- Before the requested push to master, fetched upstream commits fdbd9ad and 2e842cb by fast-forward.
- 2e842cb tracks all three previously missing view models. Historical missing-source blockers above describe 96a4bca and are superseded.
- HomeController.Index now sends ShopLandingQuery; the prior service-based home page is OldIndex.
- Documentation and the check script are the only new changes prepared by this session for Git publication.
- Full cloud solution build at 2e842cb passed: `dotnet build Window.sln --no-restore -m:2`, exit code 0. Existing compiler/package warnings remain; database/HTTP validation was not run.

## Windows development workspace — 2026-11-04
- Active checkout: `C:\Users\Parsapanahpoor\Documents\ChatGPT\Panjerehyab`; original Desktop source is preserved.
- Root cause of the historical compile errors: `.gitignore` rule `[Ll]og/` excluded `Window.Domain/ViewModels/Admin/Log`. The three original local models plus exact ignore exceptions were committed and pushed as 2e842cb. A fresh GitHub clone failed with 15 errors before that fix and built with zero errors after it.
- Pulled the original SDD documentation commit 73e8e16, preserving its workflow and baseline spec.
- Windows Setup and Release Verify passed using SDK 10.0.401 (0 errors; 1560 warnings in the clean build). EF Core tool 8.0.0 restored. No business tests ran.
- Added shared SDK selection (minimum 8.0.427, stable newer majors allowed), Windows command wrapper, editor settings, User Secrets support, and CI configuration. CI execution is tracked separately from local checks.
- Local SQL Server instance MSSQLSERVER2022 is running; read-only catalog inspection found no user databases. Development backup or initialization and representative HTTP checks remain outstanding.
- Next: supply isolated development data, validate database-backed and authorization behavior against baseline AC-4, then address the first feature with meaningful regression coverage. Do not mark the baseline fully ready yet.
- Owner confirmed a development backup exists and will provide its path. Do not initialize an empty database in its place; restore the supplied backup to a separate development database after inspecting it.
- Final Verify after SDD/SDK-policy integration passed with 0 errors (1559 warnings); the no-connection Run check exited 1 as intended. Documentation links and PowerShell/JSON syntax checks passed. Diagnostic logs remain ignored under artifacts/.

## Database restore and HTTP validation — 2026-11-04
- Setup commit 7c24484 was pushed to master. GitHub Actions build run 41731555773 passed from a clean checkout (https://github.com/parsapanahpoor/Window-Door/actions/runs/41731555773).
- Owner supplied the original project's Publish folder. Restored its June 2023 ZIP backup to SQL Server 2022 as Panjerehyab_Dev_20230610, without overwriting any database. RESTORE VERIFYONLY and DBCC CHECKDB passed; backup checksums were absent.
- The preserved restore has 47 user tables / 73 migrations and initially caused a missing ShopProducts error with current code.
- Created a second working restore, Panjerehyab_Dev, and applied the reviewed 56 pending migrations there. It has 75 user tables / 129 migrations and passed CHECKDB. The preserved restore remains at 73 migrations. Details: docs/database.md.
- Stored the working connection in local User Secrets. Home now returns HTTP 200 with expected storefront HTML; anonymous cart access returns HTTP 302 to Login with ReturnUrl. No authenticated, SMS, payment or production operations were tested.
- Added a reusable loopback-only smoke script. Remaining validation: authenticated development-user behavior and feature-specific business tests. Cloud environment UI publication was not performed by this Windows session.

## Shared Windows hosting preparation — 2026-11-04
- Verified the domain resolves to the assigned hosting server. The Plesk account offers SQL Server 2022 and has sufficient disk allocation for the Release package. The domain currently lacks a trusted public certificate; do not confuse the trusted panel certificate with the domain certificate.
- Prepared a verified copy-only SQL Server 2022 backup of the migrated development database. Keep backup files outside the web root and Git. Confirm the destination database version before restoring.
- Production publication excludes the old data protection key files and places newly generated keys in `App_Data/Auth` outside `wwwroot`. The prepared package uses deployment-specific database configuration and passed the local Production-mode storefront and anonymous cart checks. Hosting IIS runtime compatibility and live HTTP behavior still need verification.

## Live publication — 2026-11-04
- Owner explicitly approved SSL issuance and database/account provisioning and requested online publication plus local access to remote SQL. A subsequent continue instruction followed the Let's Encrypt terms prompt.
- Let's Encrypt covers panjerehyab.com and www.panjerehyab.com; ordinary TLS validation passed for both.
- Restored the migrated development backup to the hosted SQL Server 2022 database panjere4_window. Plesk reports 75 tables; remote SQL reports 129 migrations. The scoped application login is mapped correctly. Legacy backup principals remain orphaned and are not used by the site.
- Published the prepared release into httpdocs and set its IIS database connection. Archived the previous default site privately. Live HTTPS storefront/anonymous-cart/assets/configuration-path checks passed; rendered storefront was inspected.
- Remote SQL endpoint is win2016-770ir.hostnegar.com,1431. Its SQL certificate is self-signed/untrusted, so encrypted SQL profiles explicitly use TrustServerCertificate=True. HTTPS certificate validation is unaffected.
- Saved WindowRemote in local User Secrets and added Run -UseRemoteDatabase. Verified the normal local app against remote SQL. Added PANJEREHYAB_REMOTE to local SQL Server, mapping only the owner's Windows login; a four-part query returned 129 migrations.
- Generated password is retained in local User Secrets, IIS settings and a user-bound encrypted recovery file, never in Git. See docs/hosting.md for operational use and validation limits.

## Password recovery SMS — 2026-11-07

- The previous recovery flow ignored provider failures. The reset form also routed retries to account activation.
- Recovery now waits for a Kavenegar acceptance over HTTPS and only then persists the issued code. The reset form posts retries to the same protected recovery action.
- Release Verify and the offline recovery regression check passed. The latter covers provider rejection, malformed responses, transport errors, missing configuration, ineligible accounts, and the rendered retry form.
- A prior read-only workstation API request returned 416. The authenticated console showed the Register template approved, the account-wide IP restriction off, and the Standard subscription expired. VerifyLookup requires Advanced or higher. Hosting egress, IIS key configuration, and live delivery remain unverified. The release needs the deployment-specific kavenegar__recoveryApiKey setting, separate from the legacy SDK configuration.
- The owner activated the seven-day Advanced trial and authorized configuring the existing key in IIS. Release 855b49a was deployed to Plesk after retaining the previous assemblies and web.config in private storage. The public recovery form returned HTTP 200 after restart. Hosting egress and live SMS delivery still need confirmation.
- The first owner-run live attempt failed. Kavenegar logged a Register lookup POST from hosting egress IP 158.58.187.131 with status 416 while the Advanced trial was active. The provider IP policy still blocks live delivery.
- Kavenegar support directed the owner to the separate web-service IP allowlist. Added and verified 158.58.187.131 in that list with owner approval. A fresh owner-run recovery request is needed to verify acceptance and delivery.

## Account screens and SMS sign-in — 2026-11-07

- Owner confirmed successful live password recovery after the hosting egress address was added to Kavenegar's web-service allowlist.
- Replaced the account forms with a shared responsive RTL layout and window and door illustrations. Removed placeholder social and terms links.
- Implemented the SMS sign-in flow for all roles as requested. A mobile number is claimed after verification. SMS-only accounts have no password accepted by the password verifier. Recovery can set one.
- The additive SQL migration and SMS and recovery integration checks passed locally using an isolated SQL Server database, loopback HTTP and a fake provider. The owner subsequently authorized the hosted migration and installation in Plesk.
- Hosted preflight found six historical accounts sharing three normalized mobile numbers. Preserve all accounts and credentials; the migration flags those accounts and SMS sign-in fails closed for their numbers until ownership is resolved. Local tests cover duplicates, eligibility changes after issuance, and the exact idempotent SQL.
- Plesk exported a private database backup and retained the eight previous binaries and symbols. Migration 20261107120000_AddSmsLogin is registered on the hosted database (130 total); the challenge table is in dbo with verified application permissions. The live app is build fa06070.
- Login, registration, SMS sign-in, password recovery, and the new static assets returned HTTP 200 after installation. The anonymous SMS code route redirected as designed. The new live SMS delivery flow remains for owner verification.

## Default SMS sign-in and password fallback — 2026-11-07

- Release 85c324e made the SMS form the default at /login, kept /login/sms as an alias, and served password sign-in at /login/password. Ineligible numbers go directly to the password form with a one-time notice and a local return destination.
- Password sign-in uses the account selected by the supplied credentials and rejects multiple matches. Recovery and activation reject shared or excluded mobile numbers.
- Isolated SQL and loopback HTTP checks with a fake SMS provider passed, including exact account selection, duplicate fallback, and rejection of shared credentials and a previously issued recovery code. Offline password recovery checks passed.
- Installed the Release assemblies in Plesk. The previous eight files are in the private default-sms-rollback-20261107 directory. No migration was needed. All public login forms, registration and recovery returned HTTP 200; no live SMS or sign-in was performed during verification.

## Seller dashboard first pass

- Default request culture is Persian; explicit English query and cookie preferences remain supported. Seller Home now uses a dashboard-only responsive stylesheet, touch shortcuts and mobile bottom navigation.
- Implemented in the Visual Studio checkout on codex/feature/seller-panel-ui. Local Release verification and isolated browser checks passed at widths from 320 to 1440 pixels. See specs/0004-seller-dashboard/spec.md for evidence.
