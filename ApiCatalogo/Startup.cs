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
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
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
            // Adicionando CORS - VIA MIDDLEWARE -> Se fizer só a chamada, é necessário definir as politicas em Configure, basta dar um useCors
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

            // Filtros -> Scoped cria uma instancia a cada requisição
            services.AddScoped<ApiLoggingFilter>();

            // Adicionando o Automapper para injetar DTOs.
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Adicionando UnitOfWork como serviço
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
            // Adiciona o manipulador de autenticação e define o esquema de autenticação utilizado: Bearer
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

            // Serviço personalizado
            services.AddTransient<IMeuServico, MeuServico>();

            // Swagger
            services.AddSwaggerGen(c=>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "AapiCatalogo",
                    Description = "Catálogo de Produtos e Categorias",
                    TermsOfService = new Uri("https://tiago.net/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Tiago",
                        Email = "tiago.vaz@hotmail.com",
                        Url = new Uri("https://www.linkedin.com/in/vaz-tiago/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Usar sobre Alguma licensa",
                        Url = new Uri("https://tiago.net/license")
                    }
                });

                // caso queira adicionar os comentários de documentação do C# no swagger
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // Adiionando autorização
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Escreva: bearer + espaço em branco + token"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        new string[]{}
                    }
                });
            });

            // Lida com exception lançada ao carregar os relacionamentos
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

            // Middleware de Authenticação -> Sempre antes de autorização
            app.UseAuthentication();

            app.UseAuthorization();

            // CORS - Definindo políticas -> Utilizado em toda a aplicação
            //app.UseCors(options => options.AllowAnyOrigin());
            app.UseCors();

            // Swagger
            app.UseSwagger();

            //SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cátalogo de Produtos e Categorias");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
