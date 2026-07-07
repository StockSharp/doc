# Executar um Script

Para criar um novo script de analytics, selecione o separador **Analytics** no painel de fontes de dados da janela principal e escolha o modelo pretendido no menu pendente para um arranque rápido:

![hydra_analytics_main_00](../../../images/hydra_analytics_main_00.png)

A captura de ecrã apresenta a interface principal desta funcionalidade, composta por vários componentes principais:

- **Árvore de Navegação**: Fornece acesso rápido a várias funções de analytics e aos scripts de analytics criados, como análise de volume intradiário, perfil de volume, gráficos, indicadores e outras ferramentas de análise.
- **Janela de Código**: A janela de código apresenta o código-fonte do script analítico selecionado. Os utilizadores podem editar diretamente o código para personalizar ou criar cálculos analíticos e estratégias.
- **Painel de Parâmetros**: No lado direito, existe um painel de parâmetros onde pode definir parâmetros para scripts analíticos, incluindo a seleção de instrumentos, período de análise, caminho dos dados e outras definições.
- **Lista de Erros**: Na parte inferior da interface, existe uma lista de erros detetados durante a compilação ou execução do script analítico.

O script de analytics é formatado como uma classe herdada de [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript).

Ao definir parâmetros:

- O **Instrumento** pode ser definido de um a vários instrumentos, consoante a lógica do script.

![hydra_analytics_main_01](../../../images/hydra_analytics_main_01.png)

- O intervalo de datas.
- O armazenamento de onde obter os dados.
- O intervalo de tempo de trabalho do script, caso utilize um.

Ao clicar no botão **Start** ![hydra analytics compile](../../../images/hydra_analytics_compile.png), será aberto um novo separador com os resultados da execução do script.
