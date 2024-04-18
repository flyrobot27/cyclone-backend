namespace CYCLONE.API
{
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using CYCLONE.Template.Model.Exception;
    using Simphony.Simulation;

    public class Endpoints
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            var app = builder.Build();
            
            app.UseCors("AllowAll");
            
            app.MapPost("/api/cyclone", async (HttpRequest request) =>
            {
                string body = string.Empty;
                using StreamReader reader = new(request.Body, Encoding.UTF8);
                body = await reader.ReadToEndAsync();

                try
                {
                    var decoder = new JSONDecode.Decoder(body);
                    var engine = new DiscreteEventEngine();
                    var scenario = decoder.ToScenario(engine, debug: true);

                    engine.InitializeEngine();
                    var terminationReason = engine.Simulate(scenario);

                    var returnDict = new Dictionary<string, object>
                    {
                        {"terminationReason", terminationReason.ToString()},
                        {"intrinsicResult", scenario.IntrinsicResults},
                        {"nonIntrinsicResult", scenario.NonIntrinsicResults},
                        {"counterResult", scenario.CounterResults},
                        {"waitingFileResult", scenario.WaitingFileResults},
                    };

                    var options = new JsonSerializerOptions
                    {
                        NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    };

                    return Results.Json(returnDict);
                }
                catch (Exception e) when (e is JsonException || e is ArgumentException || e is ModelExecutionException)
                {
                    return Results.BadRequest(e.Message);
                }
                catch (NotImplementedException)
                {
                    return Results.Problem("Not implemented");
                }
            });

            app.Run();
        }
    }
}
