using System.Net;
using System.Text.Json;
using FormulaOne.Entities.Dtos.Common;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RestSharp;

namespace FormulaOne.Api.Services;

public class FlightService : IFlightService
{

    private static readonly AsyncRetryPolicy<RestResponse> RetryPolicy =
        Policy.HandleResult<RestResponse>(resp =>
                resp.StatusCode == HttpStatusCode.TooManyRequests || (int)resp.StatusCode >= 500)
            .WaitAndRetryAsync(4, retryAttempt =>
            {
                Console.WriteLine($"Attempt {retryAttempt} - Retrying due to error");
                return TimeSpan.FromSeconds(5 + retryAttempt);
            });
    
    private static readonly AsyncCircuitBreakerPolicy<RestResponse> CbPolicy = 
        Policy.HandleResult<RestResponse>(
            resp => (int)resp.StatusCode >= 400)
            .CircuitBreakerAsync(4, TimeSpan.FromMinutes(1));

    private static readonly AsyncCircuitBreakerPolicy<RestResponse> AdvancedCbPolicy =
        Policy.HandleResult<RestResponse>(
                resp => (int)resp.StatusCode >= 400)
            .AdvancedCircuitBreakerAsync(
                0.5, //50% of requests must fail
                TimeSpan.FromMinutes(1), //duration to measure failure rate
                10, //minimum number of requests before circuit can be tripped
                TimeSpan.FromMinutes(1)); // duration of break state
        
    
    public async Task<List<FlightDto>?> GetAllAvailableFlights()
    {
        if(CbPolicy.CircuitState is CircuitState.Open)
            throw new Exception("Service is not available");
        
        const string url = "https://localhost:7195/api/FlightsCalendar";

        var client = new RestClient();
        var request = new RestRequest(url);
        
        // var response = await client.ExecuteAsync(request); -> sin utilizar política de reintentos
        
        // Utilizando política de reintentos con Polly
        // var response = await RetryPolicy.ExecuteAsync(async () =>
        //     await client.ExecuteAsync(request));
        
        // Utilizando política de circuit breaker con Polly
        // var response = await CbPolicy.ExecuteAsync(async ()
        //     => await RetryPolicy.ExecuteAsync(async () 
        //         => await client.ExecuteAsync(request)));
        
        // Utilizando política de circuit breaker avanzada con Polly
        var response = await AdvancedCbPolicy.ExecuteAsync(async ()
            => await RetryPolicy.ExecuteAsync(async ()
                => await client.ExecuteAsync(request)));
        
        if(!response.IsSuccessful)
            throw new Exception("Service is not available");

        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<List<FlightDto>?>(response?.Content!, options);
    }
}