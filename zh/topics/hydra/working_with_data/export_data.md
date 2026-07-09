# 导出数据

[Hydra](../../hydra.md) 可以将接收到的市场数据导出为多种格式，包括 [MetaStock 数据格式](export_data/export_into_metastock.md)。

数据可以导出为 [Excel](https://en.wikipedia.org/wiki/Excel)、xml、bin、txt、Json 文件或 SQL 表。

要导出数据，请从下拉列表中选择所需的文件格式：

![hydra export](../../../images/hydra_export.png)

然后选择目标文件夹，并根据需要修改文件名。

导出为文本文件（txt）时，会出现一个窗口，可以在其中指定如下形式的导出模板：

**{OpenTime:default:yyyyMMdd};{OpenTime:default:HH:mm:ss};{OpenPrice};{HighPrice};{LowPrice};{ClosePrice};{TotalVolume}**

花括号中指定要导出的属性及其顺序，各属性之间使用分号分隔。

单击 **预览** 按钮，可以预览将写入文件的数据。

![hydra export TSLab Meta Stock 1](../../../images/hydra_export_tslab_metastock_1.png)

用户可以通过 **{SecurityId.SecurityCode}** 属性添加交易品种代码等其他属性，也可以指定 Time Frame 值。

还可以添加包含属性名称的标题行。此时记录将如下所示。

![hydra export TSLab Meta Stock 2](../../../images/hydra_export_tslab_metastock_2.png)

如果需要导出包含冒号的格式，应像上面的示例 **{OpenTime:default:HH:mm:ss}** 一样指定 `default` 关键字。

**观看[视频教程](../videos/saving_format.md)**
