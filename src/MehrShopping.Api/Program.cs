using MehrShopping.Application.Interfaces;
using MehrShopping.Application.Services.Customer.Commands.RegisterCustomer;
using MehrShopping.Application.Services.Invoice.Commands;
using MehrShopping.Application.Services.Products.Commands;
using MehrShopping.Domain.Interfaces.Repositories;
using MehrShopping.Infrastructure.Clients;
using MehrShopping.Infrastructure.Data;
using MehrShopping.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace MehrShopping.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<MehrShoppingDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddHttpClient<IPersonalInfoClient, PersonalInfoClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7120/");
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<RegisterCustomerHandler>();
            builder.Services.AddScoped<UpdateCustomerHandler>();
            builder.Services.AddScoped<RegisterProductHandler>();
            builder.Services.AddScoped<DeleteProductHandler>();
            builder.Services.AddScoped<CreateInvoiceHandler>();

            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.Run();
        }
    }
}
