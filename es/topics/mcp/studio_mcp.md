# Comandos de los programas (Studio.Mcp)

**Studio.Mcp** es un servidor MCP a través del cual el agente envía comandos a los programas de StockSharp en ejecución: registra y cancela órdenes en Terminal, gestiona las fuentes de datos de Hydra e inicia la prueba de estrategias en Designer y Shell. El servidor no pulsa botones ni lee ventanas: el comando lo ejecuta el propio programa.

El servidor no inicia nada ni guarda nada. El comando solo llega al programa cuando el programa está abierto; si no está en ejecución, el servidor lo indica en su respuesta.

## Qué programas responden

| Programa | Qué comandos acepta |
|---|---|
| [Terminal](../terminal.md) | lista de órdenes, registro y cancelación de una orden |
| [Hydra](../hydra.md) | tipos de fuentes, lista de fuentes, adición, habilitación, inicio, detención y eliminación de una fuente |
| [Designer](../designer.md) | lista de estrategias, inicio y detención de la prueba sobre historial |
| [Shell](../shell.md) | lista de estrategias del área [Emulación](../shell/user_interface/emulation.md), inicio y detención de su ejecución |

Solo aceptan comandos las versiones Avalonia de estos programas, y en una compilación normal: no hace falta una compilación aparte. El programa abre el canal de comandos junto con la ventana principal y lo cierra cuando se cierra la ventana. Las versiones WPF de Terminal, Hydra y Designer no aceptan estos comandos.

El canal es una canalización con nombre local, accesible solo para los procesos del usuario actual del sistema operativo. Los comandos no se transmiten por la red.

## Instalación

El servidor se compila a partir del código fuente de StockSharp:

```sh
dotnet build Studio/Studio.Mcp/Studio.Mcp.csproj -c Release
```

El archivo resultante es `Studio/Studio.Mcp/bin/Release/net10.0/StockSharp.Studio.Mcp.exe`. El servidor no tiene configuración propia: no necesita ni argumentos ni variables de entorno. Escribe el protocolo en la salida estándar y el registro, en el flujo de error estándar; si se inicia a mano, sin cliente, simplemente espera una entrada.

## Conexión del agente

Claude Code: archivo `.mcp.json` del proyecto:

```json
{
  "mcpServers": {
    "stocksharp": {
      "command": "Studio/Studio.Mcp/bin/Release/net10.0/StockSharp.Studio.Mcp.exe"
    }
  }
}
```

En otros clientes MCP, incluido Claude Desktop, el servidor se conecta de la misma manera: el comando es la ruta a `StockSharp.Studio.Mcp.exe`, sin argumentos ni variables de entorno.

## Qué puede hacer el agente

Los identificadores de fuentes, estrategias y órdenes se toman de las respuestas de las herramientas de lista: primero la lista y después la acción.

### Generales

| Herramienta | Qué hace |
|---|---|
| `list_products` | qué programas están en ejecución y qué comandos acepta cada uno |
| `describe_product` | comandos de un programa; `product` es `Terminal`, `Hydra`, `Designer` o `Shell` |
| `close_product` | pide al programa que se cierre |

### Hydra

| Herramienta | Qué hace |
|---|---|
| `hydra_list_source_types` | tipos de fuentes que Hydra puede crear, con sus identificadores |
| `hydra_list_sources` | fuentes de Hydra: si cada una está habilitada y si está descargando ahora |
| `hydra_add_source` (`typeId`) | añade una fuente del tipo elegido y la devuelve; la nueva fuente está deshabilitada |
| `hydra_enable_source` (`sourceId`, `isEnabled`) | incluye la fuente en la descarga o la excluye de ella |
| `hydra_start_source` (`sourceId`) | inicia la descarga con la fuente; una fuente deshabilitada no se inicia |
| `hydra_stop_source` (`sourceId`) | detiene la descarga |
| `hydra_remove_source` (`sourceId`) | elimina la fuente junto con su configuración; una fuente en funcionamiento hay que detenerla primero |

Para descargar datos con una fuente nueva se necesitan tres llamadas: `hydra_add_source`, `hydra_enable_source`, `hydra_start_source`. Estos comandos no cambian la configuración de la fuente (instrumentos, tipos de datos, parámetros de conexión): se define en la ventana de Hydra (consulte [Fuentes de datos](../hydra/data_sources.md)).

### Designer

