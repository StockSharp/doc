# 逐笔成交

要导入成交数据，请打开 **Import \=\> Ticks** 选项卡。

![hydra import trades](../../../images/hydra_import_trades.png)

## 导入过程

1. **导入设置**

   请参阅[蜡烛图](candles.md)导入说明。
2. 配置 [S\#](../../api.md) 字段的导入参数。

   请参阅[蜡烛图](candles.md)导入说明。

   **下面以从 CSV 文件导入成交（逐笔成交）为例：**
   - 要导入数据的文件使用以下模板：

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{TradeId};{TradePrice};{TradeVolume};{OriginSide}
     	  				
     ```

     其中，{SecurityId.SecurityCode} 和 {SecurityId.BoardCode} 分别对应 **Security** 和 **Board**。因此，在 **Field order** 字段中分别为其分配序号 0 和 1。
   - 对于 {ServerTime:default:yyyyMMdd} 和 {ServerTime:default:HH:mm:ss.ffffff} 字段，在 **S\# field** 窗口中分别选择 **Date** 和 **Time**，并将其序号设为 2 和 3。
   - 对于 {TradeId} 字段，在 **S\# field** 窗口中选择表示成交标识符或成交编号的 **Identifier**，并将其序号设为 4。
   - 对于 {TradePrice} 字段，在 **S\# field** 窗口中选择表示成交价格的 **Price**，并将其序号设为 5。
   - 对于 {TradeVolume} 字段，在 **S\# field** 窗口中选择表示成交量的 **Volume**，并将其序号设为 6。
   - 对于 {OriginSide} 字段，在 **S\# field** 窗口中选择表示成交发起方（卖方或买方）的 **Initiator**，并将其序号设为 7。
   - 字段设置窗口将如下所示：![hydra import prop trade](../../../images/hydra_import_prop_trade.png)

   用户可以为导入的数据配置大量属性。需要根据导入文件模板指定属性，并为其分配对应的排列序号。
3. 要预览数据，请单击 **Preview** 按钮。![hydra import preview trade](../../../images/hydra_import_preview_trade.png)
4. 单击 **Import** 按钮。
