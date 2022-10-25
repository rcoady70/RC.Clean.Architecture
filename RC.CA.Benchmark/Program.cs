// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using RC.CA.Benchmark;

Console.WriteLine("Hello, World!");
BenchmarkRunner.Run<InvokeOptionsCAResult>();
Console.WriteLine("Finished");
