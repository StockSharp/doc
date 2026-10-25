# Conexión de agentes de IA (MCP)

**MCP** (Model Context Protocol) es un protocolo abierto mediante el cual un agente de IA (Claude Code, Claude Desktop o cualquier otro cliente MCP) llama a las herramientas de un programa externo, el servidor MCP. El propio cliente inicia el servidor como proceso hijo e intercambia mensajes con él a través de la entrada y la salida estándar. El servidor comunica qué herramientas tiene, qué hace cada una y qué parámetros acepta, y el agente decide qué herramienta llamar y recibe la respuesta.

Para los programas de StockSharp hay dos servidores MCP. Ambos funcionan solo en el mismo equipo que los programas y se comunican solo con los programas de la misma cuenta de usuario.

| | [DesktopDriver](mcp/desktop_driver.md) | [Studio.Mcp](mcp/studio_mcp.md) |
|---|---|---|
| Qué hace | lee la interfaz del programa, pulsa botones, escribe texto, desplaza y toma capturas | envía comandos al programa: registrar o cancelar una orden, iniciar una fuente de Hydra, iniciar la prueba de una estrategia |
| Con qué programas | Terminal, Hydra, Designer e Installer (versiones Avalonia y WPF), Shell y UDP Dumper, solo en una compilación con la automatización activada | versiones Avalonia de Terminal, Hydra, Designer y Shell, en una compilación normal |
| Qué ve el agente | ventanas, paneles, tablas, gráficos, libros de órdenes, editores de propiedades, esquemas de estrategias | solo lo que devuelven los comandos: listas de fuentes, estrategias y órdenes |
| Límite de acciones | los clics y la escritura solo los acepta un programa iniciado con un perfil de prueba (salvo UDP Dumper, que los acepta siempre) | no hay límite por perfil: la orden va adonde esté conectado Terminal |
| Dónde obtener el servidor | paquete NuGet `StockSharp.DesktopDriver.Mcp`, extensión para Claude Desktop | se compila a partir del código fuente |

## Qué servidor elegir

- Si hay que comprobar qué muestra una ventana, recorrer un escenario con botones tal como lo haría una persona u obtener una captura de un panel: [DesktopDriver](mcp/desktop_driver.md).
- Si hay que conseguir que el programa ejecute una acción por comando, sin clics (registrar una orden, iniciar la descarga con una fuente de Hydra, iniciar la prueba de una estrategia): [Studio.Mcp](mcp/studio_mcp.md).
- Si el programa está compilado de la forma habitual, sin automatización, solo sirve Studio.Mcp, y solo para las versiones Avalonia de los programas.
- Para la versión WPF de un programa, solo DesktopDriver, y solo en una compilación con automatización.

Ambos servidores pueden conectarse al agente a la vez. Por ejemplo, Studio.Mcp inicia la prueba de una estrategia en Designer, pero no devuelve los resultados; el agente puede leerlos en los paneles de Designer mediante DesktopDriver, si Designer está compilado con automatización.

MCP no es lo mismo que el [control remoto](shell/remote_control.md) desde el sitio web y desde el [bot de Telegram](telegram_services/control_panel.md). El control remoto funciona a través de la cuenta de StockSharp y no necesita ni agente ni servidor MCP.

## Véase también

[Control de la interfaz (DesktopDriver)](mcp/desktop_driver.md)

[Comandos de los programas (Studio.Mcp)](mcp/studio_mcp.md)

[Desarrollo asistido por IA](api/ai_development.md)
