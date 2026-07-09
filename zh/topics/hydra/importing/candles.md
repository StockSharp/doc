# K线

要导入K线，请在应用程序主菜单中选择 **Import \=\> Candles**。

![hydra import candles](../../../images/hydra_import_candles.png)

## K线导入过程

1. **常规**
   - **Data type** — 导入的数据类型。
   - **Filename** — CSV 文件的完整路径。
   - **Data directory** — 保存最终 [S#](../../api.md) 文件的文件夹。
   - **File mask** — 扫描目录时使用的文件掩码，例如 `candle_*.csv`。
   - **Column separator** — 列分隔符。制表符使用 TAB 表示。
   - **Indent from the beginning** — 从文件开头跳过的行数，用于忽略包含元信息的行。
   - **Time zone** — 时区。
   - **Interval** — 数据更新频率。

   **Instruments**
   - **Extended information** — 将导入的扩展字段保存到扩展信息存储中。
   - **Duplicates** — 如果重复的交易品种已存在，是否对其进行更新。
2. 配置 [S#](../../api.md) 字段的导入参数。
   - **S# field** — S# 字段的值，例如 **Security、Board** 等。
   - **Associations** — 根据需要，将文件中的列值映射到 StockSharp 类型。
   - **Format** — 数据格式，通常用于导入日期和时间值，详见[逐笔成交](ticks.md)。
   - **Use** — 导入时是否使用该数据。
   - **Field order** — 导入对象的属性列排列顺序。

     例如，如果导入文件使用以下模板：

     ```none
     {SecurityId.SecurityCode},{SecurityId.BoardCode},{OpenTime:yyyyMMdd},{OpenTime:default:HH:mm:ss},{OpenPrice},{HighPrice},{LowPrice},{ClosePrice},{TotalVolume}
     	  				
     ```

     则应使用以下设置：![hydra import prop candles](../../../images/hydra_import_prop_candles.png)

     其中：

     **Security** 值对应序号为 **0** 的 **{SecurityId.SecurityCode}**。

     > [!TIP]
     > 在编程中，第一个元素的序号始终为 0。

     **Board** 值对应序号为 **1** 的 **{SecurityId.BoardCode}**，其余字段依此类推。
   - **By default** — 字段的默认值。例如，在导入成交、订单簿等数据时，如果数据文件中没有相应信息，可以为重复字段值（Security 或 **Board**）设置默认值，详见[逐笔成交](ticks.md)。
   - **Zero** — 某些数据在保存时可能会错误地将属性值保存为“0”。例如，由于各种原因，价格可能等于 0，但这是无效值，之后会导致数据读取错误，进而使使用这些数据的策略产生错误结果。勾选此项后，如果本字段的数据等于 0，程序会将其写为空值，即视为不存在。之后在测试等操作中，用户会看到缺少数据的错误，从而发现导入过程有误。此功能用于防止“损坏”数据影响后续工作。

   用户可以为导入的数据配置大量属性。需要根据导入文件模板指定属性，并为其分配对应的排列序号。
3. 要预览数据，请单击 **预览** 按钮。![hydra import preview candles](../../../images/hydra_import_preview_candles.png)
4. 单击 **Import** 按钮。
