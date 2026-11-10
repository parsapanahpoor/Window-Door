# 0004 — Persian default and responsive seller dashboard

Status: Verified

First-time visits currently use English request localization. The seller dashboard uses desktop widgets and icon-only shortcut targets. Seller users primarily access it on phones, including the Android TWA.

## Acceptance criteria

- AC-1: A request without an explicit language uses fa-IR for culture and resources throughout MVC, including a browser with English preferences. Explicit supported query and cookie selections still work; unsupported preferences fall back to Persian. Existing Gregorian and decimal formatting stays compatible.
- AC-2: The seller dashboard fits 320px mobile through desktop widths without sideways scrolling. The mobile view has tappable full shortcut cards, fixed bottom navigation, and space for device insets. Desktop shows a wider grid and retains the seller account and shortcuts.
- AC-3: Dashboard counts, account alerts and existing shortcut capabilities and role conditions are preserved. Add employee opens the existing employee form. Anonymous requests still require sign-in. Other seller pages retain the existing layout.

- AC-4: The four activity cards render distinct tinted backgrounds with window or storefront illustrations. Labels and counts stay readable in the mobile and desktop grids.

- AC-5: The activity summary appears only for active seller panels. The employee list and add employee shortcuts appear in both dashboard and More navigation only for active store owners.

## Design and tasks

Use one shared request-localization configuration. Scope dashboard styling to Index, reuse the existing seller components, font and icon assets, and keep the TWA wrapper and seller contracts intact. Exercise real Razor, MVC and browser layout with isolated service fixtures. No schema changes.

## Evidence

- AC-1: The loopback middleware checks passed for a fresh English-preference browser, explicit query and cookie choices, unsupported languages, Gregorian calendar and decimal parsing format.
- AC-2: Playwright with Edge passed viewport checks at 320, 390, 767, 768, 1024 and 1440px. It measured grid columns, touch targets, horizontal overflow and bottom-navigation clearance.
- AC-3: The real controller and Razor views passed with service fixtures for active managers, employees and pending sellers. The runner checked authentication, counts, notices, role-gated shortcuts and URLs. Physical Android TWA and deployment are separate verification steps.
- Release verification passed. Existing compiler and NuGet audit warnings remain.
- AC-4: The isolated browser runner verified four distinct layered backgrounds, decoded both illustrations, and passed the responsive dashboard checks.
- AC-5: Browser checks verified that pending sellers have no activity summary or employee links and active employees retain metrics without employee-management links.
