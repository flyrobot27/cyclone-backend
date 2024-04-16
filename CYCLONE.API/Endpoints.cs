namespace CYCLONE.API
{
    using System.Text;
    using System.Text.Json;
    using CYCLONE.Template.Model.Exception;
    using Simphony.Simulation;

    public class Endpoints
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

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

                    // TODO: Return the results of the simulation
                }
                catch (Exception e) when (e is JsonException || e is ArgumentException || e is ModelExecutionException)
                {
                    return Results.BadRequest(e.Message);
                }
                catch (NotImplementedException)
                {
                    return Results.Problem("Not implemented");
                }

                return Results.Ok();
            });

            app.Run();
        }
    }
}
