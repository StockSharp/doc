# 订单簿

要导入订单簿，请在应用程序主菜单中选择 **Import \=\> Order books**。

![hydra import depths](../../../images/hydra_import_depths.png)

## 导入过程

1. **导入设置**

   请参阅[K线](candles.md)导入说明。
2. 配置 [S#](../../api.md) 字段的导入参数。

   请参阅[K线](candles.md)导入说明。

   **下面以从 CSV 文件导入订单簿为例：**
   - 要导入数据的文件使用以下模板：

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{Quote.Price};{Quote.Volume};{Side}
     	  				
     ```

     其中，{SecurityId.SecurityCode} 和 {SecurityId.BoardCode} 分别对应 **Security** 和 **Board**。因此，在 **Field order** 字段中分别为其分配序号 0 和 1。
   - 对于 {ServerTime:default:yyyyMMdd} 和 {ServerTime:default:HH:mm:ss.ffffff} 字段，在 **S# field** 窗口中分别选择 **Date** 和 **Time**，并将其序号设为 2 和 3。
   - 对于 {Quote.Price} 字段，在 **S# field** 窗口中选择表示报价价格的 **Price**，并将其序号设为 4。
   - 对于 {Quote.Volume} 字段，在 **S# field** 窗口中选择表示报价数量的 **Volume**，并将其序号设为 5。
   - 对于 {Side} 字段，在 **S# field** 窗口中选择表示交易方向（买入或卖出）的 **Direction**，并将其序号设为 6。
   - 字段设置窗口将如下所示：![hydra import prop depth](../../../images/hydra_import_prop_depth.png)

   用户可以为导入的数据配置大量属性。需要根据导入文件模板指定属性，并为其分配对应的排列序号。
3. 要预览数据，请单击 **预览** 按钮。![hydra import preview depth](../../../images/hydra_import_preview_depth.png)
4. 单击 **Import** 按钮。
