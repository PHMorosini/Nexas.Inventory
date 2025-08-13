
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nexas.Inventory.Application.Auth.Interface;
using Nexas.Inventory.Application.Auth.Service;
using Nexas.Inventory.Application.Base.Interface;
using Nexas.Inventory.Application.Base.Service;
using Nexas.Inventory.Application.Mappings;
using Nexas.Inventory.Application.Product.Interface;
using Nexas.Inventory.Application.Product.Services;
using Nexas.Inventory.Application.StockItem.Interface;
using Nexas.Inventory.Application.StockItem.Service;
using Nexas.Inventory.Application.Store.Interface;
using Nexas.Inventory.Application.Store.Service;
using Nexas.Inventory.Application.User.Interface;
using Nexas.Inventory.Domain.Product.Interface;
using Nexas.Inventory.Domain.User.Entity;
using Nexas.Inventory.Infrastructure.Base.Interfaces;
using Nexas.Inventory.Infrastructure.Context;
using Nexas.Inventory.Infrastructure.JwtSettings;
using Nexas.Inventory.Infrastructure.Product.Repository;
using Nexas.Inventory.Infrastructure.StockItem.Interface;
using Nexas.Inventory.Infrastructure.StockItem.Repository;
using Nexas.Inventory.Infrastructure.Store.Interface;
using Nexas.Inventory.Infrastructure.Store.Repository;
using Nexas.Inventory.Infrastructure.User.Interface;
using Nexas.Inventory.Infrastructure.User.Repository;
using Nexas.Inventory.Infrastructure.User.Service;
using System.Text;

namespace Nexas.Inventory
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nexas_Inventory", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insert the JWT token with the 'Bearer' prefix",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
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
                            }
                        },
                        new string[] { }
                    }
                });
            });

            //register for REPOSITORY
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IStoreRepository, StoreRepository>();
            builder.Services.AddScoped<IStockItemRepository, StockItemRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();


            //REGISTER FOR SERVICES
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IStockItemService, StockItemService>();
            builder.Services.AddScoped<IStoreService, StoreService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITokenService, TokenService>();


            //REGISTER FOR AUTOMAPPER
            builder.Services.AddAutoMapper(cfg =>
            {
            }, typeof(MappingProfile).Assembly);

            //REGISTER FOR JWT AUTH
            builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));
            var jwtSettings = builder.Configuration.GetSection("Jwt").Get<Jwt>();
            var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
            .AddJwtBearer("JwtBearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });


            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
