using ApiCatalogo.Context;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Extensions;
using ApiCatalogo.Filters;
using ApiCatalogo.Logging;
using ApiCatalogo.Repository;
using ApiCatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiCatalogo
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
            // Adicionando CORS - VIA MIDDLEWARE -> Se fizer s� a chamada, � necess�rio definir as politicas em Configure, basta dar um useCors
            // Passando as politicas direto na chamada, depois basta usar direto no controlador
            services.AddCors(option =>
            {
                option.AddPolicy("PertmitirApiRequest",
                    builder => builder
                        .AllowAnyOrigin()
                        .WithMethods("GET")
                        .AllowAnyHeader()
                    );
            });

            // Filtros -> Scoped cria uma instancia a cada requisi��o
            services.AddScoped<ApiLoggingFilter>();

            // Adicionando o Automapper para injetar DTOs.
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Adicionando UnitOfWork como servi�o
            services.AddScoped<IUnityOfWork, UnityOfWork>();

            // Banco de Dados
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"))
            );

            // Adicionando Identity (Login)
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // JWT
            // Adiciona o manipulador de autentica��o e define o esquema de autentica��o utilizado: Bearer
            // Valida o emissor, a audiencia e a chave
            // Utilizando a chave secreta e assinatura
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters 
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = Configuration["TokenConfiguration:Audience"],
                    ValidIssuer = Configuration["TokenConfiguration:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                });

            // Adicioando Controle de versionamento
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            // Servi�o personalizado
            services.AddTransient<IMeuServico, MeuServico>();

            // Lida com exception lan�ada ao carregar os relacionamentos
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        // ILoggerFactory para o logger funcionar
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Adicionando logger:
            loggerFactory
                .AddProvider(new CustomLoggerProvider(
                    new CustomLoggerProviderConfiguration { LogLevel = LogLevel.Information })
                );

            // Adiciona middleware de tratamento de erro
            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            // Middleware de Authentica��o -> Sempre antes de autoriza��o
            app.UseAuthentication();

            app.UseAuthorization();

            // CORS - Definindo pol�ticas -> Utilizado em toda a aplica��o
            //app.UseCors(options => options.AllowAnyOrigin());
            app.UseCors();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
