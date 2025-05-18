using Microsoft.EntityFrameworkCore;
using System;
using WebHackathon.Models;
using MySql.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DbHackathonContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddScoped<DocumentEmbeddingService>();
builder.Services.AddScoped<DocumentChunkService>();
builder.Services.AddScoped<CohereEmbeddingService>();

// Set GroupDocs license
string licensePath = Path.Combine(builder.Environment.ContentRootPath, "groupdocs.license");
if (System.IO.File.Exists(licensePath))
{
    GroupDocs.Viewer.License license = new GroupDocs.Viewer.License();
    license.SetLicense(licensePath);
}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();