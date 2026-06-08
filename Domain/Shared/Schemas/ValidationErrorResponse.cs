namespace Domain.Shared.Schemas;
public sealed class ValidationErrorResponse
{
    public IDictionary<string, string[]>? Errors { get; set; }  
}