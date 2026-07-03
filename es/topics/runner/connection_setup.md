
# Configuración de conexión

**Runner**, además de [exportar configuración desde Designer](export_from_designer.md), ofrece la posibilidad de configurar el programa mediante su interfaz de consola. Para ello, debe ejecutar el programa con el comando **setup**:

```cmd
stocksharp.studio.runner setup
```

Aparecerá un menú:

![runner_setup_1](../../images/runner_setup_1.png)

Al seleccionar el elemento Connections, el programa entrará en el modo de configuración del conector:

![runner_setup_2](../../images/runner_setup_2.png)

Aquí puede editar una conexión guardada anteriormente o crear una nueva:

![runner_setup_3](../../images/runner_setup_3.png)

Después de seleccionar el tipo requerido de nueva conexión, el programa pasará al menú de edición de su configuración:

![runner_setup_4](../../images/runner_setup_4.png)

Para [Binance](../api/connectors/crypto_exchanges/binance.md), debe introducir su configuración principal:

![runner_setup_5](../../images/runner_setup_5.png)

![runner_setup_6](../../images/runner_setup_6.png)

![runner_setup_7](../../images/runner_setup_7.png)

Para verificar la corrección de los datos introducidos, seleccione **Check**:

![runner_setup_8](../../images/runner_setup_8.png)

Se iniciará la comprobación de conexión:

![runner_setup_9](../../images/runner_setup_9.png)

En caso de éxito, se mostrará un mensaje:

![runner_setup_10](../../images/runner_setup_10.png)

Después de introducir todos los ajustes y verificarlos, debe pulsar **Save**:

![runner_setup_11](../../images/runner_setup_11.png)

En la carpeta Data se creará un archivo **connector.json** (si no se había creado antes), que contendrá la configuración guardada.

Para configurar la integración con [Telegram](../telegram_services.md), seleccione el elemento de menú:

![runner_telegram_1](../../images/runner_telegram_1.png)

Y autentíquese con un método conveniente:

![runner_telegram_2](../../images/runner_telegram_2.png)

Para autenticación por token, introduzca el token de [https://stocksharp.ru/profile/](https://stocksharp.ru/profile/):

![Profile](../../images/profile.png)

En caso de éxito, el programa mostrará las opciones disponibles de operación con Telegram:

![runner_telegram_3](../../images/runner_telegram_3.png)
