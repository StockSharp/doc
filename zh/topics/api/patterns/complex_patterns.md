# 复杂蜡烛图形

## 概览

`ComplexCandlePattern` 类允许通过将多个简单模式（`ICandlePattern`）组合成一个来创建复杂的蜡烛图模式。在识别复杂模式时，每个内部模式都会依次检查其对应的蜡烛图段。只有当所有内部模式都匹配时，该模式才被认为是已识别。

## 蜡烛图形态

所有K线形态的基础接口：

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

`CandlePatternRegistry` 注册表包含一组内置模式：`Flat`、`White`、`Black`、`Hammer`、`BullishEngulfing`、`MorningStar`、`ThreeWhiteSoldiers` 等。

## 复杂烛形模式

`ComplexCandlePattern` 类实现了 `ICandlePattern` 并结合了多个内部模式：

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

当调用 `Recognize` 时，蜡烛数组会根据每个内部模式的 `CandlesCount` 被分割成连续的段。如果至少有一个内部模式不匹配，该方法将返回 `false`。

## 示例：创建复杂图案

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

## 蜡烛图模式提供者

`ICandlePatternProvider` 接口管理模式存储和查找：

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

### 实现

- `InMemoryCandlePatternProvider` -- 在内存中存储模式。初始化时，它从 `CandlePatternRegistry.All` 加载所有内置模式。
- `CandlePatternFileStorage` -- 将自定义模式保存到文件（JSON）。通过此提供程序也可以使用来自 `InMemoryCandlePatternProvider` 的内置模式。

## 示例：使用 ICandlePatternProvider

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

## 蜡烛图形态

对于基于公式创建的图案，使用 `ExpressionCandlePattern`。图案中的每根蜡烛由 `CandleExpressionCondition` 表达式描述，可使用以下变量：

| 变量 | 描述 |
|----------|-------------|
| `O` | 开盘价 |
| `H` | 高价 |
| `L` | 低价 |
| `C` | 收盘价 |
| `V` | 音量 |
| `B` | 蜡烛本体 |
| `LEN` | 蜡烛长度 |
| `BS` | 底部阴影 |
| `TS` | 顶部阴影 |

`p` 前缀指的是前一个蜡烛（`pO`、`pC`），`pp` -- 指的是两根蜡烛之前，以此类推。

`CandlePatternRegistry` 中的所有内置模式都是使用 `ExpressionCandlePattern` 构建的。
