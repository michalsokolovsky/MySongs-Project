using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySongs.Api.Services;
using MySongs.Mock;
using MySongs.Repository.Interfaces;
using MySongs.Repository.Repositories;
using MySongs.Services.Interfaces;
using MySongs.Services.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100MB
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // 100MB
});
builder.Services.AddDbContext<MySongsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();
builder.Services.AddScoped<IAIService>(sp =>
    new AIService(builder.Configuration["OpenAI:ApiKey"]!));
builder.Services.AddAutoMapper(typeof(MySongs.Services.MappingProfile));


// Repositories
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ISongTagRepository, SongTagRepository>();
builder.Services.AddScoped<IRecommendationRepository, RecommendationRepository>();
builder.Services.AddScoped<IListeningHistoryRepository, ListeningHistoryRepository>();

// Services
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ISongTagService, SongTagService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IListeningHistoryService, ListeningHistoryService>();
builder.Services.AddScoped<RecommendationEngineService>(sp =>
    new RecommendationEngineService(
        sp.GetRequiredService<IUserService>(),
        sp.GetRequiredService<ISongService>(),
        sp.GetRequiredService<IRecommendationService>(),
        sp.GetRequiredService<IListeningHistoryService>(),
        sp.GetRequiredService<IConfiguration>()
    ));
builder.Services.AddScoped<IContext, MySongsDbContext>();

// JWT
var jwtKey = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowReact");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();