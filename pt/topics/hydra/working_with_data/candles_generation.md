# Geração de Candles

O [Hydra](../../hydra.md) permite gerar vários tipos de candles com base nos negócios descarregados, que posteriormente podem ser exportadas para formatos [Excel](https://en.wikipedia.org/wiki/Excel), XML, SQL, BIN, JSON ou TXT.

Isto permite utilizar os dados gerados em quaisquer programas de análise técnica (WealthLab, AmiBroker, etc.).

## Processo de Geração de Candles

1. No separador **Geral**, clique no botão **Candles**; será aberta a seguinte janela:

   ![hydra candles main](../../../images/hydra_candles_main.png)

2. Na janela aberta, é necessário configurar os parâmetros de geração de candles:

   - Selecione o tipo de candle pretendido na lista pendente (todos os [tipos de candles padrão](../../api/candles.md) são suportados)
   - Especifique os parâmetros necessários para o tipo de candle selecionado:
     - Para [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) - selecione **Timeframe**
     - Para [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage) - especifique **Volume**
     - Para [TickCandleMessage](xref:StockSharp.Messages.TickCandleMessage) - especifique **Number of ticks**
     - Para [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) - especifique **Range**
     - Para [RenkoCandleMessage](xref:StockSharp.Messages.RenkoCandleMessage) - especifique **Block size**
     - Para [PnFCandleMessage](xref:StockSharp.Messages.PnFCandleMessage) - especifique **P&F Parameters**
   - Selecione o instrumento para o qual as candles serão geradas
   - Especifique um intervalo temporal (se necessário)
   - Clique no botão ![hydra find](../../../images/hydra_find.png) para iniciar a geração

### Exemplo de Geração de Candles Timeframe

Para gerar candles de 5 minutos para o instrumento AAPL@NASDAQ:

1. Selecione o tipo de candle [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage)
2. Defina **Timeframe** = 5 min
3. Selecione o instrumento AAPL@NASDAQ
4. Clique no botão de pesquisa

Depois da geração dos dados, verá o resultado:

![hydra candles tf](../../../images/hydra_candles_tf.png)

### Exemplo de Geração de Candles de Volume

Para gerar candles de volume:

1. Selecione o tipo de candle [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage)
2. Especifique o volume (por exemplo, 100)
3. Selecione o instrumento
4. No campo **Build from**, selecione **Ticks**
5. Clique no botão de pesquisa

Resultado da geração:

![hydra candles volume](../../../images/hydra_candles_volume.png)

## Origens de Dados para Construir Candles

Se os dados de mercado não puderem ser obtidos diretamente a partir da origem, pode gerar candles selecionando, no campo [**Build from**](any_market_data_types.md), o tipo de dados a partir do qual serão construídas:

- **Ticks** - construção de candles a partir de dados de ticks
- **Order Books** - construção de candles a partir de dados do livro de ordens
- **Level1** - construção de candles a partir de dados Level1
- **Smaller Timeframe** - construção de candles com um timeframe maior a partir de candles com um menor

### Exemplos de Diferentes Opções de Construção:

- Candles de 10 minutos a partir de ticks:

  ![hydra candles tf 10](../../../images/hydra_candles_tf_10.png)

- Candles de 30 minutos a partir de candles de 5 minutos:

  ![hydra candles tf 01](../../../images/hydra_candles_tf_01.png)

> [!TIP]
> Se selecionar **don't build** no campo **Build from**, serão pesquisadas apenas candles prontas que tenham sido descarregadas diretamente através da origem de dados.

## Visualização das Candles Geradas

Para apresentação gráfica das candles geradas:

1. Clique no botão ![hydra candles](../../../images/hydra_candles.png)
2. Será aberto um gráfico com as candles construídas:

   ![hydra candles tf chart](../../../images/hydra_candles_tf_chart.png)

   ![hydra candles volume chart](../../../images/hydra_candles_volume_chart.png)

## Adicionar Indicadores ao Gráfico

Podem ser adicionados indicadores técnicos ao gráfico de candles:

1. Abra o menu de contexto clicando com o botão direito do rato no painel do gráfico
2. Selecione o item **Indicator** e o indicador pretendido na lista
3. Para apresentar o indicador num painel separado:
   - Adicione um novo painel usando o botão ![hydra add](../../../images/hydra_add.png)
   - Selecione o indicador pretendido no menu de contexto

Exemplo de um gráfico com indicadores adicionados:

![hydra candles ind chart](../../../images/hydra_candles_ind_chart.png)

## Exportação de Dados

Os valores de candles obtidos podem ser [exportados para vários formatos](export_data.md) para utilização noutros programas.

**Veja também o [tutorial em vídeo](../videos/building_candles.md) sobre a construção de vários tipos de candles**
