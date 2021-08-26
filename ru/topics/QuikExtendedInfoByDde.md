# Экспорт дополнительных колонок

[QuikTrader](../api/StockSharp.Quik.QuikTrader.html) в целях оптимизации экспортирует только минимальный набор данных из стандартных таблиц (Инструменты, Заявки и т.д.). Если для алгоритма необходимы данные из дополнительных колонок (или же требуется изменить порядок колонок, например, в таблице Инструменты размер лота идет сразу за название инструмента), то для этого существуют 2 варианта решения: 

1. Добавить отдельную таблицу в 

   [Quik](Quik.md)

   , которая будет дублировать существующую. Такую таблицу можно менять полностью, и это не отразиться на экспорте 

   [DDE](https://en.wikipedia.org/wiki/Dynamic_Data_Exchange)

   , так как экспорт будет идти по первой таблице. Единственное ограничение такого подхода \- такие таблицы не должны иметь одинаковый заголовок. 
2. Поменять существующую таблицу. А так же изменить метаданные, описывающие экспорт 

   [DDE](https://en.wikipedia.org/wiki/Dynamic_Data_Exchange)

   . Ниже рассмотрен пример такого сценария. 

### Предварительные условия

[Настройка Quik](QuikSetup.md)

### Экспорт дополнительных колонок

Экспорт дополнительных колонок

1. Пример работает с фьючерсными и опционными контрактами, и показывает как экспортировать колонки Волатильность, Теоретическая Цена, Базовый актив и Стоимость шага цены (пункт). Дополнительно, пример работает с расширенным стаканом, получая информацию о собственных объемах в котировках. 

   > [!TIP]
   > Исходные коды примера лежат в папке *Samples\\Quik\\DDE\\SampleDdeExtendedInfo*. Файл настроек для [Quik](Quik.md) называется info\_extended.wnd.
2. Вначале, необходимо настроить таблицу Инструменты: 

   ![securitiesex](~/images/security_ex_dde.png)
3. А так же окно со стаканом: 

   ![quotesex](~/images/quote_ex_dde.png)

   > [!TIP]
   > Пример работает с инструментом Лукойл. Для использования других инструментов необходимо настроить окна со стаканами по аналогии. 
4. Далее, необходимо изменить метаданные для инструментов и стаканов. Это делается через таблицу [DdeTable](../api/StockSharp.Quik.DdeTable.html). Для каждого типа торгового объекта существует своя отдельная таблица с метаданными, которые получаются через [QuikTrader](../api/StockSharp.Quik.QuikTrader.html): 

   | Название таблицы
        | Свойство с метаданными
                                                                                   |
   | ------------------------- | ---------------------------------------------------------------------------------------------------------- |
   | Инструменты
             | [QuikTrader.SecuritiesTable](../api/StockSharp.Quik.QuikTrader.SecuritiesTable.html)
                     |
   | Мои сделки
              | [QuikTrader.MyTradesTable](../api/StockSharp.Quik.QuikTrader.MyTradesTable.html)
                         |
   | Все сделки
              | [QuikTrader.TradesTable](../api/StockSharp.Quik.QuikTrader.TradesTable.html)
                             |
   | Заявки
                  | [QuikTrader.OrdersTable](../api/StockSharp.Quik.QuikTrader.OrdersTable.html)
                             |
   | Стоп\-заявки
            | [QuikTrader.StopOrdersTable](../api/StockSharp.Quik.QuikTrader.StopOrdersTable.html)
                     |
   | Портфель по бумагам
     | [QuikTrader.EquityPortfoliosTable](../api/StockSharp.Quik.QuikTrader.EquityPortfoliosTable.html)
         |
   | Портфель по деривативам
 | [QuikTrader.DerivativePortfoliosTable](../api/StockSharp.Quik.QuikTrader.DerivativePortfoliosTable.html)
 |
   | Позиции по бумагам
      | [QuikTrader.EquityPositionsTable](../api/StockSharp.Quik.QuikTrader.EquityPositionsTable.html)
           |
   | Позиции по деривативам
  | [QuikTrader.DerivativePositionsTable](../api/StockSharp.Quik.QuikTrader.DerivativePositionsTable.html)
   |
   | Стакан
                  | [QuikTrader.QuotesTable](../api/StockSharp.Quik.QuikTrader.QuotesTable.html)
                             |

   Через [QuikTrader.SecuritiesTable](../api/StockSharp.Quik.QuikTrader.SecuritiesTable.html) и [QuikTrader.QuotesTable](../api/StockSharp.Quik.QuikTrader.QuotesTable.html) добавляются требуемые колонки в том порядке, в которым они были добавлены в [Quik](Quik.md): 

   ```cs
   \/\/ добавляем на экспорт необходимые колонки
   this.Trader.SecuritiesTable.Columns.Add(DdeSecurityColumns.Volatility);
   this.Trader.SecuritiesTable.Columns.Add(DdeSecurityColumns.TheorPrice);
   this.Trader.SecuritiesTable.Columns.Add(DdeSecurityColumns.BaseSecurity);
   this.Trader.SecuritiesTable.Columns.Add(DdeSecurityColumns.MinStepPrice);
   \/\/ добавляем экспорт дополнительных колонок из стакана (своя продажа и покупка)
   this.Trader.QuotesTable.Columns.Add(DdeQuoteColumns.OwnAskVolume);
   this.Trader.QuotesTable.Columns.Add(DdeQuoteColumns.OwnBidVolume);
   ```

   Если колонки добавляются не в конец, а перемешаны с основными колонками, то необходимо вставлять колонки относительно их порядка следования друг за другом в таблице: 

   ```cs
   \/\/ вставить колонку волатильность, чтобы она была 5\-ой с начала (нумерация идет с нуля)
   \/\/ все последующие колонки за волатильностью автоматически перестраивают свой порядковый номер
   this.Trader.SecuritiesTable.Columns.Insert(4, DdeSecurityColumns.Volatility);
   ```
5. После этого, через события [Connector.NewSecurity](../api/StockSharp.Algo.Connector.NewSecurity.html) и [Connector.SecurityChanged](../api/StockSharp.Algo.Connector.SecurityChanged.html) будут приходить объекты [Security](../api/StockSharp.BusinessEntities.Security.html), содержащие расширенную информацию. Чтобы ее получить в коде, необходимо воспользоваться свойством [Security.ExtensionInfo](../api/StockSharp.BusinessEntities.Security.ExtensionInfo.html): 

   ```cs
   Trader.NewSecurity +\= security \=\> \_securitiesWindow.SecurityPicker.Securities.Add(security);
   					
   ```

   > [!TIP]
   > [Security](../api/StockSharp.BusinessEntities.Security.html) имеет ряд свойств, которые упрощают доступ к расширенной информации. Это свойства [OpenPrice](../api/StockSharp.BusinessEntities.Security.OpenPrice.html), [ClosePrice](../api/StockSharp.BusinessEntities.Security.ClosePrice.html), [HighPrice](../api/StockSharp.BusinessEntities.Security.HighPrice.html), [LowPrice](../api/StockSharp.BusinessEntities.Security.LowPrice.html), [MaxPrice](../api/StockSharp.BusinessEntities.Security.MaxPrice.html), [MinPrice](../api/StockSharp.BusinessEntities.Security.MinPrice.html), [StepPrice](../api/StockSharp.BusinessEntities.Security.StepPrice.html), [MarginBuy](../api/StockSharp.BusinessEntities.Security.MarginBuy.html), [MarginSell](../api/StockSharp.BusinessEntities.Security.MarginSell.html), [ExpiryDate](../api/StockSharp.BusinessEntities.Security.ExpiryDate.html) и [SettlementDate](../api/StockSharp.BusinessEntities.Security.SettlementDate.html). Поэтому в примере Стоимость шага цены берется не через [Security.ExtensionInfo](../api/StockSharp.BusinessEntities.Security.ExtensionInfo.html), а через [Security.StepPrice](../api/StockSharp.BusinessEntities.Security.StepPrice.html)
6. Получения стакана:

   ```cs
   private void DepthClick(object sender, RoutedEventArgs e)
   {
   	var trader \= MainWindow.Instance.Trader;
   	var window \= \_quotesWindows.SafeAdd(SecurityPicker.SelectedSecurity, security \=\>
   	{
   		\/\/ начинаем получать котировки стакана
   		trader.SubscribeMarketDepth(security);
   		\/\/ создаем окно со стаканом
   		var wnd \= new QuotesWindow { Title \= security.Id + " " + LocalizedStrings.MarketDepth };
   		wnd.MakeHideable();
   		return wnd;
   	});
   	if (window.Visibility \=\= Visibility.Visible)
   		window.Hide();
   	else
   		window.Show();
   	if (\!\_initialized)
   	{
   		TraderOnMarketDepthChanged(trader.GetMarketDepth(SecurityPicker.SelectedSecurity));
   		trader.MarketDepthChanged +\= TraderOnMarketDepthChanged;
   		\_initialized \= true;
   	}
   }
   ```
7. В итоге должно получиться следующее: 

   ![samplesecuritiesex](~/images/sample_securities_ex.png)

   И для стакана Лукойл: 

   ![samplequotesex](~/images/sample_quote_ex.png)

### Следующие шаги

[Экспорт произвольных таблиц](QuikAnyTableByDde.md)

## См. также
