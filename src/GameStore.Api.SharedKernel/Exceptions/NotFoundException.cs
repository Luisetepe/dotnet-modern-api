namespace GameStore.Api.SharedKernel.Exceptions;

public class NotFoundException(string message) : Exception(message);