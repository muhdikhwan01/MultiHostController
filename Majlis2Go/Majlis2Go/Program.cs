using Majlis2Go.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// add NHibernate and generate schema.sql (development only)
builder.Services.AddNHibernate(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
