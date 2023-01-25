namespace OzonParser.Services.Base
{
    public interface IWebHtmlParser
    {
        public Task<string> GetHtmlAsync(string url);
    }
}
