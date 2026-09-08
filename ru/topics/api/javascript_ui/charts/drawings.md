# Инструменты разметки

`DrawingController` — слой ручной разметки графика: линии, фигуры, уровни Фибоначчи и заготовки позиций. Контроллер хранит фигуры как чистые JSON-объекты, привязывает их к примитивам холста, проводит каждое изменение через стек отмен графика и берёт на себя пошаговое построение мышью.

## Подключение

Слой поставляется отдельной точкой входа пакета `@stocksharp/chart`:

```ts
import { BuiltInDrawingType, DrawingController } from '@stocksharp/chart/drawings';
```

Импорт точки входа сразу регистрирует все встроенные типы разметки в общем каталоге `drawingDefinitionRegistry`.

## Создание и обновление

Контроллеру нужен только график; стек команд он по умолчанию берёт у него же (`chart.commandStack()`), поэтому отмена и повтор работают вместе с остальными действиями на графике:

```ts
import { CandlestickSeries, createChart } from '@stocksharp/chart';
import { BuiltInDrawingType, DrawingController } from '@stocksharp/chart/drawings';

const chart = createChart(document.getElementById('chart')!, { timeScale: { timeVisible: true } });
chart.addSeries(CandlestickSeries, {}).setData(candles);

const drawings = new DrawingController({ chart });

// Горизонтальный уровень: одна точка, время — Unix-время в секундах.
const level = drawings.create(
  BuiltInDrawingType.HorizontalLine,
  [{ time: 1_712_000_000, price: 68_000 }],
  { options: { color: '#f5c542', lineWidth: 2 } },
);

// Трендовая линия по двум точкам на подпанели указывается через paneId.
drawings.create(
  BuiltInDrawingType.TrendLine,
  [
    { time: 1_712_000_000, price: 67_400 },
    { time: 1_712_600_000, price: 69_150 },
  ],
  { paneId: 'main' },
);

drawings.updateOptions(level.id, { lineWidth: 3 });
drawings.setLocked(level.id, true);

// Снимок набора приходит отсортированным по zOrder.
drawings.subscribe(items => console.log(items.length));

chart.commandStack().undo();
```

`create` подставляет недостающие поля: `paneId` по умолчанию `main`, `visible` — `true`, `locked` — `false`, `zOrder` — на единицу больше текущего максимума, а опции накладываются поверх `defaultOptions` типа. `add` вставляет готовый экземпляр целиком, `duplicate` копирует существующий, `remove` и `clear` удаляют. Каждый из этих вызовов кладёт в историю ровно одну отменяемую команду.

`update` меняет любое сочетание полей (`points`, `options`, `paneId`, `visible`, `locked`, `zOrder`); `updateOptions`, `setVisible`, `setLocked` и `moveToPane` — короткие формы для частых случаев. Перед записью экземпляр нормализуется: точки и опции проверяются на JSON-совместимость и замораживаются, число точек сверяется со схемой типа, а панель — с существующими панелями графика.

## Встроенные типы

Идентификаторы собраны в `BuiltInDrawingType`; строковое значение и есть поле `type` сохранённой фигуры.

| Константа | Значение | Точек | Опции |
|---|---|---|---|
| `HorizontalLine` | `horizontal-line` | 1 | `LineDrawingOptions` |
| `VerticalLine` | `vertical-line` | 1 | `LineDrawingOptions` |
| `TrendLine` | `trend-line` | 2 | `LineDrawingOptions` |
| `Ray` | `ray` | 2 | `LineDrawingOptions` |
| `Rectangle` | `rectangle` | 2 | `RectangleDrawingOptions` |
| `Text` | `text` | 1 | `TextDrawingOptions` |
| `Note` | `note` | 1 | `TextDrawingOptions` |
| `FibonacciRetracement` | `fibonacci-retracement` | 2 | `FibonacciDrawingOptions` |
| `Measure` | `measure` | 2 | `MeasureDrawingOptions` |
| `LongPosition` | `long-position` | 3 | `PositionDrawingOptions` |
| `ShortPosition` | `short-position` | 3 | `PositionDrawingOptions` |

Наборы опций различаются по назначению:

