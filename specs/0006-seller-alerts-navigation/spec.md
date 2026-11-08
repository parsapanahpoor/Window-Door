# 0006 — Seller alerts and complete navigation

Status: Verified

## Acceptance criteria

- AC-1: Every seller page has a notification bell with the count of current notices and an empty state. The same account and operation notices appear at the top of the page, sorted as error, warning, information, then success. The bell links to the respective cards. Operation notices retain their one-request lifetime.
- AC-2: The seller dashboard welcome card appears only when the page has no notices. Dashboard metrics and shortcuts remain available. Remove the legacy page header from the seller layout and keep a meaningful title. Use severity text and icons in addition to color.
- AC-3: Dashboard, Orders, and Support remain one tap away. More opens the remaining shortcuts from every seller page. Links respect existing seller role and account-state visibility and expose the current destination. Mobile navigation remains fixed with safe-area clearance; desktop navigation remains below the header. Menus support touch, keyboard and narrow viewports.

## Design

Use the existing signed-in user, seller state, and TempData sources. No schema or persistent inbox is required for current notices. Reuse one panel model for navigation and notices. Consume seller operation notices once and render their text through Razor encoding. Leave AJAX dialogs and other areas' notification behavior in place.

## Evidence

- AC-1: Playwright verified the empty state, count, correct bell-to-card link, severity ordering, HTML encoding and removal of operation notices on refresh. The order success notice is issued by the successful command response, and the old URL flag does not create a message.
- AC-2: The browser verified the four severity colors, notices preceding dashboard content, the welcome card appearing only when notices are absent, and removal of the legacy header.
- AC-3: The browser navigated real seller routes, opened the more menu using the keyboard, and opened the employee form using touch. The menu fit at 320px. The existing seller role, language, form clearance and 320–1440px viewport checks passed.
