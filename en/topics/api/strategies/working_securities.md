# Working Securities

## Description

The `GetWorkingSecurities()` method in the base class `Strategy` is used to obtain a list of instruments and data types that the strategy uses in its operation. This method plays an important role when working with the [Designer](../../designer.md).

## Purpose

The main purpose of the method is to provide the Designer with information about which instruments and data types are necessary for the strategy to work. This allows the Designer to:

1. Check for the availability of necessary historical data in the storage before starting testing
2. Automatically load the required data when available
3. Correctly set up subscriptions when launching the strategy

## Implementation

In the base class `Strategy`, the method returns an empty collection. For correct work with the Designer, it is recommended to override it in your strategy:

```cs
public override IEnumerable<(Security sec, DataType dt)> GetWorkingSecurities()
{
    // Return a list of pairs (instrument, data type) used by the strategy
    return new[] 
    { 
        (Security, CandleType),
        // Other instrument-data type pairs if the strategy uses multiple
    };
}
```

## Importance of Overriding the Method

If the `GetWorkingSecurities()` method is not overridden in your strategy:

- The Designer will not be able to automatically check for the necessary data
- If the required historical data is missing in the storage, the Designer will not issue warnings
- The strategy may be launched for testing, but no results will be shown
- The user will not receive any information about the reason for the absence of results

## Usage Example

```cs
public class MySmaStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;
    
    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }
    
    public MySmaStrategy()
    {
        _candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
    }
    
    // Override the method for correct work with the Designer
    public override IEnumerable<(Security sec, DataType dt)> GetWorkingSecurities()
    {
        return new[] { (Security, CandleType) };
    }
    
    // The rest of the strategy code...
}
```

## Conclusion

Although the `GetWorkingSecurities()` method is not mandatory for implementing the basic functionality of a strategy, overriding it is strongly recommended for correct work with the StockSharp Designer. This helps avoid situations where a strategy is launched for testing but does not show any results due to the absence of necessary historical data.
