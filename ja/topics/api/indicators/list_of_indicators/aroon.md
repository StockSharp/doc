# Aroon

**Aroon Indicator** は、トレンド変化とトレンドの強さを特定するために Tushar Chande が 1995 年に開発したテクニカルインジケーターです。"Aroon" という名前は、「新時代の夜明け」を意味するサンスクリット語に由来します。

このインジケーターを使用するには、[Aroon](xref:StockSharp.Algo.Indicators.Aroon) クラスを使用する必要があります。

## 説明

Aroon Indicator は 2 本のラインで構成されます。
- **Aroon Up** - 上昇トレンドの強さを測定します
- **Aroon Down** - 下降トレンドの強さを測定します

Aroon は次の判断に役立ちます。
- 新しいトレンドの始まり
- 現在のトレンドの強さ
- 保ち合いと横ばいの動き
- 潜在的なトレンド反転

このインジケーターは、新しいトレンド形成の初期段階を特定し、保ち合い期間を判定するうえで特に有用です。

## パラメーター

このインジケーターには次のパラメーターがあります。
- **Length** - 計算期間（通常は 14-25 期間が使用されます）

## 計算

Aroon Indicator の計算は、指定期間内で最高価格と最低価格に到達してから経過した時間（期間数）を判定することに基づきます。

1. Aroon Up は次の式で計算されます。
   ```
   Aroon Up = ((Length - Periods since high) / Length) * 100
   ```

2. Aroon Down は次の式で計算されます。
   ```
   Aroon Down = ((Length - Periods since low) / Length) * 100
   ```

ここで:
- Length - 選択した期間
- "Periods since high" - Length 期間内で最高価格に到達してからの期間数
- "Periods since low" - Length 期間内で最低価格に到達してからの期間数

Aroon の両ラインは 0 から 100 の間で振動します。
- 値が 100 の場合、高値/安値が直近の期間で到達されたことを意味します
- 値が 0 の場合、高値/安値が Length 期間前に到達されたことを意味します

## 解釈

- **強い上昇トレンド**: Aroon Up が 100 に近く、Aroon Down が 0 に近い
- **強い下降トレンド**: Aroon Down が 100 に近く、Aroon Up が 0 に近い
- **横ばいの動き**: 両方のラインが低い水準で互いに平行に動く
- **潜在的なトレンド反転**: Aroon Up ラインと Aroon Down ラインの交差
- **保ち合い**: 両方のラインが 50 付近で振動する

![indicator_aroon](../../../../images/indicator_aroon.png)

## 関連項目

[ADX](adx.md)
[DMI](dmi.md)
