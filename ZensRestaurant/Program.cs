using Repository.DBContext;
using Repository.Infrastructures;
using Service.DTOs.JWTs;
using Service.Services.Implementations;
using Service.Services.Interfaces;
using ZensRestaurant;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//DI UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// DI Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.Configure<JWTAuth>(builder.Configuration.GetSection("JWTAuth"));
builder.Services.AddSingleton<ZRDbContext>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddConfigSwagger();
//JWT
builder.AddJwtValidation();


var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
