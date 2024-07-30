using ManageApartment.Repositories;
using ManageApartment.Repositories.Implementation;
using ManageApartment.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
                                                        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                                                        b => b.MigrationsAssembly("ManageApartment.UI")));

    //repositories DI: achieve IOC(Inversion Of Control)

    builder.Services.AddScoped<iUserRepo, UserRepo>();
    builder.Services.AddScoped<iApartmentRepository, ApartmentRepository>();
    builder.Services.AddScoped<iFlatRepository, FlatRepository>();
    builder.Services.AddScoped<iResidentRepository, ResidentRepository>();

    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    //session management
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(10);
        options.Cookie.HttpOnly = true;
    });

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseSession();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}