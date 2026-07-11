# Configuración gráfica Rithmic

Para todos los productos [S#](../../../../api.md), la configuración gráfica de la conexión se realiza en la [Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md):

![Configuración de API GUI Rithmic](../../../../../images/api_gui_settings_rithmic.png)

- **usuario** - Usuario.
- **contraseña** - Contraseña.
- **certificado** - Ruta al archivo de certificado necesario para conectarse al sistema Rithmic.
- **Archivo de registro** - Ruta al archivo de log.
- **Tipo de servidor** - Tipo de servidor.
- **Punto (administración)** - Punto de conexión para funciones administrativas (inicialización\/desinicialización).
- **Punto (datos)** - Punto de conexión a datos de mercado.
- **usuario (transacciones)** - Usuario adicional. Se usa cuando el envío de transacciones se realiza a un servidor separado.
- **Punto (transacciones)** - Punto de conexión al sistema de ejecución de transacciones.
- **contraseña (transacciones)** - Contraseña adicional. Se usa cuando el envío de transacciones se realiza a un servidor separado.
- **Punto (posiciones)** - Punto de conexión para acceder a información de portfolios y posiciones.
- **Punto (historial)** - Punto de conexión para acceder a datos históricos.
- **dominio (dirección)** - Dirección de dominio.
- **dominio (nombre)** - Nombre de dominio.
- **Licencias** - Dirección del servidor de licencias.
- **Bróker** - Dirección del bróker.
- **Registro (dirección)** - Dirección del logger.
- **Nombre de usuario (hist)** - Usuario adicional. ID de usuario usado para autenticarse con servicio histórico.
- **contraseña (historial)** - Contraseña adicional. Contraseña usada para autenticarse con servicio histórico
- **Intervalo de comprobación** - Intervalo de comprobación del servidor para rastrear si la conexión está activa. Por defecto es igual a 1 minuto.
- **Configuración de reconexión** - Mecanismo para rastrear las conexiones con la configuración del sistema de trading. ([Configuración de reconexión](../../reconnection_settings.md))

## Contenido recomendado

[Conectores](../../../connectors.md)

[Configuración gráfica](../../graphical_configuration.md)

[Creación de un conector propio](../../creating_own_connector.md)

[Guardar y cargar la configuración](../../save_and_load_settings.md)
