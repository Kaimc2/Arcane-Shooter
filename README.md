## About
Arcane Shooter is a first-person shooter (FPS) game set in a fantasy world where players engage in combat using magic powers. The game combines traditional FPS mechanics with unique magical abilities. The game can be played solo against AI-controlled bots or against other players in multiplayer (Multiplayer not finished).

## Features
### Magic Wands
The main weapons used in this game are elemental staves, which come in 5 types including:
- **Fire**: Shoot a fireball which explode upon impact dealing area-of-effect (AoE) damage and applying the Burn status effect on the target, which causes Damage Over Time (DoT).
- **Hyrdo**: Launches a water orb that collides with the target, dealing damage and applying the Wet status effect.
- **Electric**: Summon a lightning strike on the target, dealing damage and applying the Shock status effect.
- **Wind**: Summon a wind circle on the ground, pushing any target that enters into the sky. This can be used defensively or offensively as character can take fall damage.
- **Earth**: Raises a rock wall from the ground, providing protection against attack.
### Elemental Reactions
Our game also support some elemental reactions such as:
- **Overload**: When the target has both the Burn and Shock effects, an Overload reaction occurs creating a large explosion.
- **Smoke**: When the target has both the Wet and Burn effects, a Smoke reaction is triggered hindering visibility in the affected area.
### Gameplay
Arcane Shooter main gamemode is a team deathmatch mode against bots. The objective is simple: the first team to reach the target score wins. Player can customize match settings, with score goals ranging from 5 to 50.

## Demo
(⸝⸝⸝╸▵╺⸝⸝⸝) [Demo Link](https://youtu.be/0U700Ls4RRM)

## Installation Guide
if you want to run this project using the source code, please note that this project is pretty large (about 4.38 GB). Ensure you have sufficient storage space and a stable internet connection.
1. Clone the repository
```
git clone https://github.com/Kaimc2/Arcane-Shooter.git
```
or
```
git clone git@github.com:Kaimc2/Arcane-Shooter.git
```
2. Open the project in Unity. Ensure that you are using the same Unity version specified in the project (Unity 2022.3.47f1 with URP support).

## Third Party Assets
This project was made possible by the talented artists and asset creators in the Unity community. The following assets are used:
- [Lowpoly Magician RIO](https://assetstore.unity.com/packages/3d/characters/humanoids/lowpoly-magician-rio-288942) by VertexModeler Team.
- [Battle Wizard Poly Art](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/battle-wizard-poly-art-128097) by Dungeon Mason.
- [Gridbox Prototype Materials](https://assetstore.unity.com/packages/2d/textures-materials/gridbox-prototype-materials-129127) by Ciathyza.
- [Procedural Terrain Painter](https://assetstore.unity.com/packages/tools/terrain/procedural-terrain-painter-188357) by Staggart Creations.
- [Fantasy Wooden GUI : Free](https://assetstore.unity.com/packages/2d/gui/fantasy-wooden-gui-free-103811) by Black Hammer.
- [Particle Pack](https://assetstore.unity.com/packages/vfx/particles/particle-pack-127325) by Unity Technologies.
- [PolyArt - Ancient Village Pack](https://assetstore.unity.com/packages/3d/environments/fantasy/polyart-ancient-village-pack-166022) by Render Island.

## Credits
I would like to emphasize that this project is strictly for educational and non-commercial purposes. So I like to express my gratitude to many talented artists outside of Unity community too including:
- Audios and Sound Effects from [SoundCloud](https://soundcloud.com) and [Pixabay](https://pixabay.com).
- Element images borrow from a known popular games like Genshin Impact and Honkai: Star Rail by [Hoyoverse](https://www.hoyoverse.com/en-us/).

## Known Issues
### 1. Missing Textures (Pink Materials)
If you encounter pink textures after cloning the project, it is likely due to Unity's Universal Render Pipeline (URP) settings. This can happen if URP is not properly configured on your system.

**Fix**:
1. Go to Edit > Rendering > Render Pipeline Settings in Unity.
2. Assign the appropriate URP settings file:
    - Locate the URP Asset file in the project (Assets/Settings).
    - Drag and drop the URP Asset into the Graphics Settings under Edit > Project Settings > Graphics.
    - Or navigate to Edit > Project Settings > Graphics in Scriptable Render Pipeline Settings click the circle icon and select one of the setting.
3. Convert materials to URP:
    - In Project window click Select by Type (Shape icon second from search) > Material.
    - Select all the materials by using (Ctrl+A) or (CMD+A) then under Edit > Rendering > Material > Convert Selected Built-in Materials in URP.

<br/> **Note**: Make a backup before updating the materials.

### 2. Asset Import Warnings
Some assets may throw warnings during import. This is common and can usually be ignored unless they affect gameplay. 

## License
This project is for educational and non-commercial purposes. Respect the licenses of third-party assets.
