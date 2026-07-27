# HDFC Securities

O **conector HDFC Securities** conecta o StockSharp a uma corretora ou mercado eletrônico de instrumentos financeiros. Ele traduz dados e operações específicos do provedor para o modelo unificado de mensagens do StockSharp, permitindo usar as mesmas assinaturas e fluxos em diferentes mercados.

## Principais recursos

- Cobertura típica: ações, futuros, opções e commodities.
- Pesquisa de instrumentos e dados de referência do provedor.
- Dados de mercado suportados pelo adaptador: cotações de nível 1, negócios tick a tick e livros de ordens.
- Fluxos de envio de ordens e execuções suportados pelo provedor.
- Atualizações de carteiras, saldos, posições e estado das execuções.
- Assinaturas em tempo real pelo fluxo de dados do provedor.
- Transportes, sessões e formatos específicos do provedor ficam ocultos atrás da API padrão do StockSharp.

## Uso típico

Use-o em estratégias ao vivo, terminais, serviços de gestão de ordens e ferramentas de monitoramento que precisem de acesso direto ao provedor.

Instrumentos, profundidade de dados, permissões de negociação, limites e disponibilidade dependem de HDFC Securities, do plano de API e da conta conectada.

## Veja também

[Configuração do conector](hdfc_securities/configuration_hdfc_securities.md)

[Configuração gráfica](hdfc_securities/graphical_configuration_hdfc_securities.md)

[Inicialização do adaptador](hdfc_securities/adapter_initialization_hdfc_securities.md)
