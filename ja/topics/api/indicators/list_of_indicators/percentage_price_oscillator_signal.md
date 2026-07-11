# PPO シグナル

**PPO シグナル (PPOS)** インジケーターは、取引をフィルタリングするために通常使用されるシグナルラインを追加することで、標準の PPO を拡張します。

このインジケーターを使用するには、[PercentagePriceOscillatorSignal](xref:StockSharp.Algo.Indicators.PercentagePriceOscillatorSignal) クラスを使用します。

## 説明

価格パーセントオシレーター (PPO) は、2 つの指数移動平均（EMA）のパーセンテージ差を測定します。シグナル版は、PPO ラインを追加の EMA で平滑化することに重点を置き、トレーダーがより持続的なモメンタムの変化にのみ反応できるようにします。

このインジケーターは、次の構成要素で形成されます。

1. **PPO ライン** - 高速 EMA と低速 EMA のパーセンテージ差。
2. **シグナルライン** - PPO ラインから計算される EMA（既定では 9 期間）。

PPO ラインがシグナルラインを上抜けると、強気モメンタムが増加していることを示唆します。下抜けると、弱気モメンタムが強まっていることを示します。シグナルラインの上または下に留まることは、優勢なトレンドの強さを確認できます。

## 計算

1. 選択した価格系列の高速 EMA と低速 EMA を計算します。
2. 高速 EMA と低速 EMA のパーセンテージ距離として PPO ラインを計算します。
3. PPO ラインを EMA で平滑化し、シグナルラインを取得します。

```
FastEMA = EMA(Price, ShortPeriod)
SlowEMA = EMA(Price, LongPeriod)
PPO = ((FastEMA - SlowEMA) / SlowEMA) * 100
Signal = EMA(PPO, SignalPeriod)
```

## 解釈

- **シグナルのクロスオーバー。** PPO ラインが下からシグナルラインを上抜けると、強気シグナルが発生します。反対方向のクロスオーバーは、弱気モメンタムを示します。
- **トレンド確認。** シグナルラインを上回って推移することは上昇トレンドを確認し、下回って推移することは下降トレンドを支持します。
- **ダイバージェンス。** PPO ラインがシグナルラインと相互作用している間の、価格動向と PPO ラインのダイバージェンスは、反転を予測する場合があります。

![PPO シグナル](../../../../images/indicator_percentage_price_oscillator_signal.png)

## 関連項目

- [PPO](percentage_price_oscillator.md)
- [PPO ヒストグラム](percentage_price_oscillator_histogram.md)
