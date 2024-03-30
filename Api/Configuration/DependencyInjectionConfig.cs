using Application.Interfaces.Services;
using Application.Services;
using Infrastructure.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Infrastructure.Persistence;
using Infrastructure.Repositories;

namespace Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ICourseServices, CourseServices>();
            services.AddScoped<IStudentServices, StudentServices>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
        }
    }
}