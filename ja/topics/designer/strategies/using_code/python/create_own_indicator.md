# インジケーターの作成

[API](../../../../api.md) でカスタムインジケーターを作成する方法は、[カスタムインジケーター](../../../../api/indicators/custom_indicator.md)セクションで説明されています。このようなインジケーターは **Designer** と完全に互換性があります。

インジケーターを作成するには、**スキーム** パネルで **インジケーター** フォルダーを選択し、右クリックしてコンテキストメニューから **追加** を選択する必要があります。

![Designer ソースコードインジケーター 00](../../../../../images/designer_source_code_indicator_00.png)

インジケーターコードは次のようになります。

```python
import clr
import random

clr.AddReference("StockSharp.BusinessEntities")
clr.AddReference("StockSharp.Algo")

from StockSharp.Algo.Indicators import BaseIndicator, DecimalIndicatorValue
from indicator_extensions import *

class empty_indicator(BaseIndicator):
	"""
	パラメーターの保存と読み込みを示すサンプルインジケーター。

	Doc https://doc.stocksharp.com/topics/designer/strategies/using_code/python/create_own_indicator.html

	入力価格を +20% または -20% 変化させます。
	"""
	def __init__(self):
		super(empty_indicator, self).__init__()
		self._change = 20
		self._counter = 0
		self._isFormed = False

	@property
	def Change(self) -> int:
		return self._change

	@Change.setter
	def Change(self, value):
		self._change = value
		self.Reset()

	def CalcIsFormed(self):
		"""インジケーターが形成済みと見なされるだけの十分な入力を受け取ったかどうかを判定します。"""
		return self._isFormed

	def Reset(self):
		"""インジケーターの状態と内部カウンターをリセットします。"""
		super(empty_indicator, self).Reset()
		self._isFormed = False
		self._counter = 0

	def OnProcess(self, input):
		"""
		入力されたインジケーター値を処理し、ランダムな変化を適用します。

		:param input: 入力されたインジケーター値。
		:return: 変更適用後の新しい DecimalIndicatorValue。
		"""
		# 10 回に 1 回、空の値を返そうとする
		if random.randint(0, 10) == 0:
			return DecimalIndicatorValue(self, input.Time)

		if self._counter == 5:
			# 例として、このインジケーターは形成済みになるために 5 個の入力を必要とする
			self._isFormed = True
		self._counter += 1

		value = to_decimal(input)

		# 現在値に +/- _change パーセントのランダムな変化を適用する
		value += value * random.randint(-self._change, self._change) / 100.0

		result = DecimalIndicatorValue(self, value, input.Time)
		# ランダムな判定に基づいて値を最終値としてマークする
		result.IsFinal = bool(random.getrandbits(1))
		return result

	def Load(self, storage):
		"""
		永続ストレージからインジケーターパラメーターを読み込みます。

		:param storage: 読み込み元の設定ストレージ。
		"""
		super(empty_indicator, self).Load(storage)
		self.Change = storage.GetValue("Change", self.Change)

	def Save(self, storage):
		"""
		インジケーターパラメーターを永続ストレージへ保存します。

		:param storage: 保存先の設定ストレージ。
		"""
		super(empty_indicator, self).Save(storage)
		storage.SetValue("Change", self.Change)

	def __str__(self):
		return f"Change: {self.Change}"

	def ToString(self):
		return str(self)
```

このインジケーターは入力値を受け取り、指定された **変更** パラメーター値に基づいてランダムな偏差を加えます。

インジケーターメソッドの説明は、[カスタムインジケーター](../../../../api/indicators/custom_indicator.md)セクションで確認できます。

作成したインジケーターをスキームに追加するには、[インジケーター](../../using_visual_designer/elements/common/indicator.md) キューブを使用し、その内部で目的のインジケーターを設定する必要があります。

![Designer ソースコードインジケーター 01](../../../../../images/designer_source_code_indicator_01.png)

インジケーターコードで事前に設定した **変更** パラメーターが、プロパティパネルに表示されます。

> [!WARNING]
> Python コードのインジケーターは、Python コードで作成されたストラテジーでは使用できません。[キューブから](../../using_visual_designer.md)作成されたストラテジーでのみ使用できます。
