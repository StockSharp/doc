# 命令行

**Runner** 是控制台应用程序，可以通过命令行参数以不同模式启动。不带参数启动程序时，会显示帮助信息，其中列出所有可用参数：

![Runner_command_line_1](../../images/runner_command_line_1.png)

使用 **Runner** 对历史数据进行测试：

```cmd
b -s SmaStrategy.cs -h "C:\Storage" --hf 20200401 --ht 20200430 --sec SBER@TQBR -r json
```

可用参数：

- -s — 策略文件路径，文件扩展名可以是 cs、json 或 dll。
- -t — 可选。如果选择了 dll 文件，并且该程序集包含多个策略类，则需要通过此参数指定所需类型。
- -h — 历史数据目录的路径。使用[服务器](../hydra_server.md)模式时，也可以指定网络地址。
- --hl — 可选。在[服务器](../hydra_server.md)模式下使用的登录名。
- --hp — 可选。在[服务器](../hydra_server.md)模式下使用的密码。
- --hf — 测试开始日期，格式为 YYYYMMDD。
- --ht — 测试结束日期，格式为 YYYYMMDD。
- -f — 可选。存储格式，可以是 Binary 或 Csv。
- --sec — 可选。[交易品种标识符](../api/instruments/instrument_identifier.md)。
- -r — 可选。测试结果报告的格式，可以是 json、xml 或 csv。
- --tm — 可选。策略超时时间。
- --memory — 可选。最大内存大小，单位为 MB。
- --cpu — 可选。处理器掩码。
- -l — 可选。日志级别，可以是 Info、Debug、Error、Warning 或 Verbose。

使用 **Runner** 进行优化：

```cmd
o -s SmaStrategy.cs -h "C:\Storage" --hf 20200401 --ht 20200430 --sec SBER@TQBR -r json -p sma_optimization.json
```

除历史测试模式的全部参数外，还支持以下参数：

- -p — 参数文件的路径。
- --ol — 可选。最大迭代次数。
- --ob — 可选。同时测试的策略数量。

参数文件格式：

```json
[
	{
	"Name": "SMA_80",
	"Value": "200,201"
	},
	{
	"Name": "SMA_30",
	"From": "40",
	"To": "50",
	"Step": "1"
	},
	{
	"Name": "Security",
	"Value": "SBER@TQBR,GAZP@TQBR"
	}
]
```

使用 **Runner** 进行实盘交易：

```cmd
l -s SmaStrategy.cs -c connector.json --tg telegram.json
```

- -c — 连接设置文件。
- --tg — Telegram 集成设置文件。
