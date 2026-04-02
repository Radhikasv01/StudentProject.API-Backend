using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using Serilog;
using Student.Interface;
using Student.Model;
using Student.Service;
using StudentProject.API;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddEntityFrameworkNpgsql().AddDbContext<StudentDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Connection"), b => b.MigrationsAssembly("StudentProject.API")));
builder.Services.AddDbContext<StudentDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("StudentProject.API")));
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
builder.Host.UseSerilog((context, configuration) =>
   configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddTransient<ApiExceptionFilter>();
builder.Services.AddScoped<ILogin, LoginService>();
builder.Services.AddScoped<IRegister, RegisterService>();
builder.Services.AddScoped<IRole, RoleService>();
builder.Services.AddScoped<IStudent, StudentService>();
builder.Services.AddScoped<IStudentBooks,StudentBookService>();
builder.Services.AddScoped<IStudentCoarse, StudentCoarseService>();
builder.Services.AddScoped<ISports, SportsService>();
builder.Services.AddScoped<StudentDbContext>();
builder.Services.AddScoped<IRepository<RoleModel>, RepositoryService<RoleModel>>();
builder.Services.AddScoped<IRepository<StudentModel>, RepositoryService<StudentModel>>();
builder.Services.AddScoped<IRepository<RegisterModel>, RepositoryService<RegisterModel>>();
builder.Services.AddScoped<IRepository<StudentBooksModel>, RepositoryService<StudentBooksModel>>();
//builder.Services.AddScoped<IEmail, EmailService>();




Log.Logger = new LoggerConfiguration().MinimumLevel.Error().WriteTo.File("MyLog/MyLog.txt", rollingInterval: RollingInterval.Minute).CreateLogger();



var constring = builder.Configuration.GetConnectionString("DefaultConnection");
Settings.ConnectionString = constring;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "authentican",
        Version = "v2",
        Description = "Your Api Description"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";

builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();



/// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandler =
            context.Features.Get<
            Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

        var error = exceptionHandler?.Error?.ToString();

        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain";

        await context.Response.WriteAsync(error);
    });
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");

app.UseAuthentication();   // ADD THIS
app.UseAuthorization();

app.MapGet("/", () => "Student API Running Successfully");

app.Run();
