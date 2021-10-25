# .NET MAUI Benchmarks

These are setup using [BenchmarkDotNet][https://benchmarkdotnet.org/].

To run:

    cd src/Core/tests/Benchmarks
    dotnet run -c Release

This will prompt to select a benchmark:

    Available Benchmarks:
      #0 GetHandlersBenchmarker
      #1 ListUnionBenchmarker
      #2 MauiServiceProviderBenchmarker
      #3 PropertyMapperBenchmarker
      #4 RegisterHandlersBenchmarker
    You should select the target benchmark(s). Please, print a number of a benchmark (e.g. `0`) or a contained benchmark caption (e.g. `GetHandlersBenchmarker`).
    If you want to select few, please separate them with space ` ` (e.g. `1 2 3`).
    You can also provide the class name in console arguments by using --filter. (e.g. `--filter *GetHandlersBenchmarker*`).

## Results

Keeping the results of each benchmark here, so we have a record of future changes.

To measure:

1. Run the benchmark without your changes, as a baseline.
1. Apply your changes and run the benchmarks again.

Post the results on your PR, and update the `README.md` below if the
results are acceptable. I used `--` to denote "before" and `++` to
denote "after".

## PropertyMapperBenchmarker

    BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1288 (21H1/May2021Update)
    Intel Core i9-9900K CPU 3.60GHz (Coffee Lake), 1 CPU, 16 logical and 8 physical cores
    .NET SDK=6.0.100-rtm.21521.3
      [Host]     : .NET 6.0.0 (6.0.21.51812), X64 RyuJIT

|             Method |     Mean |     Error |    StdDev |  Gen 0 |  Gen 1 | Allocated |
|------------------- |---------:|----------:|----------:|-------:|-------:|----------:|
| --UpdateProperties | 5.915 us | 0.1108 us | 0.1232 us | 2.0294 | 0.0839 |     17 KB |
| ++UpdateProperties | 3.924 us | 0.0385 us | 0.0360 us | 1.6098 | 0.0610 |     13 KB |
