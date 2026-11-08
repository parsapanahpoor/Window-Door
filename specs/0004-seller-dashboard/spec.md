# 0004 — Persian default and responsive seller dashboard

Status: Verified

First-time visits currently use English request localization. The seller dashboard uses desktop widgets and icon-only shortcut targets. Seller users primarily access it on phones, including the Android TWA.

## Acceptance criteria

- AC-1: A request without an explicit language uses fa-IR for culture and resources throughout MVC, including a browser with English preferences. Explicit supported query and cookie selections still work; unsupported preferences fall back to Persian. Existing Gregorian and decimal formatting stays compatible.
- AC-2: The seller dashboard fits 320px mobile through desktop widths without sideways scrolling. The mobile view has tappable full shortcut cards, fixed bottom navigation, and space for device insets. Desktop shows a wider grid and retains the seller account and shortcuts.
- AC-3: Dashboard counts, account alerts and existing shortcut capabilities and role conditions are preserved. Add employee opens the existing employee form. Anonymous requests still require sign-in. Other seller pages retain the existing layout.

## Design and tasks

Use one shared request-localization configuration. Scope dashboard styling to Index, reuse the existing seller components, font and icon assets, and keep the TWA wrapper and seller contracts intact. Exercise real Razor, MVC and browser layout with isolated service fixtures. No schema changes.

## Evidence

- AC-1: The loopback middleware checks passed for a fresh English-preference browser, explicit query and cookie choices, unsupported languages, Gregorian calendar and decimal parsing format.
- AC-2: Playwright with Edge passed viewport checks at 320, 390, 767, 768, 1024 and 1440px. It measured grid columns, touch targets, horizontal overflow and bottom-navigation clearance.
- AC-3: The real controller and Razor views passed with service fixtures for active managers, employees and pending sellers. The runner checked authentication, counts, notices, role-gated shortcuts and URLs. Physical Android TWA and deployment are separate verification steps.
- Release verification passed. Existing compiler and NuGet audit warnings remain.
