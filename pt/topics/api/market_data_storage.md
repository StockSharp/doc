# Armazenamento de dados de mercado

[S#](../api.md) fornece funcionalidade para armazenar e carregar dados. O armazenamento de dados pode ser necessário em alguns casos:

1. Durante o funcionamento de uma estratégia de negociação, quando é necessário guardar o estado (ordens, posições, definições, etc.);
2. Guardar dados de mercado a partir do terminal de negociação para testar algoritmos em tempo real;
3. Acumulação de dados para análise e data mining.

[S#](../api.md) torna o mecanismo de gravação claro graças ao acesso de alto nível e à ocultação dos detalhes técnicos (para mais detalhes, consulte [API](market_data_storage/api.md)). E também versátil, com possibilidade de expandir os tipos de armazenamentos suportados.

## Conteúdo recomendado

[Trabalhar com a API](market_data_storage/api.md)

[Trabalhar com Armazenamento Remoto](market_data_storage/remote.md)
