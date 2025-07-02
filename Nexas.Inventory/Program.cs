
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nexas.Inventory.Application.Base.Interface;
using Nexas.Inventory.Application.Base.Service;
using Nexas.Inventory.Domain.Product.Interface;
using Nexas.Inventory.Domain.User.Entity;
using Nexas.Inventory.Infrastructure.Base.Interfaces;
using Nexas.Inventory.Infrastructure.Context;
using Nexas.Inventory.Infrastructure.Product.Repository;
using Nexas.Inventory.Infrastructure.StockItem.Interface;
using Nexas.Inventory.Infrastructure.StockItem.Repository;
using Nexas.Inventory.Infrastructure.Store.Interface;
using Nexas.Inventory.Infrastructure.Store.Repository;
using Nexas.Inventory.Infrastructure.User.Interface;
using Nexas.Inventory.Infrastructure.User.Repository;

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

            //register for REPOSITORY
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IStoreRepository, StoreRepository>();
            builder.Services.AddScoped<IStockItemRepository, StockItemRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            //REGISTER FOR SERVICES
            builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

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
