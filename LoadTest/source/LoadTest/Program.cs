// <copyright file="Program.cs" company="Omnicell Inc.">
// Copyright (c) 2020 Omnicell Inc. All rights reserved.
//
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is
// prohibited without the prior written consent of the copyright owner.
// </copyright>

using System;
using System.IO;

using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace LoadTest
{
    public class Program
    {
        private static readonly IConfiguration _configuration;

        static Program()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();
        }

        public static void Main(string[] args)
        {
            try
            {
                Log.Information("Starting load test...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Load test failed to start {0}", exception.Message);
                Log.Fatal(exception, "{0}", exception.StackTrace);

                Console.WriteLine("Load test failed to start {0}", exception.Message);
                Console.WriteLine("{0}", exception.StackTrace);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseSerilog();
                });
        }
    }
}
