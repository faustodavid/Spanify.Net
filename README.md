[![Nuget](https://img.shields.io/nuget/v/Spanify.Net)](https://www.nuget.org/packages/Spanify.Net/)

# Spanify.Net

Span extensions to increase perf while keeping the same productivity.

## Split
Split works similiar to `string.Split` but **allocation-free** and on top of `ReadOnlySpan<char>`, you can get a `ReadOnlySpan<char>` from any string using the extension `AsSpan()`.

### Example

Split and enumerate
```csharp
var input = "val0,val1,val2";
foreach(var value in input.AsSpan().Split(','))
{
  // Do some work with the value
}
```

Split and get item by index
```csharp
var input = "val0,val1,val2";
var values = input.AsSpan().Split(',');

var secondValue = values[1]; // val1
```

### Benchmarks

BenchmarkDotNet=v0.12.1, OS=macOS Catalina 10.15.3 (19D76) [Darwin 19.3.0]
Intel Core i9-9880H CPU 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=3.1.201
  [Host]     : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT
  DefaultJob : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT


**Split and enumerate collection**

`N` is the amount of item being split.


```
|       Method |   N |        Mean |      Error |     StdDev | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------- |---- |------------:|-----------:|-----------:|------:|--------:|-------:|-------:|------:|----------:|
|        Split |   1 |    45.61 ns |   0.941 ns |   1.901 ns |  1.00 |    0.00 | 0.0038 |      - |     - |      32 B |
| SpanifySplit |   1 |    35.02 ns |   0.711 ns |   1.065 ns |  0.77 |    0.04 |      - |      - |     - |         - |
|              |     |             |            |            |       |         |        |        |       |           |
|        Split |   5 |   348.14 ns |   6.942 ns |  18.166 ns |  1.00 |    0.00 | 0.0648 |      - |     - |     544 B |
| SpanifySplit |   5 |    75.51 ns |   1.499 ns |   2.504 ns |  0.22 |    0.01 |      - |      - |     - |         - |
|              |     |             |            |            |       |         |        |        |       |           |
|        Split |  10 |   668.47 ns |  13.382 ns |  31.016 ns |  1.00 |    0.00 | 0.1268 |      - |     - |    1064 B |
| SpanifySplit |  10 |   116.20 ns |   2.354 ns |   4.123 ns |  0.17 |    0.01 |      - |      - |     - |         - |
|              |     |             |            |            |       |         |        |        |       |           |
|        Split |  20 | 1,282.98 ns |  25.499 ns |  66.275 ns |  1.00 |    0.00 | 0.2499 | 0.0019 |     - |    2104 B |
| SpanifySplit |  20 |   219.02 ns |   4.407 ns |   6.178 ns |  0.17 |    0.01 |      - |      - |     - |         - |
|              |     |             |            |            |       |         |        |        |       |           |
|        Split | 100 | 6,080.55 ns | 102.985 ns | 114.467 ns |  1.00 |    0.00 | 1.2436 | 0.0458 |     - |   10424 B |
| SpanifySplit | 100 | 1,023.73 ns |  20.341 ns |  38.206 ns |  0.17 |    0.01 |      - |      - |     - |         - |
```

**Split and get element by index**

`N` is the amount of item being split.

```
|     Method |   N |        Mean |     Error |     StdDev |      Median | Ratio | RatioSD |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|----------- |---- |------------:|----------:|-----------:|------------:|------:|--------:|-------:|-------:|------:|----------:|
|        Get |   1 |    63.45 ns |  1.298 ns |   2.821 ns |    64.27 ns |  1.00 |    0.00 | 0.0038 |      - |     - |      32 B |
| SpanifyGet |   1 |    35.78 ns |  0.729 ns |   0.716 ns |    35.62 ns |  0.56 |    0.03 |      - |      - |     - |         - |
|            |     |             |           |            |             |       |         |        |        |       |           |
|        Get |   5 |   246.04 ns |  4.946 ns |  13.705 ns |   240.77 ns |  1.00 |    0.00 | 0.0648 |      - |     - |     544 B |
| SpanifyGet |   5 |    51.33 ns |  1.043 ns |   1.800 ns |    50.96 ns |  0.20 |    0.01 |      - |      - |     - |         - |
|            |     |             |           |            |             |       |         |        |        |       |           |
|        Get |  10 |   457.06 ns |  9.001 ns |  16.905 ns |   453.09 ns |  1.00 |    0.00 | 0.1268 |      - |     - |    1064 B |
| SpanifyGet |  10 |    80.33 ns |  1.607 ns |   3.210 ns |    81.35 ns |  0.18 |    0.01 |      - |      - |     - |         - |
|            |     |             |           |            |             |       |         |        |        |       |           |
|        Get |  20 |   920.37 ns | 18.382 ns |  46.789 ns |   939.72 ns |  1.00 |    0.00 | 0.2499 | 0.0019 |     - |    2104 B |
| SpanifyGet |  20 |   125.70 ns |  2.508 ns |   4.650 ns |   125.60 ns |  0.14 |    0.01 |      - |      - |     - |         - |
|            |     |             |           |            |             |       |         |        |        |       |           |
|        Get | 100 | 4,225.14 ns | 83.351 ns | 168.372 ns | 4,176.82 ns |  1.00 |    0.00 | 1.2436 | 0.0458 |     - |   10424 B |
| SpanifyGet | 100 |   513.58 ns | 10.243 ns |  20.219 ns |   522.75 ns |  0.12 |    0.01 |      - |      - |     - |         - |
```
