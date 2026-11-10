# 0005 — Shared seller navigation and visual theme

Status: Verified

The seller dashboard introduced a bottom bar on its index page. Sellers need the same primary navigation while working elsewhere in the panel. The old top bar and typography also need a consistent appearance.

## Acceptance criteria

- AC-1: The shared seller layout displays Dashboard, Orders and Support. Mobile pins the bar to the bottom with safe-area clearance and enough room to reach the last controls. Desktop displays the same links below the header. The exact destination has the page state; related order and ticket screens highlight their section. Unrelated pages highlight no tab.
- AC-2: The seller panel uses local Shabnam in regular, medium and bold weights. Page and welcome titles lead the hierarchy, followed by section and alert headings, then card actions, body copy and metadata. Metrics emphasize values over labels, and active navigation is heavier than inactive navigation. Forms and menus use the same family, while icons retain their own font.
- AC-3: The warmer palette carries through the shared header, navigation, page surfaces and dashboard. The header shows the seller avatar and keeps working language, profile and logout links. Navigation and menus support keyboard interaction and preserve the current request culture and seller access boundaries.

## Design and verification

Use the existing seller layout and navbar component. Load seller-only CSS after the legacy theme. Use server-generated links and route data for the selected section. Keep existing form actions and business behavior. Extend the existing browser suite with actual dashboard, order, support and employee-form routes, local font loading and account menu checks.

## Evidence

- AC-1: Playwright passed mobile navigation taps through Dashboard, Orders and Support, verified the selected ticket section on the create route and no selected tab on the employee form, and measured clearance above the fixed bar. The desktop bar remained visible below the header.
- AC-2: The isolated SellerDashboardChecks browser runner loaded all three local Shabnam weights and verified form input sizing, heading and navigation weights, and the relative sizes of dashboard headings, card text and metrics at 320–1440px. Navigation keeps the Font Awesome glyph font.
- AC-3: The avatar loaded, the account menu opened and closed with keyboard controls, and profile, logout and language links stayed available. Existing culture, authorization, role and 320–1440px dashboard checks passed.

The selected Navy Dawn palette uses a navy-to-blue header gradient, an ivory page surface and saffron for store setup actions. Browser verification checked the rendered header and store progress gradients and the 320–1440px seller layout.
