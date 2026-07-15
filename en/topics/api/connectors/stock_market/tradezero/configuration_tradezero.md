# TradeZero connector configuration

Create API credentials in the TradeZero portal and specify them in the connector settings.

- **Key** - the `TZ-API-KEY-ID` value.
- **Secret** - the `TZ-API-SECRET-KEY` value.
- **Default route** - an optional preferred order route returned by the account routes endpoint.

If the default route is empty, the connector selects a compatible live route automatically. Paper and live accounts use the same API host; the credentials determine the account environment.
