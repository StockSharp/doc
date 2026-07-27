# TASE Data Hub

O **conector TASE Data Hub** conecta o StockSharp a um serviço profissional de dados e análises de mercado. Ele traduz dados e operações específicos do provedor para o modelo unificado de mensagens do StockSharp, permitindo usar as mesmas assinaturas e fluxos em diferentes mercados.

## Principais recursos

- Cobertura típica: ações.
- Pesquisa de instrumentos e dados de referência do provedor.
- Dados de mercado, empresas, registros, divulgações e referência suportados pelo provedor.
- Dados de mercado suportados pelo adaptador: cotações de nível 1 e candles.
- Solicitações de dados históricos para gráficos, análises e testes de estratégias.
- Este adaptador é destinado ao acesso a dados e não encaminha ordens.
- Transportes, sessões e formatos específicos do provedor ficam ocultos atrás da API padrão do StockSharp.

## Uso típico

Use-o para alimentar gráficos, armazenamento de mercado, análises, pesquisas e testes de estratégias com dados do provedor.

Instrumentos, profundidade de dados, permissões de negociação, limites e disponibilidade dependem de TASE Data Hub, do plano de API e da conta conectada.

## Veja também

[Configuração do conector](tase_data_hub/configuration_tase_data_hub.md)

[Configuração gráfica](tase_data_hub/graphical_configuration_tase_data_hub.md)

[Inicialização do adaptador](tase_data_hub/adapter_initialization_tase_data_hub.md)
