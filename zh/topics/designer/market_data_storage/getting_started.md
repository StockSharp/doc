# 入门

要创建历史数据存储，请在 **Market data** 选项卡中单击 ![Designer Creating a repository of historical data 00](../../../images/designer_creating_repository_of_historical_data_00.png) 按钮。单击 ![Designer Creating a repository of historical data 01](../../../images/designer_creating_repository_of_historical_data_01.png) 按钮可以修改当前存储的参数。单击 ![Designer Creating a repository of historical data 02](../../../images/designer_creating_repository_of_historical_data_02.png) 按钮可以从存储列表中删除当前存储。

![Designer Creating a repository of historical data 03](../../../images/designer_creating_repository_of_historical_data_03.png)

历史数据存储可以是本地存储，也可以是远程存储。

本地存储是指所有数据都保存在本地计算机上。配置本地存储时，只需指定存储数据的文件夹路径。

远程存储可以位于远程计算机上。配置远程存储时，需要指定远程存储地址，并在必要时填写登录名和密码。

可以使用 [Hydra](../../hydra.md) 软件（代号 Hydra）在本地计算机上建立远程存储服务。Hydra 用于从不同数据源自动加载市场数据（交易品种、K线、逐笔成交、订单簿等），并将其保存到本地存储。为此，请将 [Hydra](../../hydra.md) 切换到服务器模式。

![Designer Creating a repository of historical data 04](../../../images/designer_creating_repository_of_historical_data_04.png)

然后在 [Designer](../../designer.md) 中单击 ![Designer Creating a repository of historical data 00](../../../images/designer_creating_repository_of_historical_data_00.png) 按钮，创建新存储。在存储设置的地址字段中输入 "net.tcp:\/\/localhost:8000"，然后单击 OK。使用 [Hydra](../../hydra.md) 作为远程存储时，请确保 [Hydra](../../hydra.md) 已启动并完成相应配置。

![Designer Creating a repository of historical data 05](../../../images/designer_creating_repository_of_historical_data_05.png)

添加新存储后，可以在 **Storage** 下拉列表中选择该存储。

![Designer Creating a repository of historical data 06](../../../images/designer_creating_repository_of_historical_data_06.png)

还需要选择存储文件格式：BIN 或 CSV。数据可以采用两种格式保存：专用二进制 BIN 格式可提供最高压缩率；文本 CSV 格式便于在其他程序中分析数据。需要节省磁盘空间时，建议使用 BIN 格式；需要手动修改数据时，建议使用 CSV 格式。CSV 文件可以使用标准记事本、MS Excel 等工具轻松编辑。

## 推荐内容

[下载交易品种](download_instruments.md)
