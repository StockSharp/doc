# Instrumentos

Para importar instrumentos, selecione o separador **Importar \=\> Instrumentos**.

![hydra import securities](../../../images/hydra_import_securities.png)

## Processo de importação.

1. **Definições de importação**.

   Consulte a importação de [Velas](candles.md).
2. Configure os parâmetros de importação para os campos [S#](../../api.md).

   Consulte a importação de [Velas](candles.md).

   **Vamos considerar um exemplo de importação de um instrumento a partir de um ficheiro CSV:**
   - O ficheiro a partir do qual pretende importar dados tem o seguinte modelo:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{PriceStep};{SecurityType};{VolumeStep}

     ```

     Aqui, os valores de {SecurityId.SecurityCode} e {SecurityId.BoardCode} correspondem aos valores de **Instrumento** e **Mercado**, respetivamente. Portanto, no campo **Ordem dos campos**, atribuímos os valores 0 e 1, respetivamente.
   - Para o campo {PriceStep}, selecione o campo **Nominal** na janela **campo S#** e atribua-lhe o valor 2.
   - Para o campo {SecurityType}, selecione o campo **Tipo** na janela **campo S#** - o tipo de instrumento (ação, moeda, futuros, etc.). Atribuímos-lhe o valor 3.
   - Para o campo {VolumeStep}, selecione o campo **Volume mín. (base)** na janela **campo S#** - o volume base ou mínimo do instrumento. Atribuímos-lhe o valor 4
   - A janela de definição de campos terá o seguinte aspeto:![hydra import prop securitiy](../../../images/hydra_import_prop_securitiy.png)

   O utilizador pode configurar um grande número de propriedades para os dados transferidos. Com base no modelo do ficheiro importado, é necessário especificar a propriedade e atribuir-lhe o número necessário na sequência.
3. Para pré-visualizar os dados, clique no botão **Pré-visualizar**.![hydra import preview securitiy](../../../images/hydra_import_preview_securitiy.png)
4. Clique no botão **Importar**.
