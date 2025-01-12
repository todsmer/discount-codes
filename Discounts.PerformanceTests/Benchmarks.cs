using BenchmarkDotNet.Attributes;
using Discounts.Grpc;
using Grpc.Net.Client;

namespace Discounts.PerformanceTests;

public class Benchmarks
{
    private readonly DiscountService.DiscountServiceClient _client
        = new(GrpcChannel.ForAddress("http://localhost:5001"));

    [Benchmark]
    public async Task GenerateAndUseDiscountCodes()
    {
        var response = await _client.GenerateDiscountCodesAsync(new()
        {
            Count = 10,
            Length = 8
        });

        foreach (var item in response.Codes)
        {
            await _client.UseCodeAsync(new() { Code = item });
        }
    }

    [Benchmark]
    public async Task GenerateDiscountCodesForGreatNumberOfCodes()
    {
        await _client.GenerateDiscountCodesAsync(new()
        {
            Count = 2000,
            Length = 8
        });
    }
}
