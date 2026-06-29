# 修改订单

![Designer Moving applications 00](../../../../../../images/designer_moving_applications_00.png)

该模块用于修改证券的订单。

### 输入端口

输入端口

- **Trigger** - 用于确定何时移动订单的信号。
- **Order** - 要修改的订单。
- **Price** - 新价格的数值。
- **Volume** - 新数量的数值。

### 输出端口

输出端口

- **Order** - 修改后的订单。可以使用 **Transactions by Order** 元素获取该订单的成交，也可以使用 **Chart Panel** 模块将其显示在图表上。
- **Error** - 移动订单时发生的错误。
- **Trade** - 已提交订单的成交。

参数

- **Zero Price** – 使用零价格注册市价订单。

## 另请参阅

[Cancel Order](cancel.md)
