# 复杂K线形

## 概览

`ComplexCandlePattern` 类允许通过将多个简单模式（`ICandlePattern`）组合成一个来创建复杂的K线模式。在识别复杂模式时，每个内部模式都会依次检查其对应的K线段。只有当所有内部模式都匹配时，该模式才被认为是已识别。

## K线形态

所有K线形态的基础接口：

```csharp
public interface ICandlePattern : IPersistable
{
    // 形态名称
    string Name { get; }

    // 识别所需的 K线数量
    int CandlesCount { get; }

    // 检查给定 K线上是否识别出形态
    bool Recognize(ReadOnlySpan<ICandleMessage> candles);
}
```

`CandlePatternRegistry` 注册表包含一组内置模式：`Flat`、`White`、`Black`、`Hammer`、`BullishEngulfing`、`MorningStar`、`ThreeWhiteSoldiers` 等。

## 复杂烛形模式

`ComplexCandlePattern` 类实现了 `ICandlePattern` 并结合了多个内部模式：

```csharp
public class ComplexCandlePattern : ICandlePattern
{
    // 创建空形态
    public ComplexCandlePattern() { }

    // 创建带名称和内部形态集合的形态
    public ComplexCandlePattern(string name, IEnumerable<ICandlePattern> inner);

    // 复杂形态名称
    public string Name { get; }

    // 内部形态
    public IEnumerable<ICandlePattern> Inner { get; }

    // K线总数（所有内部形态 CandlesCount 的总和）
    public int CandlesCount { get; }
}
```

当调用 `Recognize` 时，K线数组会根据每个内部模式的 `CandlesCount` 被分割成连续的段。如果至少有一个内部模式不匹配，该方法将返回 `false`。

## 示例：创建复杂图案

```csharp
using StockSharp.Algo.Candles.Patterns;

// 创建复杂形态：先是看跌 K线，然后是看涨吞没
var complex = new ComplexCandlePattern(
    "看涨反转",
    new ICandlePattern[]
    {
        CandlePatternRegistry.Black,            // 1 根 K 线：看跌
        CandlePatternRegistry.BullishEngulfing,  // 2 根 K 线：看涨吞没
    }
);

// 识别需要 3 根 K 线（1 + 2）
Console.WriteLine($"所需 K 线数量: {complex.CandlesCount}"); // 3
```

## K线模式提供者

`ICandlePatternProvider` 接口管理模式存储和查找：

```csharp
public interface ICandlePatternProvider
{
    // 形态创建、替换和删除事件
    event Action<ICandlePattern> PatternCreated;
    event Action<ICandlePattern, ICandlePattern> PatternReplaced;
    event Action<ICandlePattern> PatternDeleted;

    // 初始化存储
    ValueTask InitAsync(CancellationToken cancellationToken);

    // 所有可用形态
    IEnumerable<ICandlePattern> Patterns { get; }

    // 按名称查找形态
    bool TryFind(string name, out ICandlePattern pattern);

    // 删除形态
    bool Remove(ICandlePattern pattern);

    // 保存（创建或替换）形态
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

// 创建基于文件的形态存储
var executor = new ChannelExecutor();
var provider = new CandlePatternFileStorage(
    Paths.FileSystem,
    Paths.CandlePatternsFile,
    executor
);

// 初始化（从文件加载内置模式和自定义模式）
await provider.InitAsync(CancellationToken.None);

// 订阅新形态创建事件
provider.PatternCreated += pattern =>
{
    Console.WriteLine($"已创建形态: {pattern.Name}");
};

// 创建并保存复杂形态
var myPattern = new ComplexCandlePattern(
    "我的形态",
    new ICandlePattern[]
    {
        CandlePatternRegistry.Hammer,
        CandlePatternRegistry.White,
    }
);

provider.Save(myPattern);

// 按名称查找形态
if (provider.TryFind("我的形态", out var found))
{
    Console.WriteLine($"找到: {found.Name}, K线数量: {found.CandlesCount}");
}
```

## K线形态

对于基于公式创建的图案，使用 `ExpressionCandlePattern`。图案中的每根K线由 `CandleExpressionCondition` 表达式描述，可使用以下变量：

| 变量 | 描述 |
|----------|-------------|
| `O` | 开盘价 |
| `H` | 高价 |
| `L` | 低价 |
| `C` | 收盘价 |
| `V` | 成交量 |
| `B` | K线本体 |
| `LEN` | K线长度 |
| `BS` | 底部阴影 |
| `TS` | 顶部阴影 |

`p` 前缀指的是前一个K线（`pO`、`pC`），`pp` -- 指的是两根K线之前，以此类推。

`CandlePatternRegistry` 中的所有内置模式都是使用 `ExpressionCandlePattern` 构建的。
