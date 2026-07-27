# Connector configuration: DukasCopy JForex

Build the included Maven bridge with the official JForex SDK, or start a compatible bridge as a separate local process, and specify the connection parameters.

- `Login` - account or client identifier.
- `Password` - authentication credential.
- `IsDemo` - switch controlling connector behavior. Default value: `true`.
- `DemoAddress` - JForex JNLP service address used in demo mode.
- `LiveAddress` - JForex JNLP service address used in live mode.
- `BridgePort` - TCP port of the local bridge. Default value: `27431`.
- `BridgeJarPath` - optional path to the executable bridge JAR. Leave empty when the bridge is managed separately.

The bridge listens only on the loopback interface. Java is required because Dukascopy supports JForex as a Java API.
