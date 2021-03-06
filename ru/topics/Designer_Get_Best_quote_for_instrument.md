# Получение лучшей цены для инструмента

Для регистрации заявки на покупку по текущей лучшей цене для инструмента может использоваться следующая схема:

![Designer Get the best rates for the tool 00](../images/Designer_Get_best_quote_for_instrument_00.png)

Для кубика [Переменная](Designer_Variable.md) выбран тип данных **Инструмент**. Если инструмент не указан, но установлен флаг **Параметры** группы свойств **Общее**, то он будет взят из стратегии и передан в кубик [Стакан](Designer_Depth.md). Кубик [Стакан](Designer_Depth.md), после получения текущего инструмента из переменной, передает через выходной параметр изменения стакана по выбранному инструменту. При получении изменений стакана, кубик [Конвертер](Designer_Converter.md) выбирает из них текущее значение лучшей цены на покупку.

## См. также

[Определение объема позиции](Designer_Determination_of_volume_position.md)
