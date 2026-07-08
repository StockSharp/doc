# Modo dump

O conector FAST está disponível em modo dump. Nesse caso, os dados não vêm de uma conexão de rede real, mas dos arquivos acumulados pelo aplicativo [UDP Dumper](../../../../udp_dumper.md).

Para ativar o modo dump, passe os arquivos por meio do método de dump [IFastDialect.Dump](xref:StockSharp.Fix.Dialects.IFastDialect.Dump(System.Collections.Generic.IDictionary{Ecng.Net.MulticastSourceAddress,System.Collections.Generic.IEnumerable{System.IO.Stream}}))**(**[System.Collections.Generic.IDictionary\<Ecng.Net.MulticastSourceAddress,System.Collections.Generic.IEnumerable\<System.IO.Stream\>\>](xref:System.Collections.Generic.IDictionary`2) dumpFiles **)** antes de conectar:

```cs
// ... inicialização do conector
var fastAdapter = (FastMessageAdapter)connector.Adapters.InnerAdapters.First();
IEnumerable<string> dumpFiles = Directory.GetFiles(dumpDir, "*.bin");
var dict = dumpFiles.Select(f =>
{
	var name = Path.GetFileNameWithoutExtension(f);
	var parts = name.Split('_').Skip(1).ToArray();
	var groupAddr = parts[0];
	var port = parts[1];
	var sourceAddr = parts[2];
	if (sourceAddr.IsEmpty())
		sourceAddr = null;
	return Tuple.Create(new MulticastSourceAddress
	{
		GroupAddress = groupAddr.To<IPAddress>(),
		Port = port.To<int>(),
		SourceAddress = sourceAddr.To<IPAddress>(),
	}, f);
}).GroupBy(t => t.Item1).ToDictionary(g => g.Key, g => (IEnumerable<Stream>)g.Select(p => File.OpenRead(p.Item2)).ToArray());

fastAdapter.DialectSettings.Dump(dict);
// ...
connector.Connect();
```

Depois disso, o trabalho com o conector prossegue normalmente, como se estivesse recebendo dados da rede.
