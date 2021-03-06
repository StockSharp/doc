# Настройка TWS Interactive Brokers

Механизм взаимодействия показан на данном рисунке:

![IBTrader](../images/IBTrader.png)

Как видно из рисунка, взаимодействует происходит через терминал [TWS](https://interactivebrokers.com/en/index.php?f=1537) или [IB Gate](https://interactivebrokers.com/en/index.php?f=1325), которые должны быть заранее установлены и настроены.

## Настройка терминала Trader Workstation

1. Необходимо разрешить подключения от других программ (такие как торговый робот на [S\#](StockSharpAbout.md)). Для этого нужно открыть настройки через меню "File \-\> Global configuration...". В новом окне выбрать "Configuration \-\> API \-\> Settings":

   ![ib settings](../images/ib_settings.png)
2. Включить режим "Enable ActiveX and Socket Clients".
3. Рекомендуется также добавить адрес компьютера, на котором будет запускаться робот (локальный адрес \- 127.0.0.1). Это позволит не подтверждать каждый раз при запуске робота разрешение на его подключение в терминале.
