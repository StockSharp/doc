# Несколько графиков

`MultiChartWorkspace` раскладывает несколько независимых графиков сеткой в одном контейнере, связывает их по инструменту и таймфрейму и синхронизирует видимый диапазон и перекрестие. Класс поставляется точкой входа `@stocksharp/chart/workspace` вместе с остальными контроллерами рабочего пространства — панелей, индикаторов, шаблонов, сравнения инструментов и навигации по истории.

Рабочее пространство владеет только графиками верхнего уровня. Панели индикаторов остаются внутренним устройством своего графика: они не считаются ячейками, не участвуют в раскладке и не синхронизируются.

## Создание и обновление

Контейнер и фабрика графика обязательны. Фабрика получает `{ id, index, host }` и возвращает ячейку — сам график, необязательный контроллер данных и необязательную функцию освобождения (по умолчанию вызывается `chart.remove()`):

```ts
import { CandlestickSeries, createChart } from '@stocksharp/chart';
import { ChartDataController, type IChartDataSource } from '@stocksharp/chart/data';
import { MultiChartWorkspace } from '@stocksharp/chart/workspace';

declare const dataSource: IChartDataSource;

const workspace = new MultiChartWorkspace({
  container: document.querySelector<HTMLElement>('#workspace')!,
  count: 4,
  columns: 2,                                   // null — автоматическая сетка, близкая к квадрату
  links: { symbol: true, resolution: false },   // общий инструмент, свой таймфрейм у каждой ячейки
  sync: { range: true, crosshair: true },
  createChart: ({ host }) => {
    const chart = createChart(host, { timeScale: { timeVisible: true } });
    const series = chart.addSeries(CandlestickSeries, {
      upColor: '#26a69a',
      downColor: '#ef5350',
    });
    const data = new ChartDataController({ chart, series, dataSource, initialCount: 300 });

    return {
      chart,
      data,
      dispose: () => {
        data.dispose();
        chart.remove();
      },
    };
  },
});

const [first] = workspace.cells();
await workspace.setSelection(first.id, { symbol: 'BTC@IMEX', resolution: '1h' });

workspace.subscribe(snapshot => {
  console.log(snapshot.activeId, snapshot.columns, snapshot.rows, snapshot.errors.length);
});
```

`setCount` и `setColumns` меняют размер сетки по отдельности, `setLayout({ count, columns })` — за одну операцию. Контейнер оформляется как CSS-сетка; исходные стили запоминаются и восстанавливаются в `dispose`. Ячеек может быть от 1 до 64; удалить последнюю ячейку нельзя.

## Связывание и синхронизация

`links` описывает, что переносится на остальные ячейки при смене инструмента: `symbol` и `resolution` включаются независимо. Ячейка, у которой фабрика не вернула `data`, в связывании не участвует.

`sync` включает перенос видимого диапазона (`range`) и положения перекрестия (`crosshair`). Диапазон после применения перечитывается с графика-приёмника: ячейка с более короткой историей обрезает запрошенное окно, и в снимке публикуется то, что действительно показано.

Активная ячейка задаётся `activate`, а также автоматически по `pointerdown` и `focusin` внутри ячейки. Смена `links` или `sync` немедленно раздаёт остальным текущее состояние активной ячейки.

Ошибки синхронизации не прерывают работу остальных ячеек, а накапливаются в `snapshot.errors` — по 32 последних. У каждой записи есть `cellId`, `kind` (`WorkspaceSyncErrorKind`: `selection`, `range`, `crosshair`, `lifecycle`) и `error`. Список очищается вызовом `clearErrors`.

## Публичные методы

- `snapshot()` — полное состояние: количество ячеек, колонки и строки, активная ячейка, `links`, `sync`, ячейки и ошибки.
- `cells()` — снимки ячеек: `id`, `index`, `active`, `selection`, `visibleRange`, `crosshairTime`.
- `chart(id)` / `host(id)` — график и DOM-элемент ячейки.
- `add(id?)` — добавить ячейку; без аргумента идентификатор генерируется.
- `remove(id)` — удалить ячейку.
- `setCount(count)`, `setColumns(columns)`, `setLayout(layout)` — изменить сетку.
- `activate(id)` — сделать ячейку активной.
- `setLinks(options)`, `setSync(options)` — переключить связывание и синхронизацию.
- `setSelection(id, selection)` — задать инструмент и таймфрейм ячейки и раздать их по связям.
- `clearErrors()` — очистить накопленные ошибки.
- `subscribe(listener)` / `unsubscribe(listener)` — подписка на снимок состояния.
- `dispose()` — освободить ячейки и восстановить стили контейнера.

## Остальные контроллеры слоя

- `PaneController` — отменяемое (undo/redo) управление панелями графика: `resizePair`, `reorder`, `moveSeries`, `setState`, `toggleMinimized`, `toggleMaximized`. Работает через общий стек команд графика и не пересоздаёт содержимое панелей.
- `IndicatorController` — проверяемое редактирование индикаторов поверх движка расчётов: `update`, `setParameters`, `setSource`, `moveToPane`, `setPriceScale`, `setVisible`, `setOutputStyle`. Каждое изменение попадает в стек команд, снимок содержит определения параметров, состояние источника и стили выходов.
- `IndicatorCatalogController` — поиск по каталогу индикаторов (`search` по тексту, категории и признаку избранного) и избранное, сохраняемое хостом: `loadFavorites`, `setFavorite`, `toggleFavorite`.
- `IndicatorTemplateController` — переносимые шаблоны настроек индикатора: `create`, `replace`, `rename`, `remove`, `apply`, `load`. Метод `apply` переносит параметры, источник, видимость и стили выходов, но намеренно оставляет панель и ценовую шкалу цели без изменений.
- `serializeIndicatorTemplates`, `deserializeIndicatorTemplates`, `normalizeIndicatorTemplateDocument`, `INDICATOR_TEMPLATE_SCHEMA_VERSION` — сериализация и проверка версионированного документа шаблонов.
- `CompareController` — наложение нескольких инструментов на один график: `add`, `remove`, `setPrimary`, `setColor`, `setVisible`, `reload`, `loadMoreBefore`, `legend`. Режим нормализации задаётся `setMode` (`CompareMode.Percentage` или `CompareMode.IndexedTo100`), способ выравнивания времени — `setAlignment` (`CompareAlignment.Chart` или `CompareAlignment.PrimarySession`). У каждого инструмента свой `ChartDataController` и своя подписка.
- `ChartNavigator` — навигация по истории без привязки к DOM: `setRange`, `selectPreset`, `goToDate`, `cancel`. Контроллер сам догружает недостающие страницы истории (по умолчанию не более 100 за операцию) и публикует обзорную модель из ограниченного числа выборок (по умолчанию 600). Готовые пресеты `1D`, `5D`, `1M`, `3M`, `6M`, `YTD`, `1Y`, `5Y`, `All` возвращает `defaultNavigatorPresets`; результат операции описывают `NavigatorNavigationOutcome` (`applied`, `clamped`, `page-limit`, `empty`, `cancelled`), выравнивание даты — `NavigatorDateAlignment`.

## Смотрите также

- [JavaScript-графики](../charts.md)
- [Догрузка истории](backfill.md)
- [Индикаторы](indicators.md)
