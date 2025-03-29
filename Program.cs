using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Smart_Electric_Metering_System_BackEnd;
using Smart_Electric_Metering_System_BackEnd.Authentication;
using Smart_Electric_Metering_System_BackEnd.Context;
using Smart_Electric_Metering_System_BackEnd.Implementations.Repositories;
using Smart_Electric_Metering_System_BackEnd.Implementations.Services;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(x => x.AddPolicy("Policies", c =>
{
    c.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
}));
builder.Services.AddScoped<IMeterRepo, MeterRepo>();
builder.Services.AddScoped<IMeterUnitsRepo, MeterUnitsRepo>();
builder.Services.AddScoped<IMeterUnitAllocationRepo, MeterUnitAllocationRepo>();
builder.Services.AddScoped<IAdminRepo, AdminRepo>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IPricesRepo, PricesRepo>();
builder.Services.AddScoped<IMeterPromptRepo, MeterPromptRepo>();
builder.Services.AddScoped<IPricesRepo, PricesRepo>();
builder.Services.AddScoped<IMeterService, MeterService>();
builder.Services.AddScoped<IMeterUnitAllocationService, MeterUnitAllocationService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IMeterPromptService, MeterPromptService>();
builder.Services.AddScoped<IPricesService, PricesService>();
builder.Services.AddHostedService<BackgroundServices>();
builder.Services.AddHttpContextAccessor();
var connectionString = builder.Configuration.GetConnectionString("SmartElectricMeteringContext");
builder.Services.AddDbContext<SmartElectricMeteringContext>(x => x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))); 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {Title = $"{builder.Configuration["ApplicationDetails:AppName"]}", Version = "v1"});
});

var key = "Auth Key";
builder.Services.AddSingleton<JWTAuthentication>(new JWTAuthentication(key));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters(){
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Policies");

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
