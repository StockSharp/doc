# Usar DLL

Usar DLLs prontas é familiar para quem pretende trabalhar continuamente em ambientes **Visual Studio** e **JetBrains Rider**. Esta abordagem oferece várias vantagens em relação à escrita de [código](using_code.md) dentro do **Designer**:

- Editor de código mais avançado em comparação com o editor integrado no **Designer**.
- A recompilação do código atualiza automaticamente o conteúdo dentro do **Designer**.
- Possibilidade de dividir o código por vários ficheiros (no caso da abordagem de [código](using_code.md), só é possível a variante «um ficheiro–uma estratégia»).
- Utilização do [depurador](using_dll/debug_dll_in_visual_studio.md).

### Criar um projeto no Visual Studio

1. Para criar uma estratégia no **Visual Studio**, tem de criar um projeto:

![Designer Criação de um cubo DLL no Visual Studio 00](../../../images/designer_creating_dll_element_in_visual_studio_00.png)

2. Em seguida, tem de escrever o código da estratégia. Para um início rápido, pode copiar o código SmaStrategy, que é criado como modelo em [estratégia a partir de código](using_code/csharp/first_strategy.md):

![Designer Criação de um cubo DLL no Visual Studio 03](../../../images/designer_creating_dll_element_in_visual_studio_03.png)

3. Para compilar o código, inclua o pacote NuGet [StockSharp.Algo](https://www.nuget.org/packages/stocksharp.algo), que contém a classe base para todas as estratégias: [Strategy](xref:StockSharp.Algo.Strategies.Strategy).

![Designer Criação de um cubo DLL no Visual Studio 04](../../../images/designer_creating_dll_element_in_visual_studio_04.png)

Se a estratégia usar interfaces de gráficos, inclua o pacote NuGet [StockSharp.Charting.Interfaces](https://www.nuget.org/packages/stockSharp.charting.interfaces). Estas interfaces não contêm a lógica real dos gráficos e são necessárias apenas para compilar o código. Quando a estratégia é executada no **Designer**, a renderização real dos gráficos ocorre através destas interfaces.

4. Depois de criar a estratégia, o projeto tem de ser compilado premindo **Compilar solução** no separador **Compilar**.

![Designer Criação de um cubo DLL no Visual Studio 01](../../../images/designer_creating_dll_element_in_visual_studio_01.png)

5. Por predefinição, no Visual Studio, o projeto é compilado para a pasta …\bin\Debug\net6.0.

![Designer Criação de um cubo DLL no Visual Studio 02](../../../images/designer_creating_dll_element_in_visual_studio_02.png)

### Adicionar DLL ao Designer

1. Adicionar uma estratégia a partir de uma DLL é semelhante a criar uma estratégia a partir de [código](using_code.md). Mas, na etapa de definição do tipo de conteúdo, tem de escolher **DLL**:

![Designer criação de estratégia DLL 00](../../../images/designer_creation_strategy_dll_00.png)

2. Na janela, tem de especificar o caminho para o assembly (tem de ser compatível com .NET 6.0) e escolher o tipo. Este último passo é necessário porque uma DLL pode conter várias estratégias (ou [cubos com indicadores](using_dll/create_element_and_indicator.md)). Depois de clicar em **Confirmar**, a estratégia será adicionada ao painel **Esquema** e fica pronta para utilização:

![Designer criação de estratégia DLL 01](../../../images/designer_creation_strategy_dll_01.png)

3. O lançamento da estratégia em [teste histórico](../backtesting/user_interface.md), em [modo ao vivo](../live_execution/getting_started.md) e outras operações funciona de forma semelhante às estratégias criadas a partir de diagramas e código:

![Designer criação de estratégia DLL 02](../../../images/designer_creation_strategy_dll_02.png)
