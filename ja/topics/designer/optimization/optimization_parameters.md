# 最適化パラメーター

最適化は、次の型を持つストラテジーパラメーターに対して実行されます。

- 数値（整数および小数）
- 時間（[TimeSpan](xref:System.TimeSpan)）
- ブール値（真-偽）
- [Unit](../../api/strategies/unit_type.md) 値

既定では、これらの型を持つすべてのパラメーターが [オプティマイザーパラメーター表](brute_force.md) に表示されます。最適化からパラメーターを除外するには、次のようにします。

- [ダイアグラム](../strategies/using_visual_designer.md)の場合は、必要なキューブを選択し、そのプロパティを開き、**詳細設定** に切り替えて、**パラメーター** チェックボックスをオフにします。

![Designer 最適化 01](../../../images/designer_optimization_01.png)

- [コード](../strategies/using_code.md)の場合は、パラメーターを定義するときにコードを書き、[CanOptimize](xref:StockSharp.Algo.Strategies.IStrategyParam.CanOptimize) プロパティを変更する必要があります。

```cs
_long = this.Param(nameof(Long), 80);
_short = this.Param(nameof(Short), 20);

// 最適化対象からパラメーターを除外する
_long.CanOptimize = false;
```

利用可能な最適化パラメーターを変更した後、[最適化パネル](brute_force.md) を再度開きます。
