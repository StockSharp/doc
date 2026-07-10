# 複合ローソク足パターン

## 概要

`ComplexCandlePattern` クラスを使用すると、複数の単純なパターン（`ICandlePattern`）を 1 つに組み合わせて、複合ローソク足パターンを作成できます。複合パターンを認識するときは、各内部パターンが、それぞれに対応するローソク足セグメント上で順番にチェックされます。すべての内部パターンが一致した場合にのみ、パターンは認識されたと見なされます。

## ICandlePattern

すべてのローソク足パターンの基本インターフェイスです。

```csharp
public interface ICandlePattern : IPersistable
{
    // パターン名
    string Name { get; }

    // 認識に必要なローソク足の本数
    int CandlesCount { get; }

    // 指定されたローソク足でパターンが認識されるかどうかを確認
    bool Recognize(ReadOnlySpan<ICandleMessage> candles);
}
```

`CandlePatternRegistry` レジストリには、`Flat`、`White`、`Black`、`Hammer`、`BullishEngulfing`、`MorningStar`、`ThreeWhiteSoldiers` などの組み込みパターン一式が含まれています。

## ComplexCandlePattern

`ComplexCandlePattern` クラスは `ICandlePattern` を実装し、複数の内部パターンを組み合わせます。

```csharp
public class ComplexCandlePattern : ICandlePattern
{
    // 空のパターンを作成
    public ComplexCandlePattern() { }

    // 名前と内部パターンのセットを持つパターンを作成
    public ComplexCandlePattern(string name, IEnumerable<ICandlePattern> inner);

    // 複合パターン名
    public string Name { get; }

    // 内部パターン
    public IEnumerable<ICandlePattern> Inner { get; }

    // ローソク足の合計本数（すべての内部パターンの CandlesCount の合計）
    public int CandlesCount { get; }
}
```

`Recognize` が呼び出されると、ローソク足配列は各内部パターンの `CandlesCount` に従って連続するセグメントに分割されます。少なくとも 1 つの内部パターンが一致しない場合、メソッドは `false` を返します。

## 例: 複合パターンの作成

```csharp
using StockSharp.Algo.Candles.Patterns;

// 複合パターンを作成: 最初に弱気ローソク足、次に強気包み足
var complex = new ComplexCandlePattern(
    "Reversal Up",
    new ICandlePattern[]
    {
        CandlePatternRegistry.Black,            // 1 本のローソク足: 弱気
        CandlePatternRegistry.BullishEngulfing,  // 2 本のローソク足: 強気包み足
    }
);

// 認識には 3 本のローソク足が必要（1 + 2）
Console.WriteLine($"必要なローソク足数: {complex.CandlesCount}"); // 3
```

## ICandlePatternProvider

`ICandlePatternProvider` インターフェイスは、パターンの保存と検索を管理します。

```csharp
public interface ICandlePatternProvider
{
    // パターンの作成、置換、削除のイベント
    event Action<ICandlePattern> PatternCreated;
    event Action<ICandlePattern, ICandlePattern> PatternReplaced;
    event Action<ICandlePattern> PatternDeleted;

    // ストレージを初期化
    ValueTask InitAsync(CancellationToken cancellationToken);

    // 使用可能なすべてのパターン
    IEnumerable<ICandlePattern> Patterns { get; }

    // 名前でパターンを検索
    bool TryFind(string name, out ICandlePattern pattern);

    // パターンを削除
    bool Remove(ICandlePattern pattern);

    // パターンを保存（作成または置換）
    void Save(ICandlePattern pattern);
}
```

### 実装

- `InMemoryCandlePatternProvider` -- パターンをメモリ内に保存します。初期化時に、`CandlePatternRegistry.All` からすべての組み込みパターンを読み込みます。
- `CandlePatternFileStorage` -- カスタムパターンをファイル（JSON）に保存します。`InMemoryCandlePatternProvider` の組み込みパターンも、このプロバイダーを通じて使用できます。

## 例: ICandlePatternProvider の使用

```csharp
using StockSharp.Algo.Candles.Patterns;
using StockSharp.Configuration;

// ファイルベースのパターンストレージを作成
var executor = new ChannelExecutor();
var provider = new CandlePatternFileStorage(
    Paths.FileSystem,
    Paths.CandlePatternsFile,
    executor
);

// 初期化（ファイルから組み込み + カスタムパターンを読み込む）
await provider.InitAsync(CancellationToken.None);

// 新しいパターン作成イベントを購読
provider.PatternCreated += pattern =>
{
    Console.WriteLine($"Pattern created: {pattern.Name}");
};

// 複合パターンを作成して保存
var myPattern = new ComplexCandlePattern(
    "My Pattern",
    new ICandlePattern[]
    {
        CandlePatternRegistry.Hammer,
        CandlePatternRegistry.White,
    }
);

provider.Save(myPattern);

// 名前でパターンを検索
if (provider.TryFind("My Pattern", out var found))
{
    Console.WriteLine($"見つかりました: {found.Name}, ローソク足数: {found.CandlesCount}");
}
```

## ExpressionCandlePattern

数式に基づいてパターンを作成するには、`ExpressionCandlePattern` を使用します。パターン内の各ローソク足は `CandleExpressionCondition` 式で記述され、次の変数を使用できます。

| 変数 | 説明 |
|----------|-------------|
| `O` | 始値 |
| `H` | 高値 |
| `L` | 安値 |
| `C` | 終値 |
| `V` | 取引量 |
| `B` | ローソク足の実体 |
| `LEN` | ローソク足の長さ |
| `BS` | 下ヒゲ |
| `TS` | 上ヒゲ |

`p` プレフィックスは前のローソク足（`pO`、`pC`）を指し、`pp` は 2 本前のローソク足を指す、というように続きます。

`CandlePatternRegistry` のすべての組み込みパターンは、`ExpressionCandlePattern` を使用して構築されています。
