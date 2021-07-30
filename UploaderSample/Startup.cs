using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using UploaderSample.BusinessLayer.Services;
using UploaderSample.DataAccessLayer;
using UploaderSample.StorageProviders;

namespace UploaderSample
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UploaderSample", Version = "v1" });
            });

            services.AddDbContext<DataContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("SqlConnection");
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IImageService, ImageService>();

            //services.AddFileSystemStorageProvider(options =>
            //{
            //    options.StorageFolder = Configuration.GetValue<string>("AppSettings:StorageFolder");
            //});

            services.AddAzureStorageProvider(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("StorageConnection");
                options.ContainerName = Configuration.GetValue<string>("AppSettings:ContainerName");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UploaderSample v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
