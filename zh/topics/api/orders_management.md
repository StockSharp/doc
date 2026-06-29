# 订单管理

[S#](../api.md) 提供了广泛的功能，用于在订单生命周期的各个阶段高效管理交易订单。本节涵盖了在交易应用程序中处理订单的关键方面。

## 主要特点

- **订单创建** - 各种类型交易订单的形成（市价单、限价单、止损单等）
- **状态跟踪** - 获取有关订单当前状态的最新信息
- **订单管理** - 取消、修改和替换现有订单
- **事件处理** - 响应订单的注册、执行和取消事件
- **批量操作** - 高效处理订单组

## 订单生命周期

S# 中的每个订单都会经历特定的生命周期阶段：

1. **创建** - 使用必要的参数创建一个[Order](xref:StockSharp.BusinessEntities.Order)对象
2. **注册** - 将订单发送到交易系统
3. **执行** - 订单的部分或全部执行，交易的形成
4. **完成** - 订单的完整执行、取消或拒绝

该 API 提供了订单在每个阶段状态的详细信息，从而可以构建具有精确执行控制的复杂交易算法。

## 与交易策略的整合

订单管理机制与用于开发交易策略的组件[Strategy](xref:StockSharp.Algo.Strategies.Strategy)紧密集成，这允许：

- 将订单管理逻辑封装在策略中
- 自动跟踪和处理订单注册及执行事件
- 在实际交易和测试期间都使用统一的方法进行订单管理

## 另请参阅

[创建新订单](orders_management/create_new_order.md)

[创建新止损订单](orders_management/create_new_stop_order.md)

[订单状态](orders_management/orders_states.md)

[订单取消](orders_management/order_cancel.md)

[批量订单取消](orders_management/orders_mass_cancel.md)

[订单替换](orders_management/orders_replacement.md)

[交易编号](orders_management/transaction_number.md)