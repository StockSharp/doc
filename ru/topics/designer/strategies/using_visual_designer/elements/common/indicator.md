# Индикатор

![Designer Indicator 00](../../../../../../images/designer_indicator_00.png)

Кубик используется для вычисления значений индикаторов.

## Входящие сокеты

Входящие сокеты

- **Любые данные** – определенный тип данных, на базе которого должен вычисляться выбранный индикатор (в зависимости от индикатора это может быть числовое значение, свеча и т.д.).

## Исходящие сокеты

Исходящие сокеты

- **Индикатор** – вычисленное значение индикатора, которое может использоваться для вывода на панель графика или дальнейших вычислений.

## Параметры

Параметры

- **Тип индикатора** - параметр, который используется для выбора нужного индикатора, и нескольких дополнительных параметров, которые соответствуют выбранному типу индикатора. Набор этих параметров меняется при изменении выбранного типа индикатора.
- **Финальные** - передавать только [финальные значения](../../../../../api/indicators.md) индикатора.
- **Сформирован** - передавать только значения, когда индикатор полностью [сформирован](../../../../../api/indicators.md).

![Designer Indicator 01](../../../../../../images/designer_indicator_01.png)

## См. также

[Список индикаторов](../../../../../api/indicators/list_of_indicators.md)
[Логическое условие](logical_condition.md)
