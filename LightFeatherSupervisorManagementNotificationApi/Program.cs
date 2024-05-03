namespace LightFeatherSupervisorManagementNotificationApi;

using Service;
using Service.Impl.Api;

internal class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Bind custom configuration class
        builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
        // Validate API configuration on startup
        builder.Services.AddOptions<ApiSettings>()
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddHttpClient<ISupervisorService, ApiSupervisorService>(); // Register Supervisor service
        builder.Services.AddControllers(); // Registers controllers

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment()){
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.MapControllers();

        app.Run();
    }
}
