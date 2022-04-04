using System.Net.WebSockets;
using CoinP2P.Models.Network;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvcCore()
                .AddRazorViewEngine();
builder.Services.AddSingleton<P2PNode>();

var app = builder.Build();

app.UseRouting();

app.MapControllerRoute(
    "areas",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    "controllers",
    "{controller=Home}/{action=Index}/{id?}"
);

app.Run();