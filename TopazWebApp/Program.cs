using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Scaffold;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Information("Загрузка Core приложения...");
    builder.Services.AddCoreDi(builder.Configuration);

    Log.Information("Загрузка анализаторов Blazor Server...");
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services
        .AddBlazorise(options => { options.Immediate = true; })
        .AddBootstrap5Providers()
        .AddFontAwesomeIcons()
        .AddBootstrap5Components();

    builder.Services.AddHttpContextAccessor();

    var app = builder.Build();

    Log.Information("Разрешение использования кросс-доменных запросов...");
    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials());

    app.UseExceptionHandler("/error");

    Log.Information("Настройка Redirect HTTP на HTTPS...");
    app.UseHsts()
        .UseHttpsRedirection();

    Log.Information("Надстройка статических файлов...");
    app.UseDefaultFiles()
        .UseStaticFiles();

    Log.Information("Настройка маршрутизации по компонентам Blazor...");
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException" and "HostAbortedException")
{
    Log.Fatal(ex, $"Ошибка:{ex.Message}, Доп. информация по ошибке:${ex.InnerException?.Message ?? "Нет информации!"}");
}
finally
{
    Log.Information("Закрытие веб приложения...");
    Log.CloseAndFlush();
}