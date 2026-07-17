# コネクターの設定: Databento

プロバイダーから認証情報を取得し、接続パラメーターを指定します。

- `Key` - 認証用の資格情報です。
- `Dataset` - 接続パラメーターです。 既定値: `GLBX.MDP3`.
- `LiveAddress` - サービスのアドレスです。
- `HistoricalAddress` - サービスのアドレスです。 既定値: `https://hist.databento.com/v0/timeseries.get_range`.
- `Symbology` - コネクターの動作モードまたは選択項目です。 既定値: `DatabentoSymbologyTypes.RawSymbol`.
