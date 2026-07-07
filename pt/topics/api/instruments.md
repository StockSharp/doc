# Instrumentos

No StockSharp, os instrumentos financeiros são representados pela classe [Security](xref:StockSharp.BusinessEntities.Security), que é um elemento fundamental para trabalhar com dados de negociação. Esta secção abrange os principais aspetos do trabalho com instrumentos financeiros dentro da plataforma.

## Classe Security base

[Security](xref:StockSharp.BusinessEntities.Security) representa um instrumento financeiro negociado numa bolsa. Um instrumento pode ser uma ação, contrato de futuros, opção, par de moedas, criptomoeda e outros ativos. A classe contém toda a informação necessária para identificar e negociar o instrumento:

- **Informação de identificação** - código, ISIN, nome, classe do instrumento
- **Parâmetros de negociação** - passo de preço, tamanho do lote, volume mínimo
- **Dados de mercado** - valores atuais de preços, volumes, livros de ofertas, etc.
- **Valores calculados** - parâmetros para derivados, cálculo de risco, etc.

## Tipos de instrumentos

O StockSharp suporta o trabalho com todos os principais tipos de instrumentos financeiros:

- **Ações** - títulos de capital
- **Obrigações** - títulos de dívida
- **Futuros** - contratos derivados sobre um ativo subjacente
- **Opções** - contratos que dão o direito (mas não a obrigação) de comprar ou vender um ativo subjacente
- **Pares de moedas** - instrumentos para negociação no mercado forex
- **Criptomoedas** - ativos digitais para negociação em bolsas cripto
- **ETFs** - fundos negociados em bolsa
- **Índices** - indicadores calculados do estado de um mercado ou sector

## Cabazes de instrumentos

Além dos instrumentos normais, o StockSharp implementa classes especiais para trabalhar com grupos de instrumentos:

- [IndexSecurity](xref:StockSharp.Algo.IndexSecurity) - um instrumento que representa um índice baseado em instrumentos subjacentes
- [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) - um índice com coeficientes de peso para cada instrumento
- [ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity) - um instrumento contínuo para trabalhar com uma série de contratos de futuros

Estas classes permitem criar instrumentos compostos e trabalhar com eles da mesma forma que com instrumentos normais, recebendo dados de mercado agregados, calculando estatísticas e executando operações de negociação.

## Trabalhar com informação de instrumentos

O StockSharp fornece ferramentas poderosas para trabalhar com informação de instrumentos financeiros:

- **Pesquisa de instrumentos** - por vários critérios (código, nome, classe)
- **Filtragem** - seleção de instrumentos de acordo com parâmetros especificados
- **Armazenamento** - guardar informação de instrumentos em armazenamento local ou remoto
- **Obtenção de informação da bolsa** - carregamento de informação detalhada da bolsa

## Identificação de instrumentos

Cada instrumento no StockSharp tem um identificador único [SecurityId](xref:StockSharp.Messages.SecurityId), que é usado para identificar inequivocamente o instrumento no sistema. O identificador inclui:

- **SecurityCode** - código de bolsa do instrumento
- **BoardCode** - código da plataforma de negociação
- **Bloomberg/Reuters/ISIN** e outros códigos - métodos alternativos de identificação

## Funcionalidades especiais

- **Futuros contínuos** - "junção" automática de dados históricos para uma série de contratos de futuros
- **Instrumentos compostos** - criação de instrumentos virtuais baseados em vários instrumentos reais
- **Identificador especial \*@ALL** - para trabalhar com todos os instrumentos de uma determinada classe

## Ver também

[Identificador do instrumento](instruments/instrument_identifier.md)

[Identificador \*@ALL](instruments/identifier_@all.md)

[Futuros contínuos](instruments/continuous_futures.md)

[Índice](instruments/index.md)

[Pesquisa de instrumentos](instruments/instrument_search.md)
