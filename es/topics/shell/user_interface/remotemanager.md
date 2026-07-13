# RemoteManager

La pestaña **RemoteManager** permite habilitar el modo de control remoto. Para habilitar este modo, debe ir al menú de configuración de usuario.

![Shell administrador remoto 00](../../../images/shell_remotemanager_00.png)

En la ventana que aparece, establezca su **nombre de usuario** y **contraseña**.

![Shell administrador remoto 01](../../../images/shell_remotemanager_01.png)

Después debe habilitar el **modo servidor**.

![Shell administrador remoto 02](../../../images/shell_remotemanager_02.png)

Ahora puede conectarse a Shell desde otro Shell.

Para ello, debe ejecutar **otro Shell**. En él, vaya a la configuración de conexión.

![Shell administrador remoto 03](../../../images/shell_remotemanager_03.png)

En la ventana que se abre, configure la conexión FIX.

![Shell administrador remoto 04](../../../images/shell_remotemanager_04.png)

Después pulse el botón Conectar.

![Shell administrador remoto 05](../../../images/shell_remotemanager_05.png)

Al conectarse, todas las estrategias existentes en el servidor Shell estarán disponibles en el cliente Shell.

![Shell administrador remoto 06](../../../images/shell_remotemanager_06.png)

Al hacer clic en el botón Añadir, puede añadir otra estrategia para la negociación.

![Shell administrador remoto 07](../../../images/shell_remotemanager_07.png)

Como el cliente Shell admite múltiples servidores, al añadir una estrategia debe seleccionar el servidor a la izquierda. Todas las estrategias disponibles en el servidor aparecerán a la derecha.

![Shell administrador remoto 08](../../../images/shell_remotemanager_08.png)

Después de añadir una estrategia, aparecerá en la lista de estrategias.

![Shell administrador remoto 09](../../../images/shell_remotemanager_09.png)

Al seleccionar una estrategia, habrá pestañas a la derecha con la configuración de la estrategia, así como sus estadísticas.

Después de cambiar la configuración de la estrategia, asegúrese de hacer clic en el botón Aplicar cambios; de lo contrario, los cambios no se aplicarán a la estrategia.

![Shell administrador remoto 10](../../../images/shell_remotemanager_10.png)

Si la estrategia tiene un comando distinto de Start\/Stop, para aplicarlo debe establecerlo en el siguiente campo.

![Shell administrador remoto 11](../../../images/shell_remotemanager_11.png)

Y haga clic en el botón de enviar comando.

Para establecer su comando en la estrategia, debe sobrescribir el método [Strategy.ApplyCommand](xref:StockSharp.Algo.Strategies.Strategy.ApplyCommand(StockSharp.Messages.CommandMessage))**(**[StockSharp.Messages.CommandMessage](xref:StockSharp.Messages.CommandMessage) cmdMsg **)**.

```cs
public virtual void ApplyCommand(CommandMessage cmdMsg)

```

La clase base [Strategy](xref:StockSharp.Algo.Strategies.Strategy) solo controla el inicio y la detención de la estrategia.

## Contenido recomendado

[Configuración de conexiones](../connections_settings.md)
