namespace Domain.Shared.Exceptions;

public sealed class AccountDisabledException(string message) : Exception(message);

public sealed class ForbiddenAccessException(string message) : Exception(message);

public sealed class EntityNotFoundException(string message) : Exception(message);

public sealed class BusinessRuleException(string message) : Exception(message);
