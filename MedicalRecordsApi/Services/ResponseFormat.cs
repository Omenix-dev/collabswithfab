﻿using Microsoft.AspNetCore.Mvc;
using MedicalRecordsApi.Constants;
using System;

namespace MedicalRecordsApi.Services
{
    public static class FormatResponseExtension
    {
        public static ActionResult FormatResponse<T>(this ServiceResponse<T> serviceResponse) //where T : class
        {
            ObjectResult response;
            ProblemDetails problemDetails;

            if (serviceResponse == null)
            {
                problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = ServiceErrorMessages.OperationFailed,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6"
                };
                return new ObjectResult(problemDetails)
                {
                    StatusCode = 500
                };
            }

            switch (serviceResponse.ServiceCode)
            {
                case InternalCode.Failed:
                    problemDetails = new ProblemDetails
                    {
                        Status = 500,
                        Title = ServiceErrorMessages.OperationFailed,
                        Detail = serviceResponse.Message,
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.6"
                    };
                    response = new ObjectResult(problemDetails)
                    {
                        StatusCode = 500
                    };

                    return response;

                case InternalCode.Success:

                    if (serviceResponse.Data != null)
                    {
                        Type dataType = serviceResponse.Data.GetType();

                        if (dataType == typeof(string))
                        {
                            string data = serviceResponse.Data as string;
                            if (string.IsNullOrEmpty(data))
                            {
                                return new OkResult();
                            }
                        }
                    }

                    return new OkObjectResult(serviceResponse.Data);

                case InternalCode.UpdateError:
                    problemDetails = new ProblemDetails
                    {
                        Status = 500,
                        Title = ServiceErrorMessages.InternalServerError,
                        Detail = serviceResponse.Message,
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.6"
                    };
                    response = new ObjectResult(problemDetails)
                    {
                        StatusCode = 500,
                    };
                    return response;

                case InternalCode.Mismatch:
                    problemDetails = new ProblemDetails
                    {
                        Status = 400,
                        Title = ServiceErrorMessages.MisMatch,
                        Detail = serviceResponse.Message,
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                    };
                    return new BadRequestObjectResult(problemDetails);

                case InternalCode.EntityIsNull:
                    problemDetails = new ProblemDetails
                    {
                        Status = 400,
                        Title = ServiceErrorMessages.EntityIsNull,
                        Detail = serviceResponse.Message,
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                    };
                    return new BadRequestObjectResult(problemDetails);

                case InternalCode.InvalidParam:
                    problemDetails = new ProblemDetails
                    {
                        Status = 400,
                        Title = ServiceErrorMessages.InvalidParam,
                        Detail = serviceResponse.Message,
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                    };
                    return new BadRequestObjectResult(problemDetails);

                case InternalCode.EntityNotFound:
                    problemDetails = new ProblemDetails
                    {
                        Status = 404,
                        Title = ServiceErrorMessages.EntityNotFound,
                        Detail = string.IsNullOrEmpty(serviceResponse.Message) ? "The requested resource was not found" : serviceResponse.Message,
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
                    };
                    return new NotFoundObjectResult(problemDetails);

                case InternalCode.Incompleted:
                    return new AcceptedResult("", serviceResponse.Data);

                case InternalCode.ListEmpty:
                    problemDetails = new ProblemDetails
                    {
                        Status = 400,
                        Title = ServiceErrorMessages.EntityIsNull,
                        Detail = serviceResponse.Message,
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                    };
                    return new BadRequestObjectResult(problemDetails);

                case InternalCode.EntityExist:
                    problemDetails = new ProblemDetails
                    {
                        Status = 409,
                        Title = ServiceErrorMessages.EntityExist,
                        Detail = string.IsNullOrEmpty(serviceResponse.Message) ? $"An entity of the type exists" : serviceResponse.Message,
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8"
                    };
                    return new ConflictObjectResult(problemDetails);

                case InternalCode.Unprocessable:
                    problemDetails = new ProblemDetails
                    {
                        Status = 422,
                        Title = ServiceErrorMessages.UnprocessableEntity,
                        Detail = string.IsNullOrEmpty(serviceResponse.Message) ? "The request cannot be processed" : serviceResponse.Message
                    };
                    return new UnprocessableEntityObjectResult(problemDetails);

                case InternalCode.Unauthorized:
                    problemDetails = new ProblemDetails
                    {
                        Status = 401,
                        Title = "Unathorized request",
                        Detail = "The supplied credentials is invalid."
                    };
                    return new UnauthorizedObjectResult(problemDetails);

                default:
                    return new OkObjectResult(serviceResponse.Data);
            }
        }
    }

    public enum InternalCode
    {
        UpdateError = -1, //when DbUpdateException / UpdateConcurrencyException is caught
        Failed, //operation failed
        Success,
        EntityIsNull, //parameter is null
		EntityNotFound, //entity not found
		Mismatch, //for update when model Id != supplied Id
        InvalidParam, //invalid parmater supplied in model. usually when the parameter fails the business logic
        Incompleted, //some processing was done but some failed
        ListEmpty, //a supplied paramter list is empty
        EntityExist, //the entity that is being saved already exists
        Unprocessable, //request cannot be processed.
        Unauthorized,
        ExternalApiCallFailed //to better handle logic when external api call failed. should not be returned to controller
    }

    public class ServiceResponse<T> //where T : class
    {
        public InternalCode ServiceCode { get; set; } = InternalCode.Failed;
        public T Data { get; set; }// = null;
        public string Message { get; set; }

        public ServiceResponse(T data, InternalCode serviceCode = InternalCode.Failed, string message = "")
        {
            Message = message;
            ServiceCode = serviceCode;
            Data = data;
        }
    }
}
