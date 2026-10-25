# Comandos dos programas (Studio.Mcp)

O **Studio.Mcp** é um servidor MCP através do qual o agente envia comandos aos programas StockSharp em execução: regista e cancela ordens no Terminal, gere as fontes de dados do Hydra e inicia o teste de estratégias no Designer e no Shell. O servidor não prime botões nem lê janelas: o comando é executado pelo próprio programa.

O servidor não inicia nada nem guarda nada. O comando só chega ao programa quando o programa está aberto; se não estiver em execução, o servidor indica-o na resposta.

## Que programas respondem

| Programa | Que comandos aceita |
|---|---|
| [Terminal](../terminal.md) | lista de ordens, registo e cancelamento de uma ordem |
| [Hydra](../hydra.md) | tipos de fontes, lista de fontes, adição, ativação, início, paragem e eliminação de uma fonte |
| [Designer](../designer.md) | lista de estratégias, início e paragem do teste no histórico |
| [Shell](../shell.md) | lista de estratégias da área [Emulação](../shell/user_interface/emulation.md), início e paragem da sua execução |

Só aceitam comandos as versões Avalonia destes programas, e numa compilação normal: não é necessária uma compilação à parte. O programa abre o canal de comandos juntamente com a janela principal e fecha-o quando a janela se fecha. As versões WPF do Terminal, do Hydra e do Designer não aceitam estes comandos.

O canal é um pipe com nome local, acessível apenas aos processos do utilizador atual do sistema operativo. Os comandos não são transmitidos pela rede.

## Instalação

O servidor compila-se a partir do código-fonte da StockSharp:

```sh
dotnet build Studio/Studio.Mcp/Studio.Mcp.csproj -c Release
```

O ficheiro resultante é `Studio/Studio.Mcp/bin/Release/net10.0/StockSharp.Studio.Mcp.exe`. O servidor não tem definições próprias: não precisa nem de argumentos nem de variáveis de ambiente. Escreve o protocolo na saída padrão e o registo, no fluxo de erro padrão; iniciado manualmente, sem cliente, limita-se a esperar por uma entrada.

## Ligação do agente

Claude Code: ficheiro `.mcp.json` do projeto:

```json
{
  "mcpServers": {
    "stocksharp": {
      "command": "Studio/Studio.Mcp/bin/Release/net10.0/StockSharp.Studio.Mcp.exe"
    }
  }
}
```

Noutros clientes MCP, incluindo o Claude Desktop, o servidor liga-se da mesma forma: o comando é o caminho para `StockSharp.Studio.Mcp.exe`, sem argumentos nem variáveis de ambiente.

## O que o agente pode fazer

Os identificadores de fontes, estratégias e ordens obtêm-se das respostas das ferramentas de lista: primeiro a lista, depois a ação.

### Gerais

| Ferramenta | O que faz |
|---|---|
| `list_products` | que programas estão em execução e que comandos aceita cada um |
| `describe_product` | comandos de um programa; `product` é `Terminal`, `Hydra`, `Designer` ou `Shell` |
| `close_product` | pede ao programa que se feche |

### Hydra

| Ferramenta | O que faz |
|---|---|
| `hydra_list_source_types` | tipos de fontes que o Hydra pode criar, com os seus identificadores |
| `hydra_list_sources` | fontes do Hydra: se cada uma está ativada e se está a transferir agora |
| `hydra_add_source` (`typeId`) | adiciona uma fonte do tipo escolhido e devolve-a; a nova fonte está desativada |
| `hydra_enable_source` (`sourceId`, `isEnabled`) | inclui a fonte na transferência ou exclui-a dela |
| `hydra_start_source` (`sourceId`) | inicia a transferência com a fonte; uma fonte desativada não é iniciada |
| `hydra_stop_source` (`sourceId`) | para a transferência |
| `hydra_remove_source` (`sourceId`) | elimina a fonte juntamente com as suas definições; uma fonte em funcionamento tem de ser parada primeiro |

Para transferir dados com uma nova fonte são necessárias três chamadas: `hydra_add_source`, `hydra_enable_source`, `hydra_start_source`. Estes comandos não alteram as definições da fonte (instrumentos, tipos de dados, parâmetros de ligação): estas definem-se na janela do Hydra (ver [Fontes de dados](../hydra/data_sources.md)).

### Designer

