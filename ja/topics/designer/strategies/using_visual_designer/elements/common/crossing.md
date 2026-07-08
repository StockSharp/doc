# Crossing

![Designer Crossing 00](../../../../../../images/designer_crossing_00.png)

この要素は、2 つの値の相対的な位置を追跡するために使用します。たとえば、2 本の線が交差する瞬間を判定するために使用します。

比較は、2 つのソケット **Up** と **Down** の値に関して行われます。

## 入力ソケット

- **Up** - 比較可能な値（たとえば、数値、インジケーター値など）。
- **Down** - 比較可能な値（たとえば、数値、インジケーター値など）。

## 出力ソケット

- **Flag** - **Up** が **Down** より大きい場合は true、それ以外の場合は false。

![Designer Crossing 01](../../../../../../images/designer_crossing_01.png)

2 つの [SMA インジケーター](../../../../../api/indicators/list_of_indicators/sma.md)の交差を追跡するために Crossing ブロックを使用する例です。2 つの Crossing ブロックが使用され、それぞれ長期 SMA が短期 SMA より大きい場合と小さい場合に応じて、個別に true を出力します。

## 関連項目

[Value Delay](delay_value.md)

