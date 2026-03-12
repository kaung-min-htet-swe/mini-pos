using Microsoft.EntityFrameworkCore;
using mini_pos.Features.Orders.Services;
using mini_pos.Features.Products.Services;
using mini_pos.Features.Users.Services;
using ms_sql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PosContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection")),
    ServiceLifetime.Transient,
    ServiceLifetime.Transient
);

builder.Services.AddControllers();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IMerchantService, MerchantService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.MapGet("/",
    context => context.Response.WriteAsJsonAsync(new { IsSuccess = true, Message = "Welcome to Mini POS!" })
).WithName("Home").WithOpenApi();

app.Run();