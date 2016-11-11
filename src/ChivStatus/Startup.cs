using System;
using System.Reflection;
using System.Text;
using ChivStatus.BackgroundProcesses;
using ChivStatus.CustomTypes;
using ChivStatus.Exceptions;
using GameEngineQuery.Exceptions;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ChivStatus
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;
                settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                return settings;
            };

            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddHangfire(x => x.UseStorage(new MemoryStorage()));
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    var ex = error?.Error;

                    if (ex is InvalidPortException)
                    {
                        await context.Response.WriteAsync(new ErrorDto
                        {
                            Message = "Bad request. The provided port is not valid."
                        }.ToString(), Encoding.UTF8);
                    }
                    else if (ex is InvalidIpAddressException)
                    {
                        await context.Response.WriteAsync(new ErrorDto
                        {
                            Message = "Bad request. The provided ip is not valid."
                        }.ToString(), Encoding.UTF8);
                    }
                    else if (ex is InvalidAddressFormatException)
                    {
                        await context.Response.WriteAsync(new ErrorDto
                        {
                            Message = "Bad request. Please provide the address in the correct format (ip:port)."
                        }.ToString(), Encoding.UTF8);
                    }
                    else if (ex is QueryExecutorInitializationException)
                    {
                        await context.Response.WriteAsync(new ErrorDto
                        {
                            Message = "Server error."
                        }.ToString(), Encoding.UTF8);
                    }
                    else if (ex is TargetInvocationException && ex.InnerException is FormatException)
                    {
                        await context.Response.WriteAsync(new ErrorDto
                        {
                            Message = "Bad request. The provided ip is not valid."
                        }.ToString(), Encoding.UTF8);
                    }
                    else if (ex is SourceEngineQueryException)
                    {
                        await context.Response.WriteAsync(new ErrorDto
                        {
                            Message = ex.Message
                        }.ToString(), Encoding.UTF8);
                    }
                    else
                    {
                        await context.Response.WriteAsync(new ErrorDto
                        {
                            Message = "Bad request."
                        }.ToString(), Encoding.UTF8);
                    }
                });
            });

            app.UseMvc();
            app.UseStaticFiles();
            app.UseHangfireServer(additionalProcesses: new[] { new BackgroundServerWatcher() });
            app.UseHangfireDashboard();
        }
    }
}
