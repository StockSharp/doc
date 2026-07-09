# IQFeed

**DTN IQFeed** - proveedor de datos de mercado en tiempo real para cotizaciones de acciones, Forex, noticias, contratos de futuros, etc.

Antes de comenzar a escribir robots de trading para la plataforma de trading actual, se recomienda leer los enlaces en [Conectores](../../connectors.md). 

## Configuración IQFeed

El mecanismo de interacción se muestra en esta figura: 

![IQFeed](../../../../images/iqfeed.jpg)

Para trabajar con el conector **IQFeed**, debe instalar el router **IQ Feed Client** en el equipo; puede instalarse tanto en el equipo local como en uno remoto. El intercambio de datos entre la aplicación cliente y **IQ Feed Client**, así como entre **IQ Feed Client** y los servidores, se realiza mediante el protocolo TCP\/IP. 

Para descargar **IQ Feed Client** desde el sitio [IQFeed](https://www.iqfeed.net/stocksharp/), primero debe autorizarse con la contraseña y login recibidos de **iQFeed**.

Después de instalar **IQ Feed Client**, se recomienda reiniciar el equipo.

Después de instalar **IQ Feed Client, IQLink Launcher** debe iniciarse.

![iQFeedIQLinkLauncher](../../../../images/iqfeediqlinklauncher.png)

En la ventana **IQLink Launcher** que se abre, haga clic en **Start IQLink**.

![iQFeedIQConnectLogin](../../../../images/iqfeediqconnectlogin.png)

En la ventana **IQ Connect Login** abierta, introduzca **usuario** y **contraseña** (o PIN) recibidos del servicio **iQFeed**. Estas credenciales no son las mismas que Login y Password del sitio web **iQFeed**. Después de introducir las credenciales, haga clic en **Connect**.

Para recibir datos, la aplicación cliente usa cuatro conexiones mediante distintos puertos: 

1. Level1 (puerto 5009) se usa para obtener datos en tiempo real sobre instrumentos (ticks, precios de apertura y cierre, volatilidad, etc.) y noticias.
2. Level2 (puerto 9200) se usa para obtener cotizaciones extendidas de instrumentos; para cada ECN puede obtenerse el mejor par de cotizaciones.
3. Lookup (puerto 9100) se usa para buscar instrumentos, recuperar datos históricos y obtener información avanzada sobre noticias.
4. Admin (puerto 9300) se usa para obtener información general sobre la conexión y cambiar configuraciones.

Los números de puerto usados por defecto para conectarse a **IQ Feed Client** se indican entre paréntesis. Para conexiones cliente, los números de puerto pueden cambiarse en el registro, por ejemplo, para Level1 en la siguiente ruta: \[HKEY\_CURRENT\_USER\\SOFTWARE\\DTN\\IQFEED\\Startup\\Level1Port\]. Los números de puerto para conectarse a los servidores IQ no pueden cambiarse. 

> [!CAUTION]
> El conector solo admite el flujo de datos de mercado; las transacciones no están admitidas. 

## Contenido recomendado

[Conectores](../../connectors.md)

[Configuración gráfica](../graphical_configuration.md)

[Guardar y cargar la configuración](../save_and_load_settings.md)

[Creación de un conector propio](../creating_own_connector.md)

[Gestión de órdenes](../../orders_management.md)

[Crear una nueva orden](../../orders_management/create_new_order.md)

[Crear una nueva orden stop](../../orders_management/create_new_stop_order.md)

[Conexión IQFeed](iqfeed/connection_iqfeed.md)

[Inicialización del adaptador IQFeed](iqfeed/adapter_initialization_iqfeed.md)
