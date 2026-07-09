# 查看和导出

可以在专用面板中查看 [Hydra](../../hydra.md) 接收到的数据。

为此，请在 **常规** 选项卡中单击以下任一按钮：[逐笔成交](view_and_export/ticks.md)、[订单簿](view_and_export/order_books.md)、[生成K线](candles_generation.md)、[订单日志](view_and_export/order_log.md)、[Level 1](view_and_export/level_1_.md)、[新闻](view_and_export/news.md)、[交易事务](view_and_export/transactions.md)、[期权面板](view_and_export/option_desk.md)、[指标](view_and_export/indicators.md)、[持仓](view_and_export/positions.md)。

也可以按图中所示右键单击所需数据类型，或者直接双击该数据类型。

![hydra view export](../../../images/hydra_view_export.png)

每个面板都包含以下通用设置界面：

![hydra export 00](../../../images/hydra_export_00.png)

- 顶部一行显示市场数据存储及其格式（BIN 或 CSV）。
- 底部一行用于设置请求数据的时间范围。单击 **选择交易品种** 按钮后，会出现交易品种选择窗口，可以选择一个或多个交易品种。如果选择多个交易品种，之后导出到 Excel 或 CSV 时，程序会自动将不同交易品种的数据分别保存到不同文件。
- 如果构建数据表时下载的数据量超过设定限制，屏幕上会出现以下窗口：![hydra tick limit](../../../images/hydra_tick_limit.png)

  此时需要提高下载数据量限制。
- 如果数据来自时区与当前时区不同的数据源，可以调整时区。构建完成后，数据会按用户选择的时区显示。![hydra TZ](../../../images/hydra_tz.png)
- 部分数据源无法提供某些数据，因此程序提供了[构建来源](any_market_data_types.md)字段。借助该字段，用户可以使用另一种市场数据来构建所需市场数据。也可以在不额外下载数据的情况下，以现有数据为基础构建市场数据。
- 设置上述参数后，单击 ![hydra find](../../../images/hydra_find.png) 按钮。![hydra candles tf](../../../images/hydra_candles_tf.png)

通过上下文菜单，可以配置市场数据值表格的各种参数，例如行分组、可用列和显示格式等。

![hydra export context](../../../images/hydra_export_context.png)
