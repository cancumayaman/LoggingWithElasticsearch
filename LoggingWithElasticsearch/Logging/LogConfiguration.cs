using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace LoggingWithElasticsearch.Logging
{
    public static class LogConfiguration
    {
        public static void ConfigureLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json",optional:false,reloadOnChange:true)
                .AddJsonFile($"appsettings.{environment}.json",optional:true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
                .WriteTo.MongoDB("mongodb://localhost/log")
                .Enrich.WithProperty("Environemnt",environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".","-")}-{environment?.ToLower().Replace(".","-")}-{DateTime.UtcNow:yyyy--MM}"
            };
        }
    }
}
