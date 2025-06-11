# Shadows of Doubt Floorplan Editor

A Unity 6 based tool for creating and editing floorplans for the game Shadows of Doubt.

The system works with a tile system, where each tile is assigned to an address type, and a room type.
Doors and windows are assigned on the connections between tiles.

![Custom Park with toilet block](https://raw.githubusercontent.com/piepieonline/ShadowsOfDoubt-FloorEditorUnity/refs/heads/main/github/parkCustomInGame.png)

## Getting started

1. Open the project in Unity 6
1. Select the Editor object, and lock the inspector
1. In the Data Builder, select `Park_GroundFloor01` (Both vanilla game and custom floors will appear here), and select load
![Vanilla Park](https://raw.githubusercontent.com/piepieonline/ShadowsOfDoubt-FloorEditorUnity/refs/heads/main/github/parkVanillaOpen.png)
1. In the Grid Manager, enable Address and Room painting
1. Ctrl-click a path node
1. From the middle of one of the paths, draw a line left for two blocks, then connect it to a 3x3 grid (that sits in the middle of the Park tiles, not touching the edge)
![Custom Park](https://raw.githubusercontent.com/piepieonline/ShadowsOfDoubt-FloorEditorUnity/refs/heads/main/github/parkCustomComplete.png)
1. In the Data Builder, change the Floor Name, then Save
1. TODO: Second floor to add a roof
1. TODO: Custom exterior model for floors 2 and above 
1. TODO: Loading into game

## Working notes:

- Each floorplan is surrounded by 3 grid tiles of null.
- Each building has two default addresses - Outside, index 0. Lobby, index 1.
- Connections between rooms (even of the same type) require walls, though `NothingEntrance` can be used.
- Two disconnected rooms cannot share an ID.