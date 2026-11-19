# Standalone historical home
Status: Implementing
## Acceptance criteria
- AC1: Root/Home Index renders the standalone seven-tile landing from c351a06 without querying the catalog.
- AC2: Preserve historical headings and assets; use Persian document language. Link shop tile to /Shop/ShopLanding; retain existing service routes.
- AC3: Storefront remains available with resolvable Home partials. Do not change seller panels, database or production configuration.
## Design and validation
Restore the historical Razor document (Layout=null), keep Index action signature, remove its catalog query. Add offline controller and asset/route checks and compile Razor. Full authenticated destination flows are outside scope. No live database validation.

## Evidence
Release HomeLandingChecks passed on 2026-11-17 after final edits: controller without database dependencies, seven tile assets, historical headings, shop link and partial paths. Web and Razor compilation passed; git diff --check passed. Full browser rendering and destination business flows remain unverified; not yet marked Verified.
