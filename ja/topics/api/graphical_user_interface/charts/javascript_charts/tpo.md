# TPO（マーケットプロファイル）

TPO（Time Price Opportunity）チャートは、マーケットプロファイルとも呼ばれ、各時間スロットごとに文字またはブロックを積み上げることで、セッション中に価格が各水準でどれだけの時間取引されたかを示します。これにより、セッションのフェアバリューエリア、ポイント・オブ・コントロール（POC）、そしてシングルプリント（価格がほとんど滞在しなかった箇所）が明らかになります。

## ライブデモ

```chart-demo tpo
```

## セットアップ

`TpoSeries` を追加し、それぞれが `sessionId` を持つ OHLC バーを供給します。シリーズはセッションごとの文字／ブロック分布を自身で構築します。

```js
const series = chart.addSeries(SSChart.TpoSeries, {
  displayMode: SSChart.TpoDisplayMode.Auto,  // 表示モード: Auto | Letters | Blocks
  showPoc: true,
  showValueArea: true,
  showInitialBalance: true,
  showSinglePrints: true,
});

series.setData(tpoBars);  // { time, open, high, low, close, sessionId }

chart.timeScale().fitContent();
```

`displayMode` は文字とソリッドブロックを切り替えます（`Auto` はズームに応じて選択します）。オーバーレイ（ポイント・オブ・コントロール（POC）、バリューエリア、イニシャルバランス、シングルプリント）はそれぞれ独立してオン／オフを切り替えられます。

## 関連項目

- [JavaScript チャート](../javascript_charts.md)
- [ボリュームプロファイル](volume_profile.md)
- [フットプリント](footprint.md)
