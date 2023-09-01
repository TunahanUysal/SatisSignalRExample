using SatisSignalRExample.Hubs;
using SatisSignalRExample.Models;
using SatisSignalRExample.Subscription;
using SatisSignalRExample.Subscription.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.AllowCredentials()
          .AllowAnyHeader()
          .AllowAnyMethod()
          .SetIsOriginAllowed(x => true);
    });
});

builder.Services.AddSingleton<DatabaseSubscription<Personeller>>();    // Add Singleton tekil nesne oluþturur ve uygulamanýn ömrü ne kadarsa o zaman kadar mevcut olur.Transient ve scoped metodlarýna göre daha kalýcýdýr.(Uygulama açýk olduðu sürece)
builder.Services.AddSingleton<DatabaseSubscription<Satislar>>();    // Add Singleton tekil nesne oluþturur ve uygulamanýn ömrü ne kadarsa o zaman kadar mevcut olur.Transient ve scoped metodlarýna göre daha kalýcýdýr.(Uygulama açýk olduðu sürece)

builder.Services.AddSignalR();

var app = builder.Build();


app.UseCors();

app.UseDatabaseSubscription<DatabaseSubscription<Satislar>>("Satislar");
app.UseDatabaseSubscription<DatabaseSubscription<Personeller>>("Personeller");

app.UseRouting();

// app.MapGet("/", () => "Hello World!");

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<SatisHub>("/satishub");  
});

app.Run();
