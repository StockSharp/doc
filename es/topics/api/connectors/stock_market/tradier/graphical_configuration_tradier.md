# Configuración gráfica de Tradier

Para todos los productos StockSharp, la configuración gráfica de la conexión se realiza en el formulario de pantalla [Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md):

![API GUI Settings Tradier](../../../../../images/api_gui_settings_tradier.png)

- **Token** - Token de autorización.
- **Demo** - Modo demo.

Autorización OAuth:

1. Puede insertar directamente el token en el campo "Token".
2. Si deja vacío el campo de token y el modo "Demo" no está seleccionado, se usará autorización OAuth.

Proceso de autorización OAuth:

1. Al hacer clic en el botón "Check", se abrirá una ventana:

   ![OAuth Start](../../../../../images/oauth_start.png)

2. Después de hacer clic en "Start", el usuario será redirigido al sitio web de Tradier para iniciar sesión:

   ![Tradier Login](../../../../../images/api_gui_settings_tradier_2.png)

3. En el sitio web de Tradier, debe permitir a la aplicación StockSharp el acceso a operaciones de trading:

   ![Tradier Permissions](../../../../../images/api_gui_settings_tradier_3.png)

4. Después de eso, será redirigido de vuelta al sitio web de StockSharp y el programa iniciará sesión automáticamente.

## Ver también

[Conectores](../../../connectors.md)

[OAuth](../../oauth.md)

[Configuración gráfica](../../graphical_configuration.md)

[Creación de un conector propio](../../creating_own_connector.md)

[Guardar y cargar la configuración](../../save_and_load_settings.md)
