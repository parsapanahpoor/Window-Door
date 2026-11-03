# Durable project memory

## Confirmed context
- Owner wants incremental Spec-Driven Development with an agent; primary conversation language is Persian.
- Product name/path: Panjerehyab; repository: parsapanahpoor/Window-Door, master.
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
