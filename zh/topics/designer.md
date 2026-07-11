# Designer

**Designer** 是一款用于创建交易策略、使用历史数据测试策略以及管理实盘交易策略的程序。Designer 提供以下策略创建方式：

1. [使用可视化设计器](designer/strategies/using_visual_designer.md)：无需编程技能。通过组合模块并使用连接线将其连接起来即可创建策略，整个策略构建过程都会以可视化方式呈现。
2. [使用代码](designer/strategies/using_code.md)：适合偏好代码开发的有经验程序员。使用 **C#**、**F#** 或 **Python** 编写的策略运行速度远高于使用可视化设计器创建的策略。与功能受模块限制的可视化方案不同，代码策略在创建时没有此类限制，可以实现任意算法。策略既可以直接在 **Designer** 中开发，也可以在 C\# 开发环境中编写（其中最常用的是 **Microsoft Visual Studio**），并使用面向专业交易算法开发的 C\# 程序库和 [API](api.md)。

![Designer 截图](../images/stocksharptitle_0.png)

## 推荐内容

[安装 Designer](designer/installing_designer.md)
