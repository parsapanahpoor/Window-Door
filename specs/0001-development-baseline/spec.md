# 0001 — Reproducible development baseline

Status: Blocked (runtime/database validation outstanding)

## Problem
The project needs persistent agent context and a small, repeatable spec-first development process. The cloud SDK is installed. The initial checkout lacked source contracts; upstream commit 2e842cb restores the three missing view-model files. Runtime and database readiness still need validation.

## Scope
Document actual architecture, establish AGENTS.md and a spec workflow, preserve local/cloud setup instructions, and record repeatable validation. No wholesale architecture rewrite, fabricated domain models or production database operations.

## Acceptance criteria
- AC-1: A new agent can locate architecture, commands, current blockers and working conventions through AGENTS.md.
- AC-2: Cloud and Windows setup commands and their verification status are documented.
- AC-3: SDK activation, restore and build checks report failures accurately.
- AC-4: Full solution builds and a representative database-backed page responds correctly using isolated development data.
- AC-5: Environment instructions are saved; publication status is reported accurately.

## Tasks and evidence
- [x] Inspect project dependencies, composition root and representative service/CQRS flows.
- [x] Add architecture, memory, agent guidance, spec template and development instructions.
- [x] Recover missing view-model contracts from authoritative source (upstream 2e842cb).
- [ ] Verify complete build, configure development database, validate HTTP behavior.
- [ ] Publish environment through the product UI.

AC-1/AC-2 are documentation deliverables. AC-4 remains blocked; do not mark the environment fully ready. Windows commands have not been executed by the cloud agent. No automated business tests have been run.

## Current verification
- AC-1/AC-2: documentation created and all relative Markdown links resolve. Windows runtime checks remain unrun.
- AC-3: bash syntax passed; check-development.sh restored packages, reproduced 15 compile errors and returned exit 1 correctly.
- AC-4: original missing types restored from upstream; database/HTTP validation unrun.
- AC-5: updated start_skill saved successfully in draft; user-interface publication pending.

After reconciling upstream 2e842cb, the full cloud solution build passed (exit 0). AC-4 remains incomplete because isolated database and HTTP checks have not run.
