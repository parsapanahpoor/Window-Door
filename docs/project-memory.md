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

## Shared seller shell

- The shared seller layout now renders the primary navigation on every seller page. Its three links are fixed at the bottom on mobile and appear below the header on desktop. The layout loads local Shabnam in regular, medium and bold and uses a warm palette. Seller-specific typography separates headings, values, actions and metadata. The isolated browser runner verified the hierarchy and narrow layout. The shared content heading is a translucent glass bar with the RTL page title on the right and a browser-back button on the left with a left-pointing arrow placed before its label; the back action falls back to the seller dashboard when there is no prior history. On the seller dashboard, the action instead links to the public storefront. The glass uses a warm peach and copper tint. The new profile and language controls use the existing account and localization routes.
- Extended the isolated browser runner with navigation through real seller routes, font and avatar loading, form clearance and keyboard controls. See specs/0005-seller-shell/spec.md.

## Seller notices and full navigation

- Seller pages share one visible title at the start of content, followed by account notices; legacy breadcrumb sections are ignored by the layout. Operation feedback is a separate bottom toast that auto-dismisses after five seconds and stays above mobile navigation. The bell represents account notices only. Seller profile notices keep a 24px separation from the form; dashboard spacing uses the dashboard grid. The dashboard welcome card is conditional; metrics and shortcuts remain available.
- The navigation shares seller role visibility rules with dashboard shortcuts. Local Playwright checks passed with loopback fixtures for title-first ordering, absent breadcrumbs, five-second operation toasts, and the successful order command notification. See specs/0006-seller-alerts-navigation/spec.md.

## Seller store profile progress

- The seller dashboard uses the associated store's saved profile, link and work sample to show owner-only completion progress. It places the store management shortcut first and uses the market state for its independent approval badge. See specs/0007-seller-store-progress/spec.md.
- The isolated seller browser runner passed the percentage, next-step, approval and owner visibility checks. No migration is required.
- The short approval label stays beside its icon in the shared header. The isolated browser runner checks their alignment from 320 to 1440 pixels.

## Navy Dawn palette

- The owner selected the Navy Dawn palette for the seller panel. The shared header and dashboard hero use navy and blue gradients, with saffron on setup actions and the progress card. The isolated browser runner passed.
- Dashboard cards use warmer highlights within the existing navy, berry and copper, saffron and alert severity palettes. The isolated browser runner passed the gradient and alert text contrast checks.
- The activity overview uses four tinted illustration cards with local window and storefront SVGs. Isolated browser checks verified image loading, metric values, and responsive layout.
- The seller dashboard loads activity metrics only for active panels. Dashboard and More navigation show employee management only to active store owners. Isolated browser checks passed.

- The seller profile form uses visible file controls and validates location hierarchy before writes. Work-sample and contact-link sections have warm coordinated headers/actions, accessible table controls, custom empty states, and responsive card rows on narrow screens. Work samples are added through the profile modal; the old standalone AddWorkSample route and view were removed. A corrected rejected profile returns to review. Local browser checks and a malformed PNG test against the local database passed. See specs/0008-seller-profile-editing.md.

- Seller header store status now distinguishes active from merely verified: active stores display «فعال»; active, inactive/correction, and pending states use green, red, and yellow translucent glass backgrounds. Coverage was added to SellerDashboardChecks.

- Seller account status policy: successful edits to seller details, introduction video, or received-cheque policy set both the market and seller profile to WaitingForConfirmPersonalInformations. Expired active charges deactivate both records; a charge check also repairs legacy completion states from expired charge history. The prior three-month branch that marked a retained profile incomplete was removed. The legacy completion status now maps consistently across dashboard and shared pages. SellerDashboardChecks verifies video and cheque post-save review transitions plus completion-status consistency.

## Authoritative checkout and historical landing investigation — 2026-11-17
- Owner-designated working source: Desktop/Drives/App/Projects/Panjerehyab/Source Code/Window14011014/Window. Do not use the Documents/ChatGPT copy for development.
- Owner rejected OldIndex. Reverted all three application files to HEAD and removed only our HomeLandingChecks and spec 0009; existing uploads were preserved. Root remains the storefront.
- Found standalone Home/Index.cshtml with Layout=null, Landing assets, two heading lines and seven image tiles in c351a06ead8052e0664af5f859876553d2a2288c (parent of f8ae37d). origin/NewTemplate also retains this standalone template. f8ae37d replaced it on 2023-08-13. Current OldIndex is a different, later design.
- Fetched origin successfully. Git works with the approved filesystem access; earlier work-tree errors were environment-related, not proof of a damaged repository.
- No historical page activated or deployed. Application diff is empty after rollback; development path documentation remains corrected.
## Standalone home restored — 2026-11-17
- Owner selected the historical seven-tile document. Restored Home/Index from c351a06, with Persian lang and shop tile wired to Shop/ShopLanding. Home action no longer queries catalog data. Shop partial paths are explicit; seller routes unchanged.
- Historical customer-club tile remains inactive. Replaced the legacy notification partial's missing ShowMessage dependency with encoded inline status messages in this standalone document.
- Added spec 0009-standalone-home and offline HomeLandingChecks. No production validation or deployment.
- Final Release HomeLandingChecks and Web/Razor build passed; git diff --check passed. Browser rendering remains unverified.

