using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sarthi.Core.Interfaces;
using Sarthi.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Sarthi.Infrastructure.ServiceExtension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
        { 
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped((s) => new SqlConnection(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IDbTransaction>(s =>
            {
                SqlConnection conn = s.GetRequiredService<SqlConnection>();
                conn.Open();
                return conn.BeginTransaction();
            });
            return services;
        }
    }
}
