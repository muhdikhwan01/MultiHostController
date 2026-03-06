// Data/NHibernateHelper.cs
using System;
using System.IO;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Majlis2Go.Data
{
    public static class NHibernateHelper
    {
        // Call this from Program.cs: builder.Services.AddNHibernate(Configuration);
        public static void AddNHibernate(this IServiceCollection services, IConfiguration configuration)
        {
            // 1) Read connection string from appsettings.json
            var conn = configuration.GetConnectionString("DefaultConnection");

            // 2) Build NHibernate Configuration (don't build session factory yet)
            var nhConfig = Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2012 // use MS SQL dialect that NHibernate understands
                        .ConnectionString(conn)
                        .ShowSql() // prints generated SQL to console (useful for debugging)
                )
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildConfiguration(); // returns NHibernate.Cfg.Configuration

            // 3) Use SchemaExport to create the SQL script file (DO NOT execute against DB)
            //    - SetOutputFile: the file path where the SQL will be written
            //    - Create(true, false): write SQL to stdout/file, but do NOT execute it against DB
            // Note: The file will be created in the path we provide below.
            var outputFile = Path.Combine(Directory.GetCurrentDirectory(), "schema.sql");
            var exporter = new SchemaExport(nhConfig);

            // set the output file (full path preferred)
            exporter.SetOutputFile(outputFile);

            // Create the DDL and write it to outputFile and console, but DO NOT run it on the DB:
            // Create(script: true, export: false) -> script=true writes to stdout/file; export=false means do not execute.
            exporter.Create(useStdOut: true, execute: false);

            // (Optional) show where the file was written when running from CLI
            Console.WriteLine($"NHibernate schema script written to: {outputFile}");

            // 4) Build the session factory and register for DI (normal NHibernate setup)
            var sessionFactory = nhConfig.BuildSessionFactory();
            services.AddSingleton<ISessionFactory>(sessionFactory);
            services.AddScoped(factory => sessionFactory.OpenSession());
        }
    }
}
