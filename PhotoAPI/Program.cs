using Microsoft.EntityFrameworkCore;
using PhotoAPI.Models;
using PhotoAPI.Services;
using PhotoAPI.Repository;
using PhotoAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPhotoRespository, PhotoRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IPhotoUploadService, S3UploadService>();

//Connection String
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
if (connectionString == null)
    connectionString = builder.Configuration.GetConnectionString("photos");

//Database Connection
builder.Services.AddDbContext<PhotoContext>(options => options.UseSqlServer(connectionString));
//                                                options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using( var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PhotoContext>();
    db.Database.Migrate();
}

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

