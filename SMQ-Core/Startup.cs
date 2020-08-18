using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SMQCore.Business;
using SMQCore.Business.Interfaces;
using SMQCore.DataAccess;
using SMQCore.DataAccess.Contexts;
using SMQCore.DataAccess.Interfaces;
using SMQCore.Helpers;

namespace SMQCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SMQ_KEY"));
            var issuer = Configuration.GetValue<string>("SMQ_ISSUER");
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SMQ-Core", Version = "v1" });
            });

            services.AddMvc(options =>
                        options.InputFormatters.Add(new TextPlainFormatter()))
                    .AddJsonOptions(options =>
                        options.JsonSerializerOptions.IgnoreNullValues = true);

            services.AddTransient<IQueueBusiness, QueueBusiness>();
            services.AddSingleton<IUsersBusiness, UsersBusiness>();
            services.AddSingleton<IAppsBusiness, AppsBusiness>();
            services.AddSingleton<IPermissionCheck, PermissionCheck>();

            services.AddSingleton<IMessagesRepository, MessagesRepository>();
            services.AddSingleton<IUsersRepository, UsersRepository>();
            services.AddSingleton<IAppsRepository, AppsRepository>();

            services.AddSingleton<ISMQContext, SMQContext>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SMQ-Core V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (SMQContext context = new SMQContext(Configuration))
            {
                context.Database.Migrate();
            }
        }
    }
}