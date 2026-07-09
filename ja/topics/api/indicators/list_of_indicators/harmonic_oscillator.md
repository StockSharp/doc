# HO

**Harmonic Oscillator (HO)** は、調和振動理論に基づくテクニカル指標で、価格変動に含まれる循環成分の特定に役立ちます。

この指標を使用するには、[HarmonicOscillator](xref:StockSharp.Algo.Indicators.HarmonicOscillator) クラスを使用する必要があります。

## 説明

Harmonic Oscillator (HO) は、市場価格変動の周期性と循環的性質を特定するために開発された指標です。多くの価格変動には、分離して将来の価格変動予測に使用できる調和的 (周期的) 成分が含まれるという原理に基づいています。

この指標はスペクトル分析手法を適用して価格系列を調和成分に分解し、支配的なサイクルを強調します。その後、これらの循環成分をオシレーターとして表示し、特定されたサイクル内で価格が局所的な最大値または最小値に到達する可能性のあるタイミングをトレーダーが判断するのに役立ちます。

HO は特に次の用途に役立ちます。
- 市場の循環的性質を判断する
- 潜在的な反転ポイントを特定する
- 市場ノイズをフィルタリングする
- 価格が方向転換する可能性のあるタイミングを予測する

## パラメーター

この指標には次のパラメーターがあります。
- **Length** - 分析期間 (デフォルト値: 30)

## 計算

Harmonic Oscillator の計算には、次の手順が含まれます。

1. 価格系列の前処理 (トレンド除去):
   ```
   Detrended Price = Price - SMA(Price, Length)
   ```

2. 支配的なサイクルを特定するためのスペクトル分析の適用:
   ```
   Spectral Components = FFT(Detrended Price)
   ```
   
3. 最も重要な調和成分の抽出:
   ```
   Dominant Cycles = Extract Top N Spectral Components based on amplitude
   ```

4. 支配的なサイクルに基づく Harmonic Oscillator の合成:
   ```
   HO = Reconstruction of Dominant Cycles through Inverse FFT
   ```

ここで:
- Price - 価格 (通常は終値)
- SMA - 単純移動平均
- FFT - 高速フーリエ変換
- Length - 分析期間

## 解釈

Harmonic Oscillator は次のように解釈できます。

1. **ゼロラインのクロスオーバー**:
   - HO がゼロラインを下から上へ交差する場合、強気シグナルと見なすことができます
   - HO がゼロラインを上から下へ交差する場合、弱気シグナルと見なすことができます

2. **オシレーターの極値**:
   - HO が局所的な最大値に達した場合、潜在的な価格ピークを示す可能性があります
   - HO が局所的な最小値に達した場合、潜在的な価格ボトムを示す可能性があります

3. **ダイバージェンス**:
   - 強気ダイバージェンス: 価格が新安値を形成する一方で、HO はより高い安値を形成します
   - 弱気ダイバージェンス: 価格が新高値を形成する一方で、HO はより低い高値を形成します

4. **サイクル予測**:
   - 規則的な HO のピークと谷は、将来の反転ポイントの予測に使用できます
   - ピーク/谷の間隔を分析すると、支配的なサイクル長の判断に役立ちます

5. **振幅の変化**:
   - HO の振動振幅の増加は、循環成分の強化を示す可能性があります
   - HO の振動振幅の減少は、循環成分の減衰を示す可能性があります

6. **他の指標との組み合わせ**:
   - HO はトレンド指標と組み合わせると最も効果的です
   - トレンド相場では、HO シグナルをトレンド方向のエントリーポイント判断に使用できます

![indicator_harmonic_oscillator](../../../../images/indicator_harmonic_oscillator.png)

## 関連項目

[SineWave](sine_wave.md)
[CenterOfGravityOscillator](center_of_gravity_oscillator.md)
[FisherTransform](ehlers_fisher_transform.md)
[DetrendedSyntheticPrice](detrended_synthetic_price.md)
