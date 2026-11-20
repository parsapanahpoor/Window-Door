# 0010 — Seller panel UI coverage

Status: Implementing

## Problem

The dashboard (`/Seller`), the store profile editor and the SMS/standalone shell were redesigned and verified in specs 0004–0008. The remaining seller screens still use the legacy admin theme: duplicate page titles, desktop tables on phones, tiny badge-sized actions, ungrouped forms and no shared empty state. Sellers mostly use phones and the Android TWA, so the whole working path must use the approved visual language.

## Scope

- Shared, seller-scoped components only (`wwwroot/css/seller-panel.css`, `wwwroot/js/seller-panel.js`, `Areas/Seller/Views/Shared`). No changes to storefront, identity or admin CSS.
- Presentation only. Routes, HTTP methods, model binding names, antiforgery tokens, multipart forms, server validation, filters and role/ownership checks stay unchanged.
- The approved dashboard, notifications, navigation and store profile pages are the visual reference and the regression baseline.

## Non-goals

- No database, migration, authorization or business-state change.
- No new seller feature and no replacement of existing modals or handlers.
- No authenticated destination flow beyond the fixture-backed browser runner.

## Page inventory

Legend: **R** = approved reference/regression page; **S** = covered by the shared shell layer; **T** = page-specific structural change.

| Page or route | Type | Role / precondition | Current problem | Required change | Test evidence |
| --- | --- | --- | --- | --- | --- |
| `/Seller` | Dashboard (R) | Signed-in owner/employee | None (reference) | Regression only | SellerDashboardChecks |
| `/Seller/SellerPersonalInfo/ListOfPersonalInfo` | Owner form + lists (R) | Owner | None (reference) | Regression only | SellerDashboardChecks profile checks |
| `/Seller/ShopProduct/FilterShopProducts` | List + filter (S) | Active owner/employee | Desktop table on phones, 5 tiny badge actions | Card surface, responsive record table, touch actions, empty state | SellerPanelChecks |
| `/Seller/ShopProduct/CreateProduct`, `EditProduct` | Form (S/T) | Owner | Duplicate title, ungrouped fields, weak primary action | Card form, grouped fields, sticky-free actions, file preview | SellerPanelChecks form checks |
| `/Seller/ShopProduct/EditProductCategory` | Multi-select form (S) | Owner | Legacy list layout | Card form + responsive list | Build + view inspection |
| `/Seller/ShopProduct/ProductGallery` | List/form + file upload (S) | Owner | Small actions, raw file input | Responsive list, file field feedback | Build + view inspection |
| `/Seller/ShopProduct/ProductFeature`, `ProductBrand` | List/form (S) | Owner | Legacy table and tiny actions | Responsive table/cards, touch actions | Build + view inspection |
| `/Seller/Product/FilterProducts` | List + filter (S) | Owner | Desktop table, dense filter card | Responsive list, grouped filter, empty state | View inspection |
| `/Seller/Product/CreateProduct`, `CreateGlassPricing`, `SegmentPricing` | Forms (S) | Owner | Duplicate title, ungrouped fields | Card form, grouped fields, clear submit | View inspection |
| `/Seller/ShopOrder/ListOFUserOrdersAsSeller` | List + filter (S) | Owner/employee | Desktop table, small actions | Responsive record cards, status text, touch actions | SellerPanelChecks route |
| `/Seller/ShopOrder/ListOFUserOrdersAsCustomer` | List + filter (S) | Owner/employee | Same | Same | View inspection |
| `/Seller/ShopOrder/ManageOrderPage` | Hub (S) | Owner/employee | Legacy tiles | Shared card styling | View inspection |
| `/Seller/ShopOrder/ManageShopOrder` | Detail (T) | Owner/employee | Legacy profile header, dense stats | Detail cards, readable labels/values, tab surfaces | View inspection |
| `/Seller/ShopOrder/ChequeDetail` | Detail + POST (T) | Owner | Duplicate title, dense fields | Detail grid, action bar, state text | View inspection |
| `/Seller/OrderCheque/ManageOrderChequesPages` | Hub (S) | Owner/employee | Legacy info widget | Shared card styling | View inspection |
| `/Seller/OrderCheque/ListOfWithdrawCheques`, `ListOfDepositCheques` | List + filter (S) | Owner/employee | Desktop tables, small actions | Responsive cards, grouped filters | View inspection |
| `/Seller/OrderCheque/UploadOrderCheque`, `UploadChequeReceipt` | Upload form (S/T) | Owner | Raw file input, duplicate title | File field with name/type feedback, card form | View inspection |
| `/Seller/Contarct/ListOfContracts` | List + POST actions (S) | Owner | Desktop table, icon-only decisions | Responsive cards, labelled accept/decline actions | SellerPanelChecks route |
| `/Seller/User/Index` | List + filter (S) | Active owner | Desktop table on phones | Responsive record cards, grouped filter | SellerPanelChecks (360/768/1440) |
| `/Seller/User/AddNewUser` | Form + avatar upload (S/T) | Active owner | Raw file input, duplicate title | Card form, avatar field feedback | View inspection |
| `/Seller/Ticket/FilterTickets` | List + filter (S) | Owner/employee | Desktop table | Responsive cards, status pills | SellerPanelChecks route |
| `/Seller/Ticket/CreateTicket` | Form (S) | Owner/employee | Duplicate title | Card form, clear submit | View inspection |
| `/Seller/Ticket/TicketDetail` | Detail + reply (T) | Owner/employee | Legacy wells, raw file input | Conversation bubbles, file feedback, clear reply action | View inspection |
| `/Seller/SellerPersonalInfo/PageOfManageSellerInformation` | Hub (S) | Owner | Legacy databox tiles | Shared tile/card treatment | View inspection |
| `/Seller/SellerPersonalInfo/AddPersonalInfo`, `ManagePersonalInfoLink` | Forms (S) | Owner | Duplicate titles, ungrouped fields | Card form, grouped fields | View inspection |
| `/Seller/SellerPersonalInfo/AddSellerPersonalLinks`, `AddSellerWorkSampleTitle` | Lists/forms (S) | Owner | Legacy tables, small actions | Responsive cards, touch actions, empty state | View inspection |
| `/Seller/SellerPersonalInfo/EditAndCreateVideos` | Form + file (S/T) | Owner | Raw file input, duplicate title | File feedback, card form | View inspection |
| `/Seller/SellerChequeInfo/AddOrEditSellerChequeInfo` | Form (S) | Owner | Duplicate title, dense fields | Grouped card form | View inspection |
| `/Seller/SellerPersonalInfo/MarketChargeInformation`, `ChargeAccount/{id}`, `BankPay` | Detail/charge (S) | Owner | Legacy tiles/forms | Shared cards, readable amounts | View inspection |
| `/Seller/Account/EditProfile`, `ChangePassword` | Forms (S) | Owner | Duplicate titles, legacy widget headers | Card form, grouped fields, clear submit | View inspection |
| `/Seller/Home/FilterLogForVisitSellerProfile` | List + filter (S) | Owner | Desktop table | Responsive cards, grouped filter | View inspection |

