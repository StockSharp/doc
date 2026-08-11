# ColumnSettings

`ColumnSettings` добавляет выбор, перестановку и скрытие столбцов в HTML-таблицу, которую уже сформировал сервер или другой компонент. В отличие от [DataGrid](data_grid.md), адаптер не строит заголовок и строки и не управляет данными таблицы.

## Требования к разметке

Таблица должна содержать настоящий `<thead>`. Управляемые столбцы получают уникальные атрибуты `data-col`:

```html
<table id="trades">
  <thead>
    <tr>
      <th data-col="time">Время</th>
      <th data-col="symbol">Инструмент</th>
      <th data-col="price">Цена</th>
      <th data-col="volume">Объём</th>
      <th>Действия</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>10:15:02</td>
      <td>SBER</td>
      <td>312,45</td>
      <td>10</td>
      <td><button type="button">Открыть</button></td>
    </tr>
  </tbody>
</table>
```

Столбец без `data-col` считается фиксированным: пользователь не может скрыть его или переместить из исходной позиции. При создании адаптер помечает соответствующие ячейки тела теми же ключами. Строки с другим количеством ячеек, например строка «Нет данных» с `colspan`, остаются без изменений.

## Подключение

Хост-приложение предоставляет три части:

- таблицу с серверной разметкой;
- диалог, в который компонент добавляет переключатели и кнопки перемещения;
- хранилище с методами `read()` и `write()`.

```ts
import { ColumnSettings } from '@stocksharp/grids/column-settings';

const dialogElement = document.querySelector<HTMLElement>('#column-dialog')!;
const list = dialogElement.querySelector<HTMLElement>('#column-list')!;

const settings = new ColumnSettings({
  table: document.querySelector<HTMLTableElement>('#trades')!,
  dialog: {
    list,
    moveUpTitle: 'Переместить выше',
    moveDownTitle: 'Переместить ниже',
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

Компонент заполняет только элемент `list`. Заголовок, кнопки подтверждения и сброса, анимация, открытие и закрытие модального окна принадлежат приложению, поэтому обработчики `applyPicked()` и `resetToDefault()` также нужно назначить в приложении.

## Хранение раскладки

`ColumnLayoutStore.read()` возвращает массив видимых ключей в нужном порядке либо `null`, если применяется исходная раскладка. Конструктор читает значение сразу и применяет его до первого взаимодействия пользователя.

`write(visibleKeys)` получает только видимые управляемые столбцы. Значение `null` означает, что выбран исходный порядок и ни один столбец не скрыт. Благодаря этому хранилище в URL или `localStorage` может удалить ненужную запись вместо сохранения полного значения по умолчанию.

Ключи внутри адаптера нормализуются к нижнему регистру. Неизвестные и повторяющиеся ключи при применении отбрасываются.

## Методы

- `defaultKeys()` возвращает исходный порядок управляемых столбцов;
- `apply(visibleKeys)` сразу переставляет и скрывает столбцы, но не записывает раскладку;
- `isDefault(visibleKeys)` проверяет, совпадает ли раскладка с исходной;
- `openPicker()` считывает текущий DOM, строит список и открывает диалог;
- `applyPicked()` применяет текущий выбор, сохраняет его и закрывает диалог;
- `resetToDefault()` восстанавливает все управляемые столбцы, вызывает `write(null)` и закрывает диалог.

## Оформление

`ColumnSettings` не поставляет CSS и не зависит от конкретной модальной библиотеки или набора значков. Через `ColumnPickerClasses` приложение задаёт классы строки, флажка, подписи, кнопок и двух иконок. `moveUpTitle` и `moveDownTitle` должны быть локализованы до передачи компоненту.

## Смотрите также

- [JavaScript-таблицы](../grids.md)
- [DataGrid](data_grid.md)
- [Репозиторий JS-Grids](https://github.com/StockSharp/JS-Grids)
