# Level 1

Para importar dados Level 1, selecione **Importar \=\> Level 1** no menu principal da aplicação

![Hydra importação de Nível 1](../../../images/hydra_import_level1.png)

## Processo de importação.

1. **Definições de importação**.

   Consulte a importação de [Velas](candles.md).
2. Configure os parâmetros de importação para os campos [S#](../../api.md).

   Consulte a importação de [Velas](candles.md).

   **Vamos considerar um exemplo de importação de Level 1 a partir de um ficheiro CSV:**
   - O ficheiro a partir do qual pretende importar dados tem o seguinte modelo:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{Changes:{BestBidPrice};{BestBidVolume};{BestAskPrice};{BestAskVolume};{LastTradeTime};{LastTradePrice};{LastTradeVolume}}

     ```

     Aqui, os valores de {SecurityId.SecurityCode} e {SecurityId.BoardCode} correspondem aos valores de **Instrumento** e **Mercado**, respetivamente. Portanto, no campo **Ordem dos campos**, atribuímos os valores 0 e 1, respetivamente.
   - Para os campos {ServerTime:default:yyyyMMdd} e {ServerTime:default:HH:mm:ss.ffffff}, selecione os campos **Data** e **Hora** na janela **campo S#**, respetivamente. Atribuímos-lhes os valores 2 e 3.
   - Para o campo {BestBidPrice}, selecione o campo **Melhor preço de compra** na janela **campo S#**. Atribuímos-lhe o valor 4.
   - Para o campo {BestBidVolume}, selecione o campo **Melhor volume de compra** na janela **campo S#**. Atribuímos-lhe o valor 5.
   - Para o campo {BestAskPrice}, selecione o campo **Melhor preço de venda** na janela **campo S#**. Atribuímos-lhe o valor 6.
   - Para o campo {BestAskVolume}, selecione o campo **Melhor volume de venda** na janela **campo S#**. Atribuímos-lhe o valor 7.
   - Para o campo {LastTradeTime}, selecione o campo **Hora da última transação** na janela **campo S#**. Atribuímos-lhe o valor 8.
   - Para o campo {LastTradePrice}, selecione o campo **Preço da última transação** na janela **campo S#**. Atribuímos-lhe o valor 9.
   - Para o campo {LastTradeVolume}, selecione o campo **Volume da última transação** na janela **campo S#**. Atribuímos-lhe o valor 10.
   - A janela de definição de campos terá o seguinte aspeto:![Hydra propriedades de importação de Nível 1](../../../images/hydra_import_prop_level1.png)

   O utilizador pode configurar um grande número de propriedades para os dados transferidos. Com base no modelo do ficheiro importado, é necessário especificar a propriedade e atribuir-lhe o número necessário na sequência.
3. Para pré-visualizar os dados, clique no botão **Pré-visualizar**.![Hydra pré-visualização da importação de Nível 1](../../../images/hydra_import_preview_level1.png)
4. Clique no botão **Importar**.
