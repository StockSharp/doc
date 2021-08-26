# График эквити

[EquityCurveChart](../api/StockSharp.Xaml.Charting.EquityCurveChart.html) \- графический компонет для отбражения кривой доходности. 

Ниже приведен пример использования этого компонета. Полный код примера находится в Samples\/Testing\/SampleRandomEmulation. 

![Gui EquityCurveChart](../images/Gui_EquityCurveChart.png)

### Пример построения графика EquityCurveChart

Пример построения графика EquityCurveChart

1. В XAML добавляем графический компонент [EquityCurveChart](../api/StockSharp.Xaml.Charting.EquityCurveChart.html). Присваиваем компоненту имя **Curve**. 

   ```xaml
   <Window x:Class="SampleRandomEmulation.MainWindow"
           xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
           xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
           xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
           Title="{x:Static loc:LocalizedStrings.XamlStr564}" Height="460" Width="604"
   		xmlns:sx="clr-namespace:StockSharp.Xaml;assembly=StockSharp.Xaml"
   		xmlns:charting="http://schemas.stocksharp.com/xaml">
       
   	<Grid>
   		<Grid.ColumnDefinitions>
   			<ColumnDefinition Width="85*" />
   			<ColumnDefinition Width="497*" />
   		</Grid.ColumnDefinitions>
   		<Grid.RowDefinitions>
   			<RowDefinition Height="Auto"/>
   			<RowDefinition Height="*"/>
   		</Grid.RowDefinitions>
   		<Grid Grid.ColumnSpan="2">
   			<Grid.ColumnDefinitions>
   				<ColumnDefinition Width="100" />
   				<ColumnDefinition Width="*" />
   				<ColumnDefinition Width="Auto" />
   			</Grid.ColumnDefinitions>
   			<Grid.RowDefinitions>
   				<RowDefinition Height="Auto" />
   				<RowDefinition Height="10" />
   				<RowDefinition Height="Auto" />
   				<RowDefinition Height="10" />
   			</Grid.RowDefinitions>
   			<Button x:Name="StartBtn" Content="{x:Static loc:LocalizedStrings.Str2421}" Grid.Row="0" Click="StartBtnClick" />
   			<ProgressBar x:Name="TestingProcess" Grid.Column="1" Grid.Row="0" />
   			<Button x:Name="Report" Content="{x:Static loc:LocalizedStrings.XamlStr432}" Grid.Row="0" Width="75" IsEnabled="False" Click="ReportClick" Grid.Column="2" Margin="0,0,0,-1" />
   		</Grid>
   		
   		<Grid Grid.Row="1" Grid.ColumnSpan="2" Grid.Column="0">
   			<Grid>
   				<Grid.ColumnDefinitions>
   					<ColumnDefinition Width="180"/>
   					<ColumnDefinition Width="*"/>
   				</Grid.ColumnDefinitions>
   				<sx:StatisticParameterGrid Grid.Column="0" x:Name="ParameterGrid" />
   				<charting:EquityCurveChart Grid.Column="1" x:Name="Curve" />
   			</Grid>
   		</Grid>
   	</Grid>
   </Window>
   	  				
   ```
2. В коде главного окна создаем источник данных для отрисовки графика при помощи метода [CreateCurve](../api/Overload:StockSharp.Xaml.Charting.EquityCurveChart.CreateCurve.html). 

   ```cs
   private readonly ICollection<EquityData> _curveItems;
   .................................................
                 		
   public MainWindow()
   {
   	InitializeComponent();
   	_logManager.Listeners.Add(new FileLogListener("log.txt"));
   	_curveItems = Curve.CreateCurve("Equity", Colors.DarkGreen, ChartIndicatorDrawStyles.Line);
   }
   	  				
   ```
3. При изменении значения PnL стратегии добавляем данные в источник данных. При этом используем специальный класс [EquityData](../api/StockSharp.Xaml.Charting.EquityData.html). 

   ```cs
   _strategy.PnLChanged += () =>
   		{
   			var data = new EquityData
   			{
   				Time = _strategy.CurrentTime,
   				Value = _strategy.PnL,
   			};
   			
   			this.GuiAsync(() => _curveItems.Add(data));
   		};
   	  				
   ```
