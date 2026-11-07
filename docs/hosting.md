# Windows shared hosting

The site is deployed at https://panjerehyab.com/ and https://www.panjerehyab.com/ on IIS through Plesk. Both domain names have a trusted Let's Encrypt certificate. The previous default site was archived privately before the release was activated.

## Database connections

The hosted SQL Server 2022 database contains 75 tables and 129 EF migrations. The application uses a dedicated database-scoped Read and Write account. IIS loads its connection through `ConnectionStrings__WindowConnection` in the deployed `web.config`. Deployment credentials and database backups remain outside Git.

The remote SQL endpoint is `win2016-770ir.hostnegar.com,1431`, database `panjere4_window`. This is the SQL Server instance endpoint discovered from SQL Browser. The normal Plesk connection dialog only shows the server-local named instance.

The hosting SQL endpoint presents an untrusted/self-signed certificate. The configured remote profiles encrypt traffic and use `TrustServerCertificate=True`. This skips SQL certificate chain/hostname validation. The website's HTTPS certificate is independently valid. A provider-issued trusted SQL certificate would allow removing that exception.

## Local source using the hosted database

The local Web project's User Secrets contain `ConnectionStrings:WindowRemote`, including the application account password. The existing `WindowConnection` development connection remains the default. To select the hosted database explicitly:

```powershell
pwsh -NoProfile -File scripts/dev.ps1 Run -UseRemoteDatabase
```

This uses the real hosted database. Changes through the local application affect online data. The connection override is process-scoped and restored when the command exits. User Secrets are stored outside the checkout and are not a production secret vault. A separate user-bound encrypted recovery copy of the generated password was also retained locally in ignored deployment scratch storage.

## Local SQL Server / SSMS

Connect SSMS to the local `localhost\MSSQLSERVER2022` instance and refresh **Server Objects > Linked Servers**. `PANJEREHYAB_REMOTE` provides access to the hosted database. Its stored SQL login mapping is scoped to the owner's Windows login, and the default all-logins mapping was removed.

Read-only connection check:

```sql
SELECT COUNT(*) AS AppliedMigrations
FROM [PANJEREHYAB_REMOTE].[panjere4_window].[dbo].[__EFMigrationsHistory];
```

Expected current result: 130. SSMS can also connect directly to the remote endpoint using the SQL application account. The saved password can be obtained from the project's local User Secrets when needed; do not paste it into repository files or issue discussions.

## Publication verification — 2026-11-04

- HTTPS storefront returned 200 with the expected title and rendered window/profile banners.
- Anonymous cart access returned 302 to the HTTPS Login route with ReturnUrl.
- Representative CSS assets returned 200.
- `web.config`, `appsettings.json`, and `App_Data/Auth/` returned 404 over public HTTPS.
- The normal local Run command using WindowRemote passed the storefront and anonymous-cart smoke checks.
- The local Linked Server returned 129 migration rows from the hosted database.
- Existing authenticated business workflows, payments and SMS delivery still require feature-specific validation. No live SMS/payment transaction was sent during deployment.

Data protection keys are generated per deployment under `App_Data/Auth`. Legacy keys from `wwwroot/Auth` are excluded from publication. Existing old login cookies are not migrated with those keys.

## Password recovery SMS

Set the Kavenegar API key through the deployed IIS environment variable named **kavenegar__recoveryApiKey** before installing the recovery release. Keep the SQL environment variable and existing private application data in place. Confirm an active **Advanced** or higher Kavenegar subscription, an approved **Register** template, and that provider policy allows the host's actual outbound IP. HTTP or API status 416 indicates a source IP mismatch. The production IP and delivery require separate operational verification.

## SMS sign-in publication — 2026-11-07

- Application build `fa06070` is live. The IIS configuration and existing private application data were retained. The four updated assemblies and their symbols were installed under `httpdocs`, and the new assets under `httpdocs/wwwroot/account`.
- Plesk exported `panjere4_window-before-sms-login-20261107.zip` to the private subscription root. The previous binaries and symbols are under `sms-login-rollback-20261107`, also outside the document root. To roll back the application, take IIS briefly offline, restore those eight files, and remove the offline marker. Leave the additive SQL schema in place during an application rollback.
- Migration `20261107120000_AddSmsLogin` is registered in the hosted database. The challenge table is in `dbo` and the application account can read, insert, and update it. Six accounts share three normalized phone numbers and are excluded from SMS sign-in until their ownership is resolved. No credentials were changed.
- Anonymous GETs for login, registration, SMS sign-in and password recovery returned HTTP 200 with the new layout and antiforgery fields. All four new assets returned HTTP 200. An anonymous request to the code page redirected to the SMS sign-in form. No production SMS or login attempt was sent during verification.
