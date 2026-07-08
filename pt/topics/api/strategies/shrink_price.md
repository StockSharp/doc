# Arredondamento de Preços

## Introdução

O método [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) em StockSharp é uma ferramenta essencial para arredondar corretamente preços de acordo com os requisitos do mercado. Isto garante que as ordens enviadas cumprem as regras da bolsa ou do corretor.

## Objetivo

O principal objetivo de [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) é arredondar preços para valores permitidos, considerando:
1. O passo de preço do instrumento ([Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep))
2. O número de casas decimais ([Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals))

## Importância da Utilização

Usar [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) é crucial para:
- Evitar a rejeição de ordens pela bolsa ou pelo corretor devido a preços incorretos
- Garantir precisão nos cálculos e nas operações de negociação
- Cumprir as regras e restrições de mercados ou instrumentos específicos

## Princípio de Funcionamento

1. Se [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep) estiver definido:
   - O preço é arredondado para o valor múltiplo mais próximo do passo de preço.
2. Se [Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals) estiver definido:
   - O preço é arredondado para o número especificado de casas decimais.
3. Se ambos os parâmetros estiverem definidos:
   - É aplicado o arredondamento mais restritivo (normalmente para o passo de preço).

## Exemplo de Utilização

```cs
// Criar um objeto Security com parâmetros especificados
var security = new Security
{
	PriceStep = 0.01m,  // Passo de preço de 0,01
	Decimals = 2        // Duas casas decimais
};

// Exemplos de utilização de ShrinkPrice

// Exemplo 1: Arredondamento para o passo de preço
decimal price1 = 10.234m;
decimal shrunkPrice1 = price1.ShrinkPrice(security);
Console.WriteLine($"Original price: {price1}, After ShrinkPrice: {shrunkPrice1}");
// Saída: preço original: 10.234, após ShrinkPrice: 10.23

// Exemplo 2: Arredondamento de um preço que já corresponde ao passo
decimal price2 = 10.22m;
decimal shrunkPrice2 = price2.ShrinkPrice(security);
Console.WriteLine($"Original price: {price2}, After ShrinkPrice: {shrunkPrice2}");
// Saída: preço original: 10.22, após ShrinkPrice: 10.22

// Exemplo 3: Arredondamento de um preço com mais casas decimais
decimal price3 = 10.2345678m;
decimal shrunkPrice3 = price3.ShrinkPrice(security);
Console.WriteLine($"Original price: {price3}, After ShrinkPrice: {shrunkPrice3}");
// Saída: preço original: 10.2345678, após ShrinkPrice: 10.23

// Exemplo 4: Utilizar ShrinkPrice ao criar uma ordem
var order = new Order
{
	Security = security,
	Price = 10.237m.ShrinkPrice(security)  // Arredondar o preço antes de criar a ordem
};
Console.WriteLine($"Order price: {order.Price}");
// Saída: preço da ordem: 10.24
```

## Aplicação

[ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) deve ser usado antes de enviar quaisquer ordens ou efetuar cálculos que exijam conformidade precisa do preço com as condições de mercado.

## Conclusão

A utilização correta de [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) ajuda a evitar erros ao colocar ordens e garante que os algoritmos de negociação funcionam corretamente de acordo com os requisitos do mercado.
