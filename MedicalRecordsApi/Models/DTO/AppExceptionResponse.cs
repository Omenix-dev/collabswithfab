using System;
using System.Linq;

namespace MedicalRecordsApi.Models.DTO
{
    /// <summary>
    /// Represents a response model for handling and presenting information about an application exception.
    /// </summary>
    public class AppExceptionResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppExceptionResponse"/> class based on the provided AppException.
        /// </summary>
        /// <param name="exception">The AppException from which to extract information.</param>
        public AppExceptionResponse(AppException exception)
        {
            Reference = Guid.NewGuid().ToString();
            StatusCode = exception.StatusCode;
            ExceptionType = exception.ExceptionType;
            ErrorData = exception.ErrorData;
            TimeStamp = DateTime.Now;
        }

        /// <summary>
        /// Default constructor for AppExceptionResponse.
        /// </summary>
        public AppExceptionResponse() { }

        /// <summary>
        /// Gets or sets a unique reference identifier for the exception response.
        /// </summary>
        public string Reference { get; internal set; }

        /// <summary>
        /// Gets or sets the HTTP status code associated with the exception.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the type of exception.
        /// </summary>
        public string ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets additional error data associated with the exception.
        /// </summary>
        public string[] ErrorData { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the exception response was created.
        /// </summary>
        public DateTime TimeStamp { get; internal set; }
    }


    public class AppException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppException"/> class with a specified message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public AppException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppException"/> class with specified error data, exception type, and status code.
        /// </summary>
        /// <param name="errorData">Additional error data associated with the exception.</param>
        /// <param name="exceptionType">The type of the exception.</param>
        /// <param name="statusCode">The HTTP status code associated with the exception.</param>
        public AppException(string[] errorData, string exceptionType, int statusCode) : base()
        {
            ExceptionType = exceptionType;
            StatusCode = statusCode;
            ErrorData = errorData[0].Split(';').Select(e => e.Trim()).Where(e => !string.IsNullOrWhiteSpace(e)).ToArray();
        }

        /// <summary>
        /// Gets or sets the HTTP status code associated with the exception.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the type of exception.
        /// </summary>
        public string ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets additional error data associated with the exception.
        /// </summary>
        public string[] ErrorData { get; set; }
    }

    /// <summary>
    /// Represents a specific BadRequestException, inheriting from <see cref="AppException"/>.
    /// </summary>
    public class BadRequestException : AppException
    {
        public BadRequestException(string error)
            : base(new[] { error }, "BadRequest", 400) { }
    }

    /// <summary>
    /// Represents a specific UnauthorizedException, inheriting from <see cref="AppException"/>.
    /// </summary>
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string error)
            : base(new[] { error }, "Unauthorized", 401) { }
    }

    /// <summary>
    /// Represents a specific UnauthorizedAccessException, inheriting from <see cref="AppException"/>.
    /// </summary>
    public class UnauthorizedAccessException : AppException
    {
        public UnauthorizedAccessException(string error)
            : base(new[] { error }, "Unauthorized Access", 401) { }
    }

    /// <summary>
    /// Represents a specific ForbiddenException, inheriting from <see cref="AppException"/>.
    /// </summary>
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string error)
            : base(new[] { error }, "Forbidden", 403) { }
    }

    /// <summary>
    /// Represents a specific NotFoundException, inheriting from <see cref="AppException"/>.
    /// </summary>
    public class NotFoundException : AppException
    {
        public NotFoundException(string error)
            : base(new[] { error }, "NotFound", 404) { }
    }

    /// <summary>
    /// Represents a specific MethodNotAllowedException, inheriting from <see cref="AppException"/>.
    /// </summary>
    public class MethodNotAllowedException : AppException
    {
        public MethodNotAllowedException(string error)
            : base(new[] { error }, "MethodNotAllowed", 405) { }
    }

    /// <summary>
    /// Represents a specific ConflictException, inheriting from <see cref="AppException"/>.
    /// </summary>
    public class ConflictException : AppException
    {
        public ConflictException(string error)
            : base(new[] { error }, "Conflict", 409) { }
    }

    /// <summary>
    /// Represents a specific UnsupportedMediaTypeException, inheriting from <see cref="AppException"/>.
    /// </summary>
    public class UnsupportedMediaTypeException : AppException
    {
        public UnsupportedMediaTypeException(string error)
            : base(new[] { error }, "UnsupportedMediaType", 415) { }
    }

    /// <summary>
    /// Represents a specific UnprocessableEntityException, inheriting from <see cref="AppException"/>.
    /// </summary>
    public class UnprocessableEntityException : AppException
    {
        public UnprocessableEntityException(string error)
            : base(new[] { error }, "UnprocessableEntity", 422) { }
    }

    /// <summary>
    /// Represents a specific TooManyRequestsException, inheriting from <see cref="AppException"/>.
    /// </summary>
    public class TooManyRequestsException : AppException
    {
        public TooManyRequestsException(string error)
            : base(new[] { error }, "TooManyRequests", 429) { }
    }

    /// <summary>
    /// Represents a specific InternalServerException, inheriting from <see cref="AppException"/>.
    /// </summary>
    public class InternalServerException : AppException
    {
        public InternalServerException(string error)
            : base(new[] { error }, "InternalServerError", 500) { }
    }

    /// <summary>
    /// Represents a specific BadGatewayException, inheriting from <see cref="AppException"/>.
    /// </summary>
    public class BadGatewayException : AppException
    {
        public BadGatewayException(string error)
            : base(new[] { error }, "BadGateway", 502) { }
    }

    /// <summary>
    /// Represents a specific ServiceUnavailableException, inheriting from <see cref="AppException"/>.
    /// </summary>
    public class ServiceUnavailableException : AppException
    {
        public ServiceUnavailableException(string error)
            : base(new[] { error }, "ServiceUnavailable", 503) { }
    }

    /// <summary>
    /// Represents a specific GatewayTimeoutException, inheriting from <see cref="AppException"/>.
    /// </summary>
    public class GatewayTimeoutException : AppException
    {
        public GatewayTimeoutException(string error)
            : base(new[] { error }, "GatewayTimeout", 504) { }
    }
}