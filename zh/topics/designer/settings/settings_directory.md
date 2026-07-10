# 设置目录

使用 [Designer](../../designer.md) 时，需要注意以下目录：

1. [Designer](../../designer.md) 的安装目录。可以在此目录中运行 **Designer.exe** 启动 [Designer](../../designer.md)，也可以运行 **Designer.Update.exe** 更新 [Designer](../../designer.md)。删除此目录会删除 [Designer](../../designer.md) 程序，但不会删除 [Designer](../../designer.md) 设置。

2. **Designer** 设置目录位于用户的文档文件夹中：…\\StockSharp\\Designer\\（例如 c:\\Users\\User\\Documents\\StockSharp\\Designer\\）。删除此目录会将全部 [Designer](../../designer.md) 设置恢复为默认值，**所有已创建的策略、已下载的交易品种，以及保存在设置目录中的其他信息都将被销毁。**

![Designer 目录和手动编辑数据 00](../../../images/designer_directory_and_edit_data_manually_00.png)

该目录包含以下文件夹和文件：

- **Compositions** 以 XML 文件形式保存[策略图面板](../user_interface/schemas.md)中 **复合元素** 文件夹内的所有模块。从此目录删除文件，会从[策略图面板](../user_interface/schemas.md)的 **复合元素** 文件夹中删除对应的 **复合元素**。请勿手动编辑这些文件，否则可能导致对应的 **复合元素** 模块损坏。
- **LiveStrategies** 以 XML 文件形式保存[策略图面板](../user_interface/schemas.md)中 **交易** 文件夹内的所有模块。从此目录删除文件，会从[策略图面板](../user_interface/schemas.md)的 **交易** 文件夹中删除对应策略。请勿手动编辑这些文件，否则可能导致对应策略损坏。
- **Logs** 包含 [Designer](../../designer.md) 的全部崩溃日志，可简化 [Designer](../../designer.md) 故障排查。
- **SourceCode** 以 XML 文件形式保存[策略图面板](../user_interface/schemas.md)中 **源代码** 文件夹内的所有模块。从此目录删除文件，会从[策略图面板](../user_interface/schemas.md)的 **源代码** 文件夹中删除对应的 **源代码** 模块。请勿手动编辑这些文件，否则可能导致对应的 **源代码** 模块损坏。
- **策略** 以 XML 文件形式保存[策略图面板](../user_interface/schemas.md)中 **策略** 文件夹内的所有模块。从此目录删除文件，会从 **策略图面板** 的 **策略** 文件夹中删除对应策略。请勿手动编辑这些文件，否则可能导致对应策略损坏。如果手动将策略文件添加到此文件夹并重新启动 [Designer](../../designer.md)，该策略会显示在[策略图面板](../user_interface/schemas.md)的 **策略** 文件夹中。
- **存储** 包含 [Designer](../../designer.md) 下载到相应[市场数据存储](../market_data_storage.md)的市场数据。创建[市场数据存储](../market_data_storage.md)时会创建该文件夹，默认路径也指向此文件夹。删除此文件夹会删除对应存储中的全部已下载市场数据。如果存储中包含 CSV 文件，可以使用标准文本编辑器或 MS Excel 编辑；BIN 文件不能手动编辑。
- **exchange.csv** 和 **exchangeboard.csv** 包含**交易所**列表、交易品种代码和交易模式。这些文件可以使用标准文本编辑器或 MS Excel 编辑。
- **security.csv** 包含所有来源接收和创建的全部交易品种。删除此文件会删除 [Designer](../../designer.md) 中的所有交易品种。添加新交易品种的方法请参阅[下载交易品种](../market_data_storage/download_instruments.md)和[创建交易品种](../market_data_storage/create_instrument.md)。该文件可以使用标准文本编辑器或 MS Excel 编辑。
- **portfolio.csv** 和 **position.csv** 包含所有接收和创建的投资组合及其当前持仓。删除这些文件会删除 [Designer](../../designer.md) 中对应的全部数据。如果 [Designer](../../designer.md) 在每次连接时都会重新接收投资组合信息，持仓信息仍可能永久丢失。这些文件可以使用标准文本编辑器或 MS Excel 编辑。
- **settings.json** 包含当前设置。[Designer](../../designer.md) 会在设置更改或程序关闭时创建此文件。删除此文件会将当前设置恢复为默认值。请勿手动编辑此文件，否则可能导致 [Designer](../../designer.md) 损坏。

在手动编辑单个文件或重置 [Designer](../../designer.md) 设置之前，请先备份要修改的文件或整个目录。

## 推荐内容

[更新到新版本](../update_to_the_new_version.md)
