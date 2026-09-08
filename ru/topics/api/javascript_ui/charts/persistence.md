# Сохранение раскладки

`ChartStatePersistence` собирает раскладку графика — опции, панели, ценовые шкалы, серии, индикаторы и графические объекты — в один проверенный JSON-снимок и восстанавливает его обратно. Данные баров в снимок не попадают: сохраняется конфигурация, а котировки приходят из вашего источника.

Слой не владеет ни хранилищем, ни правилом именования ключей. Куда писать (файл, бэкенд, `localStorage`, IndexedDB) и как разделять снимки (по раскладке, по инструменту, по пользователю) решает приложение — через реализацию `ChartStateStorage` и функцию `key`.

## Создание и обновление

Импорт — из точки входа `@stocksharp/chart/persistence`:

```ts
import { createChart, CandlestickSeries } from '@stocksharp/chart';
import { DrawingController } from '@stocksharp/chart/drawings';
import {
  ChartStatePersistence,
  NativeChartLayoutAdapter,
  IndicatorEngineStateAdapter,
  type ChartStateStorage,
  type IndicatorEnginePersistenceApi,
} from '@stocksharp/chart/persistence';

declare const indicatorEngine: IndicatorEnginePersistenceApi;

const chart = createChart(document.querySelector<HTMLElement>('#chart')!, {});
chart.addSeries(CandlestickSeries, { id: 'price', upColor: '#26a69a', downColor: '#ef5350' });

const storage: ChartStateStorage = {
  load: key => localStorage.getItem(key),
  save: (key, value) => { localStorage.setItem(key, value); },
  remove: key => { localStorage.removeItem(key); },
};

const persistence = new ChartStatePersistence<{ layoutId: string; symbol: string }>({
  layout: new NativeChartLayoutAdapter({ chart, mainPaneId: 'main' }),
  indicators: new IndicatorEngineStateAdapter({ engine: indicatorEngine }),
  drawings: new DrawingController({ chart }),
  storage,
  key: ({ layoutId, symbol }) => `chart:${layoutId}:${symbol}`,
  pretty: true,
});

const context = { layoutId: 'desk', symbol: 'BTC@IMEX' };

await persistence.save(context);

const restored = await persistence.load(context);
if (restored !== null) {
  console.log(restored.state.panes.length);
  console.log(restored.drawings.skipped);   // объекты неизвестных типов пропущены, а не потеряли восстановление
}
```

Параметр-тип `TContext` — это то, что вы передаёте в `save`, `load` и `remove`; функция `key` превращает контекст в строку ключа и обязана вернуть непустую строку. `pretty: true` записывает JSON с отступами.

`DrawingController` должен быть тем же экземпляром, который обслуживает рисование на графике, — иначе сохранится пустой набор объектов.

## Адаптеры

`ChartStatePersistence` не знает ни о нативном API графика, ни о движке индикаторов: он работает через два адаптера. Готовые реализации входят в тот же модуль, но при своей архитектуре можно передать собственные — интерфейсы `ChartStateLayoutAdapter` (`capture`, `restore`) и `ChartStateIndicatorAdapter` (`capture`, `clear`, `restore`) открыты.

**`NativeChartLayoutAdapter`** снимает и возвращает раскладку самого графика: опции графика, панели с их порядком, высотой, минимальной высотой и состоянием (`normal`, `minimized`, `maximized`), настройки ценовых шкал, а также серии с их типом, панелью, шкалой и стилевыми опциями. Опции конструктора:

- `chart` — экземпляр графика (обязателен).
- `mainPaneId` — идентификатор корневой панели, которая переживает восстановление; по умолчанию `main`, иначе первая панель.
- `createSeries(series, pane)` — своё создание серии вместо реестра типов, когда серию нужно подключить к источнику данных.
- `includeSeries(series)` — фильтр: серия, для которой вернули `false`, не сохраняется и не удаляется при восстановлении.
- `onRemoveSeries(series)` — вызывается для чужой серии, которую восстановление всё же вынуждено было отцепить, потому что её панель не входит в загружаемую раскладку.
- `onUnknownSeries(series)` — тип серии отсутствует в реестре.

