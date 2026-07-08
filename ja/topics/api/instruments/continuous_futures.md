# 連続先物

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) は、通常は先物契約である連続金融商品で、有効期限のある金融商品を含みます。

たとえば、2 つの E-mini S&P 500 先物 **ESM5** と **ESU5** を考えます。**ESM5** が満期になると、連続金融商品は次の契約である **ESU5** に自動的に切り替わります。

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) は [Security](xref:StockSharp.BusinessEntities.Security) と同じ方法で取引できます。**ESM5** の満期前は、その金融商品を通じて取引が行われます。満期後は **ESU5** を通じて取引が行われ、その後も同様に続きます。

## ExpirationContinuousSecurity の作成

1. [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) に含める構成金融商品を宣言し、[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) 自体も宣言します。

   ```cs
   private Security _esm5;
   private Security _esu5;
   private ExpirationContinuousSecurity _es;
   							
   ```
2. [ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) を作成します。

   ```cs
   _es = new ExpirationContinuousSecurity { Board = ExchangeBoard.Cme, Id = "ES" };
   							
   ```
3. 構成金融商品を追加し、それぞれの有効期限の日時を指定します。

   ```cs
   _es.ExpirationJumps.Add(_esm5.ToSecurityId(), new DateTime(2015, 6, 15, 18, 45, 00));
   _es.ExpirationJumps.Add(_esu5.ToSecurityId(), new DateTime(2015, 9, 15, 18, 45, 00));

   ```

## VolumeContinuousSecurity

[ExpirationContinuousSecurity](xref:StockSharp.Algo.ExpirationContinuousSecurity) に加えて、StockSharp は [VolumeContinuousSecurity](xref:StockSharp.Algo.VolumeContinuousSecurity) も提供します。この種類の連続金融商品は、有効期限ではなく取引出来高に基づいて契約間を切り替えます。新しい契約の取引出来高が現在の契約の出来高を超えたときに、次の契約へ移行します。
