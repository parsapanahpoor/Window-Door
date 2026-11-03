# Current architecture

Baseline examined: commit 2e842cb (updated from 96a4bca before documentation publication). ASP.NET Core MVC on .NET 8 with Razor views, EF Core 8 and SQL Server. Frontend libraries are committed under Window.Web/wwwroot; their nested package manifests are not a root frontend build workflow.

## Projects and dependencies
| Project | Responsibility | Project references |
| --- | --- | --- |
| Window.Domain | Entities, DTOs, view models and repository contracts | None |
| Window.Infra.Data | WindowDbContext, EF migrations, repositories | Domain |
| Window.Application | Services, CQRS handlers, validation, unit of work | Domain, Infra.Data |
| Window.Infra.IoC | Explicit dependency registration | Application |
| Window.Web | Controllers, Razor views, middleware and composition | Application, Infra.Data, Infra.IoC |

This is a layered application with mixed service and CQRS patterns. Application directly references data infrastructure; do not describe the current system as strict Clean Architecture.

## Composition and request paths
Window.Web/Program.cs configures localization, cookie authentication, session, MVC, SQL Server, static files and area/default routes. Window.Application/ConfigureServices.cs registers MediatR handlers and a FluentValidation pipeline. Window.Infra.IoC/Container/DependencyContainer.cs registers repositories and services.

HomeController.Index → MediatR ShopLandingQuery → shop landing view. Legacy path: HomeController.OldIndex → IAdminDashboardService.FillHomeIndexViewModel → Razor view. These pages depend on persisted data.

Shop path: Controllers/Shop/OrderController → SiteBaseController.Mediator → CQRS/SiteSide/ShopOrder handlers → repository interfaces → EF repositories → WindowDbContext. UnitOfWork.SaveChangesAsync persists tracked changes. ShopOrderQueryHandler performs writes despite its Query name; inspect behavior rather than assuming names express read-only semantics.

OrderController is authorized and obtains user identity from claims. AdminBaseController adds authorization and CheckUserHasPermission. Authorization must be checked per endpoint; these examples do not prove all endpoints safe.

## Functional map
- Identity, users, roles and permissions: IdentityController, UserService, Domain/Entities/Account.
- Inquiry and window pricing: InquiryAPIController, InquiryProductController, InquryService, ProductService, entities for product/brand/glass/segment/sample.
- Seller and market management: Areas/Seller, SellerService, MarketService, market information entities.
- Shop catalog, cart, order, delivery location and cheques: Controllers/Shop, Application/CQRS, ShopProduct/ShopOrder entities and split query/command repositories.
- Wallet and payments: PaymentController, OrderPaymentController, WalletService. External gateway packages include ZarinPal.
- Content and support: articles, questions, tickets, consultants and technical issues.
- Messaging: SMSService/BulkSMSService and Kavenegar dependency.

## Review priorities, not completed audits
Payment idempotency, order ownership, monetary units/rounding, transaction boundaries, upload validation, permission coverage and migration safety require explicit specs and tests. System.Drawing.Common and Windows-style filesystem paths require Linux compatibility checks. Existing package audit warnings require a separate dependency maintenance task.
