using CodeChallenge.Repository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddSingleton<ICodeChallengeDbContext>(new CodeChallengeDbContext(builder.Configuration.GetConnectionString("DefaultConnection").ToString()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(builder =>
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(builder =>
    {
        builder
        .WithOrigins("https://wdaher-frontend-demo.azurewebsites.net")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
