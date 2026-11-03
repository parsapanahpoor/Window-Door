# ADR 0001 — Incremental spec-driven development

Status: Accepted for the initial workflow (requested by owner).

Context: A .NET 8 MVC application mixes service-based code and CQRS. The initial baseline lacked source types; they were subsequently restored upstream. Runtime validation still needs an isolated database. The owner wants reliable, stepwise development with an agent.

Decision: Keep versioned Markdown specs, AGENTS.md and an evidence-based project memory in the repository. Use acceptance criteria to guide scoped implementation and tests. Preserve the current architecture until an explicit change requires redesign. Do not require a new external spec framework or rewrite to begin.

Consequences: Documents require maintenance alongside code. Existing behavior must be characterized rather than inferred from naming. Build restoration precedes claims of application readiness. Architectural changes get separate ADRs and migration plans.
