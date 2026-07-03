# Ventana de configuración de notificaciones

[AlertSettingsWindow](xref:StockSharp.Alerts.AlertSettingsWindow) - ventana para configurar notificaciones de ciertos eventos

![API GUI AlertWindow](../../../images/api_gui_alertwindow.png)

Puede configurar notificaciones sobre cambios en los siguientes tipos de datos: Portfolio, Client code, Broker, Depository, Server time, Transaction, Data type, Cancel, Order ID, Order ID (string), Order ID (platform), Derivative, Derivative (string), Price, Volume (order), Volume (trade), Visible volume, Direction, Balance, Order type, Status, Comment, Order message, System order, Order expiration time, Execution condition, Price, Trade initiator, Open interest, Error, Condition, Uptrend, Commission, Delay, Slippage, Identifier (user), Currency, P\/L, Position, Market maker.

Las notificaciones pueden tener la siguiente forma:

- **Window** - aparecerá una pequeña ventana emergente con un mensaje en la esquina de la pantalla.
- **Melody** - se reproducirá la melodía.
- **SMS** - se enviará un mensaje por SMS.
- **Email** - se enviará un mensaje por correo electrónico.
- **Speech** - el mensaje será pronunciado por la voz generada por el equipo.
- **Log** - el mensaje se enviará a la ventana de logs de Designer.
- **Disabled** - no se mostrará la notificación.

