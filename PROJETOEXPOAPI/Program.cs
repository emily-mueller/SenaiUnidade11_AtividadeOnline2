using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PROJETOEXPOAPI.Contexts;
using PROJETOEXPOAPI.Interfaces;
using PROJETOEXPOAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options => 
{
    options.AddPolicy("CorsPolicy", policy => 
    {
        policy.WithOrigins("https://https://localhost:7291")
        .AllowAnyHeader()
        .AllowAnyMethod();
    
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = "JwtBearer";
    options.DefaultAuthenticateScheme = "JwtBearer";
}).AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("arquivo-chave-autenticacao")),
        ClockSkew = TimeSpan.FromMinutes(60),
        ValidAudience = "projeto.webapi",
        ValidIssuer = "projeto.webapi"
    };
});

builder.Services.AddScoped<SqlContext, SqlContext>();

builder.Services.AddTransient<ProjetoRepository, ProjetoRepository>();

builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
