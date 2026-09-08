# ColumnSettings

`ColumnSettings` 为已由服务器或其他组件生成的 HTML 表格添加列选择、重新排列和隐藏功能。与 [DataGrid](data_grid.md) 不同，该适配器不会创建表头和数据行，也不管理表格数据。

## 标记要求

表格必须包含真正的 `<thead>`。受管理的列需要具有唯一的 `data-col` 属性：

```html
<table id="trades">
  <thead>
    <tr>
      <th data-col="time">时间</th>
      <th data-col="symbol">交易品种</th>
      <th data-col="price">价格</th>
      <th data-col="volume">数量</th>
      <th>操作</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>10:15:02</td>
      <td>SBER</td>
      <td>312.45</td>
      <td>10</td>
      <td><button type="button">打开</button></td>
    </tr>
  </tbody>
</table>
```

没有 `data-col` 的列会被视为固定列：用户不能隐藏它，也不能将它移出初始位置。创建适配器时，它会用相同键标记表体中的对应单元格。单元格数量不同的行不会被修改，例如带有 `colspan` 的“暂无数据”行。

## 接入

宿主应用程序需要提供三个部分：

- 带有服务器端标记的表格；
- 组件向其中添加开关和移动按钮的对话框；
- 具有 `read()` 和 `write()` 方法的存储。

```ts
import { ColumnSettings } from '@stocksharp/grids/column-settings';

const dialogElement = document.querySelector<HTMLElement>('#column-dialog')!;
const list = dialogElement.querySelector<HTMLElement>('#column-list')!;

const settings = new ColumnSettings({
  table: document.querySelector<HTMLTableElement>('#trades')!,
  dialog: {
    list,
    moveUpTitle: '上移',
    moveDownTitle: '下移',
    classes: {
      item: 'column-picker-item',
      toggle: 'column-picker-toggle',
      label: 'column-picker-label',
      move: 'column-picker-move',
      moveUpIcon: 'icon-arrow-up',
      moveDownIcon: 'icon-arrow-down',
    },
    open: () => { dialogElement.hidden = false; },
    close: () => { dialogElement.hidden = true; },
  },
  store: {
    read: () => {
      const value = localStorage.getItem('trades-columns');
      return value ? JSON.parse(value) : null;
    },
    write: visible => {
      if (visible === null)
        localStorage.removeItem('trades-columns');
      else
        localStorage.setItem('trades-columns', JSON.stringify(visible));
    },
  },
});

document.querySelector('#open-columns')!
  .addEventListener('click', () => settings.openPicker());

document.querySelector('#apply-columns')!
  .addEventListener('click', () => settings.applyPicked());

document.querySelector('#reset-columns')!
  .addEventListener('click', () => settings.resetToDefault());
```

组件只填充 `list` 元素。标题、确认和重置按钮、动画以及模态窗口的打开与关闭均由应用程序负责，因此 `applyPicked()` 和 `resetToDefault()` 的处理程序也需要在应用程序中指定。

## 保存布局

`ColumnLayoutStore.read()` 返回按所需顺序排列的可见键数组；若应采用初始布局，则返回 `null`。构造函数会立即读取该值，并在用户首次操作前应用它。

`write(visibleKeys)` 只接收可见的受管理列。`null` 表示采用初始顺序且没有隐藏任何列。这样一来，URL 或 `localStorage` 中的存储可以删除不需要的记录，而无需保存完整的默认值。

适配器使用**小写**的键：它在解析表格时正是这样读取 `data-col` 的，对外给出的也是这样的键——`defaultKeys()` 的返回值以及传给 `write()` 的值都是如此。

> [!CAUTION]
> 传入 `apply()` 和来自 `read()` 的键会**按原样、不做大小写转换**地与这份列表比对。键 `Time` 不会与探测到的 `time` 匹配，而会作为未知键被丢弃；如果所有的键都这样书写，那么可见列将一个都不剩——表格打开时不会有任何一个受管理的列，而控制台里也不会有报错。请把 `write()` 传出的内容原样写入存储，不要自行重新拼装键。

应用布局时会丢弃未知键和重复键。空数组是一个合法的布局，含义是“不显示任何受管理的列”，而不是“未选择布局”；后者只用 `null` 表示。

## 方法

- `defaultKeys()` 返回受管理列的初始顺序；
- `apply(visibleKeys)` 立即重新排列并隐藏列，但不写入布局；
- `isDefault(visibleKeys)` 检查布局是否与初始布局一致；
- `openPicker()` 读取当前 DOM、构建列表并打开对话框；
- `applyPicked()` 应用当前选择、保存布局并关闭对话框；
- `resetToDefault()` 恢复所有受管理列、调用 `write(null)` 并关闭对话框。

## 外观

`ColumnSettings` 不提供 CSS，也不依赖特定的模态窗口库或图标集。应用程序通过 `ColumnPickerClasses` 指定行、复选框、标签、按钮和两个图标的类。传给组件前，`moveUpTitle` 和 `moveDownTitle` 应当已经完成本地化。

## 另请参阅

- [JavaScript 表格](../grids.md)
- [DataGrid](data_grid.md)
- [JS-Grids 仓库](https://github.com/StockSharp/JS-Grids)
