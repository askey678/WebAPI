using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BAL.Services;

namespace WebApi.BAL.Helpers
{
    public static class DependencyHelper
    {

        public static IServiceCollection AddBALDependency(this IServiceCollection services)
        {
            services.AddSingleton<IService, ServiceImpl>();

            return services;
        }
    }

}
