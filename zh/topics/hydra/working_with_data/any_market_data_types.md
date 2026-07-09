# 使用其他市场数据类型

[Hydra](../../hydra.md) 允许使用其他数据类型来生成各种市场数据。

当数据源无法直接下载所需市场数据时，可以使用此功能。例如，构建**订单簿**时可以使用多种市场数据类型。

**重要！**如果 **订单日志** 或 **Level 1** 数据中包含最优价格，则可以使用这些数据构建**订单簿**。

需要注意的是，可以从任何提供实时市场数据的数据源下载 **Level 1** 值。也可以通过[转换](../tasks/converter.md)**订单簿**来获得 **Level 1**。

构建步骤如下：

1. 选择要获取市场数据的时间范围和交易品种。![hydra LEVEL 1 build depth data](../../../images/hydra_level1_build_depth_data.png)
2. 打开 **构建来源** 字段并选择所需的数据类型。![hydra type build data](../../../images/hydra_type_build_data.png)

   **重要！**如果选择 **订单簿、订单日志、Level 1** 作为K线的数据源，还会出现其他参数供选择。![hydra ext proper build data](../../../images/hydra_ext_proper_build_data.png)
3. 设置参数后，单击 ![hydra candles](../../../images/hydra_candles.png) 按钮。![hydra LEVEL 1 build depth data result](../../../images/hydra_level1_build_depth_data_result.png)

对于**K线**，还可以使用较小时间周期的K线构建较大时间周期的K线。

例如，如果已有 1 分钟K线，可以在 **构建来源** 中选择相应类型，使用它们构建 5 分钟K线。

**观看[视频教程](../videos/building_order_books.md)**
