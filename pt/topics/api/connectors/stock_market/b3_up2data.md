# B3 UP2DATA

O **conector B3 UP2DATA** conecta o StockSharp a um serviço profissional de dados e análises de mercado. Ele traduz dados e operações específicos do provedor para o modelo unificado de mensagens do StockSharp, permitindo usar as mesmas assinaturas e fluxos em diferentes mercados.

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

Instrumentos, profundidade de dados, permissões de negociação, limites e disponibilidade dependem de B3 UP2DATA, do plano de API e da conta conectada.

## Veja também

[Configuração do conector](b3_up2data/configuration_b3_up2data.md)

[Configuração gráfica](b3_up2data/graphical_configuration_b3_up2data.md)

[Inicialização do adaptador](b3_up2data/adapter_initialization_b3_up2data.md)
