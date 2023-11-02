using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Trains_FSD.Areas.Data;
using Trains_FSD.Data;
using Trains_FSD.Models.Entities;
using Trains_FSD.Repositories;
using Trains_FSD.Repositories.Interfaces;
using Trains_FSD.Services;
using Trains_FSD.Services.Interfaces;
using Trains_FSD.Util.Mail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Train API",
        Version = "version 1",
        Description = "An API to do all sorts of actions with Trains",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "CDW",
            Email = "christophe.dewaele@vives.be",
            Url = new Uri("https://vives.be"),
        },
        License = new OpenApiLicense
        {
            Name = "Train API LICX",
            Url = new Uri("https://example.com/license"),
        }
    });
});

// Add Automapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "be.Trains.Session";
    options.IdleTimeout = TimeSpan.FromHours(1);
});

builder.Services.AddControllersWithViews();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddSingleton<IEmailSend, EmailSend>();
//Als in een Constructor een IEmailSender-parameter wordt gevonden, zal een emailSender - object worden aangemaakt.

builder.Services.AddSingleton(typeof(IConverter),
    new SynchronizedConverter(new PdfTools()));

// Dependency Injection
builder.Services.AddTransient<IService<City>, CityService>();
builder.Services.AddTransient<IDAO<City>, CityDAO>();

builder.Services.AddTransient<IService<Ticket>, TicketService>();
builder.Services.AddTransient<IDAO<Ticket>, TicketDAO>();

builder.Services.AddTransient<IService<Train>, TrainService>();
builder.Services.AddTransient<IDAO<Train>, TrainDAO>();

builder.Services.AddTransient<IService<Line>, LineService>();
builder.Services.AddTransient<IDAO<Line>, LineDAO>();

builder.Services.AddTransient<IServiceTraject<Traject>, TrajectService>();
builder.Services.AddTransient<IDAOTraject<Traject>, TrajectDAO>();

builder.Services.AddTransient<IService<TrajectLine>, TrajectLineService>();
builder.Services.AddTransient<IDAO<TrajectLine>, TrajectLineDAO>();

builder.Services.AddTransient<IServiceOrder<Order>, OrderService>();
builder.Services.AddTransient<IDAOOrder<Order>, OrderDAO>();

builder.Services.AddTransient<IServiceTicketDetails<TicketDetail>, TicketDetailService>();
builder.Services.AddTransient<IDAOTicketDetails<TicketDetail>, TicketDetailDAO>();

builder.Services.AddTransient<IService<Vacation>, VacationService>();
builder.Services.AddTransient<IDAO<Vacation>, VacationDAO>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");


var supportedCultures = new[] { "en", "nl", "fr" };
builder.Services.Configure<RequestLocalizationOptions>(options => {
    options.SetDefaultCulture(supportedCultures[0])
      .AddSupportedCultures(supportedCultures)
      .AddSupportedUICultures(supportedCultures);
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var swaggerOptions = new Trains_FSD.Options.SwaggerOptions();
builder.Configuration.GetSection(nameof(Trains_FSD.Options.SwaggerOptions)).Bind(swaggerOptions);
// Enable middleware to serve generated Swagger as a JSON endpoint.
//RouteTemplate legt het path vast waar de JSON‐file wordt aangemaakt
app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
//// By default, your Swagger UI loads up under / swagger /.If you want to change this, it's thankfully very straight‐forward.
//Simply set the RoutePrefix option in your call to app.UseSwaggerUI in Program.cs:
app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
});
app.UseSwagger();


app.UseHttpsRedirection();
app.UseStaticFiles();

// Culture from the HttpRequest
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseRouting();

// add session
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
