# Интерфейс

Для запуска тестирования на истории необходимо выбрать стратегию, схема которой будет тестироваться на истории. Стратегия выбирается на панели [Схемы](Designer_Panel_Schemas.md) в папке стратегии, двойным нажатием на интересующей стратегии. При выборе стратегии рабочей области появится новая вкладка со стратегией, при переходе на которую в ленте автоматически откроется вкладка **Эмуляция**.

![Designer Interface Backtesting 00](../images/Designer_Interface_Backtesting_00.png)

На вкладке **Эмуляция** можно изменить название стратегии и дать ей краткое описание.

Для запуска тестирования на истории необходимо на вкладке **Эмуляция** указать путь к историческим данным в поле **Маркет\-данные** и установить период тестирования. Запускается стратегия на тестирование нажатием кнопки **Старт**![Designer Interface Backtesting 01](../images/Designer_Interface_Backtesting_01.png). После запуска стратегии на тестирование становятся активными кнопка **Пауза**![Designer Interface Backtesting 02](../images/Designer_Interface_Backtesting_02.png) приостанавливающая тестирование, кнопка **Стоп**![Designer Interface Backtesting 03](../images/Designer_Interface_Backtesting_03.png) полностью останавливающая тестирование. При редактировании стратегии полезными будут кнопки **Отменить**(Ctrl+Z) ![Designer Interface Backtesting 04](../images/Designer_Interface_Backtesting_04.png) , отменяющая последнее действие, **Вернуть**(Ctrl+Y) ![Designer Interface Backtesting 05](../images/Designer_Interface_Backtesting_05.png) , возвращающая отменение, **Обновить**(Ctrl+R) ![Designer Interface Backtesting 06](../images/Designer_Interface_Backtesting_06.png) , полностью обновляющая схему. Также с вкладки **Эмуляция** можно воспользоваться **Отладчиком** ([Отладка](Designer_Debug.md)) или запустить **Оптимизацию** стратегии.

Вкладка выбраной стратегии по умолчанию содержит следующие панели:

- Панель **Схема**, в которой проходит основной процесс работы по дизайну стратегии и составных элементов, путём комбинирования кубиков и соединительных линий. Панель **Схема** подробно описана в пункте [Дизайнер стратегии](Designer_Designer_schemes_strategies_and_component_elements.md).
- Панель информационных компонентов, содержащая компоненты **График**, **Заявки**, **Сделки**, **Статистика** и др.. Добавить необходимый компонент можно, выбрав его на вкладке **Эмуляция** в группе **Компоненты**.
- Панель **Свойства** по умолчанию свернута справа во вкладки стратегии. На панели **Свойства** можно установить общие настройки **Эмуляции**. Например, **Формат хранилища маркет\-данных** можно установить **BIN** или **CSV**, в зависимости от формата файлов выбранного хранилища. Тип данных Ticks или Candles. При выботе Ticks свечи будут формироваться из тиков [Свойства тестирования](Designer_Properties_emulation.md).

## См. также

[Свойства тестирования](Designer_Properties_emulation.md)
