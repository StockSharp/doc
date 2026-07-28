# Settrade

**Settrade** conecta o StockSharp à Settrade Open API v2 para ações da Bolsa da Tailândia e derivativos da TFEX. O adaptador disponibiliza os dados de mercado e as operações de transação suportadas por meio do modelo padrão de mensagens do StockSharp.

## Principais recursos

- Recursos do adaptador verificados no código-fonte: atualizações de dados em tempo real, cotações de nível 1, livros de ofertas, dados de velas, solicitações de dados históricos, operações de carteira e ordens para contas de ações e derivativos.
- Transportes, sessões e formatos específicos do provedor ficam ocultos atrás da API padrão do StockSharp.

## Uso típico

Use este conector para monitorar instrumentos da SET e da TFEX, analisar cotações, livros de ofertas e velas, solicitar dados históricos e executar os fluxos de ordens suportados.

Os instrumentos disponíveis, a profundidade de dados, as permissões de negociação, os limites de solicitações e a disponibilidade dependem da Settrade, da corretora e da conta conectada.

## Veja também

[Configuração do conector](settrade/configuration_settrade.md)

[Configuração gráfica](settrade/graphical_configuration_settrade.md)

[Inicialização do adaptador](settrade/adapter_initialization_settrade.md)
