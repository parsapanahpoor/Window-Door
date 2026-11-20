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
- Above 640px, the disabled assistant occupies the rightmost column of the RTL grid. At and below 640px it remains the last card. Its heading uses dark gray and appends the coming-soon label in parentheses.

## Background palette
- Render one background per fresh Index request from the ten approved preview colors. The choice is independent per request and can repeat. Keep it fixed while the page is open.
- Use the preview's dark blue and gray text with the existing white wave and framed cards. The color picker belongs to the preview. Prevent HTTP caching of Index so a fresh request gets a new draw.

## Seller sign-in card
- Index displays four framed images. In desktop reading order from left to right: shop, inquiry, seller sign-in, disabled assistant. Below 640px, use one column. From 640px through widths below 1200px, use a centered two-column grid so tablets display two rows. At 1200px and above, use four columns. Keep the same card sequence at all widths.
- Seller sign-in uses the colorized storefront asset (6.jpg) and links to the public login route.
- After successful password or SMS authentication, an active Seller or SellerMaster assignment takes priority over Admin. An administrator without either seller role goes to Admin. Other users use a safe explicit return URL, or the personal dashboard when there is no explicit destination. Doctor and Support have no dedicated area.
- Resolve destination from persisted roles for the authenticated account. Existing authorization still applies at the destination.
- Verification: Release HomeLandingChecks passed. SmsLoginChecks passed with local disposable SQL and a fake provider, including mixed seller/admin priority, deleted seller assignment, admin fallback, an authenticated visit to /login, and safe local returns for ordinary accounts.
- At 1200px and above, center the title, subtitle and card row vertically as one group. Keep the group in normal flow so it can scroll on short desktop viewports.
- From 640px upward, center the title and cards vertically as a group and leave a larger gap below the subtitle. Bound the two-column grid by the available viewport height on tablets. Standard tablet and desktop sizes should fit without vertical scrolling; shorter viewports may scroll to retain readable, reachable cards.

## Responsive gallery
- Use the approved gallery treatment: translucent cards with rounded images and captions inside, two columns in portrait mobile and tablet, and four on desktop and short landscape screens.
- Center the heading and gallery as a group. Standard viewports fit without scrolling. Unusually short viewports can scroll so every card remains reachable.
- Keep the ten-color palette, white wave, current card order and destinations, and grayscale noninteractive assistant. Use the original gallery preview as the design reference.
- Release HomeLandingChecks passed. Edge rendered the actual local Razor page at 12 viewport sizes from 320x568 to 2056x1266, including 568x320 and 740x360. All had the expected columns, centered content, loaded images, a grayscale assistant and no horizontal or vertical overflow.
