# Choice FinX

O **conector Choice FinX** conecta o StockSharp a uma corretora ou mercado eletrônico de instrumentos financeiros. Ele traduz dados e operações específicos do provedor para o modelo unificado de mensagens do StockSharp, permitindo usar as mesmas assinaturas e fluxos em diferentes mercados.

## Principais recursos

- Cobertura típica: ações, futuros, opções, Forex e commodities.
- Pesquisa de instrumentos e dados de referência do provedor.
- Dados de mercado suportados pelo adaptador: cotações de nível 1, negócios tick a tick, livros de ordens e candles.
- Solicitações de dados históricos para gráficos, análises e testes de estratégias.
- Fluxos de envio de ordens e execuções suportados pelo provedor.
- Atualizações de carteiras, saldos, posições e estado das execuções.
- Assinaturas em tempo real pelo fluxo de dados do provedor.
- Transportes, sessões e formatos específicos do provedor ficam ocultos atrás da API padrão do StockSharp.

## Uso típico

Use-o em estratégias ao vivo, terminais, serviços de gestão de ordens e ferramentas de monitoramento que precisem de acesso direto ao provedor.

Instrumentos, profundidade de dados, permissões de negociação, limites e disponibilidade dependem de Choice FinX, do plano de API e da conta conectada.

## Veja também

[Configuração do conector](choice_finx/configuration_choice_finx.md)

[Configuração gráfica](choice_finx/graphical_configuration_choice_finx.md)

[Inicialização do adaptador](choice_finx/adapter_initialization_choice_finx.md)
