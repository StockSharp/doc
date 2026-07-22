# ProBit Global Connector Settings

Specify the ProBit Global connection parameters in the connector settings.

## Connection parameters

- `Key` - OAuth client ID issued in ProBit API credentials.
- `Secret` - OAuth client secret.
- `RestEndpoint` - REST API address.
- `AuthEndpoint` - OAuth token endpoint address.
- `WebSocketEndpoint` - WebSocket server address.

Public market data works without credentials. Trading, balances, order history, and private WebSocket channels require `Key` and `Secret`.

For a market buy, set the quote-currency amount in `ProBitOrderCondition.QuoteAmount`.

## Official API documentation

- [ProBit Global API documentation](https://docs-en.probit.com/)
- [ProBit Global API credentials](https://www.probit.com/en-us/my-page/api-management/api-credential)
