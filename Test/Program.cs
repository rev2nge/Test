using Microsoft.EntityFrameworkCore;
using Test.Application.Repository.Interface;
using Test.Infrastrucuture.Context;
using Test.Repository;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Test")));

services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();