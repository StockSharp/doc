# Controlos de negociação JavaScript

[Controlos de negociação JavaScript da StockSharp](https://github.com/StockSharp/JS-TradingControls) é um conjunto de painéis para navegador destinados a um terminal de negociação. O pacote está publicado no npm como [`@stocksharp/trading-controls`](https://www.npmjs.com/package/@stocksharp/trading-controls) e todos os controlos podem ser vistos na [demonstração online](https://stocksharp.github.io/JS-TradingControls/demo/).

![Ecrã de negociação com fluxo de negócios, livro de ofertas, lista de instrumentos, introdução de ordens e tabelas](../../../images/javascript_trading_controls.jpg)

A imagem também mostra um gráfico de velas do pacote separado `@stocksharp/chart`. `@stocksharp/trading-controls` inclui sete controlos autónomos:

| Controlo | Classe | Identificador |
|---|---|---|
| [Ordens ativas](javascript_trading_controls/active_orders.md) | `ActiveOrdersWidget` | `activeOrders` |
| [Posições](javascript_trading_controls/positions.md) | `PositionsWidget` | `positions` |
| [Histórico de negócios](javascript_trading_controls/trade_history.md) | `TradeHistoryWidget` | `tradeHistory` |
| [Lista de instrumentos](javascript_trading_controls/watchlist.md) | `WatchlistWidget` | `watchlist` |
| [Introdução de ordens](javascript_trading_controls/order_entry.md) | `OrderEntryWidget` | `orderEntry` |
| [Livro de ofertas](javascript_trading_controls/order_book.md) | `OrderBookWidget` | `orderbook` |
| [Fluxo de negócios](javascript_trading_controls/trade_feed.md) | `TradeFeedWidget` | `tradefeed` |

Os valores dos identificadores estão disponíveis através do objeto exportado `ControlTypes`.

## Instalação

```bash
npm install @stocksharp/trading-controls
```

Os estilos principais são obrigatórios. A paleta clara e escura pronta pode ser ligada adicionalmente ou substituída pelas suas próprias variáveis CSS `--t-*`:

```ts
import '@stocksharp/trading-controls/styles.css';
import '@stocksharp/trading-controls/theme.css'; // Opcional: tema pronto.
```

Os controlos utilizam classes de [Bootstrap Icons](https://icons.getbootstrap.com/), mas não incluem os próprios tipos de letra nem SVG. A página anfitriã deve ligar os ícones separadamente.

Para uma página sem empacotador, utilize o ficheiro `dist/sstradingcontrols.js`, que cria o objeto global `window.SSTradingControls`.

## Esquema geral de criação

Cada controlo é criado pelo método estático `create`. O método valida o anfitrião, constrói o seu próprio DOM e adiciona o elemento raiz ao contentor fornecido:

```ts
import {
  PositionsWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const positions = PositionsWidget.create(
  document.querySelector<HTMLElement>('#positions')!,
  {},
  {
    host,
    closePosition: (portfolioId, instrumentId, symbol) =>
      console.log('fechar', portfolioId, instrumentId, symbol),
    reversePosition: (portfolioId, instrumentId, symbol) =>
      console.log('inverter', portfolioId, instrumentId, symbol),
    refreshPositions: () => console.log('atualizar'),
  },
);

positions.update([]);
```

O segundo argumento é o estado guardado da instância. O conjunto de dependências no terceiro argumento varia entre controlos: por exemplo, o painel de posições recebe os processadores de fecho e inversão, enquanto o livro de ofertas recebe os processadores de seleção e execução do preço.

## Contrato TradingHost

Os controlos não acedem diretamente a um tradutor global, ao armazenamento das definições, à ligação de negociação ou ao gestor de janelas. Toda a interação externa passa por um único objeto `TradingHost`.

| Membro do anfitrião | Finalidade |
|---|---|
| `isPrimary` | Indica a instância principal do controlo na página. |
| `t(key, ...args)` | Traduz o texto visível e insere os argumentos. |
| `presentation` | Formata a direção, o tipo e o estado da ordem, as classes de lucro e a paleta do canvas. |
| `preferences`, `cache` | Guardam definições persistentes e dados temporários. |
| `trading.api` | Procura instrumentos e carrega execuções. |
| `trading.marketData` | Gere subscrições e fornece ordens ativas. |
| `trading.portfolioId()` | Devolve a carteira atual. |
| `trading.pickInstrument(...)` | Abre a seleção de instrumentos. |
| `ticker` | Recebe os instrumentos visíveis e as respetivas cotações. |
| `allow(action)` | Verifica a permissão para uma ação. |
| `close`, `spawn`, `persistState`, `saveLayout` | Gerem o ciclo de vida e o estado do painel. |
| `register`, `unregister`, `broadcast` | Registam instâncias e distribuem alterações entre elas. |
| `log(message)` | Recebe mensagens de diagnóstico. |

Todos os membros são obrigatórios. `assertHost` verifica as funções aninhadas antes de apresentar o controlo e indica o caminho exato que falta. Se a aplicação não necessitar de algumas funcionalidades, podem ser fornecidas implementações alternativas adequadas para os comandos obrigatórios, por exemplo `log: console.warn` ou um `saveLayout` vazio.

## Localização e estilo

Os controlos obtêm todo o texto visível exclusivamente através de `host.t`. A lista completa e atual de 153 chaves é fornecida em `@stocksharp/trading-controls/translation-keys.json`. Uma chave desconhecida é apresentada ao utilizador sem alterações, pelo que o anfitrião deve definir traduções para toda a lista.

O ficheiro `styles.css` contém as regras, mas obtém as cores, os tipos de letra e as dimensões das variáveis CSS `--t-*`. Se o `theme.css` pronto não for utilizado, essas variáveis são definidas pela aplicação. As cores do canvas do livro de ofertas e do fluxo em bolhas são devolvidas por `host.presentation.canvasPalette()`.

## Libertação de recursos

Ao remover um painel, chame `dispose()`. O método remove os processadores, desliga os observadores e as subscrições do controlo quando existirem e, em seguida, chama `host.unregister`.

```ts
positions.dispose();
```

## Compilação a partir do código-fonte

```bash
git clone https://github.com/StockSharp/JS-TradingControls.git
cd JS-TradingControls
npm install
npm test
npm run build
```

## Consulte também

- [Tabelas JavaScript](javascript_grids.md)
- [Gráficos JavaScript](charts/javascript_charts.md)
- [Repositório JS-TradingControls](https://github.com/StockSharp/JS-TradingControls)
- [Demonstração online](https://stocksharp.github.io/JS-TradingControls/demo/)
