# Использование DLL

Использование готовых DLL привычно для тех, кто хочет постоянно работать в средах **Visual Studio** и **JetBrains Rider**. Такой подход дает несколько преимуществ перед написанием [кода](using_code.md) внутри **Дизайнера**:

- Расширенный редактор кода по сравнению со встроенным редактором внутри **Дизайнер**.
- Перекомпилирование кода автоматически обновляет контент внутри **Дизайнера**.
- Возможно разбить код на несколько файлов (в случае подхода [кода](using_code.md) возможен только вариант ОдинФайл-ОднаСтратегия).
- Использование [отладчика](using_dll/debug_dll_in_visual_studio.md).

### Создание проекта в Visual Studio

1. Чтобы создать стратегию в **Visual Studio** необходимо создать проект:

![Designer Creating a DLL cube in Visual Studio 00](../../../images/designer_creating_dll_element_in_visual_studio_00.png)

2. Далее необходимо написать код стратегии. Для быстрого старта можно скопировать код SmaStrategy, которая создается как шаблон в [стратегии из кода](using_code/csharp/first_strategy.md):

![Designer Creating a DLL cube in Visual Studio 03](../../../images/designer_creating_dll_element_in_visual_studio_03.png)

3. Для компилирования кода необходимо подключить NuGet пакет [StockSharp.Algo](https://www.nuget.org/packages/stocksharp.algo), где находится базовый класс для всех стратегий [Strategy](xref:StockSharp.Algo.Strategies.Strategy):

![Designer Creating a DLL cube in Visual Studio 04](../../../images/designer_creating_dll_element_in_visual_studio_04.png)

Если стратегия использует программные интерфейсы графика, то необходимо подключить и NuGet пакет [StockSharp.Charting.Interfaces](https://www.nuget.org/packages/stockSharp.charting.interfaces). Данные интерфейсы не содержат логику реального графика, и необходимы только для компилирования кода. В случае запуска стратегии в **Дизайнере** через данные интерфейсы будет идти уже реальная отрисовка данных на графике.

4. После создания стратегии необходимо собрать проект, нажав **Собрать решение** во вкладке **Сборка**.

![Designer Creating a DLL cube in Visual Studio 01](../../../images/designer_creating_dll_element_in_visual_studio_01.png)

5. В Visual Studio по умолчанию проект собирается в папку …\\bin\\Debug\\net6.0 .

![Designer Creating a DLL cube in Visual Studio 02](../../../images/designer_creating_dll_element_in_visual_studio_02.png)

### Добавление DLL в Дизайнер

1. Добавление стратегии из DLL происходит аналогично созданию стратегии из [кода](using_code.md). Но на этапе определения типа контента необходимо выбрать **DLL**:

![Designer_Creation_Strategy_Dll_00](../../../images/designer_creation_strategy_dll_00.png)

2. В окне необходимо указать путь к сборке (должна быть совместима с .NET 6.0), и выбрать тип. Последнее необходимо, так как в одной DLL может быть сразу несколько стратегий (или [кубиков с индикаторами](using_dll/create_element_and_indicator.md)). После нажатия на **OK** стратегия будет добавлена на панель **Схемы** и готова к работе:

![Designer_Creation_Strategy_Dll_01](../../../images/designer_creation_strategy_dll_01.png)

3. Запуск стратегии на [бэктест](../backtesting/user_interface.md), на [live](../live_execution/getting_started.md) и другие операции - аналогично работе стратегии из схемы и кода:

![Designer_Creation_Strategy_Dll_02](../../../images/designer_creation_strategy_dll_02.png)