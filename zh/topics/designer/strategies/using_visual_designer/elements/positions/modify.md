# 修改持仓

![Designer position modify 00](../../../../../../images/designer_position_modify_00.png)

“Modify Position”组件用于根据指定条件更改交易持仓。

## 输入端口

- **Security**：要修改其持仓的证券。
- **Trigger**：激活持仓修改操作的信号。
- **Portfolio**：执行操作的投资组合。
- **Volume**（可选）：用于“Increase”和“Decrease”操作的数量。“Reverse”和“Close Position”操作不使用此值。
- **Last Price** 和 **Last Volume**：对于“VWAP”和“Iceberg”算法，需要提供最近一笔成交的价格和数量。
- **Cancel**：取消持仓设置过程的信号，例如因超时而取消。

## 输出端口

- **Order**：已提交订单的信息。
- **Transaction**：该订单产生的成交信息。
- **Balance**：此端口传递持仓修改操作结束时尚未完成的持仓部分。端口返回值表示操作结果：
  - `0` 表示组件已成功完成持仓修改操作，所有计划操作均已执行。
  - `-1` 表示由于当前持仓与指定条件不匹配，组件未开始修改持仓（例如当前持仓不为零，而条件为“OpenPosition”）。
  - 任何大于 `0` 的值都表示持仓修改过程在完成前被中断。这可能是因为策略图逻辑发出取消信号，或注册订单时发生错误。

## 参数

- **Condition**：持仓修改条件：
  - `None`：不执行任何操作。
  - `OpenPosition`：按指定方向建立持仓。
  - `ClosePosition`：平掉当前持仓。
  - `Decrease`：减少当前持仓规模。
  - `Increase`：增加当前持仓规模。
  - `Reverse`：平掉当前持仓，并按相反方向建立新持仓。
- **Direction**：指定“OpenPosition”和“None”的方向，并可作为其他条件的可选筛选器。
- **Algorithm**：可选项包括“Market Order”、“VWAP”和“Iceberg”。
- **Part**：使用“VWAP”或“Iceberg”等算法时，将总数量拆分为较小部分所采用的比例。

如果组件在已经开始更改数量后再次收到触发信号，则会忽略新的触发信号。如果修改条件与当前持仓状态不兼容（例如持仓已经建立，却尝试执行“OpenPosition”），组件会立即通过输出端口 **Balance** 返回 `-1`，表示无需执行该操作，并且操作未启动。

## 注意

如需进行低层级的订单管理，可以使用 [Order Registration](../orders/register.md) 组件。对于更高层级的持仓管理，建议使用“Modify Position”组件。

## 另请参阅

- [Order Registration](../orders/register.md)
