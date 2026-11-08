# ADR 0003 — Git Flow for incremental delivery

## Status

Accepted.

## Context

The repository used `master` for integration and production releases. The owner wants to deliver larger improvements in small, reviewable steps while keeping the existing production release branch.

## Decision

Keep `master` as the default and production release branch. Use `develop` as the integration branch, bootstrapped once from the current `master`. Feature and non-urgent fix PRs target `develop`; release and urgent hotfix PRs target `master`. Bring releases and hotfixes back to `develop` with a sync PR. The [Git Flow guide](../git-flow.md) defines branch names and merge methods.

Require PRs and the GitHub Actions `build` check on both long-lived branches. The repository owner can review and merge without a second required reviewer. Retain independent, authorized Plesk deployments.

## Consequences

Changes can be integrated before a release is selected. Releasing adds a PR to `master` and a sync back to `develop`. Releases are identified by the actual deployed commit, so repository history alone does not imply the hosted site is current.
