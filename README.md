# 🚀 AterraEngine 🚀
AterraEngine is a data-driven game engine built in C# using Raylib.cs for rendering. Designed with simplicity and flexibility in mind, it supports modding capabilities out of the box, allowing users to easily modify and extend the engine’s functionality.

## 🎮 Features
- **Data-Driven Architecture**: The engine is structured to load game data from external files, making it easier to create, modify, and manage game content.
- **Mod-Friendly**: Mods can be loaded dynamically, making AterraEngine ready for user-created content with no additional overhead.
- **Raylib.cs Rendering**: Leverages the lightweight and powerful Raylib library for 2D and eventual 3D (on the roadmap, but not natively implemented yet) rendering, enabling fast and efficient graphics.
- **Customizable Game Logic**: Game developers can easily inject custom behaviors and systems, thanks to the engine’s modular design.

## 📦 Architecture
AterraEngine is built around a flexible data-driven design. Key components include:

- **Entity Component System (ECS)**: A custom built ECS framework, `AterraCore.Nexities` manages game objects as entities composed of reusable components.
- **Rendering Engine**: Powered by Raylib.cs to provide efficient 2D and 3D rendering.
- **Dependency Injection**: The entire engine, even the ECS framework, is built around dependency injection, making it easy to extend or override core game behavior through plugins.
- **Plugins and loadorder**: Allowing players to define their own load order of plugins built by the gamedevs or modders.

## 📜 License
AterraEngine is licensed under the GPL-3.0 license.

> Note: The AterraEngine logo and ducky sprites included in this project are © AndreasSas, and may not be used without explicit permission.
