# Agent guide — Panjerehyab / Window-Door

Read docs/README.md, docs/architecture.md, docs/development.md and docs/project-memory.md before changing behavior. Read the relevant spec and source next; documentation is an index, not a substitute for code.

## Working agreement
- Communicate with the owner in Persian; keep code identifiers and technical documentation in English.
- Use the existing isolated checkout. Do not create worktrees unless requested.
- Follow specs/README.md: requirements → acceptance criteria → design/tasks → implementation → evidence. Resolve material business ambiguities before changing behavior; make routine implementation decisions independently.
- Keep scope small. Preserve existing service/CQRS boundaries; do not combine features with broad refactors or dependency upgrades.
- Trace every behavior change to an acceptance criterion and meaningful test. Never claim readiness from zero tests or a successful restore alone.
- Check git status before and after work; preserve user changes. Do not commit credentials, database contents, generated build output, or diagnostic logs.
- Never use production databases, payment gateways or SMS delivery for validation. Use isolated test resources. Do not apply migrations to shared databases without explicit authorization.
- Enforce authorization and ownership at the application boundary; never trust client-supplied user IDs. Review cancellation, transaction boundaries, input validation and failure paths.
- Treat existing compiler failures as baseline blockers, not permission to invent missing business models. Restore original contracts or specify their replacement first.
- Update docs/project-memory.md with durable facts, evidence, decisions and remaining work. Record architectural decisions under docs/adr. Do not store chat transcripts or secrets as memory.

## Commands
Cloud SDK activation and Windows setup: docs/development.md.
`dotnet restore Window.sln --disable-parallel`
`dotnet build Window.sln --no-restore -m:2`
There is currently no dedicated automated test project. Report that explicitly.
