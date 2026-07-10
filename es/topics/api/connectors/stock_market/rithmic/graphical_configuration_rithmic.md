# Configuración gráfica Rithmic

Para todos los productos [S#](../../../../api.md), la configuración gráfica de la conexión se realiza en la [Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md):

![Configuración de API GUI Rithmic](../../../../../images/api_gui_settings_rithmic.png)

- **usuario** - Login.
- **contraseña** - Password.
- **certificado** - Ruta al archivo de certificado necesario para conectarse al sistema Rithmic.
- **File log** - Ruta al archivo de log.
- **Server type** - Tipo de servidor.
- **Point (admin)** - Punto de conexión para funciones administrativas (inicialización\/desinicialización).
- **Point (data)** - Punto de conexión a datos de mercado.
- **usuario (transacciones)** - Login adicional. Se usa cuando el envío de transacciones se realiza a un servidor separado.
- **Point (transactions)** - Punto de conexión al sistema de ejecución de transacciones.
- **contraseña (transacciones)** - Contraseña adicional. Se usa cuando el envío de transacciones se realiza a un servidor separado.
- **Point (positions)** - Punto de conexión para acceder a información de portfolios y posiciones.
- **Point (history)** - Punto de conexión para acceder a datos históricos.
- **dominio (dirección)** - Dirección de dominio.
- **dominio (nombre)** - Nombre de dominio.
- **Licenses** - Dirección del servidor de licencias.
- **Broker** - Dirección del bróker.
- **Log (address)** - Dirección del logger.
- **Nombre de usuario (hist)** - Login adicional. ID de usuario usado para autenticarse con history plant.
- **contraseña (historial)** - Contraseña adicional. Contraseña usada para autenticarse con history plant
- **Intervalo de comprobación** - Intervalo de comprobación del servidor para rastrear si la conexión está activa. Por defecto es igual a 1 minuto.
- **Configuración de reconexión** - Mecanismo para rastrear las conexiones con la configuración del sistema de trading. ([Configuración de reconexión](../../reconnection_settings.md))

## Contenido recomendado

[Conectores](../../../connectors.md)

[Configuración gráfica](../../graphical_configuration.md)

[Creación de un conector propio](../../creating_own_connector.md)

[Guardar y cargar la configuración](../../save_and_load_settings.md)