## Standalone home production release — 2026-11-19
- PR #15 merged as master 8f21a1910045d055687e8dccc55dc960ea343a8d after GitHub build, offline recovery and seller browser checks passed. Local HomeLandingChecks passed.
- Published Window.Web.dll/pdb from the source-equivalent release build via FTP. Previous binaries retained privately in home-rollback-8f21a19 outside httpdocs. Removed app_offline.htm after installation. IIS configuration and database require no changes for this release.
- Live HTTPS root returned 200 with all seven historical tiles, expected heading and /Shop/ShopLanding link. Shop, Login, seller anonymous entry, representative Landing CSS/JS and images returned 200. No authenticated production mutations performed.

## Landing tile colorization — 2026-11-19
- Replaced Landing/images/6.jpg and 7.jpg with colorized illustrations generated using the built-in image tool from their grayscale references. Palette: royal blue, orange, saffron, turquoise and cream. Preserved the customer-club/shop subjects. JPEG conversion only after generation. These are newly chosen colors; no historical color originals were found.
- Three-card revision: shop and inquiry remain linked; assistant uses existing consultation illustration with grayscale CSS and no action. Removed both requested landing footer links. Desktop three-column grid becomes one column below 640px.
- Three-card checks and Release Web/Razor compilation passed; browser visual validation remains pending. Working branch: codex/feature/three-home-cards.
- Added the requested white frame to all three landing cards: 2px white border, 24px radius, 12px padding and border-box sizing. CSS-only verification by source review and git diff --check; browser appearance remains unverified.
- Owner clarified the frame using a screenshot: replaced the outer card border/padding with a 5px white border directly around each image, 24px corners. Titles are outside the frame. Source and diff checks passed; browser appearance not yet verified.

- Border follow-up: inspected owner's Chrome at localhost:44324; missing frames confirmed. Legacy h2::before produced the short white strokes. Added an overflow-clipped image-only frame and suppressed that pseudo-element. IIS still served earlier compiled Razor markup after edits; rebuild verification pending.
- Follow-up evidence: Debug build initially failed because VS/IIS held DLLs. Stopped only this checkout IIS worker, rebuilt successfully (0 errors), and restarted its IIS configuration with the development launcher. Root returns HTTP 200 with the new frame wrapper. Existing offline HomeLandingChecks passed. Final browser screenshot remains unverified.

## Seller panel UI coverage — 2026-11-20
- Added seller-only components: `wwwroot/css/seller-panel.css`, `wwwroot/js/seller-panel.js` and `Areas/Seller/Views/Shared/_SellerEmptyState.cshtml`, loaded by `_SellerLayout` after the approved shell styles.
- Applied the shared layer across the Seller area: card surfaces for legacy widgets, grouped form controls, 44px touch actions, soft button variants, responsive tables that become record cards below 768px, detail/status/empty patterns, file-field feedback, modal focus restoration and removal of widget captions that repeat the glass page title.
- Preserved routes, link targets, model-binding names, HTTP methods, antiforgery, multipart forms, server validation, filters and seller role/ownership checks. The approved dashboard and store profile pages remain the regression reference.
- SellerDashboardChecks now covers `/Seller/User/Index`, `/Seller/Contarct/ListOfContracts`, `/Seller/Ticket/FilterTickets` and the empty ticket state at 360/768/1440px. Representative screenshots are generated under `artifacts/screenshots/seller-panel/` and are not committed.
- Remaining: authenticated flows with real service data, visual review of every seller route, and physical TWA verification. Details: `specs/0010-seller-panel-ui/spec.md`.

## Seller panel PR 16 keyboard follow-up
- Seller modals use Bootstrap 3 jQuery events to focus the first visible editable field and restore the initiating control on dismissal. Added fixture-backed keyboard regression coverage for the work-sample modal. Order, product and Android TWA coverage remains limited as described in spec 0010.
- The three-card landing places the disabled assistant at the far right above 640px, preserves mobile card order, and shows its coming-soon heading in dark gray.
- Index now draws from the ten approved preview colors on server render and sends no-store. Release HomeLandingChecks passed. A 120-response loopback smoke check verified the palette and cache header.
- Index has four cards and stacks below 768px. Seller login uses the colorized storefront and the existing login route. Shared password/SMS completion resolves persisted seller and admin roles, with seller priority. Ordinary accounts retain safe explicit destinations or use UserPanel. Isolated authentication HTTP and landing checks passed.
- Landing grid uses one column below 640px, two centered columns from 640px to below 1200px, and four columns at 1200px and above. HomeLandingChecks and diff whitespace verification passed.
- At widths of 1200px and above, the landing title, subtitle and cards center vertically as a single group. The desktop container grows on short screens. Source-based HomeLandingChecks and git diff whitespace checks passed.
- Landing title and cards center vertically from 640px upward. The title gap scales from 40 to 64px and tablet cards use a viewport height bound. Release HomeLandingChecks passed. Edge confirmed no vertical overflow, vertical centering, and expected columns at nine viewport sizes between 640x800 and 2056x1266.

## Responsive landing gallery
- Applied the approved original gallery to Home/Index. Translucent cards keep titles inside, with two columns on portrait phones and tablets and four on desktop and short landscape screens. The existing wave and palette remain.
- Release HomeLandingChecks passed. Edge rendered the actual local Razor page at 12 viewport sizes from 320x568 to 2056x1266, including 568x320 and 740x360. All had the expected columns, centered content, loaded images, a grayscale assistant and no horizontal or vertical overflow.
