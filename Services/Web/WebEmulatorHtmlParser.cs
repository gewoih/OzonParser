using Microsoft.AspNetCore.Hosting.Server;
using OzonParser.Services.Base;
using PuppeteerSharp;
using System.Net;

namespace OzonParser.Services.Web
{
    public sealed class WebEmulatorHtmlParser : IWebHtmlParser
    {
        public async Task<string> GetHtmlAsync(string url, string? neededXpath = null)
        {
            var options = new LaunchOptions()
            {
                Headless = true,
                Devtools = false,
                ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
            };

            var browser = Puppeteer.LaunchAsync(options).Result;

            var page = await browser.NewPageAsync();
            await page.SetExtraHttpHeadersAsync(new Dictionary<string, string>
            {
                { "accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,th;q=0.6" },
                { "sec-ch-ua", "\"Not_A Brand\";v=\"99\", \"Google Chrome\";v=\"109\", \"Chromium\";v=\"109\"" },
                { "user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36" },
                { "sec-ch-ua-platform", "\"Windows\"" }
            });

            try
            {
                await page.GoToAsync(url, new NavigationOptions { WaitUntil = new WaitUntilNavigation[] { WaitUntilNavigation.Load } });
                await page.WaitForNetworkIdleAsync();

                return await page.GetContentAsync();
            }
            catch
            {
                return await page.GetContentAsync();
            }
            finally
            {
                await browser.CloseAsync();
                await browser.DisposeAsync();
            }
        }
    }
}
