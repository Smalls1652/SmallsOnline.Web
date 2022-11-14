using SmallsOnline.Web.Api.Helpers;
namespace SmallsOnline.Web.Api;

public class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add development settings if environment variable 'ASPNETCORE_ENVIRONMENT' is set to 'Development'.
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            // Point the config file to the development JSON file.
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", true)
                .AddEnvironmentVariables();

            // Config CORS for local development.
            // Essentially, allow all origins, methods, and headers.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                    );
            });

            // Set the environment variables for the CosmosDB service settings
            // based off what's in the development JSON file.
            Environment.SetEnvironmentVariable("CosmosDbConnectionString", builder.Configuration.GetValue<string>("CosmosDbConnectionString"));
            Environment.SetEnvironmentVariable("CosmosDbContainerName", builder.Configuration.GetValue<string>("CosmosDbContainerName"));
        }

        // Add the CosmosDB service.
        builder.Services.AddSingleton<ICosmosDbService>(
            cosmosDbSvc => new CosmosDbService(
                connectionString: AppSettings.GetSetting("CosmosDbConnectionString")!,
                containerName: AppSettings.GetSetting("CosmosDbContainerName")!
            )
        );

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}