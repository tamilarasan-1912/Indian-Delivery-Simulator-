# Indian Delivery Simulator — Build Status

## Current milestone: V0.1 Delivery Loop Foundation

Implemented in source:

- Delivery order domain model
- Order state machine
- Test order generation
- Accept-order flow
- Store pickup interaction
- Package pickup confirmation
- Customer arrival interaction
- OTP verification
- Cash-on-delivery collection
- Delivery completion
- Wallet earnings
- Basic first-person player controller
- Raycast interaction using `E`

## Target interaction flow

1. Generate test order.
2. Accept order.
3. Travel to the supermarket.
4. Interact with pickup point.
5. Confirm package pickup.
6. Travel to customer.
7. Interact with customer.
8. Verify OTP.
9. Collect COD when required.
10. Complete handover and receive earnings.

## Not yet implemented

- Unity scene/prefab assets
- Bike controller
- Vehicle physics
- Indian city environment
- Traffic AI
- Customer AI
- Phone/order UI
- Barcode scanner UI
- Navigation/GPS UI
- Save/load system
- Audio
- Final 3D art

## Important

The scripts are source-level Unity components and require a Unity project containing the normal `Assets`, `Packages`, and `ProjectSettings` structure before they can be compiled and tested in the Editor.
