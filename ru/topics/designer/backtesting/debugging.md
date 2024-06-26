# Отладка

В процессе тестирования стратегий часто возникает необходимость проверить, какие данные поступают на вход того или иного кубика или передаются на его выходе. Для этого в [Designer](../../designer.md) существует **Отладчик**.

![Designer Debug 00](../../../images/designer_debug_00.png)

На вкладке **Эмуляция** ленты в группе **Отладчик** расположены кнопки:

- ![Designer Debug 01](../../../images/designer_debug_01.png)**Добавить точку остановки** – при выделенном элементе добавить точку останова. Элементы, для которых добавлена точка останова, выделяются красной рамкой.
- ![Designer Debug 02](../../../images/designer_debug_02.png)**Убрать точку остановки** – удалить точку останова.
- ![Designer Debug 03](../../../images/designer_debug_03.png)**К следующему элементу** – при сработавшей точке останова выполняет переход к следующему элементу схемы.
- **Step to out** – при сработавшей точке останова выполняет переход к выходу текущего элемента, используется для проверки значений, которые передаются на выходе элемента.
- ![Designer Debug 04](../../../images/designer_debug_04.png)**В составной элемент** – при сработавшей точке останова выполняет переход внутрь составного элемента. При этом автоматически открывается схема составного элемента и выполняется остановка на элементе, в который первым передаются данные.
- ![Designer Debug 05](../../../images/designer_debug_05.png)**Из составного элемента** – при сработавшей точке останова и нахождении внутри составного элемента, выполняет выход на уровень вверх, где используется открытый составной элемент.
- ![Designer Debug 06](../../../images/designer_debug_06.png)**Продолжить** – продолжает выполнение до срабатывания следующей точки останова.

## См. также

[Точки остановки](debugging/break_points.md)
