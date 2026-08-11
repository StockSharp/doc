# TPO (Market profile)

A TPO (Time Price Opportunity) chart, also called Market Profile, shows how long price traded at each level during a session by stacking a letter or block for every time slot. It reveals the session's fair-value area, its point of control and where price spent little time (single prints).

## Live demo

```chart-demo tpo
```

## Setup

Add a `TpoSeries` and feed it OHLC bars that each carry a `sessionId`; the series builds the letter/block distribution per session itself:

```js
const series = chart.addSeries(SSChart.TpoSeries, {
  displayMode: SSChart.TpoDisplayMode.Auto,  // display mode: Auto | Letters | Blocks
  showPoc: true,
  showValueArea: true,
  showInitialBalance: true,
  showSinglePrints: true,
});

series.setData(tpoBars);  // { time, open, high, low, close, sessionId }

chart.timeScale().fitContent();
```

`displayMode` switches between letters and solid blocks (`Auto` picks by zoom). The overlays — point of control, value area, initial balance and single prints — can each be toggled independently.

## See also

- [JavaScript charts](../charts.md)
- [Volume profile](volume_profile.md)
- [Footprint](footprint.md)
