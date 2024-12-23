using WebApplication2.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Добавление сервисов в контейнер
        builder.Services.AddControllers();

        // Добавляем IGameService как singleton
        builder.Services.AddSingleton<GameService>();

        // Добавляем поддержку CORS
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://localhost:5173") // Разрешаем доступ только с этого источника
                      .AllowAnyMethod() // Разрешаем любые методы (GET, POST и т. д.)
                      .AllowAnyHeader() // Разрешаем любые заголовки
                      .AllowCredentials(); // Разрешаем куки
            });
        });

        // Создаем и настраиваем приложение
        var app = builder.Build();

        // Конфигурация middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCors();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        // Настройка маршрутизации контроллеров
        app.MapControllers();

        // Запуск приложения
        app.Run();
    }
}
