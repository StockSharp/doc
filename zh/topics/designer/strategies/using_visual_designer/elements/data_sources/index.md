# 指数

![Designer Index 00](../../../../../../images/designer_index_00.png)

该模块用于创建自定义指数。

### 输出端口

输出端口

- **Security** \- 计算得到的指数，以 **Security** 表示。

### 参数

参数

- **Index** \- 由多个交易品种组合而成的数学公式（例如 (AAPL@NASDAQ+10)\*(abs(20\/GOOG@NYSE))）。
- **Ignore errors** \- 设置此标志后，计算指数时会忽略错误。
- **Calculate extended information** \- 设置此标志后，计算指数时除了基本信息（总成交量、开盘价、收盘价、最高价和最低价）外，还会计算扩展信息（总成交额、开盘成交量、收盘成交量、最大成交量和最小成交量）。

可用的数学公式与 [公式](../common/formula.md) 模块相同。

## 推荐内容

[变量](variable.md)
