using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// NHibernate: ISessionFactory as singleton
builder.Services.AddSingleton<NHibernate.ISessionFactory>(_ =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    return Fluently.Configure()
        .Database(
            MsSqlConfiguration.MsSql2012
                .ConnectionString(connectionString)
                .ShowSql() // for dev troubleshooting
        )
        .Mappings(m => m.FluentMappings
            // scans this assembly for all ClassMap<> mappings
            .AddFromAssembly(typeof(Program).Assembly)
        )
        .ExposeConfiguration(cfg =>
        {
            // DEV ONLY: create schema if not exists; set first arg (script) to false to avoid console spam
            new SchemaExport(cfg).Create(false, true);
        })
        .BuildSessionFactory();
});

// NHibernate: ISession as scoped (per HTTP request)
builder.Services.AddScoped<NHibernate.ISession>(sp =>
    sp.GetRequiredService<NHibernate.ISessionFactory>().OpenSession()
);

var app = builder.Build();

// Force SessionFactory warm-up
_ = app.Services.GetRequiredService<NHibernate.ISessionFactory>();

// Pipeline
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
