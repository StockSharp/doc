# Интеграция C\# кода на общую схему

Для добавления стратегии SMA, описанной в пункте [Использование C\#](Designer_Creating_strategy_from_code.md), необходимо в **Палитре** выбрать кубик [Исходный код](Designer_Source_code.md) и перенести его в панель **Дизайнер**. В свойствах кубика **Исходный код** необходимо выбрать имя стратегии:

![Designer The integration of dice Source code in the General scheme 00](~/images/Designer_integration_Source_code_in_scheme_00.png)

Так как метод ProcessCandle(Candle candle) (пункт [Пример стратегии на C\#](Designer_Creating_strategy_from_source_code.md)) принимает свечи, то необходимо добавить кубик свечи и кубик [Переменная](Designer_Variable.md) с типом **Инструмент**. Соединить все кубики линиями. Так как [S\#.Designer](Designer.md) ни как не ограничивает количество любых кубиков в стратегии, то в одной стратегии можно использовать несколько кубиков [Исходный код](Designer_Source_code.md). Наиболее рациональным подходом является комбинирование кубиков. Те действия, которые проще описать в коде, стоит описывать в коде и интегрировать с помощью кубиков [Исходный код](Designer_Source_code.md) или [DLL стратегия](Designer_DLL_Strategy.md). А те действия, что проще сделать с помощью стандартных кубиков, стоит делать стандартными кубиками. Пример с данным подходом описан в пункте [Комбинирование C\# и стандартных кубиков](Designer_Combine_Source_code_and_standard_elements.md).

## См. также

[Комбинирование C\# и стандартных кубиков](Designer_Combine_Source_code_and_standard_elements.md)
