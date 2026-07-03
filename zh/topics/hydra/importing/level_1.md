# Level 1

要导入 Level 1 数据，请在应用程序主菜单中选择 **Import \=\> Level 1**。

![hydra import level1](../../../images/hydra_import_level1.png)

## 导入过程

1. **导入设置**

   请参阅[K线](candles.md)导入说明。
2. 配置 [S#](../../api.md) 字段的导入参数。

   请参阅[K线](candles.md)导入说明。

   **下面以从 CSV 文件导入 Level 1 数据为例：**
   - 要导入数据的文件使用以下模板：

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{Changes:{BestBidPrice};{BestBidVolume};{BestAskPrice};{BestAskVolume};{LastTradeTime};{LastTradePrice};{LastTradeVolume}}
     	  				
     ```

     其中，{SecurityId.SecurityCode} 和 {SecurityId.BoardCode} 分别对应 **Security** 和 **Board**。因此，在 **Field order** 字段中分别为其分配序号 0 和 1。
   - 对于 {ServerTime:default:yyyyMMdd} 和 {ServerTime:default:HH:mm:ss.ffffff} 字段，在 **S# field** 窗口中分别选择 **Date** 和 **Time**，并将其序号设为 2 和 3。
   - 对于 {BestBidPrice} 字段，在 **S# field** 窗口中选择 **Best buy price**，并将其序号设为 4。
   - 对于 {BestBidVolume} 字段，在 **S# field** 窗口中选择 **Best buy volume**，并将其序号设为 5。
   - 对于 {BestAskPrice} 字段，在 **S# field** 窗口中选择 **Best sale price**，并将其序号设为 6。
   - 对于 {BestAskVolume} 字段，在 **S# field** 窗口中选择 **Best sale volume**，并将其序号设为 7。
   - 对于 {LastTradeTime} 字段，在 **S# field** 窗口中选择 **Last trade time**，并将其序号设为 8。
   - 对于 {LastTradePrice} 字段，在 **S# field** 窗口中选择 **Last trade price**，并将其序号设为 9。
   - 对于 {LastTradeVolume} 字段，在 **S# field** 窗口中选择 **Last trade volume**，并将其序号设为 10。
   - 字段设置窗口将如下所示：![hydra import prop level 1](../../../images/hydra_import_prop_level1.png)

   用户可以为导入的数据配置大量属性。需要根据导入文件模板指定属性，并为其分配对应的排列序号。
3. 要预览数据，请单击 **Preview** 按钮。![hydra import preview level 1](../../../images/hydra_import_preview_level1.png)
4. 单击 **Import** 按钮。
