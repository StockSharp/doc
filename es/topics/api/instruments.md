# Instrumentos

En StockSharp, los instrumentos financieros se representan mediante la clase [Security](xref:StockSharp.BusinessEntities.Security), que es un elemento fundamental para trabajar con datos de negociación. Esta sección describe los principales aspectos del trabajo con instrumentos financieros dentro de la plataforma.

## Clase base Security

[Security](xref:StockSharp.BusinessEntities.Security) representa un instrumento financiero negociado en una bolsa. Un instrumento puede ser una acción, contrato de futuros, opción, par de divisas, criptomoneda y otros activos. La clase contiene toda la información necesaria para identificar y negociar el instrumento:

- **Información de identificación** - código, ISIN, nombre, clase del instrumento
- **Parámetros de negociación** - paso de precio, tamaño de lote, volumen mínimo
- **Datos de mercado** - valores actuales de precios, volúmenes, libros de órdenes, etc.
- **Valores calculados** - parámetros para derivados, cálculo de riesgo, etc.

## Tipos de instrumentos

StockSharp admite el trabajo con todos los principales tipos de instrumentos financieros:

- **Acciones** - valores de renta variable
- **Bonos** - valores de deuda
- **Futuros** - contratos derivados sobre un activo subyacente
- **Opciones** - contratos que otorgan el derecho (pero no la obligación) de comprar o vender un activo subyacente
- **Pares de divisas** - instrumentos para operar en el mercado forex
- **Criptomonedas** - activos digitales para operar en bolsas de criptomonedas
- **ETF** - fondos cotizados en bolsa
- **Índices** - indicadores calculados del estado de un mercado o sector

## Cestas de instrumentos

Además de instrumentos normales, StockSharp implementa clases especiales para trabajar con grupos de instrumentos:

- [IndexSecurity](xref:StockSharp.Algo.IndexSecurity) - un instrumento que representa un índice basado en instrumentos subyacentes
- [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) - un índice con coeficientes de ponderación para cada instrumento
- [ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity) - un instrumento continuo para trabajar con una serie de contratos de futuros

Estas clases permiten crear instrumentos compuestos y trabajar con ellos del mismo modo que con instrumentos normales, recibiendo datos de mercado agregados, calculando estadísticas y ejecutando operaciones de negociación.

## Trabajo con información de instrumentos

StockSharp proporciona herramientas potentes para trabajar con información de instrumentos financieros:

- **Búsqueda de instrumentos** - por diversos criterios (código, nombre, clase)
- **Filtrado** - selección de instrumentos según parámetros especificados
- **Almacenamiento** - guardado de información de instrumentos en almacenamiento local o remoto
- **Obtención de información de la bolsa** - carga de información detallada desde la bolsa

## Identificación de instrumentos

Cada instrumento en StockSharp tiene un identificador único [SecurityId](xref:StockSharp.Messages.SecurityId), que se usa para identificar inequívocamente el instrumento en el sistema. El identificador incluye:

- **Código del instrumento** - código bursátil del instrumento
- **Código de plaza** - código del mercado o plaza de negociación
- **Bloomberg/Reuters/ISIN** y otros códigos - métodos alternativos de identificación

## Características especiales

- **Futuros continuos** - "empalme" automático de datos históricos para una serie de contratos de futuros
- **Instrumentos compuestos** - creación de instrumentos virtuales basados en varios instrumentos reales
- **Identificador especial \*@ALL** - para trabajar con todos los instrumentos de una clase determinada

## Véase también

[Identificador de instrumento](instruments/instrument_identifier.md)

[Identificador \*@ALL](instruments/identifier_@all.md)

[Futuros continuos](instruments/continuous_futures.md)

[Índice](instruments/index.md)

[Búsqueda de instrumentos](instruments/instrument_search.md)
