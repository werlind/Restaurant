using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RestaurantReservation.Context;
using RestaurantReservation.Contracts;
using RestaurantReservation.Data;
using RestaurantReservation.Repository;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:5001", "http://localhost:5265", "https://localhost:7153", "https://localhost:7153")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services
    .AddSwaggerGen(
        c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT Mesa API", Version = "v1" });
        }
    );

var app = builder.Build();

// Skonfiguruj potok HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mesa API"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();


// Mapuj kontrolery Web API
app.MapControllers();

app.Run();