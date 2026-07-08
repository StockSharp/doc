# Livros de ordens

Para importar livros de ordens, selecione o item **Import \=\> Order books** no menu principal da aplicação.

![hydra import depths](../../../images/hydra_import_depths.png)

## Processo de importação.

1. **Import settings.**.

   Consulte a importação de [Velas](candles.md).
2. Configure os parâmetros de importação para os campos [S#](../../api.md).

   Consulte a importação de [Velas](candles.md).

   **Vamos considerar um exemplo de importação de um livro de ordens a partir de um ficheiro CSV:**
   - O ficheiro a partir do qual pretende importar dados tem o seguinte modelo:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{Quote.Price};{Quote.Volume};{Side}
     	  				
     ```

     Aqui, os valores de {SecurityId.SecurityCode} e {SecurityId.BoardCode} correspondem aos valores de **Security** e **Board**, respetivamente. Portanto, no campo **Field order**, atribuímos os valores 0 e 1, respetivamente.
   - Para os campos {ServerTime:default:yyyyMMdd} e {ServerTime:default:HH:mm:ss.ffffff}, selecione os campos **Date** e **Time** na janela **S# field**, respetivamente. Atribuímos-lhes os valores 2 e 3.
   - Para o campo {Quote.Price}, selecione o campo **Price** na janela **S# field** - preço da cotação. Atribuímos-lhe o valor 4.
   - Para o campo {Quote.Volume}, selecione o campo **Volume** na janela **S# field** - volume da cotação. Atribuímos-lhe o valor 5
   - Para o campo {Side}, selecione o campo **Direction** na janela **S# field** - direção da transação (Buy ou Sell). Atribuímos-lhe o valor 6.
   - A janela de definição de campos terá o seguinte aspeto:![hydra import prop depth](../../../images/hydra_import_prop_depth.png)

   O utilizador pode configurar um grande número de propriedades para os dados transferidos. Com base no modelo do ficheiro importado, é necessário especificar a propriedade e atribuir-lhe o número necessário na sequência.
3. Para pré-visualizar os dados, clique no botão **Preview**.![hydra import preview depth](../../../images/hydra_import_preview_depth.png)
4. Clique no botão **Import**.
