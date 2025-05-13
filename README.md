# D* Lite Pathfinding in Unity

[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Unity Version](https://img.shields.io/badge/Unity-2021.x+-lightgrey.svg)](https://unity.com/)
[![Tests](https://img.shields.io/badge/Tests-Included-brightgreen.svg)](./Tests)
[![Help Wanted](https://img.shields.io/badge/Help-Unit%20Tests%20Welcome!-blueviolet.svg)](./Tests)

This repository demonstrates the D* Lite pathfinding algorithm implemented in Unity, offering a visual and interactive way to understand it. For a more detailed explanation of the D* Lite algorithm from a theoretical perspective, please refer to my accompanying documentation: [D* Lite Pathfinding Documentation](DStar_Lite_Pathfinding.pdf). Ensure that you have TextMeshPro installed for Unity. 

![image](https://github.com/user-attachments/assets/1ae6d5b6-e55f-4953-951c-20d094c1befa)

## Input

Interact with the environment using the mouse:

* **Left Click:** Set a new target destination for the agent.
* **Right Click:** Toggle the traversability of a cell (walkable/unwalkable).
* **Spacebar:** Pressing the spacebar will print snapshots of the most recently found path information to the console. This provides textual details about the `gCost`, `rhsCost`, `hCost`, and `key.k1` of the cells along the current path.

## Visual Information & Snapshots (Spacebar)

The grid uses colors to represent the state of each cell:

* **Yellow:** Target vertex.
* **Black:** Unwalkable vertex.
* **Red:** Inconsistent vertex (`gCost != rhsCost`).
* **Grey:** Default walkable vertex.
* **Blue:** Consistent, processed vertex.

Pressing the **Spacebar** will output snapshots of the most recent computed path to the Unity console. For each cell in the path, you will see:

* **Top Left:** `gCost`
* **Top Right:** `rhsCost`
* **Bottom Right:** `hCost (hCost == -1 means null)`
* **Middle:** `key.k1 (key.k1 == -1 means null)`

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT). See the `LICENSE` file for more details.


