using AppComponents.Repository.Abstraction;
using AppComponents.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppComponents.Repository.EFCore
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

        public static IServiceCollection AddTimeStampedRepository<T, TContext>(this IServiceCollection services)
            where T : TimeStampedBaseEntity
            where TContext : DbContext
        {
            services.AddScoped(typeof(IRepository<T, TContext>), typeof(TimeStampedRepository<T, TContext>));
            return services;
        }
    }
}
