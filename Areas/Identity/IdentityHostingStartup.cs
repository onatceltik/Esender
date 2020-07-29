using System;
using Esender.Areas.Identity.Data;
using Esender.Data;
using Esender.ModelsAppDbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Esender.Areas.Identity.IdentityHostingStartup))]
namespace Esender.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<EsenderDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("EsenderDbContextConnection")));

                services.AddDefaultIdentity<EsenderUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<EsenderDbContext>();
            });
        }
    }
}