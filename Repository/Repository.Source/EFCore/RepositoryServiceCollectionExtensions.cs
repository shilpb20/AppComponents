using AppComponents.CoreLib.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppComponents.CoreLib.Repository.EFCore
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository<T, TContext>(this IServiceCollection services)
            where T : class
            where TContext : DbContext
        {
            services.AddScoped(typeof(IRepository<T, TContext>), typeof(Repository<T, TContext>));
            return services;
        }
    }
}
