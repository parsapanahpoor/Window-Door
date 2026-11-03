# Local development database

## Source and restore
The owner supplied the backup under the original project's `Publish` directory:
`panjereh_DbContext_2023-06-10_11-41-06.zip`.
The archive contains one SQL backup without a filename extension. Its header identifies database `panjereh_DbContext`, a full backup completed on 2023-06-10 at 11:41:16 (backup metadata time), made by SQL Server major version 15. No newer database backup was found in the two later RAR archives in that directory.

Extracted backup SHA-256: `CBDEB0F0B2A01F31DB1377A51A533548FDFBB2E4E3ED3413DB1B5BB22AD56E2F`.

Restore target: local SQL Server 2022 instance `localhost\MSSQLSERVER2022` (engine 16.0.1000.6). `RESTORE VERIFYONLY` passed. The backup does not contain backup checksums; DBCC CHECKDB was therefore also run after restore and reported no consistency errors.

## Two databases
- `Panjerehyab_Dev_20230610`: restored baseline, 47 user tables and 73 applied EF migrations; latest `20230225130855_ChargeOfNewMarkets`. Its application schema/data was not migrated.
- `Panjerehyab_Dev`: separate working restore with the 56 subsequent repository migrations applied; 75 user tables and 129 applied migrations; latest `20240719102109_Add_ProductInventory_field`.

Both restores used distinct MDF/LDF files under the instance's default DATA directory, with explicit MOVE clauses and no REPLACE. SQL Server upgraded its internal database format during restore; compatibility level remains 150. Both databases passed DBCC CHECKDB. The original ZIP and Desktop project were not modified.

## Application connection
The working database connection uses Windows integrated authentication and is stored in the Web project's local User Secrets, not committed configuration. See [development setup](development.md). Use `scripts/dev.ps1 Run` to start the application with Development configuration.

Do not use the preserved 2023 database for current application development: the current home page requires `ShopProducts`, which does not exist in that schema. The working copy resolves that mismatch. Shop-related tables added after the backup initially contain no historical shop data.

## Validation — 2026-11-04
- Before migration: local home request returned HTTP 500, `Invalid object name 'ShopProducts'`.
- After migration: home returned HTTP 200, `text/html`, and the expected storefront title.
- Anonymous `/Order/ShopCart` returned HTTP 302 to `/Login` with its return URL preserved.
- No authenticated account, order mutation, payment, SMS delivery, or production endpoint was exercised.
- No dedicated business test suite exists. Do not infer functional correctness of all features from these checks.

The restore SQL, generated migration SQL, local HTML responses, and logs are ignored scratch artifacts. Database files, backup contents, customer data and secrets must never be added to Git. Keep the restored baseline available when investigating migration regressions.
