using Assignment01.Context;
using Assignment01.Entities;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var oDataBuilder = new ODataConventionModelBuilder();
oDataBuilder.EntitySet<Show>("Show");
oDataBuilder.EntitySet<Film>("Film");
var edmModel = oDataBuilder.GetEdmModel();

builder.Services.AddControllers()
    .AddOData(opt => opt
        .AddRouteComponents("odata", edmModel)
        .Filter()
        .Expand()
        .Select()
        .OrderBy()
        .Count()
        .SetMaxTop(100));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();