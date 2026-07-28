# Dexalot

**Dexalot** conecta o StockSharp ao livro central de ordens limitadas da Dexalot que funciona na cadeia de blocos. O adaptador disponibiliza os dados de mercado e as operações de transação suportadas por meio do modelo padrão de mensagens do StockSharp.

## Principais recursos

- Recursos do adaptador verificados no código-fonte: atualizações de dados em tempo real, cotações de nível 1, negócios executados, livros de ofertas, dados de velas, solicitações de dados históricos, operações de carteira e ordens.
- Transportes, sessões e formatos específicos do provedor ficam ocultos atrás da API padrão do StockSharp.

## Uso típico

Use este conector para monitorar os mercados da Dexalot, analisar cotações, negócios, livros de ofertas e velas, solicitar dados históricos e enviar as ordens suportadas.

Os pares disponíveis, a profundidade de dados, as permissões de transação, os limites de solicitações e a disponibilidade dependem da Dexalot e da carteira conectada.

## Veja também

[Configuração do conector](dexalot/configuration_dexalot.md)

[Configuração gráfica](dexalot/graphical_configuration_dexalot.md)

[Inicialização do adaptador](dexalot/adapter_initialization_dexalot.md)
