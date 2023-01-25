using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OzonParser.Services.Base;
using OzonParser.Services.Parsing;
using OzonParser.Services.Web;

namespace OzonParser
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorPages();
			builder.Services.AddServerSideBlazor();

			builder.Services.AddScoped<IProductsParserService, OzonProductsParserService>();
			builder.Services.AddScoped<IWebHtmlParser, WebEmulatorHtmlParser>();
			builder.Services.AddScoped<IProductPageParser, OzonProductPageParser>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.MapBlazorHub();
			app.MapFallbackToPage("/_Host");

			app.Run();
		}
	}
}