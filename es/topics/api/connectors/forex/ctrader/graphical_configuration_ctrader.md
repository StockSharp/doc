# Configuración gráfica de cTrader

Para todos los productos StockSharp, la configuración gráfica de la conexión se realiza en el formulario de pantalla [Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md):

![Configuración de API GUI cTrader](../../../../../images/api_gui_settings_ctrader.png)

- **Demostración** - Conexión a la negociación de demostración.

Autorización OAuth:

cTrader solo proporciona el método de autorización OAuth.

Proceso de autorización OAuth:

1. Al hacer clic en el botón "Check", se abrirá una ventana:

   ![inicio de OAuth](../../../../../images/oauth_start.png)

2. Después de hacer clic en "Start", el usuario será redirigido al sitio web de cTrader para iniciar sesión. En el sitio web de cTrader debe permitir a la aplicación StockSharp el acceso a las operaciones de trading:

   ![Inicio de sesión cTrader](../../../../../images/api_gui_settings_ctrader_2.png)

3. Después de eso, será redirigido de vuelta al sitio web de StockSharp y el programa iniciará sesión automáticamente.

## Ver también

[Conectores](../../../connectors.md)

[OAuth](../../oauth.md)

[Configuración gráfica](../../graphical_configuration.md)

[Creación de un conector propio](../../creating_own_connector.md)

[Guardar y cargar la configuración](../../save_and_load_settings.md)
