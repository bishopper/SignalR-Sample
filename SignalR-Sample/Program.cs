using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SignalR.Bugeto.Contexts;
using SignalR.Bugeto.Hubs;
using SignalR.Bugeto.Models.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

string conectionString = "Data Source=BISHOP\\SQLSERVER2019;Initial Catalog=SignalRDB;Integrated Security=True;Trusted_Connection=True;User Id=sa;Password=M1379m1379;MultipleActiveResultSets=true;TrustServerCertificate=True";
builder.Services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(conectionString));
builder.Services.AddScoped<IChatRoomService, ChatRoomService>();
builder.Services.AddScoped<IMessageService, MessageService>();


builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(Options =>
        {
            Options.LoginPath = "/Home/login";
        });


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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<SiteChatHub>("/chathub");
app.MapHub<SupportHub>("/supporthub");

app.Run();
