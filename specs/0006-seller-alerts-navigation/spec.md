# 0006 — Seller alerts and complete navigation

Status: Verified

## Acceptance criteria

- AC-1: Every seller page has a notification bell with the count of current notices and an empty state. The same account and operation notices appear at the top of the page, sorted as error, warning, information, then success. The bell links to the respective cards. Operation notices retain their one-request lifetime.
- AC-2: The seller dashboard welcome card appears only when the page has no notices. Dashboard metrics and shortcuts remain available. Remove the legacy page header from the seller layout and keep a meaningful title. Use saturated severity accents, text and icons. Display a window illustration in the welcome card.
- AC-3: Dashboard, Orders, and Support remain one tap away. More expands the navigation itself and shows the remaining shortcuts in a grid on every seller page. Links respect existing seller role and account-state visibility and expose the current destination. Mobile navigation remains fixed with safe-area clearance; desktop navigation remains below the header. Menus support touch, keyboard and narrow viewports.

- AC-4: Dashboard cards use warmer accents within the existing color families. The welcome card stays navy, store management stays berry and copper, progress stays saffron, and alert types keep distinct red, orange, blue and green treatments. Body copy on the alert cards remains readable at the brightest gradient stop.

## Design

Use the existing signed-in user, seller state, and TempData sources. No schema or persistent inbox is required for current notices. Reuse one panel model for navigation and notices. Consume seller operation notices once and render their text through Razor encoding. Leave AJAX dialogs and other areas' notification behavior in place.

## Evidence

- AC-1: Playwright verified the empty state, count, correct bell-to-card link, severity ordering, HTML encoding and removal of operation notices on refresh. The order success notice is issued by the successful command response, and the old URL flag does not create a message.
- AC-2: The browser verified the four severity colors, notices preceding dashboard content, the welcome card and its SVG appearing only when notices are absent, and removal of the legacy header.
- AC-3: The browser navigated real seller routes, expanded and collapsed More with the keyboard and touch, and opened the employee form. The extra links stayed inside the navigation at 320px, with the last dashboard control reachable. On desktop, the expanded navigation moved page content down. The existing seller role, language, form clearance and 320–1440px viewport checks passed.
- AC-4: The isolated SellerDashboardChecks browser runner verified distinct alert gradients and a minimum 4.5:1 body text contrast against the alert base colors and opaque gradient stops. The existing 320–1440px layout checks passed.
