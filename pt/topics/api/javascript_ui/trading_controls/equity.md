# Curva de capital

`EquityWidget` desenha o P&L acumulado de uma execução como um gráfico no tempo. O painel está registado com o identificador `equity` (`ControlTypes.Equity`) e a curva propriamente dita é construída pelo motor `@stocksharp/chart`, que entra como dependência de par (*peer*).

![Curva de capital com o resultado de uma execução](../../../../images/javascript_controls_equity.png)

## Criação e atualização

```ts
import {
  EquityWidget,
  type PnlPoint,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const equity = EquityWidget.create(
  document.querySelector<HTMLElement>('#equity')!,
  {},
  { host },
);

const start = Date.now() - 3 * 60 * 60 * 1000;

const points: PnlPoint[] = [
  { time: start, value: 0 },
  { time: start + 45 * 60 * 1000, value: 320.5 },
  { time: start + 95 * 60 * 1000, value: -140.25 },
  { time: Date.now(), value: 1_180.75 },
];

equity.update(points);
```

A única dependência de `EquityDeps` é `host`; o painel não tem processadores de ações porque não há nada para executar sobre a curva. O segundo argumento de `create` — o estado guardado da instância — não é utilizado pelo controlo: o painel não guarda definições próprias nem no estado nem em `host.preferences`.

`update` substitui a execução inteira. O valor da curva é acumulado, pelo que um conjunto de pontos sem a amostra anterior é considerado outra execução e não a continuação da mesma.

## Dados e desenho

O campo `time` em `PnlPoint` é dado em milissegundos Unix; o motor conta o tempo em segundos e a conversão é feita pelo próprio controlo. Os pontos são ordenados por tempo, as amostras com `time` ou `value` não numéricos são descartadas e, de vários valores dentro do mesmo segundo, fica o último — é ele que mostra onde a execução realmente estava nesse momento.

Enquanto houver menos de dois pontos utilizáveis, o painel mostra a mensagem da chave `NoEquity`, o gráfico não é criado e `chart()` devolve `null`. O gráfico é construído no primeiro desenho e não no construtor: o motor precisa de um contentor já colocado na página. As alterações de tamanho da área são acompanhadas por um `ResizeObserver`, que chama o `resize` do motor.

As cores, o tipo de letra e a cor da grelha vêm de `host.presentation.canvasPalette()`. A curva é colorida pelo último valor da execução: `up` quando o valor não é inferior a zero e `down` quando é negativo; o preenchimento sob a linha usa a mesma cor, atenuada para 28 % em cima e 2 % em baixo. O eixo do tempo mostra as horas e os segundos no fuso horário do navegador e, se `Intl` não estiver disponível, em UTC.

## Cabeçalho, dica e botões

No cabeçalho do painel é apresentado o último valor da execução no formato `formatPnl` — com sinal e duas casas decimais; a classe de cor é devolvida por `host.presentation.pnlClass`. Por cima da curva é apresentado o valor sob o cursor: o instante em palavras através de `host.presentation.timeText` e, ao lado, o próprio valor. Quando o cursor sai do gráfico, a dica é limpa.

O botão do cabeçalho com a dica `ResetView` invoca `resetZoom` e o botão de fecho invoca `host.close()`. O painel obtém o texto visível através das chaves `Equity`, `ResetView`, `ClosePanel`, `PnLChart` e `NoEquity`.

O controlo é responsável pela forma da curva, pela sua cor e pela escala. Ao anfitrião ficam o idioma das etiquetas, a paleta, o formato do instante de tempo e a origem dos próprios pontos: o controlo não calcula o P&L nem pede dados a lado nenhum.

## Métodos públicos

- `EquityWidget.create(hostEl, state, deps)` — criar o painel e adicionar o seu elemento raiz ao contentor.
- `EquityWidget.TYPE` — o identificador `equity`.
- `update(points)` — mostrar a execução completa.
- `resetZoom()` — voltar à vista de toda a execução depois de ampliar.
- `chart()` — a instância de `IChartApi`, para que o anfitrião desenhe o que lhe faltar: uma linha de referência, uma marca de drawdown. Devolve `null` enquanto o gráfico não existir.
- `dispose()` — desligar o observador de tamanho, remover o gráfico e cancelar o registo no anfitrião.

A propriedade `rootEl` dá acesso ao elemento raiz do painel.

## Ligar o motor de gráficos

O pacote `@stocksharp/chart` instala-se em separado:

```bash
npm install @stocksharp/chart
```

O pacote pronto `sstradingcontrols.js` não inclui o motor, pelo que numa página sem empacotador os respetivos scripts são ligados ao lado:

```html
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/indicators/dist/ssindicators.js"></script>
<script src="https://cdn.jsdelivr.net/npm/@stocksharp/chart/dist/sschart.js"></script>
```

## Consulte também

- [Controlos de negociação JavaScript](../trading_controls.md)
- [Posições](positions.md)
- [Histórico de negócios](trade_history.md)
- [Gráficos em JavaScript](../charts.md)
