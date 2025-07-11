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

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders(); ;

//Rutas protegidas 
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/";
	options.AccessDeniedPath = "/";
});

//Agregando servicios
builder.Services.AddScoped<IEmailService,EmailService>();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Initialize
using (var scope = app.Services.CreateScope())
{
	await Initializer.SeedAsync(scope.ServiceProvider);
}

app.Run();
