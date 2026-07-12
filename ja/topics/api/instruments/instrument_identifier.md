# 金融商品の識別子

[S#](../../api.md) では、異なるソースの金融商品は統一された [Security.Id](xref:StockSharp.BusinessEntities.Security.Id) を使用します。これにより、取引アルゴリズムのコードは、[OpenECry](../connectors/stock_market/openecry.md)、[Rithmic](../connectors/stock_market/rithmic.md)、[Interactive Brokers](../connectors/stock_market/interactive_brokers.md) などの接続タイプに依存しなくなります。

金融商品識別子は、**\[銘柄コード\]@\[ボードコード\]** という構文を使用します。Apple Inc. 株式の場合、識別子は **AAPL@NASDAQ** です。デリバティブの場合、ボードコードは契約が取引されるボードです。たとえば、ES 指数の 6 月限先物契約は **ESM5@NYSE** として識別できます。

> [!TIP]
> [Hydra](../../hydra.md) は、履歴マーケットデータのフォルダー名に同じ仕組みを使用します。

## 識別子生成アルゴリズムのオーバーライド

1. 独自のアルゴリズムで金融商品識別子を生成するには、[SecurityIdGenerator](xref:StockSharp.Messages.SecurityIdGenerator) クラスの派生クラスを作成し、[SecurityIdGenerator.GenerateId](xref:StockSharp.Messages.SecurityIdGenerator.GenerateId(System.String,System.String))**(**[System.String](xref:System.String) secCode, [System.String](xref:System.String) boardCode **)** メソッドをオーバーライドします。

   ```cs
   class CustomSecurityIdGenerator : SecurityIdGenerator
   {
      public override string GenerateId(string secCode, string boardCode)
      {
         // CODE--BOARD 形式で識別子を生成します
         return secCode + "--" + boardCode;
      }
   }
   ```

2. 作成したジェネレーターをコネクターに渡します。

   ```cs
   connector.SecurityIdGenerator = new CustomSecurityIdGenerator();
   ```
