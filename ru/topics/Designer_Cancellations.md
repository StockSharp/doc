# Отмена заявки

![Designer Cancellations 00](../images/Designer_Cancellations_00.png)

Кубик используется для отмены заявки по инструменту.

### Входящие сокеты

Входящие сокеты

- **Триггер** – событие, при котором происходит отмена заявки.
- **Заявка** – сигнал, с помощью которого определяется момент, когда необходимо отменить заявку.

### Исходящие сокеты

Исходящие сокеты

- **Заявка** – отмененная заявка, которая может использоваться для получения сделок по ней с помощью элемента **Сделки**, а также отображения на графике с помощью кубика **Панель графика**.
- **Ошибка** – ошибка отмены заявки (например, заявка уже была исполнена или отменена ранее).

## См. также

[Регистрация заявки](Designer_Position_opening.md)
