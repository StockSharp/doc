# Controlo da interface (DesktopDriver)

O **DesktopDriver** é um servidor MCP através do qual o agente trabalha com as janelas de um programa em execução tal como uma pessoa: encontra painéis e botões, lê tabelas, gráficos, livros de ordens e editores de propriedades, prime, escreve texto, desloca e faz capturas.

O servidor lê a própria interface, e não uma imagem do ecrã. Uma tabela responde com as suas colunas e linhas; um gráfico, com os pontos que desenhou; um livro de ordens, com os seus níveis de preços. A resposta não depende do tema, do tipo de letra nem da língua do programa. As capturas destinam-se às pessoas; o conteúdo verifica-se lendo-o.

O servidor é distribuído como pacote NuGet `StockSharp.DesktopDriver.Mcp` e identifica-se como `StockSharp.DesktopDriver`; no registo MCP Registry consta como `io.github.StockSharp/desktop-driver`. O código-fonte está no repositório [DesktopDriver.Mcp](https://github.com/StockSharp/DesktopDriver.Mcp).

## Como funciona

1. O controlador é incluído no programa quando este é compilado. Numa compilação normal não existe.
2. Um programa com o controlador, iniciado com o parâmetro `--ui-automation`, abre um canal local (um pipe com nome acessível apenas ao utilizador atual do sistema operativo) e escreve o seu endereço no ficheiro indicado com o parâmetro `--ui-automation-endpoint=<caminho>`. O ficheiro aparece quando a janela já está construída e desenhada. Sem o parâmetro `--ui-automation`, o programa não abre nada.
3. O servidor MCP inicia o programa a partir do seu catálogo com estes parâmetros ou liga-se a um programa que outra pessoa iniciou. Ao ligar-se, verifica que respondeu o programa certo, a cópia certa e uma versão compatível do protocolo, e só depois executa as chamadas do agente.

As mesmas operações estão disponíveis a partir da linha de comandos, com o programa `desktop-driver` do pacote `StockSharp.DesktopDriver.Cli`.

## Que compilações se podem ligar

Só é possível ligar a um programa compilado com a automatização ativada, ou seja, com a propriedade `EnableUiAutomation=true`. Por predefinição, a propriedade está desativada, e uma compilação normal não tem nem controlador nem canal. Uma compilação normal iniciada com `--ui-automation` escreve no fluxo de erro padrão a mensagem `<identificador> was built without automation, so --ui-automation does nothing. Build it with EnableUiAutomation=true.` e funciona como sempre. O servidor MCP espera pelo canal desse programa durante 90 segundos, depois informa que o programa não arrancou e fecha-o.

A compilação com automatização faz-se a partir do código-fonte do programa, junto ao qual deve estar o repositório DesktopDriver.Mcp. É colocada numa pasta à parte, `uiauto\bin`, para não se misturar com a normal:

```sh
dotnet build Terminal/Avalonia/Terminal.Avalonia/Terminal.Avalonia.csproj -c Release -p:EnableUiAutomation=true
```

O executável dessa compilação é `Terminal/Avalonia/Terminal.Avalonia/uiauto/bin/Release/net10.0/StockSharp.Terminal.Avalonia.exe`.

| Programa | Identificador da versão Avalonia | Identificador da versão WPF | Cliques e escrita sem perfil de teste |
|---|---|---|---|
| [Terminal](../terminal.md) | `stocksharp.terminal` | `stocksharp.terminal.wpf` | são rejeitados |
| [Hydra](../hydra.md) | `stocksharp.hydra` | `stocksharp.hydra.wpf` | são rejeitados |
| [Designer](../designer.md) | `stocksharp.designer` | `stocksharp.designer.wpf` | são rejeitados |
| [Installer](../installer.md) | `stocksharp.installer` | `stocksharp.installer.wpf` | são rejeitados |
| [Shell](../shell.md) | `stocksharp.shell` | — | são rejeitados |
| [UDP Dumper](../udp_dumper.md) | `tool.udpdumper` | — | são aceites |

O programa comunica o seu identificador por si só, e o mesmo identificador é indicado no catálogo do servidor.

Nas versões Avalonia, o controlador lê tabelas, a área de trabalho com painéis, gráficos, o livro de ordens, o editor de propriedades, o esquema da estratégia, o editor de código e a vista de Markdown. Nas versões WPF, tabelas, painéis acopláveis, gráficos, o livro de ordens, o editor de propriedades, o esquema da estratégia, campos de entrada, o friso, menus e barras de ferramentas. Nas versões Avalonia do Terminal, do Hydra e do Designer, tudo o que está num painel é endereçado em relação a esse painel: `panel:<tipo de painel>/<identificador>`.

## Instalação do servidor

### Ferramenta .NET

É necessário o [SDK do .NET 10](https://dotnet.microsoft.com/download/dotnet/10.0):

```sh
dotnet tool install StockSharp.DesktopDriver.Mcp --tool-path tools
```

O comando do servidor é `tools/desktop-driver-mcp`. O servidor escreve o protocolo na saída padrão e o registo, no fluxo de erro padrão. Iniciado manualmente, sem cliente, limita-se a esperar por uma entrada.

### A partir do código-fonte

```sh
dotnet build DesktopDriver.Mcp/DesktopDriver.Mcp.csproj -c Release
```

### Extensão para o Claude Desktop

Para o Claude Desktop, o servidor é empacotado numa extensão: o ficheiro `stocksharp-desktop-driver-<versão>.mcpb`, gerado pelo script `scripts/Pack-McpBundle.ps1` do repositório DesktopDriver.Mcp. A extensão inicia o servidor através do .NET instalado no computador, pelo que é necessário o SDK do .NET 10 acessível através do `PATH`.

1. No Claude Desktop, abra **Settings → Extensions → Advanced settings → Install Extension** e selecione o ficheiro da extensão.
2. Indique o catálogo de programas (um ficheiro JSON, ver abaixo) e a pasta para as capturas.

## Catálogo de programas

O agente não pode iniciar um ficheiro qualquer. O servidor só inicia programas do catálogo, um ficheiro JSON escrito por quem configura o servidor. O agente designa o programa pelo identificador, e não por um caminho.

O servidor obtém as definições apenas a partir das variáveis de ambiente:

| Variável | O que define | Se não estiver definida |
|---|---|---|
| `STOCKSHARP_UI_CATALOGUE` | caminho para o catálogo de programas | ficheiro `applications.json` junto ao servidor |
| `STOCKSHARP_UI_ARTIFACTS` | pasta para as capturas | pasta `artifacts` junto ao servidor |

Num servidor instalado como ferramenta .NET, «junto ao servidor» é a pasta de armazenamento das ferramentas, pelo que é necessário definir ambas as variáveis explicitamente. Sem catálogo, o servidor só pode ligar-se a programas que outra pessoa iniciou.

Exemplo de catálogo com um programa:

```json
{
  "applications": [
    {
      "appId": "stocksharp.terminal",
      "executable": "..\\Terminal\\Avalonia\\Terminal.Avalonia\\uiauto\\bin\\Release\\net10.0\\StockSharp.Terminal.Avalonia.exe",
      "arguments": "--ui-test",
      "description": "O terminal de negociação."
    }
  ]
}
```

- `appId`: identificador do programa da tabela acima. Se o programa iniciado comunicar outro identificador, a ligação é rejeitada.
- `executable`: caminho para a compilação com automatização. Um caminho relativo conta-se a partir do ficheiro do catálogo.
- `arguments`: parâmetros de arranque adicionais; o servidor acrescenta-lhes `--ui-automation` e `--ui-automation-endpoint`. A cadeia é dividida em parâmetros pelos espaços, pelo que aqui não é possível passar um caminho com espaços.
- `description`: descrição que o agente verá.

No ficheiro do catálogo é possível escrever comentários `//`.

## Perfil de teste

Os programas StockSharp ligam-se a corretoras, bolsas e fontes de dados, pelo que só aceitam os cliques e a escrita do agente quando são iniciados com um perfil de teste. Sem ele, o programa responde à leitura e às capturas, mas rejeita os cliques, a escrita de texto, as teclas, o deslocamento e o arrasto com o erro `inputUnavailable`: `Input is delivered only to a run started on a test profile.` A exceção é o UDP Dumper: ao arrancar não se liga a nada, e a entrada é sempre permitida.

O perfil de teste ativa-se com parâmetros de arranque; no catálogo escrevem-se em `arguments`.

| Parâmetro | O que faz |
|---|---|
| `--ui-test` | inicia o programa com uma pasta de definições à parte |
| `--ui-test-dir=<caminho>` | define essa pasta; sem este parâmetro é criada uma nova pasta `ui-test\<identificador da execução>` junto ao executável |
| `--ui-test-lang=<código da língua>` | língua da interface, por exemplo, `pt` |
| `--ui-test-view=<identificador>` | secção em que o programa abre; o que significa o identificador decide-o o próprio programa |

Com um perfil de teste, o programa guarda tudo na pasta do perfil, não lê nem altera as definições do utilizador do computador e começa com a disposição predefinida. O Terminal, o Hydra, o Designer e o Installer também transferem para o perfil a pasta comum da StockSharp, onde fica guardado o início de sessão na conta, pelo que, com um perfil de teste, não iniciam sessão na conta. Nas versões Avalonia dos programas, as caixas de diálogo de ficheiros respondem a partir da subpasta `Files` do perfil: coloque aí o ficheiro que o programa deve abrir e procure aí mesmo o que ele tiver guardado.

> [!WARNING]
> O perfil de teste é uma pasta de definições à parte, e não um ambiente isolado. Se as definições de ligação forem colocadas no perfil ou se o próprio agente as introduzir, o programa liga-se a sério, e os cliques do agente continuam a ser aceites. Premir o botão de compra no Terminal com um perfil assim registará uma ordem.

## Ligação do agente

Claude Code: ficheiro `.mcp.json` do projeto. Exemplo da descrição do servidor, para uma compilação a partir do código-fonte:

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

Para um servidor instalado como ferramenta .NET, indique em `command` o caminho para `desktop-driver-mcp` e defina ambas as variáveis:

```json
{
  "mcpServers": {
    "desktop-driver": {
      "command": "tools/desktop-driver-mcp",
      "env": {
        "STOCKSHARP_UI_CATALOGUE": "<caminho para o catálogo de programas>",
        "STOCKSHARP_UI_ARTIFACTS": "<pasta para as capturas>"
      }
    }
  }
}
```

Codex: ficheiro `~/.codex/config.toml`:

```toml
[mcp_servers.desktop-driver]
command = "DesktopDriver.Mcp/bin/Release/net10.0/StockSharp.DesktopDriver.Mcp.exe"
env = { STOCKSHARP_UI_CATALOGUE = "DesktopDriver.Mcp/applications.json" }
```

Claude Desktop: instale a extensão como descrito acima; o catálogo e a pasta para as capturas definem-se nas suas definições.

## O que o agente pode fazer

Todas as ferramentas, exceto `ui_list_applications`, `ui_start_application` e `ui_attach_application`, designam o programa com o parâmetro `instance`, o nome devolvido por `ui_start_application` ou `ui_attach_application`. Uma segunda cópia iniciada do mesmo programa recebe um nome com o sufixo `#2`.

Um elemento da interface designa-se por um endereço `área/identificador`, por exemplo, `panel:OrdersPanel/OrdersGrid`, a tabela do painel de ordens do Terminal. Os endereços são devolvidos por `ui_windows`, `ui_find` e `ui_tree`. O endereço não muda quando o painel é movido para outro grupo, desacoplado numa janela à parte ou reorganizado.

### Programas

| Ferramenta | O que faz |
|---|---|
| `ui_list_applications` | programas do catálogo: se cada um está compilado e que cópias dele já estão em execução |
| `ui_start_application` | inicia um programa do catálogo e espera que a sua janela seja desenhada (até 90 segundos) |
| `ui_attach_application` | liga-se a um programa que alguém iniciou com `--ui-automation`; precisa do ficheiro de endereço que este escreveu |
| `ui_release_application` | liberta o programa: se foi iniciado por este servidor, fecha-o; se é alheio, deixa-o a funcionar |
| `ui_session` | que programa respondeu e o que sabe fazer: versão, perfil de teste, método de entrada, limite do tamanho da resposta |

### O que está no ecrã

| Ferramenta | O que faz |
|---|---|
| `ui_windows` | janelas abertas e janelas pop-up com os endereços dos seus elementos raiz |
| `ui_find` | pesquisa de elementos por nome, tipo (`grid`, `chart`, `orderBook`, `propertyEditor`, `workspace`, `button`, `textBox` e outros), identificador de automatização ou texto |
| `ui_tree` | árvore de elementos, limitada em profundidade e quantidade |
| `ui_snapshot` | todo o estado de um elemento: se existe, se está disponível, se está visível, se tem o foco, bem como o estado próprio do seu tipo: número de linhas e ordenação de uma tabela, intervalos de um gráfico, melhores preços de um livro de ordens |

### Leitura de elementos

| Ferramenta | O que faz |
|---|---|
| `ui_grid_columns`, `ui_grid_rows`, `ui_grid_groups` | colunas, linhas e grupos de uma tabela pela ordem em que a própria tabela os mostra após ordenar, filtrar e agrupar |
| `ui_chart_series`, `ui_chart_points` | séries do gráfico e os pontos que ele realmente desenhou; hora em UTC |
| `ui_order_book_levels` | níveis do livro de ordens por lado e preço |
| `ui_property_items` | propriedades mostradas pelo editor de propriedades: valor, texto no ecrã, tipo de editor, erros |
| `ui_tree_items` | elementos de uma árvore pelo seu caminho |
| `ui_dock_layout` | disposição da área de trabalho: painéis, grupos de separadores, janelas flutuantes |
| `ui_document_content` | linhas do editor de código ou da vista de Markdown, numeradas a partir de um |
| `ui_diagram_nodes`, `ui_diagram_connections` | cubos do esquema da estratégia e ligações entre eles tal como estão desenhados |

### Ações

| Ferramenta | O que faz |
|---|---|
| `ui_click` | clique num elemento ou numa parte dele: cabeçalho de coluna, célula, separador de painel e o seu botão de fecho, elemento de lista ou de árvore, propriedade e, num gráfico, sobre um preço ou sobre uma ordem; é possível manter premidas `Shift`, `Control`, `Alt`, `Meta` |
| `ui_drag` | arrasto dentro de um mesmo elemento, por exemplo, da linha de uma ordem no gráfico para outro preço |
| `ui_type_text` | escrita de texto carácter a carácter: acrescentá-lo ao existente (`Append`) ou substituí-lo (`Replace`) |
| `ui_press_key` | premir uma tecla, por exemplo, `Enter`, `Escape`, `Tab`, `Delete`, opcionalmente em conjunto com `Shift`, `Control`, `Alt`, `Meta` |
| `ui_scroll` | deslocamento com a roda num número dado de passos |
| `ui_hover` | coloca o ponteiro sobre um elemento ou uma parte dele sem premir nada: aparece o que só se vê sob o ponteiro (botões da linha, dica, cruz do gráfico) |
| `ui_leave` | afasta o ponteiro da janela onde está o elemento: nada na janela fica destacado como apontado. É necessário antes de uma captura da janela «em repouso» |
| `ui_bring_into_view` | mostra no ecrã uma parte do elemento, por exemplo, uma linha muito abaixo numa tabela, sem enviar entrada |

### Verificação do resultado

| Ferramenta | O que faz |
|---|---|
| `ui_wait` | espera que um elemento apareça ou desapareça, ou que um campo da resposta de `ui_snapshot` assuma o valor pretendido |
| `ui_action_status` | o que aconteceu a uma entrada já enviada, segundo o identificador do recibo |

### Capturas e diagnóstico

| Ferramenta | O que faz |
|---|---|
| `ui_screenshot` | captura de um elemento para um ficheiro; a resposta é o caminho do ficheiro e os parâmetros da captura |
| `ui_screenshot_image` | captura de um elemento que é devolvida ao agente como imagem |
| `ui_diagnostics` | entradas do próprio módulo de automatização: recusas, o que não foi possível ler e a entrada que não foi possível entregar |

> [!NOTE]
> `ui_drag`, `ui_hover`, `ui_leave`, o clique com teclas mantidas, o clique sobre um preço, sobre uma ordem e sobre a cruz do painel no gráfico e a moldura `highlight` nas capturas existem no código-fonte do servidor, mas não no pacote publicado da versão 1.0.1.

A leitura não altera nada: o servidor não abre um painel que nunca foi mostrado, não expande uma propriedade nem desloca uma tabela. Se por isso não for possível obter a resposta, isso é indicado na própria resposta. O tamanho de uma resposta é limitado (por predefinição, 256 KB), pelo que as tabelas grandes são lidas por partes, com os parâmetros `limit`, `startIndex`, `columns` e `rowKeys`.

## Limitações e segurança

- **Apenas programas do catálogo.** O agente escolhe um programa do catálogo e não pode passar ao servidor o seu próprio caminho para um ficheiro.
- **Apenas este computador e este utilizador.** O canal só é acessível aos processos do utilizador atual do sistema operativo. Ao ligar-se, o servidor verifica o programa, a sua cópia e a versão do protocolo.
- **Cliques apenas com perfil de teste.** Sem perfil de teste, o programa responde à leitura e às capturas, mas rejeita a entrada (ver «Perfil de teste»). Se a entrada é permitida, mostra-o `ui_session`: `safeTestProfile` é `true` e `inputBackend` não é `none`.
- **A entrada não mexe no rato nem no teclado do computador.** Os eventos chegam ao pipeline interno de entrada do programa, pelo que o foco e os manipuladores reagem tal como quando uma pessoa prime. Um clique esquerdo normal num elemento inteiro é executado como uma ação de automatização desse elemento. Antes de enviar, o controlador verifica que o elemento está visível, disponível e não está tapado por nada.
- **«Entregue» não significa «feito».** O recibo `dispatched` significa apenas que a entrada chegou ao programa. Se o programa fez o esperado verifica-se com `ui_snapshot` e `ui_wait`.
- **O ponteiro, como o de uma pessoa.** Cada ação com o ponteiro leva-o para um novo sítio, e os elementos de onde sai e aonde chega sabem-no tal como com um rato real: o botão destacado apaga-se e a cruz do gráfico desaparece quando o ponteiro sai do gráfico.
- **As versões WPF** rejeitam o clique com teclas mantidas, o arrasto, `ui_hover` e `ui_leave`, em vez de os substituírem por um clique simples.
- **Ordens a partir do gráfico.** Um clique com `Control` sobre um preço no gráfico regista uma ordem. Num perfil de teste com uma ligação configurada, é uma ordem real.
- **Fecho sem guardar.** `ui_release_application` fecha o programa iniciado por este servidor sem propor guardar nada. Um programa ao qual o servidor apenas se ligou não é fechado.
- **As capturas** são escritas na pasta `STOCKSHARP_UI_ARTIFACTS`, um ficheiro `<identificador da captura>.png` por captura. Se for definido o limite `maxPixelWidth` ou `maxPixelHeight`, uma captura demasiado grande é rejeitada em vez de ser reduzida.

## Exemplo de sessão

O Terminal está compilado com automatização e o catálogo é como no exemplo acima. O utilizador pede: «Abre no Terminal as definições do painel de ordens e mostra que campos tem».

1. `ui_list_applications`: no catálogo está `stocksharp.terminal`, compilado e não iniciado.
2. `ui_start_application`, `appId` = `stocksharp.terminal`: o servidor inicia o Terminal com um perfil de teste e devolve `instance` = `stocksharp.terminal`.
3. `ui_session`: `safeTestProfile` = `true`, a entrada é permitida.
4. `ui_wait`, `node` = `panel:BuySellPanel/SettingsButton`, `timeoutSeconds` = `180`: esperar que o Terminal construa a área de trabalho, já que a janela aparece antes dos painéis.
5. `ui_click`, `node` = `panel:BuySellPanel/SettingsButton`: abrir as definições do painel de ordens. O recibo é `dispatched`.
6. `ui_property_items`, `node` = `panel:BuySellPanel/SettingsPropertyGrid`: as propriedades com os seus valores atuais, entre elas `Security` (instrumento) e `Depth`.
7. `ui_screenshot` com o mesmo `node`: o caminho para o ficheiro com a captura do editor, para o mostrar ao utilizador.
8. `ui_release_application`: o Terminal fecha-se.

## Ver também

[Ligação de agentes de IA (MCP)](../mcp.md)

[Comandos dos programas (Studio.Mcp)](studio_mcp.md)
