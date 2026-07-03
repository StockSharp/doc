# 匹配交易品种与连接

同一交易品种在不同交易系统中可能使用不同的名称。您可以将交易品种与用于交易该交易品种的连接进行匹配，并指定该交易品种在外部交易系统中的标识方式。

这样可以整理接收到的数据并简化存储。实际上，来自不同数据源的全部传入数据都会按交易品种名称集中保存到同一位置，而不是按数据源名称分别保存。

当您需要在不同交易板块或通过不同连接（或经纪商）交易同一交易品种时，此功能同样有用。此外，还可以通过一个连接获取数据，再通过另一个连接执行交易。

要匹配交易品种与连接，请执行以下操作：

1. 进入 **Securities** 选项卡，然后单击 **Securities and Connections** 按钮。![Designer Security mapping 01 00](../../../images/designer_security_mapping_01_00.png)
2. 在连接列表中选择所需的连接。![Designer Security mapping 01](../../../images/designer_security_mapping_01.png)
3. 填写所有列。

   例如：

   以 APPLE 股票为例。
   - 连接选择 **Interactive Brokers**。单击 ![Designer Creation tool 00](../../../images/designer_creation_tool_00.png) 按钮，随后将添加一行。
   - 在 **Security code** 和 **Board code** 列中指定交易品种代码和交易板块代码。在 **Security code in adapter** 和 **Board code in adapter** 列中，按照外部交易系统中的定义填写交易品种代码和交易板块代码。单击 **OK**。![Designer Security mapping 01 01](../../../images/designer_security_mapping_01_01.png)
   - 以相同方式为 **Interactive Brokers** 和 **CQG Continuum** 连接重复上述步骤。

   | **Interactive Brokers**                                                           | **CQG Continuum**                                                                 |
   | --------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
   | ![Designer Security mapping 01 02](../../../images/designer_security_mapping_01_02.png) | ![Designer Security mapping 01 03](../../../images/designer_security_mapping_01_03.png) |
4. 现在，所有下载的数据（本例中为 APPLE 股票的数据）都会保存到同一位置。
