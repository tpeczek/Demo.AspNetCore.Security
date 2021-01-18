using Microsoft.AspNetCore.Mvc;
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

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.UseSecurityHeaders(builder =>
            {
                builder.WithCsp(
                    fontSources: "fonts.gstatic.com",
                    imageSources: ContentSecurityPolicyHeaderValue.SelfSource,
                    scriptSources: (new ContentSecurityPolicySourceListBuilder())
                        .WithSelfKeyword()
                        .WithUrls("cdnjs.cloudflare.com")
                        .Build(),
                    scriptInlineExecution: ContentSecurityPolicyInlineExecution.Hash,
                    styleSources: (new ContentSecurityPolicySourceListBuilder())
                        .WithSelfKeyword()
                        .WithUrls("fonts.googleapis.com")
                        .Build(),
                    styleInlineExecution: ContentSecurityPolicyInlineExecution.Hash,
                    reportUri: "/report-csp"
                )
                .WithReportOnlyExpectCt("https://localhost:44300/report-ct")
                .WithDenyXFrameOptions()
                .WithBlockXssFiltering()
                .WithXContentTypeOptions()
                .WithXDownloadOptions()
                .WithReferrerPolicy(ReferrerPolicyDirectives.NoReferrer)
                .WithNoneXPermittedCrossDomainPolicies()
                .WithPermissionsPolicy
                (
                    PolicyControlledFeature.CreateAllowedForAllowList("camera", "https://other.com"),
                    PolicyControlledFeature.CreateAllowedForAllowList("microphone", "https://other.com")
                );
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapContentSecurityPolicyReporting("/report-csp");
                endpoints.MapExpectCtReporting("/report-ct");
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Demo}/{action=Index}");
            });
        }
    }
}
