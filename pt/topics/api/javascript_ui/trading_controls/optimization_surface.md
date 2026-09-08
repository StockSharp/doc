# Superfície de otimização

`SurfaceWidget` apresenta os resultados de um varrimento de parâmetros como uma paisagem tridimensional: uma métrica sobre dois eixos discretos, em que o valor da métrica define tanto a altura como a cor. O controlo aceita os mesmos dados que o [mapa de calor de otimização](optimization_heatmap.md), pelo que um mesmo conjunto de resultados pode ser visto como mapa plano, como superfície ou como ambos ao mesmo tempo.

![Superfície de otimização: o resultado como paisagem sobre dois parâmetros](../../../../images/javascript_controls_optimization_surface.png)

## Criação e atualização

```ts
import {
  HeatDirections,
  SurfaceWidget,
  type SurfaceData,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const surface = SurfaceWidget.create(
  document.querySelector<HTMLElement>('#surface')!,
  {},
  { host },
);

const sweep: SurfaceData = {
  xLabel: 'Fast',
  yLabel: 'Slow',
  metricLabel: 'Net profit',
  betterWhen: HeatDirections.Higher,
  cells: [
    { x: '10', y: '50', value: 1_250 },
    { x: '10', y: '80', value: -320 },
    { x: '20', y: '50', value: 2_480 },
    { x: '20', y: '80', value: 640 },
  ],
};

surface.update(sweep);
```

A única dependência é `host`; a interface `SurfaceDeps` não tem outros campos. O segundo argumento de `create` é o estado guardado da instância: a superfície não o lê nem escreve nele.

`update` substitui todo o conjunto. A superfície corresponde a um varrimento completo, pelo que não existe atualização parcial: metade de um varrimento sobre metade de outro daria uma paisagem feita de dois resumos diferentes.

A propriedade estática `SurfaceWidget.TYPE` é igual a `ControlTypes.OptimizationSurface` — ao identificador `optimizationSurface`.

## Dados

A célula `HeatCell` é um par de valores dos eixos mais a métrica medida: `{ x, y, value }`. Os valores dos eixos são texto porque o eixo é discreto: `10`, `00:05:00` e `True` são posições equivalentes nele. Os valores numéricos são ordenados como números e os restantes como texto, pelo que um varrimento de 5, 8, 12, 40 se mantém em sequência.

O campo `betterWhen` é obrigatório e aceita `HeatDirections.Higher` ou `HeatDirections.Lower`. Sem ele, uma paisagem de drawdowns elevaria o pior canto ao cume.

Várias execuções sobre o mesmo par são reduzidas à sua média — isso é uma célula. Um par por onde o varrimento não passou fica como buraco: uma face só é desenhada quando os seus quatro cantos são conhecidos, e a lacuna não é interpolada — um resultado ausente não é igual a um resultado nulo. Se não houver células de todo, ou se pelo menos um dos eixos tiver menos de dois valores distintos, em vez da paisagem é apresentada uma mensagem substituta com o texto da chave `NoOptimizationResults`.

As etiquetas `xLabel`, `yLabel` e `metricLabel` vão para os rótulos dos eixos; `metricLabel` é ainda apresentada no cabeçalho do painel.

## Apresentação

A altura de uma face é a posição do valor em relação ao ponto de referência da escala, comprimida entre o piso e o topo: o ponto de referência cai a meia altura, pelo que o piso não significa «pior resultado», mas sim o limite inferior da escala. A cor vem de `host.presentation.canvasPalette()`: `up` para os valores melhores do que o ponto de referência, `down` para os piores, com a saturação a aumentar à medida que se afastam dele. As faces são preenchidas das mais distantes para as mais próximas, de modo que uma crista em primeiro plano tapa o que está atrás, e são contornadas com a cor `grid` para que a grelha se leia onde duas faces vizinhas tenham quase o mesmo tom.

A projeção é ortográfica: a superfície lê-se comparando alturas em todo o campo, e a perspetiva encurtaria o lado distante de uma crista em relação ao próximo.

Sob a paisagem são desenhadas duas arestas do piso e o eixo vertical da escala. Nos eixos paramétricos são apresentadas até oito marcas: enquanto os valores couberem, todas; depois disso, uma em cada duas, uma em cada três e assim sucessivamente, sendo a primeira e a última sempre rotuladas. No eixo vertical há cinco marcas, rotuladas com valores da métrica. As etiquetas passam para as arestas do piso mais próximas do observador — ao rodar, isso é recalculado para que os números não fiquem por cima da grelha.

## Vista e gestos

