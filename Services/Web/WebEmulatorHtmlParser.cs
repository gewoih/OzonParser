using OzonParser.Services.Base;
using PuppeteerSharp;

namespace OzonParser.Services.Web
{
    public sealed class WebEmulatorHtmlParser : IWebHtmlParser
    {
        private readonly IBrowser _browser;

        public WebEmulatorHtmlParser()
        {
            var options = new LaunchOptions()
            {
                Headless = true,
                ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
            };

            _browser = Puppeteer.LaunchAsync(options).Result;
        }

        public async Task<string> GetHtmlAsync(string url)
        {
            var page = await _browser.NewPageAsync();
            await page.SetExtraHttpHeadersAsync(new Dictionary<string, string>
            {
                { "accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,th;q=0.6" },
                { "cookie", "__Secure-ab-group=54; __Secure-user-id=0; xcid=863af8e4afea59cd9ad1e640c62affe7; __Secure-ext_xcid=863af8e4afea59cd9ad1e640c62affe7; guest=true; rfuid=NjkyNDcyNDUyLDEyNC4wNDM0NzUyNzUxNjA3NCwtMjg3MDM2NTYzLC0xLDk4MDIxNjIxNCxXM3NpYm1GdFpTSTZJbEJFUmlCV2FXVjNaWElpTENKa1pYTmpjbWx3ZEdsdmJpSTZJbEJ2Y25SaFlteGxJRVJ2WTNWdFpXNTBJRVp2Y20xaGRDSXNJbTFwYldWVWVYQmxjeUk2VzNzaWRIbHdaU0k2SW1Gd2NHeHBZMkYwYVc5dUwzQmtaaUlzSW5OMVptWnBlR1Z6SWpvaWNHUm1JbjBzZXlKMGVYQmxJam9pZEdWNGRDOXdaR1lpTENKemRXWm1hWGhsY3lJNkluQmtaaUo5WFgwc2V5SnVZVzFsSWpvaVEyaHliMjFsSUZCRVJpQldhV1YzWlhJaUxDSmtaWE5qY21sd2RHbHZiaUk2SWxCdmNuUmhZbXhsSUVSdlkzVnRaVzUwSUVadmNtMWhkQ0lzSW0xcGJXVlVlWEJsY3lJNlczc2lkSGx3WlNJNkltRndjR3hwWTJGMGFXOXVMM0JrWmlJc0luTjFabVpwZUdWeklqb2ljR1JtSW4wc2V5SjBlWEJsSWpvaWRHVjRkQzl3WkdZaUxDSnpkV1ptYVhobGN5STZJbkJrWmlKOVhYMHNleUp1WVcxbElqb2lRMmh5YjIxcGRXMGdVRVJHSUZacFpYZGxjaUlzSW1SbGMyTnlhWEIwYVc5dUlqb2lVRzl5ZEdGaWJHVWdSRzlqZFcxbGJuUWdSbTl5YldGMElpd2liV2x0WlZSNWNHVnpJanBiZXlKMGVYQmxJam9pWVhCd2JHbGpZWFJwYjI0dmNHUm1JaXdpYzNWbVptbDRaWE1pT2lKd1pHWWlmU3g3SW5SNWNHVWlPaUowWlhoMEwzQmtaaUlzSW5OMVptWnBlR1Z6SWpvaWNHUm1JbjFkZlN4N0ltNWhiV1VpT2lKTmFXTnliM052Wm5RZ1JXUm5aU0JRUkVZZ1ZtbGxkMlZ5SWl3aVpHVnpZM0pwY0hScGIyNGlPaUpRYjNKMFlXSnNaU0JFYjJOMWJXVnVkQ0JHYjNKdFlYUWlMQ0p0YVcxbFZIbHdaWE1pT2x0N0luUjVjR1VpT2lKaGNIQnNhV05oZEdsdmJpOXdaR1lpTENKemRXWm1hWGhsY3lJNkluQmtaaUo5TEhzaWRIbHdaU0k2SW5SbGVIUXZjR1JtSWl3aWMzVm1abWw0WlhNaU9pSndaR1lpZlYxOUxIc2libUZ0WlNJNklsZGxZa3RwZENCaWRXbHNkQzFwYmlCUVJFWWlMQ0prWlhOamNtbHdkR2x2YmlJNklsQnZjblJoWW14bElFUnZZM1Z0Wlc1MElFWnZjbTFoZENJc0ltMXBiV1ZVZVhCbGN5STZXM3NpZEhsd1pTSTZJbUZ3Y0d4cFkyRjBhVzl1TDNCa1ppSXNJbk4xWm1acGVHVnpJam9pY0dSbUluMHNleUowZVhCbElqb2lkR1Y0ZEM5d1pHWWlMQ0p6ZFdabWFYaGxjeUk2SW5Ca1ppSjlYWDFkLFd5SnlkUzFTVlNKZCwwLDEsMCwyNCwyMzc0MTU5MzAsOCwyMjcxMjY1MjAsMSwxLDAsLTQ5MTI3NTUyMyxSMjl2WjJ4bElFbHVZeTRnVG1WMGMyTmhjR1VnUjJWamEyOGdWMmx1TXpJZ05TNHdJQ2hYYVc1a2IzZHpJRTVVSURFd0xqQTdJRmRwYmpZME95QjROalFwSUVGd2NHeGxWMlZpUzJsMEx6VXpOeTR6TmlBb1MwaFVUVXdzSUd4cGEyVWdSMlZqYTI4cElFTm9jbTl0WlM4eE1Ea3VNQzR3TGpBZ1UyRm1ZWEpwTHpVek55NHpOaUF5TURBek1ERXdOeUJOYjNwcGJHeGgsZXlKamFISnZiV1VpT25zaVlYQndJanA3SW1selNXNXpkR0ZzYkdWa0lqcG1ZV3h6WlN3aVNXNXpkR0ZzYkZOMFlYUmxJanA3SWtSSlUwRkNURVZFSWpvaVpHbHpZV0pzWldRaUxDSkpUbE5VUVV4TVJVUWlPaUpwYm5OMFlXeHNaV1FpTENKT1QxUmZTVTVUVkVGTVRFVkVJam9pYm05MFgybHVjM1JoYkd4bFpDSjlMQ0pTZFc1dWFXNW5VM1JoZEdVaU9uc2lRMEZPVGs5VVgxSlZUaUk2SW1OaGJtNXZkRjl5ZFc0aUxDSlNSVUZFV1Y5VVQxOVNWVTRpT2lKeVpXRmtlVjkwYjE5eWRXNGlMQ0pTVlU1T1NVNUhJam9pY25WdWJtbHVaeUo5ZlgxOSw2NSwyMTI5MDQxMjI2LDEsMSwtMSwxNjk5OTU0ODg3LDE2OTk5NTQ4ODcsMzM2MDA3OTMzLDY=; _ga=GA1.1.122628229.1674200520; tmr_lvid=cd2d7dfe830d1484f5a6a6c43c34eee5; tmr_lvidTS=1674200521781; __exponea_etc__=7a95d1de-0bd6-4bcb-819f-ca93f1a2153a; AREA_ID=2; __Secure-access-token=3.0.xlCkAWzpRCOPD-Cc-SGdqQ.54.l8cMBQAAAABjykXGDMva2KN3ZWKgAICQoA..20230120183236.BVHDXGvykz9F2PfVvdsDuLIaZgoN2qUJw20YJ-ZO5LY; __Secure-refresh-token=3.0.xlCkAWzpRCOPD-Cc-SGdqQ.54.l8cMBQAAAABjykXGDMva2KN3ZWKgAICQoA..20230120183236.N5PqmtMAcDHUBTxhmk2cW0zul1hZkplrmdC1Vf33rpo; __exponea_time2__=-0.7738103866577148; is_adult_confirmed=true; adult_user_birthdate=2001-04-24; __cf_bm=d7Ys0g0XMysqNVuYEV997bhfSIuwQFD9wCeBNpYxIlY-1674236066-0-AfTUITkq5HI+BOftiDgDClgje5bOv3QdEcDFJDelCkkBhlpjHYsf8w3XgF+g+aFXbGzkXCwgFN5g5Ckb1hCnlHawPfxTxKN8dpKYIcEIeR0Q+8xTR4BLMLR4xrvmFLuLqssIMRczCwX+SfXMtOJWyeoJwOXV4eE26NKQ2z3Qlm87TrtBHIxzr7G2UAtBAY5jEQ==; tmr_detect=1%7C1674236358068; _ga_JNVTMNXQ6F=GS1.1.1674232397.2.1.1674236381.34.0.0" },
                { "sec-ch-ua", "\"Not_A Brand\";v=\"99\", \"Google Chrome\";v=\"109\", \"Chromium\";v=\"109\"" },
                { "user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36" },
                { "sec-ch-ua-platform", "\"Windows\"" }
            });

            await page.GoToAsync(url);
            await page.WaitForTimeoutAsync(5000);
            return await page.GetContentAsync();
        }
    }
}
