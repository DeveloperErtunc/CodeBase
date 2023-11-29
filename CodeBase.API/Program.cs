var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigurationIServiceCollection(builder.Configuration);


builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy<string, CreateOrderPolicy>("CreateOrderPolicy");
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var someContext = scope.ServiceProvider.GetRequiredService<CodeBaseInterViewContext>();
    someContext.Database.EnsureCreated();
    await someContext.SeedData();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseMiddleware<ApiAuthMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();
app.Run();