Серия с опцией `persist: false` исключается из снимка так же, как отклонённая фильтром `includeSeries`.

**`IndicatorEngineStateAdapter`** сохраняет конфигурацию индикаторов — тип, параметры, стили отрисовки, привязку к панели и шкале, видимость и источник — но не вычисленные значения: после восстановления они считаются заново. Опции конструктора:

- `engine` — движок индикаторов, реализующий `IndicatorEnginePersistenceApi` (`getIndicators`, `removeAll`, `add`, `setVisible`).
- `resolveTargetPaneId(indicator)` — сопоставление сохранённой панели с панелью хоста, когда идентификаторы различаются.
- `onUnknownIndicator(indicator)` — движок не смог создать индикатор такого типа.
- `onUnknownStyle(indicator, styleId)` — в стилях встретился идентификатор, которого у индикатора нет.

Индикатор, считающийся по выводу другого индикатора, восстанавливается после него: адаптер сам упорядочивает цепочку источников и сообщает об ошибке, если ссылка ведёт на отсутствующий индикатор или граф зациклен.

## Формат состояния и миграции

Снимок описывается типом `ChartStateV1` с полями `schemaVersion`, `chartOptions`, `panes`, `series`, `indicators`, `drawings`; текущая версия схемы — константа `CHART_STATE_SCHEMA_VERSION` (равна 1).

- `serializeChartState(state, { pretty })` — проверить и превратить состояние в строку JSON.
- `deserializeChartState(value, { migrations })` — разобрать строку (или принять уже готовый объект), прогнать миграции до текущей версии и проверить результат.
- `normalizeChartStateV1(value)` — проверка и заморозка состояния: посторонние ключи, дубли идентификаторов, ссылки на несуществующие панели и раскладка без единой панели отклоняются.
- `normalizePersistedObject(value, path, { omitUndefined })` — глубокое копирование произвольного JSON в неизменяемый объект; циклы, нечисловые значения, чрезмерная вложенность и ключи `__proto__`, `prototype`, `constructor` запрещены.

Старые снимки поднимаются пошаговыми миграциями. Общий реестр `chartStateMigrations` уже содержит переход с версии 0 на версию 1, а свои шаги регистрируют так:

```ts
import {
  ChartStateMigrationRegistry,
  deserializeChartState,
} from '@stocksharp/chart/persistence';

const migrations = new ChartStateMigrationRegistry();
migrations.register(1, state => ({ ...state, schemaVersion: 2 }));

const state = deserializeChartState(json, { migrations });
```

Каждая миграция переводит состояние ровно на одну версию вперёд и обязана выставить новое значение `schemaVersion`. Снимок, версия которого выше поддерживаемой, к загрузке не принимается.

## Публичные методы

- `snapshot()` — собрать текущее состояние графика в `ChartStateV1`, не обращаясь к хранилищу.
- `restore(state)` — применить состояние к графику; возвращает `{ state, drawings }`, где `drawings` содержит списки `restored` и `skipped`.
- `save(context)` — снять снимок, сериализовать и записать в хранилище по вычисленному ключу; возвращает сохранённое состояние.
- `load(context)` — прочитать запись по ключу, прогнать миграции и восстановить её; `null`, если записи нет.
- `remove(context)` — удалить запись из хранилища.

Порядок восстановления фиксирован: сначала освобождаются индикаторы, затем восстанавливается раскладка панелей и серий, затем индикаторы, и последними — графические объекты.

Модуль экспортирует также типы для описания снимка и адаптеров: `ChartStateLayoutSnapshot`, `ChartStateRestoreResult`, `ChartStatePersistenceOptions`, `PersistedPane`, `PersistedPriceScale`, `PersistedSeries`, `PersistedIndicator`, `PersistedDrawing`, `PersistedChartOptions`, `PersistedSeriesOptions`, `PersistedIndicatorParameters`, `PersistedIndicatorStyles`, `PersistedObject`, `PersistedJsonValue`, `PersistableIndicatorEntry`, `RawChartState`, `ChartStateMigration`, `MaybePromise`.

## Смотрите также

- [JavaScript-графики](../charts.md)
- [Индикаторы](indicators.md)
- [Догрузка истории](backfill.md)
