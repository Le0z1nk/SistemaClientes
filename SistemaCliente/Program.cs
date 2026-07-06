var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "API de Integração .NET & COBOL",
        Version = "v1",
        Description = "Interface para um sistema de clientes"
    });
});

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API .NET/COBOL v1");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();
app.Run();
