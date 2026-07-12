
# Configuración de conexión

**Runner**, además de [exportar configuración desde Designer](export_from_designer.md), ofrece la posibilidad de configurar el programa mediante su interfaz de consola. Para ello, debe ejecutar el programa con el comando **setup**:

```cmd
stocksharp.studio.runner setup
```

Aparecerá un menú:

![Configuración de conexión 1 (1)](../../images/runner_setup_1.png)

Al seleccionar el elemento Conexiones, el programa entrará en el modo de configuración del conector:

![Configuración de conexión 2 (1)](../../images/runner_setup_2.png)

Aquí puede editar una conexión guardada anteriormente o crear una nueva:

![Configuración de conexión 3 (1)](../../images/runner_setup_3.png)

Después de seleccionar el tipo requerido de nueva conexión, el programa pasará al menú de edición de su configuración:

![Configuración de conexión 4](../../images/runner_setup_4.png)

Para [Binance](../api/connectors/crypto_exchanges/binance.md), debe introducir su configuración principal:

![Configuración de conexión 5](../../images/runner_setup_5.png)

![Configuración de conexión 6](../../images/runner_setup_6.png)

![Configuración de conexión 7](../../images/runner_setup_7.png)

Para verificar la corrección de los datos introducidos, seleccione **Comprobar**:

![Configuración de conexión 8](../../images/runner_setup_8.png)

Se iniciará la comprobación de conexión:

![Configuración de conexión 9](../../images/runner_setup_9.png)

En caso de éxito, se mostrará un mensaje:

![Configuración de conexión 10](../../images/runner_setup_10.png)

Después de introducir todos los ajustes y verificarlos, debe pulsar **Guardar**:

![Configuración de conexión 11](../../images/runner_setup_11.png)

En la carpeta Data se creará un archivo **connector.json** (si no se había creado antes), que contendrá la configuración guardada.

Para configurar la integración con [Telegram](../telegram_services.md), seleccione el elemento de menú:

![Configuración de conexión 1 (2)](../../images/runner_telegram_1.png)

Y autentíquese con un método conveniente:

![Configuración de conexión 2 (2)](../../images/runner_telegram_2.png)

Para autenticación por token, introduzca el token de [https://stocksharp.ru/profile/](https://stocksharp.ru/profile/):

![Perfil](../../images/profile.png)

En caso de éxito, el programa mostrará las opciones disponibles de operación con Telegram:

![Configuración de conexión 3 (2)](../../images/runner_telegram_3.png)
