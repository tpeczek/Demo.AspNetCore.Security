using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Lib.AspNetCore.Security;
using Lib.AspNetCore.Security.Http.Headers;
using Demo.AspNetCore.Security.Services;

namespace Demo.AspNetCore.Security
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ISecurityHeadersReportingService, LoggerSecurityHeadersReportingService>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseStaticFiles();

            app.UseSecurityHeaders(builder =>
            {
                builder.WithCsp(
                    fontSources: "fonts.gstatic.com",
                    imageSources: ContentSecurityPolicyHeaderValue.SelfSource,
                    scriptSources: ContentSecurityPolicyHeaderValue.SelfSource + " cdnjs.cloudflare.com",
                    scriptInlineExecution: ContentSecurityPolicyInlineExecution.Hash,
                    styleSources: ContentSecurityPolicyHeaderValue.SelfSource + " fonts.googleapis.com",
                    styleInlineExecution: ContentSecurityPolicyInlineExecution.Hash
                );
                
                builder.WithReportOnlyExpectCt("https://localhost:44300/report-ct");
            })
            .MapExpectCtReporting("/report-ct");

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Demo}/{action=Index}");
            });
        }
    }
}
