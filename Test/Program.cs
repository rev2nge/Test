using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Test.Application.Configuration;
using Test.Application.Repository.Interface;
using Test.Config;
using Test.Infrastructure.Context;
using Test.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Test.Infrastructure")));

services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
services.Configure<AnnouncementSettings>(builder.Configuration.GetSection("AnnouncementSettings"));
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.OperationFilter<FileUploadOperationFilter>(); 
});
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