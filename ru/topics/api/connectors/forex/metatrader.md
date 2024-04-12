# MetaTrader

[S\#](../../../api.md) имеет интеграцию с терминалами MT4 и MT5 через специальные коннекторы. Для установки данных коннекторов необходимо использовать [Installer](../../../installer.md) (подробнее, [Установка и удаление программ ](../../../installer/install_and_remove_apps.md)).

Оба коннектора используются одинаково, поэтому ниже будет описан процесс подключения к MT5:

## Настройка MT коннектора

> [!Video https://www.youtube.com/embed/qGnIa7YIS5Q]

1. Выбрать в [Installer](../../../installer.md) MT коннектор и начать его установку

   ![MT Install 1](../../../../images/mt_install_1.png)

2. [Installer](../../../installer.md) спросит, в какую папку установить коннектор (должно быть установлено в папку Experts).

   ![MT Install 2](../../../../images/mt_install_2.png)

3. В случае установленных несколько терминалов, необходимо выбрать тот, куда требуется установить коннектор.

   ![MT Install 3](../../../../images/mt_install_3.png)

4. После выбора нужного терминала будет показан путь до папки Experts.

   ![MT Install 4](../../../../images/mt_install_4.png)

   > [!TIP]
   > - Если путь невозможно определить автоматически, то путь необходимо выбрать самостоятельно через поиск директории *C:\\Users\\%ваш\_ник\_юзера%\\AppData\\Roaming\\MetaQuotes\\Terminal\\%много\_букв\_и\_цифр%\\MQL4\\Experts\\* (в случае MT5 путь будет содержать MQL5).

5. Выполнить установку и дождаться окончания. По окончанию установки [Installer](../../../installer.md) предупредит, что теперь необходимо настроить терминал. Для этого необходимо запустить терминал MT и подключиться к торгам.
6. В меню Tools\-\>Options выбрать вкладу **Experts Advisors** и убедиться, что включено разрешение для торговли внешним dll (**Allow DLL imports**):

   ![MT 1](../../../../images/mt_1.png)

7. В случае, если при установке коннектора (пункт 2) терминал был запущен, то необходимо обновить список экспертов, нажав правой кнопкой на Experts и выбрав в меню **Refresh**:

   ![MT 2](../../../../images/mt_2.png)

8. Выбираем S\# эксперт, нажимаем правую кнопку и выбираем в меню пункт **Attach to a chart**:
    
   ![MT 3](../../../../images/mt_3.png)

9. Появится окно с настройками, где можно задать логин\-пароль (по\-умолчанию включена анонимная авторизация), а так же адрес подключения (в случае подключения сразу к нескольким терминалам адреса должны содержать уникальные порты).
10. На графике (выбирается первый попавшийся) справа вверху должна появится иконка смайлика:

    ![MT 4](../../../../images/mt_4.png)

    A также в окне логов эксперта должна появиться информация об успешном запуске скрипта, количество инструментов.
11. Если не получена лицензия MT4 или MT5, то в логе появится похожая на следующую строка:

    ![MT 5](../../../../images/mt_5.png)

12. Подключение к МТ идет по FIX протоколу, и используется коннектор [FIX протокол](../common/fix_protocol.md). В качестве демонстрации использована программа [Terminal](../../../terminal.md). Ниже настройки для транзакционного подключения и подключения с маркет\-данными (в случае MT5 порт по\-умолчанию равен 23001 вместо 23000):

    ![MT 6](../../../../images/mt_6.png)![MT 7](../../../../images/mt_7.png)

    Аналогичные настройки необходимо сделать в [Designer](../../../designer.md), [Hydra](../../../hydra.md) или любых API программах.

    Логин и пароль оставляются пустыми в случае анонимной авторизации (пред. пункт). В случае подключения к МТ несколькими роботами, необходимо указывать уникальный логин для идентификации разных подключений.

    > [!TIP]
    > - Скрипт необходимо запускать перед подключением StockSharp к МетаТрейдеру и не останавливать его, пока это подключение необходимо.  
    > - Чтобы в StockSharp видеть исторические свечки, их необходимо выгрузить с сервера МетаТрейдера. Как это сделать, читайте в документации самого МетаТрейдера.

    В случае успешного подключения пример должен показать список инструментов и счетов:

    ![MT 8](../../../../images/mt_8.png)

13. В случае возникновения ошибок ведутся логи коннектора, которые доступны в папке **Experts\\StockSharp\\Data\\Log**:

    ![MT 9](../../../../images/mt_9.png)