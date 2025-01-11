using Discounts.Grpc;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new DiscountService.DiscountServiceClient(channel);

var response = await client.GenerateDiscountCodesAsync(new()
{
    Count = 10,
    Length = 8
});

foreach (var item in response.Codes)
{
    Console.WriteLine($"Code generated: {item}");

    var useCodeResponse = await client.UseCodeAsync(new() { Code = item });
    Console.WriteLine($"Code used: {useCodeResponse.Result}");
}
