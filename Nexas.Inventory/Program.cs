
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nexas.Inventory.Domain.Product.Interface;
using Nexas.Inventory.Domain.User.Entity;
using Nexas.Inventory.Infrastructure.Base.Interfaces;
using Nexas.Inventory.Infrastructure.Context;
using Nexas.Inventory.Infrastructure.Product.Repository;
using Nexas.Inventory.Infrastructure.Store.Interface;
using Nexas.Inventory.Infrastructure.Store.Repository;

namespace Nexas.Inventory
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //register for services
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IStoreRepository, StoreRepository>();

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