## Design

- `seller-panel.css` is loaded only by `_SellerLayout` after `seller-shell.css`. It provides: card surfaces for legacy `.widget`/`.well`, section headers, control sizing (44px), button variants, grouped form grids, responsive tables that become record cards below 768px, empty states, file-field feedback, detail grids, status pills and conversation bubbles.
- `seller-panel.js` adds `data-label` values from table headers, marks action cells, wraps them in `.seller-row-actions`, removes a widget caption that repeats the glass page title, reports invalid file types and restores focus when a Bootstrap modal closes.
- `_SellerEmptyState.cshtml` replaces the bare "Not Found" notice.
- The approved pages keep their dedicated CSS; changes are additive and scoped to `body.seller-shell`.

## Preserved contracts

- All controller routes, form actions, field names, `multipart/form-data`, `[ValidateAntiForgeryToken]`, filters and seller role/ownership checks are untouched.
- Successful edits to seller info, introduction video and received-cheque info still move the store to review through the existing service calls.
- No client-side rule replaces a server check; hidden or restyled controls are not authorization.

## Verification plan

1. `dotnet build Window.sln --no-restore -m:2` (Release via `scripts/dev.ps1 Verify`).
2. `dotnet run --project tests/SellerDashboardChecks -c Release` (regression).
3. `dotnet run --project tests/SellerPanelChecks -c Release` with fixture-backed routes at 360, 768 and 1440px: no horizontal overflow, navigation does not cover the primary action, actions are at least 44px, tables become cards on phones, empty state renders.
4. Representative screenshots with test data.

## Evidence

- `dotnet build Window.sln --no-restore -m:2`: 0 errors, 1595 warnings.
- `dotnet run --project tests/SellerDashboardChecks -c Release` (Edge channel): PASS. Existing dashboard, profile, navigation, notice and store-state checks still pass.
- Added list-screen checks: `/Seller/User/Index`, `/Seller/Contarct/ListOfContracts` and `/Seller/Ticket/FilterTickets` render with one page title, fixture rows, labelled cells, the shared stylesheet, no horizontal overflow at 360/768/1440px, card-mode rows and touch-sized actions at 360px. The filtered ticket list with an empty fixture shows `_SellerEmptyState`.
- Representative screenshots with fixture data were generated under `artifacts/screenshots/seller-panel/` (dashboard, store profile, employees, contracts and tickets at 360 and 1440px). They are ignored build artifacts and are not committed.
- Not yet verified: authenticated flows against real service data, browser rendering of every remaining route, physical Android TWA, and form submit round-trips outside the existing profile/video/cheque coverage.
