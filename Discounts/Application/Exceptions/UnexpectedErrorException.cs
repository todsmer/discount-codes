using Grpc.Core;

namespace Discounts.Application.Exceptions;

public class UnexpectedErrorException()
    : RpcException(new(StatusCode.Internal, "An unexpected error occurred"));
