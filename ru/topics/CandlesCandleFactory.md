# Собственный тип свечей

[S\#](StockSharpAbout.md) позволяет расширить возможности построения свечей, давая возможность работать с произвольными типами свечей. Это полезно в тех случаях, когда требуется работать со свечами, не поддерживаемыми в данный момент [S\#](StockSharpAbout.md). Ниже описан план действия по добавлению тиковых свечей (свечи, которые формируются по количеству сделок).

### Реализация тиковых свечей

Реализация тиковых свечей

1. В начале необходимо создать свой тип свечи. Тип должен наследоваться от класса [Candle](xref:StockSharp.Algo.Candles.Candle):

   > [!CAUTION]
   > Тиковые свечи поддерживаются [S\#](StockSharpAbout.md) стандартно и данный шаг представлен лишь в качестве примера.

   ```cs
   /// <summary>
   /// Свеча, группируемая по количеству сделок.
   /// </summary>
   public class TickCandle : Candle
   {
       /// <summary>
       /// Параметр свечи.
       /// </summary>
       public override object Arg
       {
           get
           {
               return this.TradeCount;
           }
           set
           {
               this.TradeCount = (int) value;
           }
       }
       /// <summary>
       /// Максимальное количество сделок, которое может содержать свеча.
       /// </summary>
       public int TradeCount { get; set; }
   }
   ```
2. Дополнительно, необходимо создать свой тип сообщения свечи. Подробнее, о [сообщениях](Messages.md). Тип должен наследоваться от класса [CandleMessage](xref:StockSharp.Messages.CandleMessage):

   ```cs
   /// <summary>
   /// Свеча, группируемая по количеству сделок.
   /// </summary>
   public class TickCandleMessage : CandleMessage
   {
   	public TickCandleMessage()
   		: base(MessageTypes.CandleTick)
   	{
   	}
   	/// <summary>
   	/// Максимальное количество сделок, которое может содержать свеча.
   	/// </summary>
   	public int MaxTradeCount { get; set; }
   	/// <summary>
   	/// Создать копию <see cref="TickCandleMessage"/>.
   	/// </summary>
   	/// <returns>Копия.</returns>
   	public override Message Clone()
   	{
   		return CopyTo(new TickCandleMessage
   		{
   			MaxTradeCount = MaxTradeCount
   		});
   	}
   	/// <summary>
   	/// Параметр свечи.
   	/// </summary>
   	public override object Arg
   	{
   		get => MaxTradeCount;
   		set => MaxTradeCount = (int)value;
   	}
   }
   ```
3. Далее требуется создать построителя свечей, для нового типа свечей. Для этого необходимо создать реализацию [CandleBuilder\`1](xref:StockSharp.Algo.Candles.Compression.CandleBuilder`1). В метод [ProcessValue](xref:StockSharp.Algo.Candles.Compression.CandleBuilder`1.ProcessValue) будет поступать значение типа [ICandleBuilderValueTransform](xref:StockSharp.Algo.Candles.Compression.ICandleBuilderValueTransform). В зависимости от настроек может содержать данные как о тиковой сделке [TickCandleBuilderValueTransform](xref:StockSharp.Algo.Candles.Compression.TickCandleBuilderValueTransform), так и о стакане [QuoteCandleBuilderValueTransform](xref:StockSharp.Algo.Candles.Compression.QuoteCandleBuilderValueTransform).

   Метод [ProcessValue](xref:StockSharp.Algo.Candles.Compression.CandleBuilder`1.ProcessValue) должен возвратить или новую свечу (если новые данные привели к формированию свечи), или обновить переданную (если данных пока недостаточно для создания новой свечи). Если метод [ProcessValue](xref:StockSharp.Algo.Candles.Compression.CandleBuilder`1.ProcessValue) возвращает новую свечу, то [CandleBuilder\`1](xref:StockSharp.Algo.Candles.Compression.CandleBuilder`1) вызывает его еще раз, передав в метод то же самое значение [ICandleBuilderValueTransform](xref:StockSharp.Algo.Candles.Compression.ICandleBuilderValueTransform). Метод будет вызываться до тех пор, пока [ProcessValue](xref:StockSharp.Algo.Candles.Compression.CandleBuilder`1.ProcessValue) не вернет переданную свечу. Это сделано для тех случаев, когда по одному входящему значению [ICandleBuilderValueTransform](xref:StockSharp.Algo.Candles.Compression.ICandleBuilderValueTransform) может быть сформировано несколько свечей: 

   ```cs
   /// <summary>
   /// Построитель свечей типа <see cref="T:StockSharp.Algo.Candles.TickCandle" />.
   /// </summary>
   public class TickCandleBuilder : CandleBuilder<TickCandleMessage>
   {
       /// <summary>
       /// Создать <see cref="T:StockSharp.Algo.Candles.Compression.TickCandleBuilder" />.
       /// </summary>
       public TickCandleBuilder()
       {
       }
       /// <summary>
       /// Создать <see cref="T:StockSharp.Algo.Candles.Compression.TickCandleBuilder" />.
       /// </summary>
       public TickCandleBuilder()
       {
       }
       /// <summary>
       /// Создать новую свечу.
       /// </summary>
       /// <param name="series">Серия свечей.</param>
       /// <param name="transform">Данные, с помощью которых необходимо создать новую свечу.</param>
       /// <returns>Созданная свеча.</returns>
       protected override TickCandle CreateCandle(CandleSeries series, ICandleBuilderValueTransform transform)
       {
           var candle = new TickCandleMessage
           {
               TradeCount = (int)series.Arg,
               OpenTime = transform.Time,
               CloseTime = transform.Time
           };
           return this.FirstInitCandle(series, candle, transform);
       }
       /// <summary>
       /// Получить временные диапазоны, для которых у данного источника для передаваемой серии свечей есть данные.
       /// </summary>
       /// <param name="series">Серия свечей.</param>
       /// <returns>Временные диапазоны.</returns>
       public override IEnumerable<Range<DateTime>> GetSupportedRanges(CandleSeries series)
       {
           IEnumerable<Range<DateTime>> supportedRanges = base.GetSupportedRanges(series);
           if (!supportedRanges.IsEmpty<Range<DateTime>>())
           {
               if (!(series.Arg is int))
               {
                   throw new ArgumentException();
               }
               if (((int) series.Arg) <= 0)
               {
                   throw new ArgumentOutOfRangeException();
               }
           }
           return supportedRanges;
       }
       /// <summary>
       /// Сформирована ли свеча до добавления данных.
       /// </summary>
       /// <param name="series">Серия свечей.</param>
       /// <param name="candle">Свеча.</param>
       /// <param name="transform">Данные, с помощью которых принимается решение о необходимости окончания формирования текущей свечи.</param>
       /// <returns>True, если свечу необходимо закончить. Иначе, false.</returns>
       protected override bool IsCandleFinishedBeforeChange(CandleSeries series, TickCandleMessage candle, ICandleBuilderValueTransform transform)
       {
           return candle.TotalTicks != null && candle.TotalTicks.Value >= candle.MaxTradeCount;
       }
       /// <summary>
       /// Обновить свечу данными.
       /// </summary>
       /// <param name="series">Серия свечей.</param>
       /// <param name="candle">Свеча.</param>
       /// <param name="transform">Данные.</param>
       protected override void UpdateCandle(CandleSeries series, TickCandleMessage candle, ICandleBuilderValueTransform transform)
       {
   		base.UpdateCandle(series, candle, transform);
   		candle.TotalTicks++;
       }
   }
   ```
4. Затем необходимо получить [CandleBuilderProvider](xref:StockSharp.Algo.Candles.Compression.CandleBuilderProvider) из подключения и добавить в него [TickCandleBuilder](xref:StockSharp.Algo.Candles.Compression.TickCandleBuilder):

   > [!CAUTION]
   > [TickCandleBuilder](xref:StockSharp.Algo.Candles.Compression.TickCandleBuilder), как источник свечей, стандартно присутствует в [CandleBuilderProvider](xref:StockSharp.Algo.Candles.Compression.CandleBuilderProvider). Данный шаг представлен лишь в качестве примера.

   ```cs
   private Connector _connector;
   ...
   _connector.Adapter.CandleBuilderProvider.Register(new TickCandleBuilder());
   ```
5. Создать [CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries) и запросить по ней данные:

   ```cs
   var series = new CandleSeries(typeof(TickCandle), _security, 1000);
   ...
   _connector.SubscribeCandles(series);
   ```
