# Complex Candle Patterns

## Overview

The `ComplexCandlePattern` class allows creating complex candle patterns by combining multiple simple patterns (`ICandlePattern`) into one. When recognizing a complex pattern, each inner pattern is checked sequentially on its segment of candles. The pattern is considered recognized only if all inner patterns match.

## ICandlePattern

The base interface for all candle patterns:

```csharp
public interface ICandlePattern : IPersistable
{
    // Pattern name
    string Name { get; }

    // Number of candles required for recognition
    int CandlesCount { get; }

    // Check whether the pattern is recognized on the given candles
    bool Recognize(ReadOnlySpan<ICandleMessage> candles);
}
```

The `CandlePatternRegistry` registry contains a set of built-in patterns: `Flat`, `White`, `Black`, `Hammer`, `BullishEngulfing`, `MorningStar`, `ThreeWhiteSoldiers`, and others.

## ComplexCandlePattern

The `ComplexCandlePattern` class implements `ICandlePattern` and combines multiple inner patterns:

```csharp
public class ComplexCandlePattern : ICandlePattern
{
    // Create an empty pattern
    public ComplexCandlePattern() { }

    // Create a pattern with a name and set of inner patterns
    public ComplexCandlePattern(string name, IEnumerable<ICandlePattern> inner);

    // Complex pattern name
    public string Name { get; }

    // Inner patterns
    public IEnumerable<ICandlePattern> Inner { get; }

    // Total number of candles (sum of CandlesCount for all inner patterns)
    public int CandlesCount { get; }
}
```

When `Recognize` is called, the candle array is split into sequential segments according to each inner pattern's `CandlesCount`. If at least one inner pattern does not match, the method returns `false`.

## Example: Creating a Complex Pattern

```csharp
using StockSharp.Algo.Candles.Patterns;

// Create a complex pattern: first a bearish candle, then bullish engulfing
var complex = new ComplexCandlePattern(
    "Reversal Up",
    new ICandlePattern[]
    {
        CandlePatternRegistry.Black,            // 1 candle: bearish
        CandlePatternRegistry.BullishEngulfing,  // 2 candles: bullish engulfing
    }
);

// 3 candles are required for recognition (1 + 2)
Console.WriteLine($"Candles required: {complex.CandlesCount}"); // 3
```

## ICandlePatternProvider

The `ICandlePatternProvider` interface manages pattern storage and lookup:

```csharp
public interface ICandlePatternProvider
{
    // Events for pattern creation, replacement, and deletion
    event Action<ICandlePattern> PatternCreated;
    event Action<ICandlePattern, ICandlePattern> PatternReplaced;
    event Action<ICandlePattern> PatternDeleted;

    // Initialize storage
    ValueTask InitAsync(CancellationToken cancellationToken);

    // All available patterns
    IEnumerable<ICandlePattern> Patterns { get; }

    // Find a pattern by name
    bool TryFind(string name, out ICandlePattern pattern);

    // Remove a pattern
    bool Remove(ICandlePattern pattern);

    // Save (create or replace) a pattern
    void Save(ICandlePattern pattern);
}
```

### Implementations

- `InMemoryCandlePatternProvider` -- stores patterns in memory. On initialization, it loads all built-in patterns from `CandlePatternRegistry.All`.
- `CandlePatternFileStorage` -- saves custom patterns to a file (JSON). Built-in patterns from `InMemoryCandlePatternProvider` are also available through this provider.

## Example: Working with ICandlePatternProvider

```csharp
using StockSharp.Algo.Candles.Patterns;
using StockSharp.Configuration;

// Create file-based pattern storage
var executor = new ChannelExecutor();
var provider = new CandlePatternFileStorage(
    Paths.FileSystem,
    Paths.CandlePatternsFile,
    executor
);

// Initialize (loads built-in + custom patterns from file)
await provider.InitAsync(CancellationToken.None);

// Subscribe to new pattern creation event
provider.PatternCreated += pattern =>
{
    Console.WriteLine($"Pattern created: {pattern.Name}");
};

// Create and save a complex pattern
var myPattern = new ComplexCandlePattern(
    "My Pattern",
    new ICandlePattern[]
    {
        CandlePatternRegistry.Hammer,
        CandlePatternRegistry.White,
    }
);

provider.Save(myPattern);

// Find a pattern by name
if (provider.TryFind("My Pattern", out var found))
{
    Console.WriteLine($"Found: {found.Name}, candles: {found.CandlesCount}");
}
```

## ExpressionCandlePattern

For creating patterns based on formulas, `ExpressionCandlePattern` is used. Each candle in the pattern is described by a `CandleExpressionCondition` expression, with the following variables available:

| Variable | Description |
|----------|-------------|
| `O` | Open price |
| `H` | High price |
| `L` | Low price |
| `C` | Close price |
| `V` | Volume |
| `B` | Candle body |
| `LEN` | Candle length |
| `BS` | Bottom shadow |
| `TS` | Top shadow |

The `p` prefix refers to the previous candle (`pO`, `pC`), `pp` -- to two candles back, and so on.

All built-in patterns in `CandlePatternRegistry` are built using `ExpressionCandlePattern`.
