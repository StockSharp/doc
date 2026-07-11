
# Exportación desde Designer

**Runner** permite ejecutar estrategias creadas en [Designer](../designer.md). Esta es la forma más cómoda de configurar **Runner**, ya que toda la configuración se realiza visualmente.

Para exportar una estrategia desde [Designer](../designer.md):

- Seleccione la estrategia deseada en el árbol, haga clic derecho sobre ella y elija el elemento de menú **Runner**:

  ![Designer exportación a Runner](../../images/designer_runner_1.png)

- En la ventana que aparece, debe seleccionar qué tipos de conexiones deben exportarse a **Runner**, así como la configuración para gestionar la estrategia mediante [Telegram](../telegram_services.md):

  ![Designer exportación a Runner](../../images/designer_runner_2.png)

Los siguientes archivos se copiarán al directorio de exportación seleccionado:

- connector.json - archivo que contiene la configuración de conexión
- params.json - archivo que contiene parámetros de la estrategia
- start.bat - archivo bat con una línea de comandos ya escrita para iniciar rápidamente **Runner**
- strategy.json - archivo que contiene la estrategia
- connector.json - archivo que contiene la configuración de conexión
- telegram.json - archivo que contiene la configuración de integración con [Telegram](../telegram_services.md)
