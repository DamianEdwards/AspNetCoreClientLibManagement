using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.AspNetCore.Builder;

public static class ClientAssetsExtensions
{
    private const string ClientAssetsDirectoryKey = "ClientAssetsDirectory";

    /// <summary>
    /// Configures the <see cref="IWebHostEnvironment.WebRootFileProvider"/> to use client assets from the intermediate output
    /// directory built by npm.
    /// </summary>
    /// <param name="builder">The <see cref="IWebHostBuilder"/>.</param>
    public static void UseClientAssets(this IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            // Enable serving static files from the Microsoft.AspNetCore.ClientAssets intermedite output dir
            var webHostEnvironment = context.HostingEnvironment;
            var infoVerAttr = typeof(Program).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
                .FirstOrDefault(a => string.Equals(a.Key, ClientAssetsDirectoryKey, StringComparison.OrdinalIgnoreCase));
            if (infoVerAttr is not null && Directory.Exists(infoVerAttr.Value))
            {
                var clientAssetsOutputDir = new PhysicalFileProvider(Path.Combine(webHostEnvironment.ContentRootPath, infoVerAttr.Value));
                webHostEnvironment.WebRootFileProvider = new CompositeFileProvider(new[] { clientAssetsOutputDir, webHostEnvironment.WebRootFileProvider });
            }
        });
    }
}
