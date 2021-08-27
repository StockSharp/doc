# Экспорт произвольных таблиц

[QuikTrader](xref:StockSharp.Quik.QuikTrader) поддерживает экспорт не только стандартных таблиц, как Инструменты, Заявки и т.д., но и произвольных. Эта возможность необходима тем торговым алгоритмам, которые используют особый набор данных. Например, информацию, выводимую **QPILE** портфелем. 

### Предварительные условия

[Настройка Quik](QuikSetup.md)

### Экспорт свечей, экспортируемых с помощью QPILE из графика цены и объема

Экспорт свечей, экспортируемых с помощью QPILE из графика цены и объема

1. В начале необходимо загрузить **QPILE** скрипт в [Quik](Quik.md)\-е и создать по нему таблицу как показано на рисунке: 

   ![candleqpile](../images/candle_qpile.png)

   > [!TIP]
   > Исходные коды примера + qpile файл лежат в дистрибутиве в папке *Samples\\Quik\\DDE\\SampleDdeCustomTable*. 
2. Затем, описать строку таблицы со свечами в виде [.NET](https://ru.wikipedia.org/wiki/.NET_Framework) класса: 

   ```cs
   public class QuikCandleDateTime : Equatable<QuikCandleDateTime>
   {
   	[DdeCustomColumn("Дата", Order = 0)]
   	public string Date { get; set; }
   	[DdeCustomColumn("Время", Order = 1)]
   	public string Time { get; set; }
   	public override QuikCandleDateTime Clone()
   	{
   		throw new NotImplementedException();
   	}
   	public override int GetHashCode()
   	{
   		return this.Date.GetHashCode() ^ this.Time.GetHashCode();
   	}
   	protected override bool OnEquals(QuikCandleDateTime other)
   	{
   		return this.Date == other.Date && this.Time == other.Time;
   	}
   }
   [DdeCustomTable("Исторические свечи")]
   public class QuikCandle
   {
   	[Identity]
   	[InnerSchema]
   	public QuikCandleDateTime DateTime { get; set; }
   	[DdeCustomColumn("Цена открытия", Order = 2)]
   	public decimal OpenPrice { get; set; }
   	[DdeCustomColumn("Максимальная цена", Order = 3)]
   	public decimal HighPrice { get; set; }
   	[DdeCustomColumn("Минимальная цена", Order = 4)]
   	public decimal LowPrice { get; set; }
   	[DdeCustomColumn("Цена закрытия", Order = 5)]
   	public decimal ClosePrice { get; set; }
   	[DdeCustomColumn("Объем", Order = 6)]
   	public int Volume { get; set; }
   }
   ```

   DdeCustomTableAttribute Атрибутом [DdeCustomTableAttribute](xref:StockSharp.Quik.DdeCustomTableAttribute) задается название таблицы, из которой будут читаться данные. Дополнительно, через [StockSharp.Quik.DdeCustomColumnAttribute](xref:StockSharp.Quik.DdeCustomColumnAttribute) задаются название колонки в таблице и ее порядковый номер (нумерация идет с 0). 

   Через специальный атрибут [Ecng.Serialization.IdentityAttribute](xref:Ecng.Serialization.IdentityAttribute) указывается идентификатор экспортируемой строки. Это позволит не создавать каждый раз объекты, а использовать уже ранее созданные. В случае с таблицей исторических свечей, идентификатор состоит их двух параметров: дата и время. Поэтому они были вынесены в отдельный класс *QuikCandleDateTime*. 
3. Далее, с помощью типа *QuikCandle* создается [DdeCustomTable](xref:StockSharp.Quik.DdeCustomTable), описывающий формат таблицы в терминале [Quik](Quik.md): 

   ```cs
   _table = new DdeCustomTable(typeof(QuikCandle));
   ```
4. Созданная таблица добавляется через свойство [QuikTrader.CustomTables](xref:StockSharp.Quik.QuikTrader.CustomTables), чтобы [QuikTrader](xref:StockSharp.Quik.QuikTrader) смог начать обрабатывать неизвестный поток [DDE](https://en.wikipedia.org/wiki/Dynamic_Data_Exchange) данных 

   ```cs
   this.Trader.CustomTables.Add(_table);
   ```
5. Подключение к событию [QuikTrader.NewCustomTables](xref:StockSharp.Quik.QuikTrader.NewCustomTables): 

   ```cs
   this.Trader.NewCustomTables += (type, objects) =>
   {
   	// нас интересует только QuikCandle
   	if (type == typeof(QuikCandle))
   		_candlesWindow.Candles.AddRange(objects.Cast<QuikCandle>());
   };
   ```

   Если необходимо не только получать новые строчки таблицы, но так же знать, когда обновились полученные ранее строчки, то необходимо использовать событие [QuikTrader.CustomTablesChanged](xref:StockSharp.Quik.QuikTrader.CustomTablesChanged). В случае, если ни одно из полей не было помечено атрибутом [Ecng.Serialization.IdentityAttribute](xref:Ecng.Serialization.IdentityAttribute), то событие [QuikTrader.CustomTablesChanged](xref:StockSharp.Quik.QuikTrader.CustomTablesChanged) не будет никогда вызываться, и все изменения будут приходит как новые строчки через [QuikTrader.NewCustomTables](xref:StockSharp.Quik.QuikTrader.NewCustomTables). 
6. В итоге должно получиться следующее: 

   ![samplecandleqpile](../images/sample_candle_qpile.png)
7. Завершение работы: 

   ```cs
   this.Trader.Disconnect();
   ```
