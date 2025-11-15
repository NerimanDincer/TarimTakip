using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TarimTakip.API.Data;
using TarimTakip.API.Services;
using TarimTakip.API.Hubs;

var builder = WebApplication.CreateBuilder(args);


// Veritabaný baðlantýsýný appsettings'den al
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext'i servislere ekle (Dependency Injection)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


// Servislerimizi Dependency Injection'a ekliyoruz
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFarmFieldService, FarmFieldService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IIrrigationService, IrrigationService>();
builder.Services.AddScoped<IFertilizationService, FertilizationService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ICalendarNoteService, CalendarNoteService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
// Swagger'a JWT desteði ekliyoruz
builder.Services.AddSwaggerGen(options =>
{
    // 1. Güvenlik Tanýmýný (Security Definition) ekle
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header'ý. \r\n\r\n 'Bearer' [boþluk] ve ardýndan token'ý girin.\r\n\r\nÖrnek: \"Bearer 12345abcdef\""
    });

    // 2. Bu tanýmý bir "Gereksinim" (Requirement) olarak ekle
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");
app.Run();
