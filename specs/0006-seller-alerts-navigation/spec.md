# 0006 — Seller alerts and complete navigation

Status: Verified

## Acceptance criteria

- AC-1: Every seller page has a notification bell with the count of current notices and an empty state. The bell links to account notice cards, ordered by severity. Operation messages are shown once in a separate toast that appears at the bottom of the viewport and disappears after five seconds, above mobile navigation.
- AC-2: The seller dashboard welcome card appears only when the page has no notices. Dashboard shortcuts remain subject to role and account state. Activity metrics are available for active seller panels. Remove the legacy page header and breadcrumbs from the seller layout. The visible page title is the first item in content, before account notices, inside a wide glass header. Align the title on the right and a browser-back button on the left with a left-pointing arrow before its label; fall back to seller dashboard when there is no previous history entry. On the seller dashboard, replace back with a link to the public storefront. Keep the glass tint warm. Keep a clear gap between account notices and the page content that follows. Use saturated severity accents, text and icons. Display a window illustration in the welcome card.
- AC-3: Dashboard, Orders, and Support remain one tap away. More expands the navigation itself and shows the remaining shortcuts in a grid on every seller page. Links respect existing seller role and account-state visibility and expose the current destination. Mobile navigation remains fixed with safe-area clearance; desktop navigation remains below the header. Menus support touch, keyboard and narrow viewports.

- AC-4: Dashboard cards use warmer accents within the existing color families. The welcome card stays navy, store management stays berry and copper, progress stays saffron, and alert types keep distinct red, orange, blue and green treatments. Body copy on the alert cards remains readable at the brightest gradient stop.

## Design

Use the existing signed-in user, seller state, and TempData sources. No schema or persistent inbox is required for current notices. Reuse one panel model for navigation and notices. Consume seller operation notices once and render their text through Razor encoding. Leave AJAX dialogs and other areas' notification behavior in place.

## Evidence

- AC-1: Playwright verified the empty state, count, correct bell-to-card link, severity ordering, HTML encoding and removal of operation notices on refresh. The order success notice is issued by the successful command response, and the old URL flag does not create a message.
- AC-2: The browser verified the glass heading with RTL title/back-button placement and working browser-back navigation, the dashboard storefront link, warm glass tint, the page title preceding account notices, no rendered breadcrumbs, the welcome card and its SVG appearing only when notices are absent, and removal of the legacy header.
- AC-3: The browser navigated real seller routes, expanded and collapsed More with the keyboard and touch, and opened the employee form. The extra links stayed inside the navigation at 320px, with the last dashboard control reachable. On desktop, the expanded navigation moved page content down. The existing seller role, language, form clearance and 320–1440px viewport checks passed.
- AC-4: The isolated SellerDashboardChecks browser runner verified distinct alert gradients and a minimum 4.5:1 body text contrast against the alert base colors and opaque gradient stops. The existing 320–1440px layout checks passed.

- Seller status in the header uses the concise «فعال» label for active stores, with green translucent glass styling for active, red for inactive/correction, and yellow for pending review/payment.
