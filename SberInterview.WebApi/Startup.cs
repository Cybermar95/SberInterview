using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SberInterview.WebApi.Users;

namespace SberInterview.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Необходимо создать интерфейс ISberDbContext и внедрять зависимость на этот интерфейс (Ioc)
            services.AddDbContext<SberDbContext>();

            // Использование AddSingleton потенциально приведёт к ошибке DbContext при одновременно обращении к API.
            // Опять внедрение зависимости не через интерфейс. (Ioc).
            services.AddScoped<UsersRepository>();

            services.AddControllers();

            // Отсутствуют внедрения зависимостей на ILoggerFactory, IMediator, UsersRepository, IMapper
            // хотя данные зависимости используются в классах.

            // Что бы не засорять ConfigureServices, Внедрение всех зависимостей можно вынести в отдельный метод/методы расширения IServiceCollection.
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}