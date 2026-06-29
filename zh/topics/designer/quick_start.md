# 快速入门

首次运行 [Designer](../designer.md) 时，程序会提示打开[下载市场数据](market_data_storage/download_market_data.md)窗口。也可以使用 [Hydra](../hydra.md)（代号 Hydra）下载历史数据。Hydra 用于从不同数据源自动下载市场数据（证券、蜡烛、逐笔成交、订单簿等），并将其保存到本地存储。有关历史数据下载和存储的详细说明，请参阅[市场数据存储](market_data_storage.md)。

单击 **Download securities** 按钮后，会显示[下载证券](market_data_storage/download_instruments.md)窗口。要下载证券，请输入证券代码和证券类型，选择数据源，然后单击 **OK**。[Designer](../designer.md) 会向数据源查询可用证券，找到的所有证券都会显示在 **All securities** 面板中。[Designer](../designer.md) 默认已指定一个数据源，也可以使用交易终端作为数据源。有关配置终端连接的方法，请参阅[连接设置](connections_settings.md)。

![Designer Quick start 01](../../images/designer_quick_start_01.png)

要获取证券的历史数据，请从 **All securities** 列表中选择所需证券，设置历史数据时间段，并选择数据类型；如果下载蜡烛，还需选择 Time Frame。然后单击 **Start** 按钮。所有数据都会保存到[市场数据存储](market_data_storage.md)中。

![Designer Quick start 02](../../images/designer_quick_start_02.png)

获取历史数据后，选择一个演示策略。在 **Strategy** 文件夹的 [Schemas](user_interface/schemas.md) 面板中双击 **SMA** 示例策略，工作区随后会显示 Sma 选项卡。切换到该策略后，工具栏中的 **Emulation** 选项卡会自动打开，其中包含创建、调试和测试策略所需的主要功能（参阅[策略](strategies/using_visual_designer.md)和[入门](backtesting/getting_started.md)）。

![Designer Quick start 03](../../images/designer_quick_start_03.png)

在 **Emulation** 选项卡中设置测试时间段，并在 **Market Data** 字段中选择[市场数据存储](market_data_storage.md)。

单击 **Security** 字段中的 ![Designer Quick start 04](../../images/designer_quick_start_04.png) 图标，会打开 **Select security** 窗口。请在该窗口中选择所需证券。

![Designer Quick start 05](../../images/designer_quick_start_05.png)

在 **Designer** 面板中选择任意模块后，该模块的属性会显示在 **Properties** 面板中。在 **Candles** 模块的 **Properties** 面板中，可以设置[蜡烛](../api/candles.md)的类型和 Time Frame。

单击 **Start** 按钮后，交易仿真开始运行。

![Designer Quick start 06](../../images/designer_quick_start_06.png)
