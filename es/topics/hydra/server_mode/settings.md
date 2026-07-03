# Configuración

[Hydra](../../hydra.md) puede usarse en modo servidor; en este modo puede conectarse remotamente a [Hydra](../../hydra.md) y obtener los datos existentes en el almacenamiento. Puede conectarse a [Hydra](../../hydra.md) ejecutado en modo servidor desde [Designer](../../designer.md) (consulte [Primeros pasos](../../designer/market_data_storage/getting_started.md) en la documentación de [Designer](../../designer.md) para saber cómo hacerlo). También puede conectarse a [Hydra](../../hydra.md) mediante la [API](../../api.md) (consulte la sección [Conectividad FIX/FAST](fix_fast_connectivity.md) para más detalles).

En modo servidor, el programa [Hydra](../../hydra.md) permite al usuario trabajar con una conexión desde varios programas a la vez. Al establecer la clave de acceso en la configuración del programa, el usuario puede trabajar simultáneamente con una fuente bajo una misma cuenta.

En la práctica, la conexión a la fuente se produce a través de [Hydra](../../hydra.md), al que, por ejemplo, se conectan simultáneamente [Designer](../../designer.md) y [Terminal](../../terminal.md). Este método permite evitar reconexiones entre programas y la compra de una conexión adicional. Con este trabajo se excluyen conflictos que pueden surgir por el registro de órdenes u operaciones desde distintos programas. [Hydra](../../hydra.md) recibe la señal y devuelve el resultado al programa desde el que se recibió, sin alterar la secuencia del resto del trabajo.

Para habilitar el modo servidor de [Hydra](../../hydra.md), seleccione la pestaña **Server mode** en el menú superior del programa.

![hydra server menu](../../../images/hydra_server_menu.png)

Después haga clic en el botón **Settings** para abrir la ventana de configuración del modo servidor.

![hydra server](../../../images/hydra_server.png)

**Hydra Server**

- **FIX server** - cambiar [Hydra](../../hydra.md) al modo servidor, distribuyendo negociación en vivo y datos históricos mediante el protocolo FIX.

  En esta sección se configura la conexión para trabajar con fuentes:
  1. **ConvertToLatin** - convertir cirílico a latín.
  2. **QuotesInterval** - período de actualización de cotizaciones.
  3. **TransactionSession** - configuración de una sesión de negociación. Configuración para operar mediante el programa [Hydra](../../hydra.md).

     Este ajuste permite configurar el dialecto del protocolo FIX, remitente y destinatario, formato de datos y otros parámetros. Consulte [FIXServer properties](https://doc.stocksharp.ru/html/Properties_T_StockSharp_Fix_FixServer.htm) para más detalles.
  4. **MarketDataSession** - configuración para transferir datos de mercado recibidos mediante [Hydra](../../hydra.md). Consulte [FIXServer properties](https://doc.stocksharp.ru/html/Properties_T_StockSharp_Fix_FixServer.htm) para más detalles.
  5. **KeepSubscriptionsOnDisconnect** - conservar suscripciones al desconectarse de la fuente.
  6. **DeadSessionCleanupInterval** - intervalo tras el cual se limpiará la información si la conexión está desconectada.
- **Authorization** - autorización para obtener acceso al servidor Hydra.
- **Number of securities** - número máximo de instrumentos que se pueden solicitar al servidor.
- **Candles (days)** - número máximo de días disponibles para descargar historial de velas.
- **Ticks (days)** - número máximo de días disponibles para descargar historial de datos tick.
- **Order books (days)** - número máximo de días disponibles para descargar historial del libro de órdenes.
- **OL (days)** - número máximo de días disponibles para descargar historial de datos OL.
- **Transactions (days)** - número máximo de días disponibles para descargar historial de transacciones.
- **Simulator** - activar el modo simulador.
- **Security mapping** - habilitar el modo de transferencia solo para los instrumentos especificados.

Si establece **Authorization** en un valor distinto de **Anonymous**, aparecerá el botón **Users** en la pestaña **Common**. Al hacer clic en él, aparecerá la ventana **Users**.

![hydra users](../../../images/hydra_users.png)

En la parte izquierda de la ventana puede añadir un nuevo usuario, y a la derecha establecer sus derechos de acceso.
