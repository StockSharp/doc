# Определение объема позиции

Для получения объема, необходимого для переворота текущий позиции на противоположную, может использоваться схема, используемая в пример\-стратегии SMA:

![Designer Determination of the volume position 00](../../../../../images/designer_determination_of_volume_position_00.png)

Для кубика [Переменная](../elements/data_sources/variable.md) выбран тип данных **Инструмент**. Если инструмент не указан, но установлен флаг **Параметры** группы **Общее**, то он будет взят из стратегии, после чего будет передан в элемент [Позиция](../elements/common/position.md).

Для кубика [Позиция](../elements/common/position.md) свойство позиция также не указано, но установлен флаг **Параметры** группы **Общее**, что означает, что позиция будет получена для портфеля, который указан в настройках стратегии.

После передачи инструмента и изменении позиции, с помощью математической функции с одним аргументом (abs(pos)) вычисляется абсолютное значение и подается сигнал для кубика переменной (2). Данный кубик содержит множитель, равный 2\-м, на передачу сохраненного значения через выходной параметр, после чего, с помощью математической формулы с двумя аргументами (abs(pos) \* 2), вычисляется их произведение. Далее, с помощью составного кубика Условный оператор (pos \=\= 0 ? 1 : pos), определяется фактическое значение необходимого объема, которое может отличаться от текущего значения позиции, умноженного на 2. Например, в момент запуска стратегии, когда еще не была исполнена ни одна заявка. В этом случае элемент Условный оператор возвращает значение по умолчанию равное 1. Т.к. один выходной параметр может быть соединен только один раз с входным параметром другого элемента, для передачи одного и того же значения между формулой и условным оператором добавлен дополнительный кубик Combination.

## См. также

[Получение указанного уровня в стакане](get_order_book_price_level.md)