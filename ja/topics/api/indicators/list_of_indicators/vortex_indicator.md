# VI

**Vortex インジケーター (VI)** は、2009年に Etienne Boisse と Julia Boisse によって開発されたテクニカルインジケーターです。このインジケーターは VI+ と VI- の2本のラインで構成され、上方向および下方向の価格変動を示し、新しいトレンドの始まりを特定し、既存のトレンドを確認するのに役立ちます。

このインジケーターを使用するには、[VortexIndicator](xref:StockSharp.Algo.Indicators.VortexIndicator) クラスを使用する必要があります。

## 説明

Vortex インジケーターは自然界における渦の動きの原理に着想を得ており、市場の動きの周期的な性質を反映することを目的としています。これは2本のラインで構成されます。

- **VI+** (正の Vortex インジケーター) - 上方向の価格変動を測定します
- **VI-** (負の Vortex インジケーター) - 下方向の価格変動を測定します

主なインジケーターシグナル:
- VI+ が VI- を下から上へクロスしたときに買い
- VI- が VI+ を下から上へクロスしたときに売り
- ライン間の乖離度はトレンドの強さを示します

Vortex インジケーターは、特に次の用途に有用です。
- 新しいトレンドの開始を判断する
- 既存トレンドの強さを評価する
- 潜在的な反転ポイントを特定する

## パラメーター

- **Length** - 計算期間。通常は 14 の値を使用します。

## 計算

Vortex インジケーターの計算は、いくつかの手順で実行されます。

1. 正の変動と負の変動を計算します。
   ```
   VM+ = |Current High - Previous Low|
   VM- = |Current Low - Previous High|
   ```

2. 真の値幅を計算します。
   ```
   TR = Max(High - Low, |High - Previous Close|, |Low - Previous Close|)
   ```

3. Length 期間にわたって VM+ と VM- の値を合計します。
   ```
   Sum_VM+ = Sum(VM+, Length)
   Sum_VM- = Sum(VM-, Length)
   ```

4. Length 期間にわたって真の値幅を合計します。
   ```
   Sum_TR = Sum(TR, Length)
   ```

5. 正規化された VI+ と VI- の値を計算します。
   ```
   VI+ = Sum_VM+ / Sum_TR
   VI- = Sum_VM- / Sum_TR
   ```

これら2本のラインのクロスによって売買シグナルが生成されます。VI+ が VI- を上回ると強気トレンドを示し、反対に VI- が VI+ を上回ると弱気トレンドを示します。

![IndicatorVortexIndicator](../../../../images/indicator_vortex_indicator.png)

## 関連項目

[ADX](adx.md)
[DMI](dmi.md)
