using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using VilllaParks.Core.IRepository;
using VilllaParks.Core.Repository;
using VilllaParks.Data;
using VilllaParks.Mapper;
using VilllaParks.Model;

var builder = WebApplication.CreateBuilder(args);

//Configuring the serilog thirdparty Logger
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.File("log/villaParkLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

//Adding the serilog configuration to the application
builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});
builder.Services.AddScoped<IVillaParkRepository, VillaParkRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaParkNumberRepository>();
builder.Services.AddAutoMapper(typeof(MapperConfig));
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
