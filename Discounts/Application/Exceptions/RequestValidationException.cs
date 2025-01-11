using Grpc.Core;

namespace Discounts.Application.Exceptions;

public class RequestValidationException(string message)
    : RpcException(new(StatusCode.InvalidArgument, message));
