# Shell

**Shell** は、完全なオープンソースの C# で書かれた、取引端末の既製のグラフィカルフレームワークです。[Avalonia](https://avaloniaui.net/) で構築されており、Windows、Linux、macOS で動作します。

GUI（グラフィカルユーザーインターフェース）をゼロから構築する必要はありません。データソースへの接続、ポートフォリオ・銘柄・注文・約定のテーブル、チャート、履歴でのテスト、パラメーターの最適化、作業ログがすでに含まれています。残るのは、ストラテジー自体を書くことだけです。

![Shell のスクリーンショット](../images/shell_title_00.png)

## 主な機能

1. 完全なソースコード。カスタム取引ロボットにも、独自のソリューションにも適しています。
2. [コネクター](api/connectors.md) を通じた取引所およびブローカーへの **StockSharp プラットフォームのすべての接続のサポート**。
3. サイドパネルで切り替える 4 つの作業領域: [共通](shell/user_interface/common.md)、[リアルタイム](shell/user_interface/real_time.md)、[エミュレーション](shell/user_interface/emulation.md)、[ログ](shell/user_interface/log.md)。
4. 柔軟なインターフェイス: 作業領域内のすべてのパネルは、ドラッグ、分割、別ウィンドウへの切り離し、元の位置へのドッキングができます。レイアウトは次回の起動時にも保持されます。
5. 統計、約定のチャート、収益曲線による、履歴でのストラテジーのテスト。
6. [最適化](shell/user_interface/optimization.md) — 同じ履歴でのストラテジーパラメーターの総当たり。
7. リスクルールを含む、ストラテジー設定の保存と復元。
8. 複数のストラテジーの同時実行: 各ストラテジーは独立して動作し、独自の注文、約定、統計を持ちます。
9. [Designer](designer.md) で作成したストラテジーを、コードを書かずに実行。
10. [リモート管理](shell/remote_control.md) — サイトや Telegram ボットからのストラテジーの開始と停止。

## 付属のストラテジー

Shell には、起動直後に実行できる 4 つのストラテジーが付属しています。

| ストラテジー | 動作 |
|---|---|
| **SMA strategy** | 短期移動平均と長期移動平均の交差 |
| **SMA Index strategy** | 同じ内容ですが、式で指定した合成銘柄を対象にします |
| **StairsTrend strategy** | 同じ方向のローソク足が数本続いた後に、トレンドに沿ってエントリーします |
| **StairsCountertrend strategy** | 同じ状況で、トレンドに逆らってエントリーします |

## 推奨コンテンツ

[Shell のインストール](shell/installing_shell.md)

[クイックスタート](shell/quick_start.md)
