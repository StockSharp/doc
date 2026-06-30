# 生成蜡烛图

[Hydra](../../hydra.md) 可以根据已下载的成交数据生成多种类型的蜡烛图，随后可将其导出为 [Excel](https://en.wikipedia.org/wiki/Excel)、XML、SQL、BIN、JSON 或 TXT 格式。

因此，生成的数据可以在 WealthLab、AmiBroker 等任意技术分析程序中使用。

## 蜡烛图生成过程

1. 在 **General** 选项卡中单击 **Candles** 按钮，打开以下窗口：

   ![hydra candles main](../../../images/hydra_candles_main.png)

2. 在打开的窗口中配置蜡烛图生成参数：

   - 从下拉列表中选择所需的蜡烛图类型，支持所有[标准蜡烛图类型](../../api/candles.md)。
   - 指定所选蜡烛图类型所需的参数：
     - 对于 [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage)，选择 **Timeframe**。
     - 对于 [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage)，指定 **Volume**。
     - 对于 [TickCandleMessage](xref:StockSharp.Messages.TickCandleMessage)，指定 **Number of ticks**。
     - 对于 [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage)，指定 **Range**。
     - 对于 [RenkoCandleMessage](xref:StockSharp.Messages.RenkoCandleMessage)，指定 **Block size**。
     - 对于 [PnFCandleMessage](xref:StockSharp.Messages.PnFCandleMessage)，指定 **P&F Parameters**。
   - 选择要为其生成蜡烛图的交易品种。
   - 根据需要指定时间范围。
   - 单击 ![hydra find](../../../images/hydra_find.png) 按钮开始生成。

### 生成时间周期蜡烛图的示例

要为 AAPL@NASDAQ 交易品种生成 5 分钟蜡烛图：

1. 选择蜡烛图类型 [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage)。
2. 将 **Timeframe** 设置为 5 分钟。
3. 选择 AAPL@NASDAQ 交易品种。
4. 单击搜索按钮。

数据生成后会显示以下结果：

![hydra candles tf](../../../images/hydra_candles_tf.png)

### 生成成交量蜡烛图的示例

要生成成交量蜡烛图：

1. 选择蜡烛图类型 [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage)。
2. 指定成交量，例如 100。
3. 选择交易品种。
4. 在 **Build from** 字段中选择 **Ticks**。
5. 单击搜索按钮。

生成结果：

![hydra candles volume](../../../images/hydra_candles_volume.png)

## 用于构建蜡烛图的数据源

如果无法直接从数据源获取市场数据，可以在 [**Build from**](any_market_data_types.md) 字段中选择用于构建蜡烛图的数据类型：

- **Ticks** — 使用逐笔成交数据构建蜡烛图。
- **Order Books** — 使用订单簿数据构建蜡烛图。
- **Level1** — 使用 Level1 数据构建蜡烛图。
- **Smaller Timeframe** — 使用较小时间周期的蜡烛图构建较大时间周期的蜡烛图。

### 不同构建方式的示例

- 使用逐笔成交构建 10 分钟蜡烛图：

  ![hydra candles tf 10](../../../images/hydra_candles_tf_10.png)

- 使用 5 分钟蜡烛图构建 30 分钟蜡烛图：

  ![hydra candles tf 01](../../../images/hydra_candles_tf_01.png)

> [!TIP]
> 如果在 **Build from** 字段中选择 **don't build**，程序只会搜索直接通过数据源下载的现成蜡烛图。

## 显示生成的蜡烛图

要以图形方式显示生成的蜡烛图：

1. 单击 ![hydra candles](../../../images/hydra_candles.png) 按钮。
2. 程序会打开包含已构建蜡烛图的图表：

   ![hydra candles tf chart](../../../images/hydra_candles_tf_chart.png)

   ![hydra candles volume chart](../../../images/hydra_candles_volume_chart.png)

## 向图表添加指标

可以向蜡烛图图表添加技术指标：

1. 右键单击图表面板，打开上下文菜单。
2. 选择 **Indicator**，然后从列表中选择所需指标。
3. 要在单独的面板中显示指标：
   - 使用 ![hydra add](../../../images/hydra_add.png) 按钮添加新面板。
   - 从上下文菜单中选择所需指标。

添加指标后的图表示例：

![hydra candles ind chart](../../../images/hydra_candles_ind_chart.png)

## 导出数据

可以将生成的蜡烛图值[导出为各种格式](export_data.md)，以便在其他程序中使用。

**另请观看有关构建各种蜡烛图的[视频教程](../videos/building_candles.md)**
