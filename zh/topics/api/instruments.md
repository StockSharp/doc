# 交易品种

在 StockSharp 中，金融工具由 [Security](xref:StockSharp.BusinessEntities.Security) 类表示，这是处理交易数据的基本元素。本节涵盖了在平台内处理金融工具的主要方面。

## 基础安全类

[Security](xref:StockSharp.BusinessEntities.Security) 表示在交易所交易的金融工具。金融工具可以是股票、期货合约、期权、货币对、加密货币及其他资产。该类包含识别和交易该金融工具所需的所有信息：

- **识别信息** - 代码、ISIN、名称、工具类别
- **交易参数** - 价格步长、手数、最小交易量
- **市场数据** - 当前的价格、交易量、订单簿等数值。
- **计算值** - 衍生品、风险计算等的参数。

## 交易品种类型

StockSharp 支持处理所有主要类型的金融工具：

- **股票** - 股权类交易品种
- **债券** - 债务交易品种
- **期货** - 基于标的资产的衍生合约
- **期权** - 赋予买入或卖出标的资产权利（但没有义务）的合约
- **货币对** - 外汇市场上的交易品种
- **加密货币** - 用于在加密交易所交易的数字资产
- **ETF** - 交易型开放式指数基金
- **指数** - 市场或行业状况的计算指标

## 器械篮

除了常规交易品种外，StockSharp 还实现了用于处理交易品种组合的特殊类：

- [指数交易品种](xref:StockSharp.Algo.IndexSecurity) - 一种基于基础工具的指数的工具
- [加权指数交易品种](xref:StockSharp.Algo.WeightedIndexSecurity) - 每个工具都有权重系数的指数
- [ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity) - 用于操作一系列期货合约的连续工具

这些类允许您创建复合工具，并以与常规工具相同的方式使用它们，包括接收汇总的市场数据、计算统计数据以及执行交易操作。

## 处理交易品种信息

StockSharp 提供了用于处理金融工具信息的强大工具：

- **工具搜索** - 按各种条件（代码、名称、类别）
- **筛选** - 根据指定参数选择交易品种
- **存储** - 将交易品种信息保存到本地或远程存储
- **正在获取交易所信息** - 正在从交易所加载详细信息

## 交易品种识别

StockSharp 中的每个工具都有一个唯一标识符 [SecurityId](xref:StockSharp.Messages.SecurityId)，用于在系统中明确识别该工具。该标识符包括：

- **SecurityCode** - 该工具的交易代码
- **BoardCode** - 交易场所代码
- **彭博/路透/ISIN** 及其他代码 - 替代识别方法

## 特殊功能

- **连续期货** - 对一系列期货合约的历史数据进行自动“拼接”
- **复合交易品种** - 基于多种真实交易品种创建虚拟交易品种
- **特殊标识符\*@ALL**——用于处理某一类交易品种的全部交易品种

## 另请参阅

[交易品种标识符](instruments/instrument_identifier.md)

[ 标识符 *@ALL ](instruments/identifier_@all.md)

[ 连续期货 ](instruments/continuous_futures.md)

[ 指数 ](instruments/index.md)

[ 交易品种搜索 ](instruments/instrument_search.md)