| Ferramenta | O que faz |
|---|---|
| `designer_list_strategies` | estratégias do Designer: tipo (`Backtest`, `Optimization`, `Live`, `Remote`, `Other`) e estado atual |
| `designer_start_backtest` (`strategyId`, `startTime`, `stopTime`) | inicia no histórico uma estratégia do tipo `Backtest` ou `Optimization`; o período indica-se em UTC no formato ISO 8601 e substitui o período das definições da estratégia; sem ele, é usado o período das definições |
| `designer_stop_backtest` (`strategyId`) | para uma estratégia em funcionamento |

### Terminal

| Ferramenta | O que faz |
|---|---|
| `terminal_list_orders` | ordens do Terminal com o número de transação de cada uma |
| `terminal_register_order` (`securityId`, `portfolioName`, `side`, `volume`, `price`, `orderType`) | regista uma ordem e devolve o seu número de transação |
| `terminal_cancel_order` (`orderTransactionId`) | cancela uma ordem pelo número de transação |

O instrumento indica-se tal como o Terminal o conhece, por exemplo, `SBER@TQBR`; a carteira, pelo nome que o Terminal conhece; `side` é `Buy` ou `Sell`. Uma ordem limitada precisa de um preço; nas ordens de outros tipos, o preço passado não é usado. A ordem é identificada pelo número de transação devolvido por `terminal_register_order` ou `terminal_list_orders`, e não pelo número que a bolsa lhe atribuirá mais tarde.

### Shell

As ferramentas do Shell são construídas pelo servidor a partir da descrição dos comandos enviada pelo próprio programa. Por isso só existem se o Shell estava em execução quando o servidor foi iniciado ou na última chamada a `list_products` ou `describe_product`.

| Ferramenta | O que faz |
|---|---|
| `shell_list_strategies` | estratégias da área «Emulação» e o seu estado |
| `shell_start_backtest` (`StrategyId`, `StartTime`, `StopTime`) | inicia a execução da estratégia no histórico |
| `shell_stop_backtest` (`StrategyId`) | para a execução |

O Shell testa sempre a estratégia com o período das suas definições: se passar `StartTime` ou `StopTime`, o comando é rejeitado, por isso passe `null` neles.

## Limitações e segurança

> [!CAUTION]
> `terminal_register_order` regista uma ordem real onde o Terminal estiver ligado. O servidor não pede confirmação: a confirmação cabe ao cliente MCP, no qual uma pessoa vê a chamada antes de ser executada.

- **Não são iniciadas estratégias que negoceiam a sério.** O Designer só inicia estratégias do tipo `Backtest` e `Optimization`; para as restantes, o comando é rejeitado. O Shell só abre aos comandos as estratégias da área «Emulação».
- **A eliminação é irreversível.** `hydra_remove_source` elimina a fonte juntamente com as suas definições.
- **Fecho.** `close_product` fecha o programa.
- **O programa tem de estar em execução.** Caso contrário, o comando devolve o erro `<programa> is not running.` A espera pela resposta de um programa que não está em execução é de cerca de 5 segundos, pelo que `list_products` com os programas fechados não responde de imediato. A recusa de um programa em execução chega ao agente como texto: `<programa> refused the command: <causa>`.
- **Sem palavra-passe.** O canal está aberto a qualquer programa iniciado com a mesma conta de utilizador: não há uma verificação à parte de quem enviou o comando.
- **As cópias dos programas não se distinguem.** Se estiverem em execução várias cópias do Terminal ou do Designer, ou se junto ao Hydra normal funcionar um Hydra com perfil de teste, o comando chega a uma delas, e não se sabe de antemão a qual.
- **Não há resultados do teste.** O servidor só devolve o estado da estratégia, e não as estatísticas nem os negócios. É possível lê-los nos painéis do programa com o [DesktopDriver](desktop_driver.md).

## Exemplo de sessão

O utilizador pede: «Inicia no Designer o teste da estratégia para janeiro e verifica que está a decorrer».

1. `list_products`: o Designer está em execução; sobre os restantes programas, `… is not running.`
2. `designer_list_strategies`: entre as estratégias está a pretendida, do tipo `Backtest`, no estado `Stopped`.
3. `designer_start_backtest`, `strategyId` do passo 2, `startTime` = `2026-01-01T00:00:00Z`, `stopTime` = `2026-02-01T00:00:00Z`: a resposta é `Strategy <identificador> was started.`
4. `designer_list_strategies`: o estado da estratégia mudou.
5. `designer_stop_backtest` com o mesmo `strategyId`, se for preciso interromper o teste.

## Ver também

[Ligação de agentes de IA (MCP)](../mcp.md)

[Controlo da interface (DesktopDriver)](desktop_driver.md)

[Controlo remoto](../shell/remote_control.md)