Todos os gestos chegam por eventos de ponteiro, pelo que o rato, a caneta e o dedo seguem o mesmo caminho:

- arrastar com um ponteiro roda a superfície: na horizontal muda o `yaw`, na vertical o `pitch`;
- dois ponteiros alteram a escala através da distância entre eles; a rotação continua a caber a um único ponteiro;
- a roda também altera a escala. O delta é convertido em «cliques» independentemente de o navegador o reportar em píxeis, linhas ou páginas, e é limitado a dois cliques por evento, para que o rato e o painel tátil deem um passo comparável.

O controlo reserva para si o gesto sobre o `canvas`, caso contrário arrastar no telemóvel e a roda num navegador de secretária deslocariam a página em vez da paisagem.

A inclinação e a escala são limitadas: o `pitch` vai de `MIN_PITCH` (0,12) a `MAX_PITCH` (1,45) e a escala de 0,4 a 4. Com inclinação zero cada face degeneraria numa linha e, num ângulo reto, a superfície tornar-se-ia um mapa plano, isto é, outro controlo. A rotação `yaw` não é limitada, dá a volta: virar a paisagem ao contrário para ver o lado oposto de uma crista é um gesto com sentido. A vista inicial é `DEFAULT_VIEW`; o botão no cabeçalho do painel (`ResetView`) devolve-a.

Quando o ponteiro não está a rodar a superfície, o controlo procura o ponto medido mais próximo num raio de 22 píxeis CSS. O ponto encontrado é contornado com um anel da cor `up` e, na faixa por cima do `canvas`, aparece uma linha com o valor do eixo `xLabel`, o valor do eixo `yLabel` e a métrica. A faixa fica sobre o `canvas` e não no cabeçalho do painel: a leitura diz respeito ao ponto sob o ponteiro e deve estar junto dele. Um par por onde o varrimento não passou não é oferecido ao ponteiro; em caso de igualdade de distância é escolhido o ponto mais próximo do observador.

## O que faz o controlo e o que fica a cargo do anfitrião

O controlo trata dos gestos, acompanha o tamanho do `canvas` através de `ResizeObserver` e redesenha a paisagem para o tamanho e a densidade de píxeis atuais do ecrã, trata do botão de reposição da vista e do botão de fecho do painel, que invoca `host.close()`, e ainda se regista com `host.register` e cancela o registo em `dispose`. As cores e o tipo de letra do `canvas` vêm de `host.presentation.canvasPalette()`. Todo o texto visível é pedido através de `host.t`: `OptimizationSurface`, `ResetView`, `ClosePanel`, `OptimizationSurfaceChart`, `NoOptimizationResults`.

Os dados são fornecidos pelo anfitrião: o controlo não lança a otimização, não subscreve o seu andamento e não sabe como os resultados foram obtidos — desenha o que lhe for passado em `update`. Não há clique sobre uma face: a face corresponde a uma célula e não a uma execução isolada, pelo que não há nada para abrir a partir dela — premir roda a paisagem.

O controlo não guarda definições próprias em `host.preferences`, não tem chaves e não chama `host.persistState`. A vista atual está disponível através do método `view()` — se for preciso repô-la entre sessões, é o anfitrião que guarda e devolve esses valores.

## Métodos públicos

- `update(data)` — mostrar o conjunto de resultados completo.
- `view()` — devolver uma cópia da vista atual (`yaw`, `pitch`, `zoom`).
- `resetView()` — repor a vista em `DEFAULT_VIEW`.
- `dispose()` — desligar o observador de tamanho, chamar `host.unregister` e retirar o elemento raiz.

## Funções auxiliares

A geometria foi retirada do controlo para um módulo separado e é exportada pelo pacote — sobre ela pode construir-se um desenho próprio:

- `surfaceLayout(input)` — dispor as células em faces, eixos e vértices para dadas dimensões e vista; `null` quando não há nada para desenhar.
- `project(nx, ny, nz, view, box)` — projetar um ponto do cubo unitário no `canvas`.
- `dragView(view, dx, dy)` — a vista depois de arrastar esse número de píxeis.
- `zoomView(view, factor)` — a vista depois de alterar a escala.
- `clampView(view)` — a vista limitada aos valores admissíveis.
- `DEFAULT_VIEW`, `MIN_PITCH`, `MAX_PITCH` — a vista inicial e os limites da inclinação.

## Consulte também

- [Controlos de negociação JavaScript](../trading_controls.md)
- [Mapa de calor de otimização](optimization_heatmap.md)
- [Estatísticas](statistics.md)
- [Curva de capital](equity.md)
