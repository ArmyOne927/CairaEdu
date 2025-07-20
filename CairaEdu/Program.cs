using CairaEdu.Core.Configuration;
using CairaEdu.Core.Interfaces;
using CairaEdu.Core.Services;
using CairaEdu.Data.Context;
using CairaEdu.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//Conexion a bd
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Agregar Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Configura el bloqueo
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30); // tiempo de bloqueo
    options.Lockout.MaxFailedAccessAttempts = 3; // máximo de intentos fallidos
    options.Lockout.AllowedForNewUsers = true;   // aplicar también a usuarios recién creados
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


//Rutas protegidas 
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/";
	options.AccessDeniedPath = "/";
});

//Agregando servicios
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));
builder.Services.AddScoped<IEmailService, EmailService>();


// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.Use(async (context, next) =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        context.Response.Headers["Pragma"] = "no-cache";
        context.Response.Headers["Expires"] = "0";
    }

    await next();
});


app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Initialize
using (var scope = app.Services.CreateScope())
{
	await Initializer.SeedAsync(scope.ServiceProvider);
}

app.Run();
