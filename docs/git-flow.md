# Git Flow

| Branch | Purpose | Starts from | Pull request target |
| --- | --- | --- | --- |
| `master` | Production release history and GitHub default | Existing branch | — |
| `develop` | Integration for the next release | Bootstrapped from `master` | — |
| `codex/feature/<slug>` or `codex/fix/<slug>` | One change that can be reviewed and verified on its own | `develop` | `develop` |
| `codex/release/<version>` | Freeze and verify a specific release | `develop` | `master` |
| `codex/hotfix/<slug>` | An urgent fix for the production line | `master` | `master` |

Use the `codex/` prefix for agent branches. A bug that can wait for the next release goes through a fix branch from `develop`.

## Work on a change

1. Update `develop` with `git pull --ff-only`, then create the topic branch, for example `git switch -c codex/feature/seller-catalog`.
2. Follow the [spec workflow](../specs/README.md). Put the acceptance criteria, local evidence, and any rollout or rollback implications in the PR to `develop`.
3. Wait for GitHub Actions' `build` check and review the diff. Squash merging is suitable for an independent feature or fix. Delete the topic branch after merging.

## Release and hotfix

1. Start a release from the chosen `develop` commit. Start a hotfix from `master` and include only the urgent correction. Use a PR to `master` and merge with a merge commit so the history can be brought back into `develop`.
2. A merge updates GitHub. When the owner authorizes production installation, follow the [hosting guide](hosting.md), record the rollback plan, and deploy the exact reviewed `master` commit. After verifying the rollout, tag that commit with the actual release version (for example, `v1.2.0`).
3. Open a sync PR from `master` to `develop` and use a merge commit to preserve the shared history. If the branches conflict, open a sync branch from `develop`, merge `origin/master` there, resolve and verify the conflict, and open its PR to `develop`. Check that the sync is merged before cutting the next release.

## Checks and safeguards

Both long-lived branches use pull requests and require GitHub Actions' `build` check. The workflow runs on pull requests and pushes to `master` and `develop`. It restores and builds Release on Windows and runs the offline password recovery checks. GitHub Actions does not deploy to Plesk or apply database migrations.

Use the isolated development resources in the [development guide](development.md) for application validation. A local checkout configured to read the hosted database must use an isolated override before testing application behavior. Keep secrets and deployment artifacts out of Git.
