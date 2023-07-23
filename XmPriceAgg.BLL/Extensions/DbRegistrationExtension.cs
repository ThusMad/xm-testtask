using Microsoft.Extensions.DependencyInjection;
using XmPriceAgg.DAL.Repositories;
using XmPriceAgg.DAL.Repositories.Interfaces;

namespace XmPriceAgg.BLL.Extensions
{
    public static class DbRegistrationExtension
    {
        public static void RegisterSqLiteDb(this IServiceCollection services)
        {
            SQLitePCL.Batteries.Init();
            services.AddTransient<IUnitOfWorkFactory, SqLiteUnitOfWorkFactory>();
        }
    }
}
