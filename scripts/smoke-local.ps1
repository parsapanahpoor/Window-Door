[CmdletBinding()]
param([uri]$BaseUrl = 'http://127.0.0.1:5075')

$ErrorActionPreference = 'Stop'
if (-not $BaseUrl.IsLoopback -or $BaseUrl.Scheme -ne 'http') {
    throw 'This smoke check only targets a local HTTP development server.'
}
$handler = [System.Net.Http.HttpClientHandler]::new()
$handler.AllowAutoRedirect = $false
$client = [System.Net.Http.HttpClient]::new($handler)
$client.Timeout = [TimeSpan]::FromSeconds(20)
try {
    $response = $client.GetAsync([uri]::new($BaseUrl, '/')).GetAwaiter().GetResult()
    $html = $response.Content.ReadAsStringAsync().GetAwaiter().GetResult()
    if ([int]$response.StatusCode -ne 200 -or
        $response.Content.Headers.ContentType.MediaType -ne 'text/html' -or
        [System.Net.WebUtility]::HtmlDecode($html) -notmatch 'فروشگاه پنجره یاب') {
        throw 'Home page did not return the expected storefront HTML with HTTP 200.'
    }
    $response.Dispose()

    $response = $client.GetAsync([uri]::new($BaseUrl, '/Order/ShopCart')).GetAwaiter().GetResult()
    if ([int]$response.StatusCode -ne 302 -or -not $response.Headers.Location) {
        throw 'An unauthenticated cart request did not redirect to login.'
    }
    $loginUrl = [uri]::new($BaseUrl, $response.Headers.Location)
    if ($loginUrl.AbsolutePath -ne '/Login' -or
        [uri]::UnescapeDataString($loginUrl.Query) -notmatch 'ReturnUrl=/Order/ShopCart') {
        throw 'Cart redirect does not preserve the expected login return URL.'
    }
    $response.Dispose()
    Write-Host 'PASS: storefront HTML and unauthenticated cart redirect.'
    Write-Host 'Not covered: authenticated flows, payments, SMS, or full business behavior.'
}
finally { $client.Dispose(); $handler.Dispose() }
