# Spec-driven workflow

Each meaningful feature or bug fix has `specs/NNNN-short-name/spec.md`. Start from templates/spec.md. Small documentation corrections can update the existing spec directly.

1. Discover the relevant code and current behavior. Separate verified facts, assumptions and open questions.
2. Specify actors, problem, scope, non-goals, observable acceptance criteria and failure cases. Ask the owner only about consequential product decisions that cannot be inferred.
3. Record the design, affected contracts, data/migration implications and ordered tasks. Do not change business behavior while requirements remain materially ambiguous.
4. Implement the smallest coherent change. Add tests that fail for the original bug or unmet criterion; include authorization/ownership and failure cases when relevant.
5. Verify each criterion and record the command, result and limitations. Review the diff and update architecture/memory if needed.
6. Publish only to the authorized destination; distinguish saved draft, Git push, deployment and environment publication.

Statuses: Draft → Ready → Implementing → Verified; use Blocked with the exact prerequisite when necessary. An agent must not mark a spec Verified while required acceptance checks are unrun or failing. These are project conventions, not claims that a tool enforces the process.
