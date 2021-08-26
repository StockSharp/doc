# Отладка DLL кубика с помощью Visual Studio

В Visual Studio существует механизм присоединения к выполняемым процессам с использованием отладчика Visual Studio. Наиболее подробно об отладчике Visual Studio написано в документации [Присоединиться к процессам](https://msdn.microsoft.com/ru-ru/library/3s68z0b3.aspx). Далее отладка будет рассмотрена на примере кубика, созданного в пункте [Создание DLL кубика в Visual Studio](Designer_Creating_DLL_element_in_Visual_Studio.md).

1. Для того чтобы присоединить к процессу и начать отладку DLL кубика необходимо, чтобы он был загружен в память. Загрузка DLL в память происходит после выбора имени стратегии в поле **Имя типа стратегии**. После загрузки DLL в память можно будет присоединить к процессу.

![Designer Creating a DLL cube in Visual Studio 03](~/images/Designer_Creating_DLL_element_in_Visual_Studio_03.png)

2. В Visual Studio выбрать пункт **Отладка \-\> Присоединить к процессу**.

![Designer Debugging DLL cube using Visual Studio 00](~/images/Designer_Debugging_DLL_using_Visual_Studio_00.png)

3. В диалоговом окне **Присоединение к процессу** найти в списке **Доступные процессы** процесс **Designer.exe**, к которому требуется присоединиться.

![Designer Debugging DLL cube using Visual Studio 01](~/images/Designer_Debugging_DLL_using_Visual_Studio_01.png)

Если процесс выполняется с другой учетной записи пользователя, необходимо установить флажок **Показать процессы всех пользователей**.

4. Важно, чтобы в окне **Присоединиться** был указан тип кода, который необходимо отладить. Параметр по умолчанию **Авто** пытается определить тип кода, который нужно отладить, но не всегда правильно определяет тип кода. Чтобы вручную задать тип кода, необходимо выполнить следующие действия.

- В поле Присоединиться кликнуть **Выбрать**.
- В диалоговом окне **Выбор типа кода** нажать кнопку **Выполнять** отладку кода следующих типов и выберите типы для отладки.
- Нажать кнопку ОК.

![Designer Debugging DLL cube using Visual Studio 02](~/images/Designer_Debugging_DLL_using_Visual_Studio_02.png)

5. Нажать кнопку Присоединить.

6. В Visual Studio в коде необходимо расставить точки останова. Если точки останова красные и заполненные красным ![Designer Debugging DLL cube using Visual Studio 03](~/images/Designer_Debugging_DLL_using_Visual_Studio_03.png) (и Студия в режиме отладки) то значит загрузилась именно та версия dll. А если точки останова красные и заполненные белым ![Designer Debugging DLL cube using Visual Studio 04](~/images/Designer_Debugging_DLL_using_Visual_Studio_04.png) (и Студия в режиме отладки), то значит загрузилась не та версия dll. 

7. В примере точка останова стоит в первой строчке метода **public void ProcessCandle(Candle candle)**. При запуске стратегии в [S\#.Designer](Designer.md), как только в DLL кубик начнут передаваться значения свечей, в Visual Studio произойдет остановка в месте установки точки останова. Далее можно будет отследить ход выполнения кода:

![Designer Debugging DLL cube using Visual Studio 05](~/images/Designer_Debugging_DLL_using_Visual_Studio_05.png)

## См. также

[Экспорт стратегий](Designer_Export_strategies.md)
