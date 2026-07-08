# MetaStock へのエクスポート

MetaStock 形式のファイルへデータをエクスポートするには、ドロップダウンリストから Txt 形式を選択します。

![hydra export](../../../../images/hydra_export.png)

テキスト形式 (Txt) ファイルへエクスポートすると、ウィンドウが表示されます。 

![hydra export Meta Stock 2](../../../../images/hydra_export_tslab_metastock_2.png)

このウィンドウでエクスポートテンプレートを指定します。中括弧は、エクスポートするプロパティとその順序を示します。

```none
{SecurityId.SecurityCode},5,{OpenTime:yyyyMMdd},{OpenTime:HHmmss},{OpenPrice},{HighPrice},{LowPrice},{ClosePrice},{TotalVolume}
	  				
```

この例では、5 分足の時間枠が 2 番目の位置に指定されています。

また、ファイルには先頭行 (Header) を設定する必要があります。 

```none
<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>
	  				
```

