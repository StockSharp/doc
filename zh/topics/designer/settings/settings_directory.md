# 设置目录

使用 [Designer](../../designer.md) 时，需要注意以下目录：

1. [Designer](../../designer.md) 的安装目录。可以在此目录中运行 **Designer.exe** 启动 [Designer](../../designer.md)，也可以运行 **Designer.Update.exe** 更新 Designer。删除此目录会删除 Designer 程序，但不会删除 Designer 设置。

2. [Designer](../../designer.md) 设置目录位于用户的文档文件夹中：…\\StockSharp\\Designer\\（例如 c:\\Users\\User\\Documents\\StockSharp\\Designer\\）。删除此目录会将全部 Designer 设置恢复为默认值，**所有已创建的策略、已下载的证券，以及保存在设置目录中的其他信息都将被销毁。**

![Designer Directory and edit the data manually 00](../../../images/designer_directory_and_edit_data_manually_00.png)

该目录包含以下文件夹和文件：

- **Compositions** 文件夹：以 XML 文件形式保存 [Schemas](../user_interface/schemas.md) 面板中 Composite elements 文件夹内的所有模块。从此目录删除文件，会同时从 Schemas 面板的 **Composite elements** 文件夹中删除对应的 **Composite element**。请勿手动编辑此目录中的文件，否则可能导致对应的 **Composite elements** 模块发生故障。
- **LiveStrategies** 文件夹：以 XML 文件形式保存 [Schemas](../user_interface/schemas.md) 面板中 **Trading** 文件夹内的所有模块。从此目录删除文件，会同时从 Schemas 面板的 Trading 文件夹中删除对应策略。请勿手动编辑此目录中的文件，否则可能导致对应策略发生故障。
- **Logs** 文件夹：保存 [Designer](../../designer.md) 的全部日志，可用于简化 Designer 故障排查。
- **SourceCode** 文件夹：以 XML 文件形式保存 [Schemas](../user_interface/schemas.md) 面板中 SourceCode 文件夹内的所有模块。从此目录删除文件，会同时从 **Schemas** 面板的 **SourceCode** 文件夹中删除对应的 **SourceCode** 模块。请勿手动编辑此目录中的文件，否则可能导致对应的 **SourceCode** 模块发生故障。
- **Strategies** 文件夹：以 XML 文件形式保存 [Schemas](../user_interface/schemas.md) 面板中 Strategies 文件夹内的所有模块。从此目录删除文件，会同时从 Schemas 面板的 **Strategies** 文件夹中删除对应策略。请勿手动编辑此目录中的文件，否则可能导致对应策略发生故障。如果手动将策略文件添加到此文件夹并重新启动 Designer，该策略会显示在 **Schemas** 面板的 **Strategies** 文件夹中。
- **Storage** 文件夹 \- 保存 [Designer](../../designer.md) 下载到相应[市场数据存储](../market_data_storage.md)中的市场数据。创建市场数据存储时会同时创建该文件夹，并默认指定其路径。删除此文件夹会删除对应存储中的全部已下载市场数据。如果存储中包含 CSV 格式文件，可以使用标准记事本或 MS Excel 编辑；BIN 文件不能手动编辑。
- **exchange.csv 和 exchangeboard.csv** 文件：包含**证券交易所**列表、证券代码列表和交易模式。这些文件可以使用标准记事本或 MS Excel 编辑。
- **security.csv** 文件：包含所有数据源接收和创建的全部证券。删除此文件会删除 [Designer](../../designer.md) 中的所有证券。添加新证券的方法请参阅[下载证券](../market_data_storage/download_instruments.md)和[创建证券](../market_data_storage/create_instrument.md)。该文件可以使用标准记事本或 MS Excel 编辑。
- **portfolio.csv 和 position.csv** 文件：包含所有接收和创建的投资组合及其当前持仓。删除这些文件会删除 [Designer](../../designer.md) 中对应的全部数据。如果 Designer 每次连接时都会重新接收投资组合信息，持仓信息仍可能永久丢失。这些文件可以使用标准记事本或 MS Excel 编辑。
- **settings.json** 文件：包含当前设置。[Designer](../../designer.md) 会在设置发生更改或程序关闭时创建此文件。删除此文件会将当前设置恢复为默认值。请勿手动编辑该文件，否则可能导致 [Designer](../../designer.md) 发生故障。

如果需要编辑个别文件或重置 Designer 设置，建议先备份相关文件以及整个目录。

## 推荐内容

[更新到新版本](../update_to_the_new_version.md)
