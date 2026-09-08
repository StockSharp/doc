# TableSort

`TableSort<TRow>` — самостоятельный контроллер сортировки, который используется внутри [DataGrid](data_grid.md), но может подключаться и к таблице приложения. Он хранит выбранный столбец и направление, обрабатывает щелчки по заголовкам с `data-sort` и возвращает отсортированную копию массива строк.

## Разметка

Сопоставьте каждый сортируемый заголовок с ключом из словаря функций чтения:

```html
<table id="quotes">
  <thead>
    <tr>
      <th data-sort="symbol">Инструмент</th>
      <th data-sort="bid">Покупка</th>
      <th data-sort="ask">Продажа</th>
    </tr>
  </thead>
  <tbody></tbody>
</table>
```

## Создание контроллера

```ts
import { SortDirections, TableSort } from '@stocksharp/grids/table-sort';

interface Quote {
  symbol: string;
  bid: number | null;
  ask: number | null;
}

const table = document.querySelector<HTMLTableElement>('#quotes')!;
let quotes: Quote[] = [];

const sort = new TableSort<Quote>(
  table.tHead,
  {
    symbol: quote => quote.symbol,
    bid: quote => quote.bid,
    ask: quote => quote.ask,
  },
  render,
  { col: 'symbol', dir: SortDirections.Asc },
  new Intl.Collator('ru-RU', { numeric: true, sensitivity: 'base' }),
);

function render(): void {
  const body = table.tBodies[0];
  body.replaceChildren();

  for (const quote of sort.apply(quotes)) {
    const row = body.insertRow();
    row.insertCell().textContent = quote.symbol;
    row.insertCell().textContent = quote.bid?.toString() ?? '—';
    row.insertCell().textContent = quote.ask?.toString() ?? '—';
  }
}
```

Конструктор принимает:

1. элемент заголовка или `null`, если обработка щелчков не нужна;
2. словарь функций чтения значений по ключу столбца;
3. функцию `onChange`, которая перерисует строки;
4. сортировку по умолчанию или `null` для исходного порядка;
5. заранее созданный `Intl.Collator` для сравнения текста.

Если функция чтения для выбранного ключа не задана, контроллер пытается прочитать одноимённое свойство строки.

## Поведение сортировки

Щелчок по новому заголовку включает сортировку по возрастанию. Следующий щелчок меняет её на убывание, а третий возвращает порядок по умолчанию. Отдельного неупорядоченного состояния нет, если для таблицы передан `defaultSort`.

`apply(rows)` всегда возвращает новый массив и не изменяет массив приложения. `null`, `undefined` и пустая строка помещаются в конец при обоих направлениях.

Сравнение выбирается **по значениям, а не по объявленному типу столбца**: если оба значения приводятся к конечному числу, они сравниваются численно, и только в остальных случаях в дело идёт переданный `Intl.Collator`. Строка `"42"` поэтому встанет между 41 и 43, а не там, где её поставил бы алфавит; строка вида `"1e3"` тоже считается числом. Если столбец должен сортироваться как текст при любых значениях, отдавайте из него значение, которое числом не становится.

Контроллер назначает активному заголовку класс `sort-asc` или `sort-desc`; стрелки и другое визуальное оформление этих классов задаёт приложение.

## Управление из кода

```ts
sort.set('bid', SortDirections.Desc);

const explicitSort = sort.current();
// { col: 'bid', dir: 'desc' }

sort.set(null, null);
// Возврат к defaultSort.
```

`current()` возвращает только явный выбор пользователя. Пока действует порядок по умолчанию, результат равен `null`, даже если строки фактически отсортированы.

Если приложение заново создало элементы `<th>` внутри того же заголовка, вызовите `refreshHeader()`, чтобы снова расставить классы направления. Слушатель щелчков назначен на сам переданный элемент заголовка и продолжит работать с новыми дочерними элементами.

## Смотрите также

- [JavaScript-таблицы](../grids.md)
- [DataGrid](data_grid.md)
- [TableExport](table_export.md)
