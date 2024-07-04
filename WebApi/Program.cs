using WebApi;
using Application.GlobalExceptionHandling;
using WebAPI.Middlewares;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddWebApplicationService();

var app = builder.Build();
app.UseCors();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware(typeof(GlobalErrorHandlingMiddleware));
app.UseMiddleware<PerformanceMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
