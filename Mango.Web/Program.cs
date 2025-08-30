using Mango.Web.Services.AuthService;
using Mango.Web.Services.BaseServices;
using Mango.Web.Services.CouponServices;
using Mango.Web.Services.IServices;
using Mango.Web.Services.ProductServices;
using Mango.Web.Services.TokenProvider;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
SD.APIGateWay = builder.Configuration["ServiceUrls:LoadBalancerURL"] ;
//SD.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"] ;
//SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"] ;
//SD.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"] ;
//SD.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"] ;
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<ICartService, CartService>();
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme
    ).AddCookie(
    op => {
        op.LoginPath = "/Auth/Login";
        op.AccessDeniedPath = "/Auth/AccessDenied";
        op.ExpireTimeSpan = TimeSpan.FromHours(10);
    }

    );
//builder.Services.AddHttpClient("MangoAPI", c =>
//{
//    c.BaseAddress = new Uri(SD.CouponAPIBase);
//    c.DefaultRequestHeaders.Add("Accept", "application/json");
//});
builder.Services.AddScoped<ITokenProvider, TokenProvider>();

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartService, CartService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for couponion scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
