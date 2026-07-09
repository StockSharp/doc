# Velas

Para importar velas, selecione o item **Importar \=\> Velas** no menu principal da aplicação.

![hydra import candles](../../../images/hydra_import_candles.png)

## Processo de importação de velas

1. **Comum**
   - **Tipo de dados** - tipo dos dados importados.
   - **Nome do ficheiro** - caminho completo para o ficheiro CSV.
   - **Diretório de dados** - pasta onde serão guardados os ficheiros finais [S#](../../api.md).
   - **Máscara de ficheiro** - máscara de ficheiro usada ao analisar o diretório. Por exemplo, candle \_\*.csv.
   - **Separador de colunas** - separador de colunas. A tabulação é indicada por TAB.
   - **Recuo desde o início** - número de linhas desde o início do ficheiro a ignorar (se contiverem metainformação).
   - **Fuso horário** - fuso horário.
   - **Intervalo** - frequência de atualização dos dados.

   **Instrumentos**
   - **Informação alargada** - guardar os campos importados adicionais no armazenamento de informação adicional
   - **Duplicados** - indica se os instrumentos duplicados serão atualizados caso já existam.
2. Configure os parâmetros de importação para os campos [S#](../../api.md).
   - **campo S#** - valor do campo S# (**Instrumento**, **Mercado**, etc.).
   - **Associações** - corresponder o valor da coluna no ficheiro ao tipo StockSharp (se necessário).
   - **Formato** - formato dos dados. Normalmente usado ao importar valores de data e hora (consulte [Trades](ticks.md)).
   - **Usar** - indica se os dados devem ser usados durante a importação.
   - **Ordem dos campos** - sequência em que estão dispostas as colunas de propriedades do item importado.

     Por exemplo, se o ficheiro importado tiver o seguinte modelo:

     ```none
     {SecurityId.SecurityCode},{SecurityId.BoardCode},{OpenTime:yyyyMMdd},{OpenTime:default:HH:mm:ss},{OpenPrice},{HighPrice},{LowPrice},{ClosePrice},{TotalVolume}

     ```

     Então a seguinte definição corresponderá a esse modelo:![hydra import prop candles](../../../images/hydra_import_prop_candles.png)

     Aqui:

     O valor **Instrumento** corresponde a **{SecurityId.SecurityCode}** com o número de sequência **0**.

     > [!TIP]
     > Em programação, o número ordinal do primeiro elemento é sempre 0

     O valor **Mercado** corresponde a **{SecurityId.BoardCode}** com o número de sequência **1**. E assim sucessivamente.
   - **Por predefinição** - valor predefinido do campo. Por exemplo, pode ser usado para valores de campo repetidos (**Instrumento** ou **Mercado** ao importar transações, livros de ordens, etc.; consulte [Trades](ticks.md)), se a informação correspondente não estiver no ficheiro de dados.
   - **Zero** - em alguns casos, ao guardar dados, algumas propriedades podem ser guardadas como "0", o que é um erro. Por exemplo, o valor do preço, por vários motivos, pode ser igual a 0, o que não é aceitável e, no futuro, levará a uma leitura incorreta. Isto pode causar o funcionamento incorreto das estratégias que trabalham com esses dados e, consequentemente, um resultado errado. Ao assinalar a caixa, o utilizador especifica que os dados nesta secção, se forem iguais a 0, são escritos como vazios, ou seja, como ausentes. Durante o trabalho posterior, por exemplo em testes, o utilizador verá um erro de ausência de dados, que indicará uma importação incorreta. Na prática, isto protege o utilizador contra dados "danificados", permitindo um trabalho mais correto.

   O utilizador pode configurar um grande número de propriedades para os dados transferidos. Com base no modelo do ficheiro importado, é necessário especificar a propriedade e atribuir-lhe o número necessário na sequência.
3. Para pré-visualizar os dados, clique no botão **Pré-visualizar**.![hydra import preview candles](../../../images/hydra_import_preview_candles.png)
4. Clique no botão **Importar**.
