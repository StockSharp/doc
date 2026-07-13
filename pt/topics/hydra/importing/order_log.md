# Registo de ordens

Para importar o registo de ordens, selecione o item **Importar \=\> Registo de ordens** no menu principal da aplicação.

![Captura de tela de Registo de ordens 1](../../../images/hydra_import_orderlog.png)

## Processo de importação.

1. **Definições de importação.**.

   Consulte a importação de [Velas](candles.md).
2. Configure os parâmetros de importação para os campos [S#](../../api.md).

   Consulte a importação de [Velas](candles.md).

   **Vejamos um exemplo de importação de um Registo de Ordens a partir de um ficheiro CSV:**
   - O ficheiro a partir do qual pretende importar dados tem o seguinte modelo:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{OrderId};{OrderPrice};{OrderVolume};{Side};{OrderState};{TimeInForce};{TradeId};{TradePrice}
     	  				
     ```

     Aqui, os valores de {SecurityId.SecurityCode} e {SecurityId.BoardCode} correspondem aos valores de **Instrumento** e **Mercado**, respetivamente. Por isso, no campo **Ordem dos campos** atribuímos os valores 0 e 1, respetivamente.
   - Para os campos {ServerTime:default:yyyyMMdd} e {ServerTime:default:HH:mm:ss.ffffff}, selecione os campos **Data** e **Hora** na janela **campo S#**, respetivamente. Atribuímos os valores 2 e 3.
   - Para o campo {OrderId}, selecione o campo **ID** na janela **campo S#** - ID da ordem. Atribuímos-lhe o valor 4.
   - Para o campo {OrderPrice}, selecione o campo **Preço** na janela **campo S#** - preço da ordem. Atribuímos-lhe o valor 5
   - Para o campo {OrderVolume}, selecione o campo **Volume** na janela **campo S#** - volume da ordem. Atribuímos-lhe o valor 6.
   - Para o campo {Side}, selecione o campo **Direção** na janela **campo S#** - direção da ordem (compra ou venda). Atribuímos-lhe o valor 7.
   - Para o campo {OrderState}, selecione o campo **Ação** na janela **campo S#** - o estado da ordem (ativa, inativa ou erro). Atribuímos-lhe o valor 8.
   - Para o campo {TimeInForce}, selecione **Validade** na janela **campo S#** - uma condição de execução da ordem limite. Atribuímos-lhe o valor 9.
   - Para o campo {TradeId}, selecione o campo **ID (transação)** na janela **campo S#** - o identificador da transação. Atribuímos-lhe o valor 10.
   - Para o campo {TradePrice}, selecione o campo **Preço (transação)** na janela **campo S#** - o preço da transação. Atribuímos-lhe o valor 11.
   - A janela de definição dos campos terá este aspeto:![Captura de tela de Registo de ordens 2](../../../images/hydra_import_prop_orderlog.png)

   O utilizador pode configurar um grande número de propriedades para os dados descarregados. Com base no modelo do ficheiro importado, é necessário especificar a propriedade e atribuir-lhe o número necessário na sequência. 
3. Para pré-visualizar os dados, clique no botão **Pré-visualizar**.![Captura de tela de Registo de ordens 3](../../../images/hydra_import_preview_orderlog.png)
4. Clique no botão **Importar**.
