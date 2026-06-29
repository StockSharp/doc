# 界面

将策略添加到 **Live** 文件夹后，双击该策略，会打开标题为“Live [策略名称]”的选项卡。切换到此选项卡时，**Ribbon** 中会自动打开 **Live** 选项卡。可以在 **Live** 选项卡中指定策略使用的证券和投资组合。单击 **Start** 按钮可启动策略的实盘交易，单击 **Stop** 按钮可停止交易。

![Designer Interface Live trade 00](../../../images/designer_interface_live_trade_00.png)

策略选项卡包含用于编辑策略图和组件元素的策略设计器，与[策略设计器](../strategies/using_visual_designer/diagram_panel.md)中介绍的界面相似。此外，该选项卡还包含[实盘交易属性](../user_interface/components/live_settings.md)面板。该面板默认折叠并停靠在选项卡右侧。

将策略添加到 **Live** 时，系统会从原始内容复制一份策略；使用[策略图](../strategies/using_visual_designer.md)或[代码](../strategies/using_code.md)时均是如此。因此，在 **Live** 副本中修改算法不会影响原始策略。启动策略时，如果 **Live** 副本与原始策略存在差异，程序会显示警告：

![Designer Interface Live trade 01](../../../images/designer_interface_live_trade_01.png)

- **Yes** 表示将原始策略中的更改应用到 **Live** 副本。
- **No** 表示忽略差异，不应用更改并直接启动 **Live** 副本。
- **Cancel** 表示取消启动。

对 **Live** 副本的修改应尽量少，仅用于测试，并应在之后将这些修改转移到原始策略。否则，当 **Live** 副本更新为原始策略版本时，修改可能会丢失。

## 另请参阅

[连接设置](../connections_settings.md)
