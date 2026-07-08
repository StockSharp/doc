# Log de ordens

Para importar o log de ordens, selecione o item **Import \=\> Order log** no menu principal da aplicação.

![hydra import orderlog](../../../images/hydra_import_orderlog.png)

## Processo de importação.

1. **Definições de importação.**.

   Consulte a importação de [Velas](candles.md).
2. Configure os parâmetros de importação para os campos [S#](../../api.md).

   Consulte a importação de [Velas](candles.md).

   **Vejamos um exemplo de importação de um Log de Ordens a partir de um ficheiro CSV:**
   - O ficheiro a partir do qual pretende importar dados tem o seguinte modelo:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{OrderId};{OrderPrice};{OrderVolume};{Side};{OrderState};{TimeInForce};{TradeId};{TradePrice}
     	  				
     ```

     Aqui, os valores de {SecurityId.SecurityCode} e {SecurityId.BoardCode} correspondem aos valores de **Security** e **Board**, respetivamente. Por isso, no campo **Field order** atribuímos os valores 0 e 1, respetivamente.
   - Para os campos {ServerTime:default:yyyyMMdd} e {ServerTime:default:HH:mm:ss.ffffff}, selecione os campos **Date** e **Time** na janela **S# field**, respetivamente. Atribuímos os valores 2 e 3.
   - Para o campo {OrderId}, selecione o campo **ID** na janela **S# field** - ID da ordem. Atribuímos-lhe o valor 4.
   - Para o campo {OrderPrice}, selecione o campo **Price** na janela **S# field** - preço da ordem. Atribuímos-lhe o valor 5
   - Para o campo {OrderVolume}, selecione o campo **Volume** na janela **S# field** - volume da ordem. Atribuímos-lhe o valor 6.
   - Para o campo {Side}, selecione o campo **Direction** na janela **S# field** - direção da ordem (compra ou venda). Atribuímos-lhe o valor 7.
   - Para o campo {OrderState}, selecione o campo **Action** na janela **S# field** - o estado da ordem (ativa, inativa ou erro). Atribuímos-lhe o valor 8.
   - Para o campo {TimeInForce}, selecione **Time** in force na janela **S# field** - uma condição de execução da ordem limite. Atribuímos-lhe o valor 9.
   - Para o campo {TradeId}, selecione o campo **ID (trade)** na janela **S# field** - o identificador da transação. Atribuímos-lhe o valor 10.
   - Para o campo {TradePrice}, selecione o campo **Price (trade)** na janela **S# field** - o preço da transação. Atribuímos-lhe o valor 11.
   - A janela de definição dos campos terá este aspeto:![hydra import prop orderlog](../../../images/hydra_import_prop_orderlog.png)

   O utilizador pode configurar um grande número de propriedades para os dados descarregados. Com base no modelo do ficheiro importado, é necessário especificar a propriedade e atribuir-lhe o número necessário na sequência. 
3. Para pré-visualizar os dados, clique no botão **Preview**.![hydra import preview orderlog](../../../images/hydra_import_preview_orderlog.png)
4. Clique no botão **Import**.
