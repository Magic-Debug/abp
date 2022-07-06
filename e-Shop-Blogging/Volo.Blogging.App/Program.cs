﻿using System;
using System.Net;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Volo.Blogging.App;

public class Program
{
    public static int Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.File("Logs/logs.txt")
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            Log.Information("Starting web host.");
            CreateHostBuilder(args).Build().Run();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    internal static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder
                .UseKestrel()
                .ConfigureKestrel((context, options) =>
                {
                    options.Listen(IPAddress.Any, 666, (listenOptions) =>
                    {
                        listenOptions.UseConnectionLogging("Socket Connection Log");
                        listenOptions.Use(connection =>
                        {
                            return connection;
                        });
                        listenOptions.UseConnectionHandler<LogConnectionHandler>();
                    });
                });
            })
            .UseAutofac()
            .UseSerilog();
}
