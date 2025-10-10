# Shadows of Doubt Floorplan Editor

A Unity 6 based tool for creating and editing floorplans for the game Shadows of Doubt.

The system works with a tile system, where each tile is assigned to an address type, and a room type.
Doors and windows are assigned on the connections between tiles.

!\[Custom Park with toilet block](https://raw.githubusercontent.com/piepieonline/ShadowsOfDoubt-FloorEditorUnity/refs/heads/main/github/parkCustomInGame.png)

## Getting started

1. Open the project in Unity 6000.0.23f1
2. Select the Editor object, and lock the inspector
3. In the Data Builder, select whatever you want as a base (Both vanilla game and custom floors will appear here), and select load
   !\[Vanilla Park](https://raw.githubusercontent.com/piepieonline/ShadowsOfDoubt-FloorEditorUnity/refs/heads/main/github/parkVanillaOpen.png)
4. You can now edit the floorplan (Example edit)
   !\[Custom Park](https://raw.githubusercontent.com/piepieonline/ShadowsOfDoubt-FloorEditorUnity/refs/heads/main/github/parkCustomComplete.png)
5. In the Data Builder, change the Floor Name, then Save

* This tool just makes the floorplans, it doesn't load them in.

## Working notes:

* Each floorplan is surrounded by 3 grid tiles of null.
* Each building has two default addresses - Outside, index 0. Lobby, index 1.
* Connections between rooms (even of the same type) require walls, though `NothingEntrance` can be used.
* Two disconnected rooms cannot share an ID.
