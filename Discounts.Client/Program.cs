using Discounts.Grpc;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("http://localhost:5001");

var client = new DiscountService.DiscountServiceClient(channel);

var request = new GenerateDiscountCodesRequest
{
    Count = 10,
    Length = 8
};

var response = await client.GenerateDiscountCodesAsync(request);

foreach (var item in response.Codes)
{
    Console.WriteLine($"Code generated: {item}");

    var useCodeResponse = await client.UseCodeAsync(new() { Code = item });
    Console.WriteLine($"Code used: {useCodeResponse.Result}");
}