- `LineDrawingOptions` — `color`, `lineWidth` (в диапазоне (0, 20]), `lineStyle` (0…4).
- `RectangleDrawingOptions` — то же плюс `fillColor` для заливки.
- `TextDrawingOptions` — `text` (до 10 000 символов, переносы строк учитываются), `color`, `backgroundColor`, `borderColor`, `borderWidth`, `fontSize`, `fontFamily`, `padding`. `Note` отличается от `Text` только значениями по умолчанию: подложка, рамка и увеличенные отступы.
- `FibonacciDrawingOptions` — `levels` (от 2 до 32 значений в диапазоне [-5, 5]; дубликаты убираются, список сортируется), `labelsVisible`, `extendRight`, а также `color`, `lineWidth`, `lineStyle`, `fillColor`, `fontSize`.
- `MeasureDrawingOptions` — `color`, `lineWidth`, `fillColor`, `labelColor`, `labelBackgroundColor`, `fontSize`. Подпись показывает изменение цены, процент и длительность выделенного интервала.
- `PositionDrawingOptions` — `entryColor`, `targetColor`, `stopColor`, `targetFillColor`, `stopFillColor`, `textColor`, `lineWidth`, `fontSize` и `quantity`. Три точки задаются по порядку: вход, цель, стоп; по ним считаются прибыль, риск и отношение R:R в подписях.

Значение опции, не проходящее проверку типа, приводит к исключению — сохранить фигуру с некорректной шириной линии или пустым цветом нельзя.

## Построение мышью

Пошаговый ввод ведёт сам контроллер: он переводит график в режим рисования и подписывается на клики и перекрестие.

```ts
drawings.subscribeCreation(state => {
  if (state === null) return;                    // построение завершено или отменено
  console.log(state.name, state.points.length, state.minimumPoints, state.maximumPoints);
});

drawings.beginCreation(BuiltInDrawingType.Rectangle, { options: { color: '#26a69a' } });

// Отмена по Esc, пока фигура не набрала нужное число точек.
document.addEventListener('keydown', event => {
  if (event.key === 'Escape') drawings.cancelCreation();
});
```

Каждый клик добавляет точку, пропущенную через магнит; движение курсора обновляет черновик, который рисуется тем же примитивом, что и готовая фигура, но в историю не попадает. Панель фиксируется по первому клику, клики в других панелях игнорируются. Как только точек набрано столько, сколько разрешает максимум типа, построение завершается само и создаётся обычная фигура. `finishCreation` закрывает построение досрочно и возвращает `null`, если точек меньше минимума; `cancelCreation` сбрасывает черновик; `creation` отдаёт текущий снимок `DrawingCreationSnapshot`.

## Привязка к барам

Магнит подтягивает точку к значениям серий текущей панели — расчёт идёт в экранных координатах, по вертикальному расстоянию до кандидата.

```ts
import { DrawingMagnetMode } from '@stocksharp/chart/drawings';

const drawings = new DrawingController({
  chart,
  magnet: { mode: DrawingMagnetMode.Weak, maxDistance: 12 },
});

drawings.applyMagnetOptions({ mode: DrawingMagnetMode.Strong });
console.log(drawings.magnetOptions());
```

`DrawingMagnetMode.None` отключает привязку, `Weak` (режим по умолчанию) притягивает только в пределах `maxDistance` — по умолчанию 10 CSS-пикселей, `Strong` притягивает к ближайшему значению всегда. Смена настроек во время построения сразу пересчитывает точку предпросмотра.

## Сохранение и восстановление

`DrawingInstance` намеренно не содержит объектов времени выполнения, поэтому набор разметки сериализуется как есть:

```ts
import type { DrawingInstance } from '@stocksharp/chart/drawings';

const saved = JSON.stringify(drawings.drawings());

const result = drawings.replaceAll(JSON.parse(saved) as DrawingInstance[], { unknownType: 'skip' });
console.log(result.restored.length, result.skipped);
```

`replaceAll` заменяет весь набор целиком: сначала проверяются все входные экземпляры (повторяющиеся идентификаторы — ошибка), затем старые фигуры снимаются с графика, а новые добавляются. Если хотя бы одна фигура не встала, предыдущее состояние восстанавливается. Незнакомый `type` при политике `skip` (по умолчанию) попадает в `skipped` с причиной `unknown-type`, при `error` — прерывает восстановление. Восстановление очищает историю команд, поэтому вызывать его внутри транзакции нельзя.

