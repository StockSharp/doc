# Sorriso de volatilidade

`OptionSmileWidget` desenha o sorriso de volatilidade de uma série de opções: a volatilidade implícita das calls e das puts por strike, duas linhas na mesma escala. O gráfico é construído pelo motor do pacote `@stocksharp/chart`, declarado como dependência de par (*peer*).

![Sorriso de volatilidade ao longo dos strikes de uma cadeia de opções](../../../../images/javascript_controls_option_smile.png)

## Criação e atualização

```ts
import {
  OptionSmileWidget,
  type OptionStrike,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const smile = OptionSmileWidget.create(
  document.querySelector<HTMLElement>('#smile')!,
  {},
  { host },
);

const chain: OptionStrike[] = [
  { strike: 67_000, call: { ivLast: 0.52 }, put: { ivBid: 0.55, ivAsk: 0.57 } },
  { strike: 67_500, call: { ivLast: 0.48 }, put: { ivLast: 0.50 } },
  { strike: 68_000, call: { ivBid: 0.45, ivAsk: 0.47 }, put: { ivLast: 0.47 } },
  { strike: 69_000, call: { ivLast: 0.49 }, put: {} },
];

smile.update(chain, { assetPrice: 68_120 });
```

A única dependência é `host`; a interface `OptionSmileDeps` não tem outros campos. O segundo argumento de `create` é o estado guardado da instância, que o sorriso não lê nem escreve.

`update` substitui toda a cadeia. O segundo parâmetro, `context`, é opcional e por predefinição é um objeto vazio; é passado com o mesmo tipo `OptionChainContext` da mesa de opções, mas o sorriso só precisa do preço do ativo subjacente, `assetPrice`.

A propriedade estática `OptionSmileWidget.TYPE` é igual a `ControlTypes.OptionSmile` — ao identificador `optionSmile`.

## Dados e apresentação

Os strikes são ordenados de forma ascendente e os registos com `strike` não numérico são descartados. A volatilidade de um lado vem de `ivLast` e, se não houve negócios, da média de `ivBid` e `ivAsk`; só são considerados valores finitos e positivos. Se nenhum deles existir, o ponto não é desenhado: o strike continua no eixo e a linha interrompe-se — assim vê-se o strike que só está cotado de um lado. No gráfico os valores são apresentados em percentagem.

O eixo dos strikes funciona em modo `ordinal`: os passos são uniformes ao longo da lista de cotação e não proporcionais à distância entre os números, pelo que o salto entre 67_500 e 68_000 não se transforma num buraco. As etiquetas do eixo e a marca do cursor em cruz são produzidas pelo mesmo formatador de preço.

O preço do ativo subjacente não é desenhado como linha — num eixo ordinal não existe degrau para ele —, sendo apresentado como texto na legenda ao lado das chaves `Call` e `Put`. Ao passar o cursor sobre o gráfico aparece uma linha com o strike e os valores dos dois lados nesse ponto: o sorriso lê-se pela distância entre as curvas, por isso mostram-se ambos.

Enquanto nenhum strike estiver cotado, em vez do gráfico é apresentada uma mensagem substituta com o texto da chave `NoOptions`.

## O que faz o controlo e o que fica a cargo do anfitrião

O controlo cria o gráfico ao receber os primeiros dados, obtém as cores, o tipo de letra e a cor da grelha de `host.presentation.canvasPalette()` (`up` é a call, `down` é a put), acompanha o tamanho do contentor através de `ResizeObserver` e ajusta o `canvas`, trata o botão de reposição do zoom e o botão de fecho do painel, que invoca `host.close()`. Todo o texto visível é pedido através de `host.t`: `OptionSmile`, `ResetView`, `ClosePanel`, `ImpliedVolatility`, `Call`, `Put`, `OptionChain`, `NoOptions`, `Underlying`.

O anfitrião fornece os dados: o sorriso não subscreve o mercado, não calcula a volatilidade e não distingue uma série de outra — desenha o que lhe for passado em `update`. O controlo não guarda definições próprias em `host.preferences` e não tem chaves.

## Métodos públicos

- `update(strikes, context)` — mostrar a cadeia e o contexto em que foi observada.
- `resetZoom()` — voltar à vista de toda a cadeia depois de ampliar.
- `chart()` — devolver o objeto do gráfico (`IChartApi`) ou `null` se o gráfico ainda não tiver sido criado; é útil ao anfitrião que acrescente ao mesmo `canvas` uma segunda série ou um marcador.
- `dispose()` — desligar o observador de tamanho, remover o gráfico, chamar `host.unregister` e retirar o elemento raiz.

## Funções auxiliares

O pacote exporta também as funções sobre as quais assenta o desenho — podem ser usadas separadamente:

- `sideVolatility(side)` — a volatilidade de um lado ou `null` se for desconhecida.
- `sortedChain(strikes)` — a cadeia pela ordem de desenho: por strike ascendente e sem registos inválidos.
- `toSmileSeries(chain, put)` — um dos lados sob a forma de conjunto de pontos para o gráfico.

## Consulte também

- [Controlos de negociação JavaScript](../trading_controls.md)
- [Mesa de opções](option_desk.md)
- [Curva de capital](equity.md)
- [Livro de ofertas](order_book.md)
