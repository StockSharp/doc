# Ticks

Para importar transações, selecione o separador **Import \=\> Ticks**.

![hydra import trades](../../../images/hydra_import_trades.png)

## Processo de importação.

1. **Definições de importação.**.

   Consulte a importação de [Velas](candles.md).
2. Configure os parâmetros de importação para os campos [S#](../../api.md).

   Consulte a importação de [Velas](candles.md).

   **Vejamos um exemplo de importação de transações (ticks) a partir de um ficheiro CSV:**
   - O ficheiro a partir do qual pretende importar dados tem o seguinte modelo:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{TradeId};{TradePrice};{TradeVolume};{OriginSide}

     ```

     Aqui, os valores de {SecurityId.SecurityCode} e {SecurityId.BoardCode} correspondem aos valores de **Security** e **Board**, respetivamente. Por isso, no campo **Field order** atribuímos os valores 0 e 1, respetivamente.
   - Para os campos {ServerTime:default:yyyyMMdd} e {ServerTime:default:HH:mm:ss.ffffff}, selecione os campos **Date** e **Time** na janela **S# field**, respetivamente. Atribuímos os valores 2 e 3.
   - Para o campo {TradeId}, selecione o campo **Identificador** na janela **S# field** - o identificador da transação ou o número da transação. Atribuímos-lhe o valor 4.
   - Para o campo {TradePrice}, selecione o campo **Price** - o preço da transação na janela **S# field**. Atribuímos-lhe o valor 5.
   - Para o campo {TradeVolume}, selecione o campo **Volume** na janela **S# field** - o volume da transação. Atribuímos-lhe o valor 6.
   - Para o campo {OriginSide}, selecione o campo **Initiator** na janela **S# field** - o iniciador da transação (Seller ou Buyer). Atribuímos-lhe o valor 7.
   - A janela de definição dos campos terá este aspeto:![hydra import prop trade](../../../images/hydra_import_prop_trade.png)

   O utilizador pode configurar um grande número de propriedades para os dados descarregados. Com base no modelo do ficheiro importado, é necessário especificar a propriedade e atribuir-lhe o número necessário na sequência.
3. Para pré-visualizar os dados, clique no botão **Preview**.![hydra import preview trade](../../../images/hydra_import_preview_trade.png)
4. Clique no botão **Import**.
