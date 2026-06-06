namespace Infrastructure.Microservices;

public interface IMicroserviceClient
{
    Task<TResponse?> GetAsync<TResponse>(string path, CancellationToken cancellationToken = default);
    
    Task<TResponse?> PostAsync<TRequest, TResponse>(string path, TRequest payload, CancellationToken cancellationToken = default);
    Task PostAsync<TRequest>(string path, TRequest payload, CancellationToken cancellationToken = default);
    
    Task<TResponse?> PutAsync<TRequest, TResponse>(string path, TRequest payload, CancellationToken cancellationToken = default);
    Task PutAsync<TRequest>(string path, TRequest payload, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(string path, CancellationToken cancellationToken = default);
    
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);
}
