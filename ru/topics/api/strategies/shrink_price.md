# Округление цены

## Введение

Метод [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) в StockSharp является важным инструментом для корректного округления цен в соответствии с требованиями рынка. Это необходимо для обеспечения того, чтобы отправляемые заявки соответствовали правилам биржи или брокера.

## Назначение

Основная цель [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) - округлить цену до допустимого значения, учитывая:
1. Шаг цены инструмента ([Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep))
2. Количество десятичных знаков после запятой ([Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals))

## Важность использования

Использование [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) критично для:
- Предотвращения отклонения заявок биржей или брокером из-за некорректной цены
- Обеспечения точности в расчетах и торговых операциях
- Соответствия правилам и ограничениям конкретного рынка или инструмента

## Принцип работы

1. Если [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep) задан:
   - Цена округляется до ближайшего значения, кратного шагу цены
2. Если [Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals) задан:
   - Цена округляется до указанного количества знаков после запятой
3. Если оба параметра заданы:
   - Применяется более строгое округление (обычно до шага цены)

## Пример использования

```cs
// Создаем объект Security с заданными параметрами
var security = new Security
{
    PriceStep = 0.01m,  // Шаг цены 0.01
    Decimals = 2        // Два знака после запятой
};

// Примеры использования ShrinkPrice

// Пример 1: Округление до шага цены
decimal price1 = 10.234m;
decimal shrunkPrice1 = price1.ShrinkPrice(security);
Console.WriteLine($"Исходная цена: {price1}, После ShrinkPrice: {shrunkPrice1}");
// Выведет: Исходная цена: 10.234, После ShrinkPrice: 10.23

// Пример 2: Округление цены, которая уже соответствует шагу
decimal price2 = 10.22m;
decimal shrunkPrice2 = price2.ShrinkPrice(security);
Console.WriteLine($"Исходная цена: {price2}, После ShrinkPrice: {shrunkPrice2}");
// Выведет: Исходная цена: 10.22, После ShrinkPrice: 10.22

// Пример 3: Округление цены с большим количеством знаков после запятой
decimal price3 = 10.2345678m;
decimal shrunkPrice3 = price3.ShrinkPrice(security);
Console.WriteLine($"Исходная цена: {price3}, После ShrinkPrice: {shrunkPrice3}");
// Выведет: Исходная цена: 10.2345678, После ShrinkPrice: 10.23

// Пример 4: Использование ShrinkPrice при создании ордера
var order = new Order
{
    Security = security,
    Price = 10.237m.ShrinkPrice(security)  // Округляем цену перед созданием ордера
};
Console.WriteLine($"Цена ордера: {order.Price}");
// Выведет: Цена ордера: 10.24
```

## Применение

[ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) следует использовать перед отправкой любых ордеров или при расчетах, где требуется точное соответствие цены рыночным условиям.

## Заключение

Правильное использование [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) помогает избежать ошибок при выставлении заявок и обеспечивает корректную работу торговых алгоритмов в соответствии с требованиями рынка.