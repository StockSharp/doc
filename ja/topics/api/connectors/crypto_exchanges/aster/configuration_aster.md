# Aster 設定

コネクターを使用するには、取引所アカウントで **APIキー** と **シークレット** を生成し、接続設定で指定します。

主な設定:

- **キー** と **シークレット**。
- **Section**: `Spot` または `Derivatives`。
- **Derivatives mode**: `Legacy` または `V3 Agent`。
- **Spot REST / Spot WS** エンドポイント。
- **Derivatives REST / Derivatives WS** エンドポイント。
- **Demo** モード。

公式 API ドキュメント:

- [Spot API 概要](https://asterdex.github.io/aster-api-website/spot/spot-api-overview/)
- [Spot アカウントおよび取引 API](https://asterdex.github.io/aster-api-website/spot/spot-account-and-trading-api/)
- [Spot WebSocket マーケットデータ](https://asterdex.github.io/aster-api-website/spot/websocket-market-data/)
- [Spot WebSocket アカウント情報](https://asterdex.github.io/aster-api-website/spot/websocket-account-info/)
- [Futures v3 一般情報](https://asterdex.github.io/aster-api-website/futures-v3/general-info/)
- [Futures ユーザーデータストリーム](https://asterdex.github.io/aster-api-website/futures/user-data-streams/)
- [Aster Code エンドポイント](https://asterdex.github.io/aster-api-website/asterCode/endpoints/)

> [!TIP]
> Aster デリバティブには 2 つのプロトコルファミリーがあります。取引を有効にする前に、正しい **Derivatives mode** を選択してください。
