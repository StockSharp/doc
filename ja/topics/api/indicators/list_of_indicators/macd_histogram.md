# MACD ヒストグラム

**移動平均収束拡散 (MACD)** は、証券価格の 2 つの移動平均の関係をヒストグラムとして表示するモメンタム指標です。

この指標を使用するには、[MovingAverageConvergenceDivergenceHistogram](xref:StockSharp.Algo.Indicators.MovingAverageConvergenceDivergenceHistogram) クラスを使用する必要があります。

この指標の計算には、期間の異なる 3 つの指数移動平均が使用されます。短い期間の高速移動平均 (EMA_s) から、長い期間の低速移動平均 (EMA_l) を差し引きます。得られた値から MACD ラインが構築されます。

MACD = EMA_s(P) - EMA_l(P)

デフォルトの期間は 12 と 26 です。その後、このラインは 3 つ目の指数移動平均 (EMA_a) により、通常は期間 9 で平滑化され、いわゆる MACD シグナルライン (Signal) が得られます。

Signal = EMA_a(EMA_s(P) - EMA_l(P))

これら 2 つの結果の曲線が、通常の線形 MACD を表します。また、指標ウィンドウには通常、曲線がその周辺で変動するゼロラインも表示されます。

MACD ヒストグラムを構築する際、ヒストグラムのバーはシグナルラインと MACD ラインの差を示し、指標の把握をさらに簡単にします。

![IndicatorMovingAverageConvergenceDivergenceHistogram](../../../../images/indicatormovingaverageconvergencedivergencehistogram.png)

## 関連項目

[シグナルライン付き MACD](macd_with_signal_line.md)
