# Создание хранилища исторических данных

Создать хранилище исторических данных можно, нажав на кнопку ![Designer Creating a repository of historical data 00](../../../images/designer_creating_repository_of_historical_data_00.png) вкладки **Маркет\-данные**. Нажав на кнопку ![Designer Creating a repository of historical data 01](../../../images/designer_creating_repository_of_historical_data_01.png), можно изменить параметры текущего хранилища. Нажав на кнопку ![Designer Creating a repository of historical data 02](../../../images/designer_creating_repository_of_historical_data_02.png), можно удалить из списка хранилищ текущее хранилище.

![Designer Creating a repository of historical data 03](../../../images/designer_creating_repository_of_historical_data_03.png)

Хранилище исторических данных может быть локальное или удаленное.

Локальное хранилище \- это когда все данные хранятся на локальном компьютере. Для задания локального хранилища достаточно задать путь до папки, где хранятся данные.

Удаленное хранилище может быть расположено на удаленном компьютере. Для задания удаленного хранилища необходимо задать адрес удаленного хранилища, логин и пароль при необходимости. 

Воспроизвести удаленное хранилище на локальной машине можно с помощью программы [Hydra](../../hydra.md) (кодовое название Hydra), предназначенной для автоматической загрузки маркет\-данных (инструменты, свечи, тиковые сделки и стаканы и др.) из различных источников, и хранения их в локальном хранилище. Для этого [Hydra](../../hydra.md) необходимо перевести в серверный режим.

![Designer Creating a repository of historical data 04](../../../images/designer_creating_repository_of_historical_data_04.png)

После чего в [Designer](../../designer.md), нажав на кнопку ![Designer Creating a repository of historical data 00](../../../images/designer_creating_repository_of_historical_data_00.png), добавить новое хранилище. В настройках хранилища в поле адрес указываем «net.tcp:\/\/localhost:8000». Нажать ОК. При использовании [Hydra](../../hydra.md) в качестве удаленного хранилища не стоит забывать, что [Hydra](../../hydra.md) должна быть запущена и настроена соответствующим образом.

![Designer Creating a repository of historical data 05](../../../images/designer_creating_repository_of_historical_data_05.png)

После добавления нового хранилища, его можно выбрать в выпадающем списке **Хранилище**.

![Designer Creating a repository of historical data 06](../../../images/designer_creating_repository_of_historical_data_06.png)

Также необходимо выбрать формат файлов хранилища BIN или CSV. Данные могут сохраняться в двух форматах: в специальном бинарном формате BIN, что обеспечивает максимальную степень сжатия, или в текстовом формате CSV, что удобно при анализе данных в других программах. BIN формат предпочтителен, когда есть необходимость экономить место на диске. CSV формат предпочтителен, когда есть необходимость корректировать данные вручную. CSV легко редактируется стандартным блокнотом, MS Excel и др.

## См. также

[Скачивание инструментов](download_instruments.md)
