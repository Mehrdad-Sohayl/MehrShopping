using Microsoft.EntityFrameworkCore;
using PersonalInfo.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<PersonalInfoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPersonalRepository, PersonalRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("api/PersonalInfo/{nationalCode}", async (string nationalCode, IPersonalRepository repository) =>
{
    var result = await repository.FindByNationalCodeAsync(nationalCode);

    if (result == null)
        return Results.NotFound();

    return Results.Ok(result);
})
.WithName("GetPersonalInfo");

app.MapSwagger();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

