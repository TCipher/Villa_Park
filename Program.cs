using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VilllaParks.Core.IRepository;
using VilllaParks.Core.Repository;
using VilllaParks.Data;
using VilllaParks.Mapper;
using VilllaParks.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
          ));
builder.Services.AddScoped<IVillaParkRepository, VillaParkRepository>();
builder.Services.AddAutoMapper(typeof(MapperConfig));
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
