# Графическое конфигурирование Transaq

Для всех продуктов [S\#](../../../../api.md) графическая настройка подключения выполняется в экранной форме [Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md):

![API GUI Settings Transaq](../../../../../images/api_gui_settings_transaq.png)

- **Логин** \- Логин.
- **Пароль** \- Пароль.
- **Адрес** \- Адрес сервера.
- **Прокси** \- Прокси.
- **HFT** \- Подключаться ли к НЕТ серверу Финам.
- **Путь к dll** \- Полный путь к dll файлу, содержащее Transaq API. По умолчанию равно txmlconnector.dll.
- **Уровень логирования** \- Уровень логирования коннектора. По умолчанию Standard.
- **Данные для фондового рынка** \- Передавать ли данные для фондового рынка.
- **Период агрегирования** \- Период агрегирования данных на сервере Transaq.
- **Путь к логам** \- Путь к логам коннектора.
- **Перезаписать** \- Перезаписать файл библиотеки из ресурсов. По умолчанию файл будет перезаписан.
- **Настройки переподключения** \- Настройки механизма отслеживания подключения с торговой системой ([Настройки переподключения](../../reconnection_settings.md)). 
- **Интервал контроля** \- Интервал оповещения сервера о том, что подключение еще живое. По умолчанию равно 1 минуте. 
- **Код объединенной площадки** \- Код площадки для объединенного инструмента. 

## См. также

[Коннекторы](../../../connectors.md)

[Графическое конфигурирование](../../graphical_configuration.md)

[Создание собственного коннектора](../../creating_own_connector.md)

[Сохранение и загрузка настроек](../../save_and_load_settings.md)