| Herramienta | Qué hace |
|---|---|
| `designer_list_strategies` | estrategias de Designer: tipo (`Backtest`, `Optimization`, `Live`, `Remote`, `Other`) y estado actual |
| `designer_start_backtest` (`strategyId`, `startTime`, `stopTime`) | inicia sobre historial una estrategia del tipo `Backtest` u `Optimization`; el período se indica en UTC en formato ISO 8601 y sustituye al período de la configuración de la estrategia; sin él se toma el período de la configuración |
| `designer_stop_backtest` (`strategyId`) | detiene una estrategia en funcionamiento |

### Terminal

| Herramienta | Qué hace |
|---|---|
| `terminal_list_orders` | órdenes de Terminal con el número de transacción de cada una |
| `terminal_register_order` (`securityId`, `portfolioName`, `side`, `volume`, `price`, `orderType`) | registra una orden y devuelve su número de transacción |
| `terminal_cancel_order` (`orderTransactionId`) | cancela una orden por su número de transacción |

El instrumento se indica tal como lo conoce Terminal, por ejemplo, `SBER@TQBR`; la cartera, por el nombre que conoce Terminal; `side` es `Buy` o `Sell`. Una orden limitada necesita un precio; en las órdenes de otros tipos, el precio pasado no se usa. La orden se identifica con el número de transacción que devolvieron `terminal_register_order` o `terminal_list_orders`, no con el número que le asignará más tarde la bolsa.

### Shell

Las herramientas de Shell las construye el servidor a partir de la descripción de los comandos que envía el propio programa. Por eso solo existen si Shell estaba en ejecución cuando se inició el servidor o en la última llamada a `list_products` o `describe_product`.

| Herramienta | Qué hace |
|---|---|
| `shell_list_strategies` | estrategias del área «Emulación» y su estado |
| `shell_start_backtest` (`StrategyId`, `StartTime`, `StopTime`) | inicia la ejecución de la estrategia sobre historial |
| `shell_stop_backtest` (`StrategyId`) | detiene la ejecución |

Shell siempre prueba la estrategia con el período de su configuración: si se pasa `StartTime` o `StopTime`, el comando se rechaza, así que pase `null` en ellos.

## Limitaciones y seguridad

> [!CAUTION]
> `terminal_register_order` registra una orden real allí donde esté conectado Terminal. El servidor no pide confirmación: la confirmación corresponde al cliente MCP, en el que una persona ve la llamada antes de que se ejecute.

- **No se inician estrategias que operan de verdad.** Designer solo inicia estrategias del tipo `Backtest` y `Optimization`; para las demás, el comando se rechaza. Shell solo abre a los comandos las estrategias del área «Emulación».
- **La eliminación es irreversible.** `hydra_remove_source` elimina la fuente junto con su configuración.
- **Cierre.** `close_product` cierra el programa.
- **El programa debe estar en ejecución.** Si no, el comando devuelve el error `<programa> is not running.` La espera de respuesta de un programa que no está en ejecución es de unos 5 segundos, por lo que `list_products` con los programas cerrados no responde de inmediato. El rechazo de un programa en ejecución llega al agente como texto: `<programa> refused the command: <causa>`.
- **Sin contraseña.** El canal está abierto a cualquier programa iniciado con la misma cuenta de usuario: no hay una comprobación aparte de quién ha enviado el comando.
- **Las copias de los programas no se distinguen.** Si hay varias copias de Terminal o de Designer en ejecución, o si junto a la Hydra normal funciona una Hydra con perfil de prueba, el comando llegará a una de ellas, y no se sabe de antemano a cuál.
- **No hay resultados de la prueba.** El servidor solo devuelve el estado de la estrategia, no las estadísticas ni las operaciones. Las puede leer en los paneles del programa [DesktopDriver](desktop_driver.md).

## Ejemplo de sesión

El usuario pide: «Inicia en Designer la prueba de la estrategia para enero y comprueba que está en curso».

1. `list_products`: Designer está en ejecución; sobre los demás programas, `… is not running.`
2. `designer_list_strategies`: entre las estrategias está la necesaria, del tipo `Backtest`, en el estado `Stopped`.
3. `designer_start_backtest`, `strategyId` del paso 2, `startTime` = `2026-01-01T00:00:00Z`, `stopTime` = `2026-02-01T00:00:00Z`: la respuesta es `Strategy <identificador> was started.`
4. `designer_list_strategies`: el estado de la estrategia ha cambiado.
5. `designer_stop_backtest` con el mismo `strategyId`, si hay que interrumpir la prueba.

## Véase también

[Conexión de agentes de IA (MCP)](../mcp.md)

[Control de la interfaz (DesktopDriver)](desktop_driver.md)

[Control remoto](../shell/remote_control.md)
