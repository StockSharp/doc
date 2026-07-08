# HurstExponent

**Hurst Exponent** は、時系列がトレンドを形成する傾向、または平均へ回帰する傾向を評価するために使用される統計的尺度です。

この指標を使用するには、[HurstExponent](xref:StockSharp.Algo.Indicators.HurstExponent) クラスを使用する必要があります。

## 説明

ハースト指数 (H) は 0 から 1 の範囲を取り、系列の長期記憶を表します。
- **H > 0.5** は、持続的なトレンド形成の挙動を示します。
- **H < 0.5** は、平均回帰への傾向を示します。
- **H ≈ 0.5** は、ランダムウォークに対応します。

これは市場効率性の推定や、発生しつつあるサイクルまたはトレンドの発見に役立ちます。

## パラメーター

- **Length** - 計算に使用するバーの数。

## 計算

一般的な方法は、再スケール範囲 (R/S) アプローチに基づきます。
1. `Length` ポイントの各ウィンドウについて、平均からの累積偏差を計算します。
2. 最大累積偏差と最小累積偏差の差として範囲 `R` を求めます。
3. ウィンドウの標準偏差 `S` を計算します。
4. 再スケール範囲 `R/S` を評価します。
5. `log(Length)` に対する `log(R/S)` の傾きとして `H` を推定します。

指数の値が高いほど、より強いトレンド形成の挙動を示し、値が低いほど、より大きな平均回帰を示唆します。

![indicator_hurst_exponent](../../../../images/indicator_hurst_exponent.png)

## 関連項目

[Fractal Adaptive Moving Average](fractal_adaptive_moving_average.md)
[Market Meanness Index](market_meanness_index.md)
