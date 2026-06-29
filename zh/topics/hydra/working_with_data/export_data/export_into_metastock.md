# 导出为 MetaStock 格式

要将数据导出为 MetaStock 格式文件，请从下拉列表中选择 Txt 格式：

![hydra export](../../../../images/hydra_export.png)

导出为文本格式（Txt）文件时，会出现以下窗口：

![hydra export Meta Stock 2](../../../../images/hydra_export_tslab_metastock_2.png)

在该窗口中指定导出模板。花括号用于表示要导出的属性及其顺序：

```none
{SecurityId.SecurityCode},5,{OpenTime:yyyyMMdd},{OpenTime:HHmmss},{OpenPrice},{HighPrice},{LowPrice},{ClosePrice},{TotalVolume}
	  				
```

在此示例中，第二个位置的值表示 5 分钟蜡烛图时间周期。

还需要在文件第一行设置标题（Header）：

```none
<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>
	  				
```
