# Instrumentos

Para importar instrumentos, selecione o separador **Import \=\> Instruments**.

![hydra import securities](../../../images/hydra_import_securities.png)

## Processo de importação.

1. **Import settings.**.

   Consulte a importação de [Candles](candles.md).
2. Configure os parâmetros de importação para os campos [S#](../../api.md).

   Consulte a importação de [Candles](candles.md).

   **Vamos considerar um exemplo de importação de um instrumento a partir de um ficheiro CSV:**
   - O ficheiro a partir do qual pretende importar dados tem o seguinte modelo:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{PriceStep};{SecurityType};{VolumeStep}
     	  				
     ```

     Aqui, os valores de {SecurityId.SecurityCode} e {SecurityId.BoardCode} correspondem aos valores de **Security** e **Board**, respetivamente. Portanto, no campo **Field order**, atribuímos os valores 0 e 1, respetivamente.
   - Para o campo {PriceStep}, selecione o campo **Nominal** na janela **S# field** e atribua-lhe o valor 2.
   - Para o campo {SecurityType}, selecione o campo **Type** na janela **S# field** - o tipo de instrumento (ação, moeda, futuros, etc.). Atribuímos-lhe o valor 3.
   - Para o campo {VolumeStep}, selecione o campo **Min volume (base)** na janela **S# field** - o volume base ou mínimo do instrumento. Atribuímos-lhe o valor 4
   - A janela de definição de campos terá o seguinte aspeto:![hydra import prop securitiy](../../../images/hydra_import_prop_securitiy.png)

   O utilizador pode configurar um grande número de propriedades para os dados transferidos. Com base no modelo do ficheiro importado, é necessário especificar a propriedade e atribuir-lhe o número necessário na sequência.
3. Para pré-visualizar os dados, clique no botão **Preview**.![hydra import preview securitiy](../../../images/hydra_import_preview_securitiy.png)
4. Clique no botão **Import**.
