# Global-Wetness

Global-Wetness is a Unity tool designed to dynamically adjust material glossiness across a scene, simulating effects like wetness. This tool leverages Unity's Material Property Blocks to achieve its effects efficiently without increasing the number of draw calls.

## Features

- **Layer-Based Adjustments**: Apply glossiness effects to specified layers, allowing for targeted visual enhancements.
- **Dynamic Interpolation**: Smoothly transition between dry and wet states over a defined duration using coroutines.
- **Optimal Performance**: Utilizes Material Property Blocks to modify material properties without creating new material instances, maintaining optimal performance.

## Getting Started

### Prerequisites

- Unity 2019.4 LTS or newer.

### Setup

1. Attach the `GlobalWetness` script to a GameObject in your scene.
2. Set the LayerMask to the layers you want the wetness effect applied to.
3. Adjust the `initialGlossiness`, `targetGlossiness`, and `interpolationDuration` parameters as needed in the inspector.

## Understanding Material Property Blocks

Material Property Blocks are a powerful feature in Unity that allow developers to modify material properties on a per-renderer basis without the need to create new material instances. This is particularly beneficial for performance:

- **Reduced Draw Calls**: By modifying properties via Material Property Blocks, multiple objects can share the same material while displaying different visual properties, reducing the need for additional draw calls that would be required for unique materials.
- **Dynamic Changes**: Property blocks make it easy to adjust visuals dynamically at runtime without permanently altering the underlying material, ideal for effects like weather changes, health indications, or temporary buffs in gameplay.
- **Memory Efficiency**: Using Material Property Blocks helps keep the project's memory footprint low, as fewer materials are loaded into memory.

## License

Distributed under the MIT License. See `LICENSE` for more information.
