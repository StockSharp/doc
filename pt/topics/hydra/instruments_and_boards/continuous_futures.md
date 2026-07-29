# Futuros contínuos

O programa [Hydra](../../hydra.md) permite ao utilizador combinar diferentes tipos de dados de mercado de vários contratos num único instrumento contínuo.

Para o fazer, no separador **Comum**, selecione **Instrumentos** para que apareça o separador **Todos os instrumentos**. Antes de combinar os dados, verifique que dados de mercado estão disponíveis. Selecione o caminho onde os dados se encontram e reveja os instrumentos que planeia combinar. Se existirem lacunas, descarregue os dados de mercado em falta (por exemplo, a partir de uma fonte de dados suportada).

![Hydra verificação de dados de futuros contínuos](../../../images/hydragluingcheckdata.png)

Como exemplo, considere a combinação de futuros E-mini S&P 500.

1. Para criar um contrato de futuros contínuo, clique no botão **Criar instrumento \=\> Instrumento contínuo** no separador **Todos os instrumentos**.![Hydra Verificação de dados ao unir 00](../../../images/hydragluingcheckdata_00.png)

   Depois disso, aparecerá a seguinte janela:![Captura de ecrã de Futuros contínuos 1](../../../images/hydragluingwindow.png)
2. Para criar um futuro contínuo, é necessário especificar um nome e adicionar contratos.

   Existem duas formas de adicionar contratos.
   - Manualmente, clicando no botão ![Hydra botão Adicionar](../../../images/hydra_add.png).![Hydra futuro contínuo personalizado](../../../images/hydragluingcscustom.png)
   - Se definir as duas primeiras letras do contrato como nome, por exemplo, RI, e clicar no botão **Auto**, todos os instrumentos encontrados na base de dados serão adicionados.![Captura de ecrã de Futuros contínuos 2](../../../images/hydragluingcsauto.png)
3. Selecione os contratos necessários e defina as respetivas datas de transição. ![Captura de ecrã de Futuros contínuos 3](../../../images/hydragluingcsauto_00.png)
4. Em seguida, atribua o identificador de instrumento **ES\_continuous@CME** e clique no botão **OK**, após o que será criado um novo instrumento.
5. Em seguida, clique no botão [Velas](../working_with_data/view_and_export/candles.md) no separador **Comum**, selecione o instrumento resultante e o período dos dados, defina o valor **Elemento composto** no campo **Criar a partir de** e depois clique no botão ![Hydra botão Procurar](../../../images/hydra_find.png). ![Hydra negócios de futuros contínuos](../../../images/hydragluingtrades.png)

Os dados gerados podem ser exportados para os formatos Excel, XML, JSON ou TXT. A exportação é efetuada através da lista pendente.

![Hydra exportação](../../../images/hydra_export.png)
