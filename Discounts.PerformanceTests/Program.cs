using BenchmarkDotNet.Running;
using Discounts.PerformanceTests;

var summary = BenchmarkRunner.Run<Benchmarks>();
Console.WriteLine(summary);