## Свои типы разметки

Каталог типов расширяемый. Достаточно описать определение и вернуть привязку к примитиву — готовую оболочку с выделением, маркерами и перетаскиванием даёт `createInteractiveDrawingBinding`:

```ts
import { createInteractiveDrawingBinding, registerDrawing } from '@stocksharp/chart/drawings';

registerDrawing({
  type: 'price-band',
  name: 'Price Band',
  points: { min: 2, max: 2 },
  defaultOptions: { color: '#4a9eff' },
  normalizeOptions: options => Object.freeze({ color: String(options.color).trim() }),
  create: (instance, events) => createInteractiveDrawingBinding(instance, events, {
    draw(context) {
      const [first, second] = context.points;
      if (second === undefined) return;
      context.context.strokeStyle = String(context.instance.options.color);
      context.context.strokeRect(
        context.plot.x, Math.min(first.y, second.y),
        context.plot.width, Math.abs(second.y - first.y),
      );
    },
    hitTest(point, context) {
      const [first, second] = context.points;
      if (second === undefined) return null;
      return point.y >= Math.min(first.y, second.y) && point.y <= Math.max(first.y, second.y)
        ? { cursor: 'move' }
        : null;
    },
  }),
});

drawings.create('price-band', [
  { time: 1_712_000_000, price: 67_800 },
  { time: 1_712_600_000, price: 68_900 },
]);
```

`draw` получает экранные точки, прямоугольник области построения, тему, коэффициент масштабирования и признак выделения; `hitTest` отвечает, попал ли курсор в тело фигуры. Необязательные `autoscaleInfo` и `handleColor` задают участие в автомасштабировании и цвет маркеров. `normalizeOptions` вызывается перед каждой записью в модель — это единственное место, где стоит проверять значения опций.

Перетаскивание тела или отдельной точки идёт через события `preview` (промежуточные состояния, в историю не пишутся), `commit` (одна команда «Edit drawing») и `cancel` (возврат к состоянию до жеста). Заблокированная фигура (`locked`) не перетаскивается и не показывает маркеры.

Каталогом можно управлять и напрямую: `unregisterDrawing(type)`, `getDrawingDefinition(type)`, `getDrawingTypes()`, а `DrawingDefinitionRegistry` позволяет завести отдельный каталог и передать его в контроллер параметром `registry`.

## Публичные методы

`DrawingController`:

- `drawings()`, `get(id)`, `has(id)` — чтение текущего набора.
- `create(type, points, options?)`, `add(instance)`, `duplicate(id, duplicateId?)` — добавление фигур.
- `update(id, patch)`, `updateOptions(id, patch)`, `setVisible(id, visible)`, `setLocked(id, locked)`, `moveToPane(id, paneId)` — изменение.
- `remove(id)`, `clear()` — удаление.
- `beginCreation(type, options?)`, `finishCreation()`, `cancelCreation()`, `creation()` — построение мышью.
- `magnetOptions()`, `applyMagnetOptions(patch)` — привязка к барам.
- `replaceAll(instances, options?)` — восстановление сохранённого набора.
- `subscribe(listener)` / `unsubscribe(listener)`, `subscribeCreation(listener)` / `unsubscribeCreation(listener)` — подписки.
- `dispose()` — освободить ресурсы.

Конструктор принимает `chart` (обязателен), а также `registry`, `commandStack`, `idFactory` и `magnet`.

Точка входа экспортирует и остальные части слоя: `DrawingMagnet` для самостоятельного расчёта привязки, `InteractiveDrawingPrimitive` вместе с `createInteractiveDrawingBinding`, функции проверки `normalizeDrawingInstance` и `normalizeDrawingOptions`, готовые наборы определений `builtInLineDrawingDefinitions`, `builtInShapeDrawingDefinitions`, `builtInAnalysisDrawingDefinitions`, `builtInPositionDrawingDefinitions` и парные им функции `registerBuiltInLineDrawings`, `registerBuiltInShapeDrawings`, `registerBuiltInAnalysisDrawings`, `registerBuiltInPositionDrawings` для регистрации в собственном каталоге.

## Смотрите также

- [JavaScript-графики](../charts.md)
- [Свечи](candlestick.md)
- [Индикаторы](indicators.md)
- [Догрузка истории](backfill.md)
