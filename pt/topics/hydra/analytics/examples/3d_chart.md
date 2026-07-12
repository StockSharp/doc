# Gráfico 3D

O script `Chart3DScript` demonstra a criação de um gráfico 3D para visualizar a distribuição dos volumes de negociação por hora para vários instrumentos financeiros. Este método de visualização permite representar claramente a dinâmica de negociação e identificar picos de atividade do mercado.

![Gráfico 3D](../../../../images/hydra_analytics_chart3d.png)

## Descrição do Funcionamento do Script

O script analisa dados de candles para o período especificado, agrupa-os por hora e calcula o volume total de negociação para cada hora. Os resultados são apresentados num gráfico 3D, onde os eixos representam:

- **Eixo X**: Instrumentos financeiros.
- **Eixo Y**: Horas da sessão de negociação (de 0 a 23).
- **Eixo Z**: Volumes de negociação.

## Utilidade de Usar um Gráfico 3D

### Análise da Atividade do Mercado

O gráfico 3D permite avaliar quando ocorre a maior atividade para vários instrumentos em simultâneo. Isto pode ser útil para identificar janelas de negociação ideais ou estudar o impacto de eventos globais no mercado.

### Comparação de Instrumentos

Graças à visualização dos volumes de negociação por hora em espaço tridimensional, os traders podem comparar instrumentos entre si em termos de nível de atividade e horários de negociação preferenciais. Isto pode ajudar a selecionar os instrumentos mais líquidos em determinadas horas ou a encontrar instrumentos com padrões de atividade semelhantes para diversificação da carteira.

### Otimização da Estratégia

A análise da distribuição dos volumes de negociação pode servir de base para otimizar estratégias de negociação, permitindo a adaptação a intervalos temporais com maior atividade de mercado. Isto é particularmente relevante para negociação algorítmica e de alta frequência.

## Implementação do Script

O script executa as seguintes ações:

1. Verificar a presença de instrumentos financeiros para análise.
2. Formar etiquetas para os eixos X (instrumentos) e Y (horas).
3. Carregar e agrupar dados de candles.
4. Calcular os volumes totais de negociação por hora e preencher os dados do eixo Z.
5. Desenhar o gráfico 3D usando o método `panel.Draw3D`.

## Código do Script em C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// O script analítico, calcula a distribuição do maior volume por horas
	/// e mostra-a num gráfico 3D.
	/// </summary>
	public class Chart3DScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("Sem instrumentos.");
				return Task.CompletedTask;
			}

			var x = new List<string>();
			var y = new List<string>();

			// preencher etiquetas Y
			for (var h = 0; h < 24; h++)
				y.Add(h.ToString());

			var z = new double[securities.Length, y.Count];

			for (var i = 0; i < securities.Length; i++)
			{
				// parar o cálculo se o utilizador cancelar a execução do script
				if (cancellationToken.IsCancellationRequested)
					break;

				var security = securities[i];

				// preencher etiquetas X
				x.Add(security.ToStringId());

				// obter o armazenamento de candles
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				// obter datas disponíveis para o período especificado
				var dates = candleStorage.GetDates(from, to).ToArray();

				if (dates.Length == 0)
				{
					logs.LogWarning("Sem dados.");
					return Task.CompletedTask;
				}

				// agrupar candles pela hora de abertura (apenas a parte da hora) com truncagem de 1 hora
				var byHours = candleStorage.Load(from, to)
					.GroupBy(c => c.OpenTime.TimeOfDay.Truncate(TimeSpan.FromHours(1)))
					.ToDictionary(g => g.Key.Hours, g => g.Sum(c => c.TotalVolume));

				// preencher valores Z
				foreach (var pair in byHours)
					z[i, pair.Key] = (double)pair.Value;
			}

			panel.Draw3D(x, y, z, "Instrumentos", "Horas", "Volume negociado");

			return Task.CompletedTask;
		}
	}
}

```

## Código do Script em Python

```python
import clr

# Adicionar referências .NET
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Algo.Analytics")
clr.AddReference("Ecng.Drawing")

from Ecng.Drawing import DrawStyles
from System.Threading.Tasks import Task
from StockSharp.Algo.Analytics import IAnalyticsScript
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from numpy_extensions import nx

# O script analítico, calcula a distribuição do maior volume por horas e mostra-a num gráfico 3D.
class chart3d_script(IAnalyticsScript):
	def Run(
		self,
		logs,
		panel,
		securities,
		from_date,
		to_date,
		storage,
		drive,
		format,
		data_type,
		cancellation_token
	):
		# Verificar se não existem instrumentos
		if not securities:
			logs.LogWarning("Sem instrumentos.")
			return Task.CompletedTask

		x = []  # Etiquetas X para instrumentos
		y = []  # Etiquetas Y para horas

		# Preencher etiquetas Y com horas de 0 a 23
		for h in range(24):
			y.append(str(h))

		# Criar uma matriz 2D para valores Z com dimensões: (número de instrumentos) x (número de horas)
		z = [[0.0 for _ in range(len(y))] for _ in range(len(securities))]

		if data_type is None:
			logs.LogWarning(f"Tipo de dados não suportado {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for i, security in enumerate(securities):
			# Parar o cálculo se o utilizador cancelar a execução do script
			if cancellation_token.IsCancellationRequested:
				break

			# Preencher etiquetas X com identificadores do instrumento
			x.append(to_string_id(security))

			# Obter o armazenamento de candles para o instrumento atual
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			# Obter datas disponíveis para o período especificado
			dates = get_dates(candle_storage, from_date, to_date)

			if len(dates) == 0:
				logs.LogWarning("Sem dados.")
				return Task.CompletedTask

			# Agrupar candles pela hora de abertura (truncada à hora mais próxima) e somar volumes
			candles = load_range(candle_storage, message_type, from_date, to_date)
			by_hours = {}
			for candle in candles:
				hour = int(candle.OpenTime.TimeOfDay.TotalHours)
				by_hours[hour] = by_hours.get(hour, 0) + candle.TotalVolume

			# Preencher valores Z para o instrumento atual
			for hour, volume in by_hours.items():
				if hour < len(y):
					z[i][hour] = float(volume)

		# Desenhar o gráfico 3D usando o painel
		panel.Draw3D(x, y, nx.to2darray(z), "Instrumentos", "Horas", "Volume negociado")

		return Task.CompletedTask

```
