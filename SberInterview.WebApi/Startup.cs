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
            // ���������� ������� ��������� ISberDbContext � �������� ����������� �� ���� ��������� (Ioc)
            services.AddDbContext<SberDbContext>();

            // ������������� AddSingleton ������������ ������� � ������ DbContext ��� ������������ ��������� � API.
            // ����� ��������� ����������� �� ����� ���������. (Ioc).
            services.AddScoped<UsersRepository>();

            services.AddControllers();

            // ����������� ��������� ������������ �� ILoggerFactory, IMediator, UsersRepository, IMapper
            // ���� ������ ����������� ������������ � �������.

            // ��� �� �� �������� ConfigureServices, ��������� ���� ������������ ����� ������� � ��������� �����/������ ���������� IServiceCollection.
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