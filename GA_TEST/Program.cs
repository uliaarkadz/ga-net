using GA_TEST;
using GA_TEST.DbContexts;
using GA_TEST.Services;
using GA_TEST.Extentions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
builder.Host.UseSerilog();


// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson();
builder.Services.AddProblemDetails();

//builder.Services.AddProblemDetails(options =>
//{
//    options.CustomizeProblemDetails = ctx =>
//    {
//        ctx.ProblemDetails.Extensions.Add("additionalInfo", "Additional Info Example");
//        ctx.ProblemDetails.Extensions.Add("server", Environment.MachineName);
//    };
//});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
#if DEBUG
builder.Services.AddTransient<ILocalMailService,LocalMailService>();
#else
builder.Services.AddTransient<ILocalMailService, CloudMailService>();
#endif

builder.Services.AddSingleton<CitiesDataStore>();

builder.Services.AddDbContext<CityInfoContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
//builder.Services.AddDbContext<CityInfoContext>(dbContextoptions => dbContextoptions.UseSqlite("Data Sourse=CityInfo.db"));
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new()
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Authentication:Issuer"],
//            ValidAudience = builder.Configuration["Authentication:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(
//               Convert.FromBase64String(builder.Configuration["Authentification:SecretForKey"]))
//        };
//    }
//    );

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("MustBeFromAntwerp", policy =>
//    {
//        policy.RequireAuthenticatedUser();
//        policy.RequireClaim("city", "Antwerp");
//    });
//});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
    | ForwardedHeaders.XForwardedProto;
});


var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}


app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.ApplyMigrations();
}

//app.ApplyMigrations();
app.UseHttpsRedirection();
app.UseRouting();
//app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});



app.Run();

