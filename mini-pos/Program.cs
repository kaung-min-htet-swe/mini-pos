using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ms_sql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PosContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection")),
    ServiceLifetime.Transient,
    ServiceLifetime.Transient
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/",
    context => context.Response.WriteAsJsonAsync(new { IsSuccess = true, Message = "Welcome to Mini POS!" })
).WithName("Home").WithOpenApi();


app.Run();