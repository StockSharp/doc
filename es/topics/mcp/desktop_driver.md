# Control de la interfaz (DesktopDriver)

**DesktopDriver** es un servidor MCP a través del cual el agente trabaja con las ventanas de un programa en ejecución igual que lo hace una persona: encuentra paneles y botones, lee tablas, gráficos, libros de órdenes y editores de propiedades, pulsa, escribe texto, desplaza y toma capturas.

El servidor lee la propia interfaz, no una imagen de la pantalla. Una tabla responde con sus columnas y filas; un gráfico, con los puntos que ha dibujado; un libro de órdenes, con sus niveles de precios. La respuesta no depende del tema, la fuente ni el idioma del programa. Las capturas son para las personas; el contenido se comprueba leyéndolo.

El servidor se distribuye como paquete NuGet `StockSharp.DesktopDriver.Mcp` y se identifica como `StockSharp.DesktopDriver`; en el registro MCP Registry figura como `io.github.StockSharp/desktop-driver`. El código fuente está en el repositorio [DesktopDriver.Mcp](https://github.com/StockSharp/DesktopDriver.Mcp).

## Cómo funciona

1. El controlador se incluye en el programa al compilarlo. En una compilación normal no está.
2. Un programa con el controlador, iniciado con el parámetro `--ui-automation`, abre un canal local (una canalización con nombre accesible solo para el usuario actual del sistema operativo) y escribe su dirección en el archivo indicado con el parámetro `--ui-automation-endpoint=<ruta>`. El archivo aparece cuando la ventana ya está construida y dibujada. Sin el parámetro `--ui-automation`, el programa no abre nada.
3. El servidor MCP inicia el programa desde su catálogo con estos parámetros o se conecta a un programa que ha iniciado otra persona. Al conectarse, comprueba que ha respondido el programa correcto, la copia correcta y una versión compatible del protocolo, y solo después ejecuta las llamadas del agente.

Las mismas operaciones están disponibles desde la línea de comandos con el programa `desktop-driver` del paquete `StockSharp.DesktopDriver.Cli`.

## Qué compilaciones se pueden conectar

Solo se puede conectar con un programa compilado con la automatización activada, es decir, con la propiedad `EnableUiAutomation=true`. De forma predeterminada, la propiedad está desactivada, y una compilación normal no tiene ni controlador ni canal. Una compilación normal iniciada con `--ui-automation` escribe en el flujo de error estándar el mensaje `<identificador> was built without automation, so --ui-automation does nothing. Build it with EnableUiAutomation=true.` y funciona como siempre. El servidor MCP espera el canal de ese programa durante 90 segundos, luego informa de que el programa no se ha iniciado y lo cierra.

La compilación con automatización se hace a partir del código fuente del programa, junto al cual debe estar el repositorio DesktopDriver.Mcp. Se coloca en una carpeta aparte, `uiauto\bin`, para no mezclarse con la normal:

```sh
dotnet build Terminal/Avalonia/Terminal.Avalonia/Terminal.Avalonia.csproj -c Release -p:EnableUiAutomation=true
```

El ejecutable de esa compilación es `Terminal/Avalonia/Terminal.Avalonia/uiauto/bin/Release/net10.0/StockSharp.Terminal.Avalonia.exe`.

| Programa | Identificador de la versión Avalonia | Identificador de la versión WPF | Clics y escritura sin perfil de prueba |
|---|---|---|---|
| [Terminal](../terminal.md) | `stocksharp.terminal` | `stocksharp.terminal.wpf` | se rechazan |
| [Hydra](../hydra.md) | `stocksharp.hydra` | `stocksharp.hydra.wpf` | se rechazan |
| [Designer](../designer.md) | `stocksharp.designer` | `stocksharp.designer.wpf` | se rechazan |
| [Installer](../installer.md) | `stocksharp.installer` | `stocksharp.installer.wpf` | se rechazan |
| [Shell](../shell.md) | `stocksharp.shell` | — | se rechazan |
| [UDP Dumper](../udp_dumper.md) | `tool.udpdumper` | — | se aceptan |

El programa comunica su identificador por sí mismo, y el mismo identificador se indica en el catálogo del servidor.

En las versiones Avalonia, el controlador lee tablas, el área de trabajo con paneles, gráficos, el libro de órdenes, el editor de propiedades, el esquema de la estrategia, el editor de código y la vista de Markdown. En las versiones WPF, tablas, paneles acoplables, gráficos, el libro de órdenes, el editor de propiedades, el esquema de la estrategia, campos de entrada, la cinta, menús y barras de herramientas. En las versiones Avalonia de Terminal, Hydra y Designer, todo lo que está en un panel se direcciona respecto a ese panel: `panel:<tipo de panel>/<identificador>`.

## Instalación del servidor

### Herramienta de .NET

Se necesita el [SDK de .NET 10](https://dotnet.microsoft.com/download/dotnet/10.0):

```sh
dotnet tool install StockSharp.DesktopDriver.Mcp --tool-path tools
```

El comando del servidor es `tools/desktop-driver-mcp`. El servidor escribe el protocolo en la salida estándar y el registro, en el flujo de error estándar. Si se inicia a mano, sin cliente, simplemente espera una entrada.

### Desde el código fuente

```sh
dotnet build DesktopDriver.Mcp/DesktopDriver.Mcp.csproj -c Release
```

### Extensión para Claude Desktop

Para Claude Desktop, el servidor se empaqueta en una extensión: el archivo `stocksharp-desktop-driver-<versión>.mcpb`, que genera el script `scripts/Pack-McpBundle.ps1` del repositorio DesktopDriver.Mcp. La extensión inicia el servidor mediante el .NET instalado en el equipo, por lo que se necesita el SDK de .NET 10 accesible a través de `PATH`.

1. En Claude Desktop, abra **Settings → Extensions → Advanced settings → Install Extension** y seleccione el archivo de la extensión.
2. Indique el catálogo de programas (un archivo JSON, consulte más abajo) y la carpeta para las capturas.

## Catálogo de programas

El agente no puede iniciar un archivo cualquiera. El servidor solo inicia programas del catálogo, un archivo JSON que escribe quien configura el servidor. El agente nombra el programa con su identificador, no con una ruta.

El servidor toma la configuración solo de las variables de entorno:

| Variable | Qué define | Si no está definida |
|---|---|---|
| `STOCKSHARP_UI_CATALOGUE` | ruta al catálogo de programas | archivo `applications.json` junto al servidor |
| `STOCKSHARP_UI_ARTIFACTS` | carpeta para las capturas | carpeta `artifacts` junto al servidor |

En un servidor instalado como herramienta de .NET, «junto al servidor» es la carpeta de almacenamiento de herramientas, por lo que hay que definir ambas variables explícitamente. Sin catálogo, el servidor solo puede conectarse a programas que ha iniciado otra persona.

Ejemplo de catálogo con un programa:

```json
{
  "applications": [
    {
      "appId": "stocksharp.terminal",
      "executable": "..\\Terminal\\Avalonia\\Terminal.Avalonia\\uiauto\\bin\\Release\\net10.0\\StockSharp.Terminal.Avalonia.exe",
      "arguments": "--ui-test",
      "description": "El terminal de negociación."
    }
  ]
}
```

- `appId`: identificador del programa de la tabla anterior. Si el programa iniciado comunica otro identificador, la conexión se rechaza.
- `executable`: ruta a la compilación con automatización. Una ruta relativa se cuenta a partir del archivo del catálogo.
- `arguments`: parámetros de inicio adicionales; el servidor les añade `--ui-automation` y `--ui-automation-endpoint`. La cadena se divide en parámetros por los espacios, así que aquí no se puede pasar una ruta con espacios.
- `description`: descripción que verá el agente.

En el archivo del catálogo se pueden escribir comentarios `//`.

## Perfil de prueba

Los programas de StockSharp se conectan a brókeres, bolsas y fuentes de datos, por lo que solo aceptan los clics y la escritura del agente cuando se inician con un perfil de prueba. Sin él, el programa responde a la lectura y a las capturas, pero rechaza los clics, la escritura de texto, las teclas, el desplazamiento y el arrastre con el error `inputUnavailable`: `Input is delivered only to a run started on a test profile.` La excepción es UDP Dumper: al iniciarse no se conecta a nada, y la entrada siempre está permitida.

El perfil de prueba se activa con parámetros de inicio; en el catálogo se escriben en `arguments`.

| Parámetro | Qué hace |
|---|---|
| `--ui-test` | inicia el programa con una carpeta de configuración aparte |
| `--ui-test-dir=<ruta>` | define esa carpeta; sin este parámetro se crea una nueva carpeta `ui-test\<identificador de ejecución>` junto al ejecutable |
| `--ui-test-lang=<código de idioma>` | idioma de la interfaz, por ejemplo, `es` |
| `--ui-test-view=<identificador>` | sección en la que se abrirá el programa; qué significa el identificador lo decide el propio programa |

Con un perfil de prueba, el programa lo guarda todo en la carpeta del perfil, no lee ni cambia la configuración del usuario del equipo y empieza con la disposición predeterminada. Terminal, Hydra, Designer e Installer también trasladan al perfil la carpeta común de StockSharp, donde se guarda el inicio de sesión en la cuenta, por lo que con un perfil de prueba no inician sesión en la cuenta. En las versiones Avalonia de los programas, los diálogos de archivos responden desde la subcarpeta `Files` del perfil: coloque allí el archivo que debe abrir el programa y busque allí mismo lo que haya guardado.

> [!WARNING]
> El perfil de prueba es una carpeta de configuración aparte, no un entorno aislado. Si se colocan en el perfil los ajustes de conexión o si el propio agente los introduce, el programa se conectará de verdad, y los clics del agente se seguirán aceptando. Pulsar el botón de compra en Terminal con un perfil así registrará una orden.

## Conexión del agente

Claude Code: archivo `.mcp.json` del proyecto. Ejemplo de la descripción del servidor, para una compilación desde el código fuente:

```json
{
  "mcpServers": {
    "desktop-driver": {
      "command": "DesktopDriver.Mcp/bin/Release/net10.0/StockSharp.DesktopDriver.Mcp.exe",
      "env": {
        "STOCKSHARP_UI_CATALOGUE": "DesktopDriver.Mcp/applications.json"
      }
    }
  }
}
```

Para un servidor instalado como herramienta de .NET, indique en `command` la ruta a `desktop-driver-mcp` y defina ambas variables:

```json
{
  "mcpServers": {
    "desktop-driver": {
      "command": "tools/desktop-driver-mcp",
      "env": {
        "STOCKSHARP_UI_CATALOGUE": "<ruta al catálogo de programas>",
        "STOCKSHARP_UI_ARTIFACTS": "<carpeta para las capturas>"
      }
    }
  }
}
```

Codex: archivo `~/.codex/config.toml`:

```toml
[mcp_servers.desktop-driver]
command = "DesktopDriver.Mcp/bin/Release/net10.0/StockSharp.DesktopDriver.Mcp.exe"
env = { STOCKSHARP_UI_CATALOGUE = "DesktopDriver.Mcp/applications.json" }
```

Claude Desktop: instale la extensión como se describe más arriba; el catálogo y la carpeta para las capturas se definen en su configuración.

## Qué puede hacer el agente

Todas las herramientas, salvo `ui_list_applications`, `ui_start_application` y `ui_attach_application`, nombran el programa con el parámetro `instance`, el nombre que devolvieron `ui_start_application` o `ui_attach_application`. Una segunda copia iniciada del mismo programa recibe un nombre con el sufijo `#2`.

Un elemento de la interfaz se nombra con una dirección `área/identificador`, por ejemplo, `panel:OrdersPanel/OrdersGrid`, la tabla del panel de órdenes de Terminal. Las direcciones las devuelven `ui_windows`, `ui_find` y `ui_tree`. La dirección no cambia cuando el panel se mueve a otro grupo, se desacopla en una ventana aparte o se reorganiza.

### Programas

| Herramienta | Qué hace |
|---|---|
| `ui_list_applications` | programas del catálogo: si cada uno está compilado y qué copias suyas ya están en ejecución |
| `ui_start_application` | inicia un programa del catálogo y espera a que su ventana se dibuje (hasta 90 segundos) |
| `ui_attach_application` | se conecta a un programa que alguien ha iniciado con `--ui-automation`; necesita el archivo de dirección que este ha escrito |
| `ui_release_application` | libera el programa: si lo inició este servidor, lo cierra; si es ajeno, lo deja funcionando |
| `ui_session` | qué programa ha respondido y qué sabe hacer: versión, perfil de prueba, método de entrada, límite de tamaño de la respuesta |

### Qué hay en pantalla

| Herramienta | Qué hace |
|---|---|
| `ui_windows` | ventanas abiertas y ventanas emergentes con las direcciones de sus elementos raíz |
| `ui_find` | búsqueda de elementos por nombre, tipo (`grid`, `chart`, `orderBook`, `propertyEditor`, `workspace`, `button`, `textBox` y otros), identificador de automatización o texto |
| `ui_tree` | árbol de elementos, limitado en profundidad y cantidad |
| `ui_snapshot` | todo el estado de un elemento: si existe, si está disponible, si es visible, si tiene el foco, así como el estado propio de su tipo: número de filas y ordenación de una tabla, rangos de un gráfico, mejores precios de un libro de órdenes |

### Lectura de elementos

| Herramienta | Qué hace |
|---|---|
| `ui_grid_columns`, `ui_grid_rows`, `ui_grid_groups` | columnas, filas y grupos de una tabla en el orden en que los muestra la propia tabla tras ordenar, filtrar y agrupar |
| `ui_chart_series`, `ui_chart_points` | series del gráfico y los puntos que realmente ha dibujado; hora en UTC |
| `ui_order_book_levels` | niveles del libro de órdenes por lado y precio |
| `ui_property_items` | propiedades que muestra el editor de propiedades: valor, texto en pantalla, tipo de editor, errores |
| `ui_tree_items` | elementos de un árbol por su ruta |
| `ui_dock_layout` | disposición del área de trabajo: paneles, grupos de pestañas, ventanas flotantes |
| `ui_document_content` | líneas del editor de código o de la vista de Markdown, numeradas desde uno |
| `ui_diagram_nodes`, `ui_diagram_connections` | cubos del esquema de la estrategia y conexiones entre ellos tal como están dibujados |

### Acciones

| Herramienta | Qué hace |
|---|---|
| `ui_click` | clic en un elemento o en una parte suya: encabezado de columna, celda, pestaña de panel y su botón de cierre, elemento de lista o de árbol, propiedad y, en un gráfico, sobre un precio o sobre una orden; se pueden mantener pulsadas `Shift`, `Control`, `Alt`, `Meta` |
| `ui_drag` | arrastre dentro de un mismo elemento, por ejemplo, de la línea de una orden en el gráfico a otro precio |
| `ui_type_text` | escritura de texto carácter a carácter: añadirlo al existente (`Append`) o sustituirlo (`Replace`) |
| `ui_press_key` | pulsación de una tecla, por ejemplo, `Enter`, `Escape`, `Tab`, `Delete`, opcionalmente junto con `Shift`, `Control`, `Alt`, `Meta` |
| `ui_scroll` | desplazamiento con la rueda un número dado de pasos |
| `ui_hover` | coloca el puntero sobre un elemento o una parte suya sin pulsar nada: aparece lo que solo se ve bajo el puntero (botones de la fila, descripción emergente, cruz del gráfico) |
| `ui_leave` | aparta el puntero de la ventana en la que está el elemento: ya nada en la ventana aparece resaltado como señalado. Se necesita antes de una captura de la ventana «en reposo» |
| `ui_bring_into_view` | muestra en pantalla una parte del elemento, por ejemplo, una fila muy abajo en una tabla, sin enviar entrada |

### Comprobación del resultado

| Herramienta | Qué hace |
|---|---|
| `ui_wait` | espera a que un elemento aparezca o desaparezca, o a que un campo de la respuesta de `ui_snapshot` tome el valor deseado |
| `ui_action_status` | qué ha pasado con una entrada ya enviada, según el identificador del acuse |

### Capturas y diagnóstico

| Herramienta | Qué hace |
|---|---|
| `ui_screenshot` | captura de un elemento en un archivo; la respuesta es la ruta al archivo y los parámetros de la captura |
| `ui_screenshot_image` | captura de un elemento que se devuelve al agente como imagen |
| `ui_diagnostics` | entradas del propio módulo de automatización: rechazos, lo que no se pudo leer y la entrada que no se pudo entregar |

> [!NOTE]
> `ui_drag`, `ui_hover`, `ui_leave`, el clic con teclas mantenidas, el clic sobre un precio, sobre una orden y sobre la cruz del panel en el gráfico y el marco `highlight` en las capturas están en el código fuente del servidor, pero no en el paquete publicado de la versión 1.0.1.

La lectura no cambia nada: el servidor no abre un panel que nunca se ha mostrado, no despliega una propiedad ni desplaza una tabla. Si por eso no se puede obtener la respuesta, se indica en la propia respuesta. El tamaño de una respuesta está limitado (de forma predeterminada, 256 KB), por lo que las tablas grandes se leen por partes, con los parámetros `limit`, `startIndex`, `columns` y `rowKeys`.

## Limitaciones y seguridad

- **Solo programas del catálogo.** El agente elige un programa del catálogo y no puede pasar al servidor su propia ruta a un archivo.
- **Solo este equipo y este usuario.** El canal solo es accesible para los procesos del usuario actual del sistema operativo. Al conectarse, el servidor comprueba el programa, su copia y la versión del protocolo.
- **Clics solo con perfil de prueba.** Sin perfil de prueba, el programa responde a la lectura y a las capturas, pero rechaza la entrada (consulte «Perfil de prueba»). Si la entrada está permitida lo muestra `ui_session`: `safeTestProfile` es `true` e `inputBackend` no es `none`.
- **La entrada no toca el ratón ni el teclado del equipo.** Los eventos llegan a la canalización interna de entrada del programa, por lo que el foco y los controladores reaccionan igual que cuando pulsa una persona. Un clic izquierdo normal en un elemento completo se ejecuta como una acción de automatización de ese elemento. Antes de enviar, el controlador comprueba que el elemento es visible, está disponible y no está tapado por nada.
- **«Entregado» no significa «hecho».** El acuse `dispatched` solo significa que la entrada ha llegado al programa. Si el programa ha hecho lo esperado se comprueba con `ui_snapshot` y `ui_wait`.
- **El puntero, como el de una persona.** Cada acción con el puntero lo lleva a un nuevo sitio, y los elementos de los que sale y a los que llega se enteran igual que con un ratón real: el botón resaltado se apaga y la cruz del gráfico desaparece cuando el puntero sale del gráfico.
- **Las versiones WPF** rechazan el clic con teclas mantenidas, el arrastre, `ui_hover` y `ui_leave`, en lugar de sustituirlos por un clic simple.
- **Órdenes desde el gráfico.** Un clic con `Control` sobre un precio en el gráfico registra una orden. En un perfil de prueba con una conexión configurada, es una orden real.
- **Cierre sin guardar.** `ui_release_application` cierra el programa iniciado por este servidor sin ofrecer guardar nada. Un programa al que el servidor solo se ha conectado no lo cierra.
- **Las capturas** se escriben en la carpeta `STOCKSHARP_UI_ARTIFACTS`, un archivo `<identificador de la captura>.png` por captura. Si se define el límite `maxPixelWidth` o `maxPixelHeight`, una captura demasiado grande se rechaza en lugar de reducirse.

## Ejemplo de sesión

Terminal está compilado con automatización y el catálogo es como en el ejemplo anterior. El usuario pide: «Abre en Terminal la configuración del panel de órdenes y muestra qué campos tiene».

1. `ui_list_applications`: en el catálogo está `stocksharp.terminal`, compilado y sin iniciar.
2. `ui_start_application`, `appId` = `stocksharp.terminal`: el servidor inicia Terminal con un perfil de prueba y devuelve `instance` = `stocksharp.terminal`.
3. `ui_session`: `safeTestProfile` = `true`, la entrada está permitida.
4. `ui_wait`, `node` = `panel:BuySellPanel/SettingsButton`, `timeoutSeconds` = `180`: esperar a que Terminal construya el área de trabajo, ya que la ventana aparece antes que los paneles.
5. `ui_click`, `node` = `panel:BuySellPanel/SettingsButton`: abrir la configuración del panel de órdenes. El acuse es `dispatched`.
6. `ui_property_items`, `node` = `panel:BuySellPanel/SettingsPropertyGrid`: las propiedades con sus valores actuales, entre ellas `Security` (instrumento) y `Depth`.
7. `ui_screenshot` con el mismo `node`: la ruta al archivo con la captura del editor, para mostrársela al usuario.
8. `ui_release_application`: Terminal se cierra.

## Véase también

[Conexión de agentes de IA (MCP)](../mcp.md)

[Comandos de los programas (Studio.Mcp)](studio_mcp.md)
