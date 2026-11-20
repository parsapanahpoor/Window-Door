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

## Three-card revision — 2026-11-19
- Exactly three cards: shop links to Shop/ShopLanding, free inquiry links to InquiryProduct/InquiryStep1, intelligent assistant uses existing consultation illustration (3.jpg) with grayscale CSS and no link or focusable control.
- Remove the developer credit and TermsConditions footer links from this landing.
- Responsive grid uses three columns on desktop and one column below 640px. Update offline regression checks for card count, destinations, disabled semantics and removed links.
- Three-card revision evidence: Release HomeLandingChecks and Web/Razor compilation passed on 2026-11-19. git diff --check passed. Browser visual checks remain pending.
- Card frame criterion: all three images have a continuous 5px white border and 24px rounded corners, directly against the image with no padding. Titles sit outside the frame; border-box keeps images inside responsive columns.

- Frame implementation uses a dedicated image wrapper (5px white, 24px corners, clipped overflow); suppress legacy heading decoration. Confirm live rendered markup after rebuilding, not source presence alone.
