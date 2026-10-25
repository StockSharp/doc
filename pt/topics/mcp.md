# Ligação de agentes de IA (MCP)

O **MCP** (Model Context Protocol) é um protocolo aberto através do qual um agente de IA (Claude Code, Claude Desktop ou qualquer outro cliente MCP) chama as ferramentas de um programa externo, o servidor MCP. O próprio cliente inicia o servidor como processo filho e troca mensagens com ele através da entrada e da saída padrão. O servidor comunica que ferramentas tem, o que faz cada uma e que parâmetros aceita, e o agente decide que ferramenta chamar e recebe a resposta.

Para os programas StockSharp existem dois servidores MCP. Ambos funcionam apenas no mesmo computador que os programas e comunicam apenas com os programas da mesma conta de utilizador.

| | [DesktopDriver](mcp/desktop_driver.md) | [Studio.Mcp](mcp/studio_mcp.md) |
|---|---|---|
| O que faz | lê a interface do programa, prime botões, escreve texto, desloca e faz capturas | envia comandos ao programa: registar ou cancelar uma ordem, iniciar uma fonte do Hydra, iniciar o teste de uma estratégia |
| Com que programas | Terminal, Hydra, Designer e Installer (versões Avalonia e WPF), Shell e UDP Dumper, apenas numa compilação com a automatização ativada | versões Avalonia do Terminal, Hydra, Designer e Shell, numa compilação normal |
| O que o agente vê | janelas, painéis, tabelas, gráficos, livros de ordens, editores de propriedades, esquemas de estratégias | apenas o que os comandos devolvem: listas de fontes, estratégias e ordens |
| Limite de ações | os cliques e a escrita só são aceites por um programa iniciado com um perfil de teste (exceto o UDP Dumper, que os aceita sempre) | não há limite por perfil: a ordem vai para onde o Terminal estiver ligado |
| Onde obter o servidor | pacote NuGet `StockSharp.DesktopDriver.Mcp`, extensão para o Claude Desktop | compila-se a partir do código-fonte |

## Que servidor escolher

- Se for preciso verificar o que uma janela mostra, percorrer um cenário com botões tal como faria uma pessoa ou obter uma captura de um painel: [DesktopDriver](mcp/desktop_driver.md).
- Se for preciso que o programa execute uma ação por comando, sem cliques (registar uma ordem, iniciar a transferência com uma fonte do Hydra, iniciar o teste de uma estratégia): [Studio.Mcp](mcp/studio_mcp.md).
- Se o programa estiver compilado da forma habitual, sem automatização, só serve o Studio.Mcp, e apenas para as versões Avalonia dos programas.
- Para a versão WPF de um programa, só o DesktopDriver, e apenas numa compilação com automatização.

Ambos os servidores podem ser ligados ao agente ao mesmo tempo. Por exemplo, o Studio.Mcp inicia o teste de uma estratégia no Designer, mas não devolve os resultados; o agente pode lê-los nos painéis do Designer através do DesktopDriver, se o Designer estiver compilado com automatização.

O MCP não é o mesmo que o [controlo remoto](shell/remote_control.md) a partir do site e do [bot do Telegram](telegram_services/control_panel.md). O controlo remoto funciona através da conta StockSharp e não precisa nem de agente nem de servidor MCP.

## Ver também

[Controlo da interface (DesktopDriver)](mcp/desktop_driver.md)

[Comandos dos programas (Studio.Mcp)](mcp/studio_mcp.md)

[Desenvolvimento Assistido por IA](api/ai_development.md)
