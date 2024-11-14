using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplication2;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Добавление сервисов в контейнер
        builder.Services.AddControllers();

        // Добавляем IGameService как singleton
        builder.Services.AddSingleton<GameService>(); 
        // Создаем и настраиваем приложение
        var app = builder.Build();

        // Конфигурация middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        // Настройка маршрутизации контроллеров
        app.MapControllers();

        // Запуск приложения
        app.Run();
    }
}
