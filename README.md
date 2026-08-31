# CleanArchitecture.OrderManagement

Small solution demonstrating a clean architecture approach for an Order Management system.

Projects included (high level)
- CleanArchitecture.OrderManagement.API — Web API layer (Controllers)
- CleanArchitecture.OrderManagement.Application — Application services, use cases, DTOs
- CleanArchitecture.OrderManagement.Infrastructure — External concerns (persistence, messaging, integrations)
- (Optional) Domain / Tests projects — core domain model and automated tests

Purpose
This sample shows how to structure a small-to-medium sized backend using Clean Architecture principles: keep business rules in the application/domain layers and keep framework-specific code (ASP.NET Core) in the API layer.

Why Controllers (rather than Minimal APIs) for this solution
- Clear separation of concerns: Controllers group related endpoints into classes which map naturally to resources and make it easy to organize behavior, filters, and per-controller services.
- Familiar patterns: Controllers follow the established MVC/WebAPI pattern used across many teams and older codebases — this improves readability for new maintainers.
- Attribute-based features: Controllers work well with attributes for routing, authorization, model validation, and API versioning without additional wiring.
- Testability and lifecycle: Controllers are straightforward to unit-test and integrate with controller-level filters and action results; dependency injection at the controller level is explicit.
- Scalability and complexity: For larger APIs with many routes, complex parameter binding, or cross-cutting concerns, Controllers provide better structure than a flat Minimal API approach.

When to consider Minimal APIs
- Minimal APIs are a great fit for very small services, prototypes, or simple endpoints where minimal ceremony and fewer files are desired. If you prefer a minimal surface area and have few endpoints, consider using Minimal APIs for those specific endpoints.

Notes
- Store secrets (JWT keys, connection strings) in secure configuration (user-secrets, environment variables, or Azure Key Vault) for production.
- This repository targets .NET 10 and uses C# 14 language features where applicable.
