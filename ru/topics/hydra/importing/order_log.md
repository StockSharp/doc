# Лог заявок

Для импорта лога заявок нужно выбрать пункт **Импорт \=\> Лог заявок** из главного меню приложения.

![hydra import orderlog](../../../images/hydra_import_orderlog.png)

## Процесс импорта сделок

1. **Настройки импорта**.

   См. импорт [Свечи](candles.md).
2. Настроить параметры импорта для полей [S\#](../../api.md).

   См. импорт [Свечи](candles.md).

   **Рассмотрим пример импорта Лог заявок из CSV файла:**
   - Файл, данные из которого необходимо импортировать, имеет следующий шаблон записи:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{OrderId};{OrderPrice};{OrderVolume};{Side};{OrderState};{TimeInForce};{TradeId};{TradePrice}
     	  				
     ```

     Здесь значение {SecurityId.SecurityCode} и {SecurityId.BoardCode}, соответсвуют значениям **Инструмент** и **Площадка** соответсвенно. Поэтому в поле **Порядок поля** мы присваиваем значение 0 и 1 соответсвенно.
   - Для полей {ServerTime:default:yyyyMMdd} и {ServerTime:default:HH:mm:ss.ffffff} выбираем из окна **Поле S\#** поля **Дата** и **Время** соответсвенно. Присваиваем значение 2 и 3.
   - Для поля {OrderId} выбираем из окна **Поле S\#** поле **Идентификатор** \- идентификатор заявки. Присваиваем ему значение 4.
   - Для поля {OrderPrice} выбираем из окна **Поле S\#** поле **Цена** \- цена заявки. Присваиваем ему значение 5.
   - Для поля {OrderVolume} выбираем из окна **Поле S\#** поле **Объем** \- объем заявки. Присваиваем ему значение 6.
   - Для поля {Side} выбираем из окна **Поле S\#** поле **Направление** \- направление заявки (покупка или продажа). Присваиваем ему значение 7.
   - Для поля {OrderState} выбираем из окна **Поле S\#** поле **Действие** \- состояние заявки (активна, неактивна или ошибка). Присваиваем ему значение 8.
   - Для поля {TimeInForce} выбираем из окна **Поле S\#** поле **Время жизни** \- условие исполнения лимитированной заявки. Присваиваем ему значение 9.
   - Для поля {TradeId} выбираем из окна **Поле S\#** поле **Идентификатор (сделки))** \- идентификатор сделки. Присваиваем ему значение 10.
   - Для поля {{TradePrice} выбираем из окна **Поле S\#** поле **Цена (сделки)** \- цена сделки. Присваиваем ему значение 11.
   - Окно настройки полей будет выглядить следующим образом:![hydra import prop orderlog](../../../images/hydra_import_prop_orderlog.png)

   Пользователь может настроить большое количество свойств для скачиваемых данных. Исходя из шаблона импортируемого файла, нужно указывать свойство и присваивать ему нужный номер в последовательности. 
3. Для преварительного просмотра данных, нажать кнопку **Предпросмотр**.![hydra import preview orderlog](../../../images/hydra_import_preview_orderlog.png)
4. Для преварительного просмотра данных, нажать кнопку **Предпросмотр**.
5. Нажать кнопку **Импорт**.