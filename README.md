# PacManUnity

<div align="center">
    <img src="https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white" alt="UnityLogo" style="height: 50px"/>
    <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="CsharpLogo" style="height: 50px"/>
<img src="https://user-images.githubusercontent.com/59691442/201502753-3eb47182-da10-4900-b566-6c360a5ede74.png" alt="webGlLogo" style="height: 50px">
</div>

## Description

Pac-Man 3D game made in Unity with C#, using Krita for the game assets.

The original map is playable as well as two other custom maps.

The game is available with three different levels:
- Original Pac-Man map
- Custom map 1 (custom maze)
- Custom map 2 (huge custom maze with camera movement)

> **Note**
> The project is made using Unity 2020.3.9f1.

## Images

| Title Screen                                                                                                           |
|------------------------------------------------------------------------------------------------------------------------|
| ![title_screen](https://user-images.githubusercontent.com/59691442/201502391-7c8a733c-fef2-45cc-bf5b-1b9f8809d171.png) |

| Original level |
|----------------|
|![original_level](https://user-images.githubusercontent.com/59691442/201502390-3773523b-03ef-4025-bf51-e3b7a6ef3a93.png)|

| Custom level 1                                                                                                            |
|---------------------------------------------------------------------------------------------------------------------------|
| ![custom_level1](https://user-images.githubusercontent.com/59691442/201502388-f1f495ce-eba5-4662-b758-b0394b254d04.png)   |

| Custom level 2                                                                                                           |
|--------------------------------------------------------------------------------------------------------------------------|
| ![Custom level 2](https://user-images.githubusercontent.com/59691442/201503022-16f7018a-a41e-4488-8d01-348f0334f120.png) |

## Videos

https://user-images.githubusercontent.com/59691442/201504782-ba127e81-e382-4138-904f-7fb5537dfe3e.mp4

## Features

- Sound and musics
- Original map
- Two Custom maps
- Original Pac-Man ghost AI
- High-score system with saving
- Sound/music settings
- Keyboard and controller support

## Quickstart

### Play online

The game is playable online at the following link:

<a href="https://im-rises.github.io/PacManUnity/"><img src="https://user-images.githubusercontent.com/59691442/201502753-3eb47182-da10-4900-b566-6c360a5ede74.png" alt="webGlLogo" style="height: 50px"></a>

Or follow the direct link below:

<https://im-rises.github.io/PacManUnity/>

## Play on your computer

The game is also downloadable as a desktop application for Windows, Linux and macOS by clicking on the link below (click on the image of ):

[![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)](https://github.com/Im-Rises/PacManUnity/releases/download/1.0/build-windows-1.0.zip)

Or click the direct link belwo to access the release page.

<https://github.com/Im-Rises/PacManUnity/releases/latest>

## Controls

The game is fully playable with a keyboard and a mouse or a gamepad.

### Game controls

| Action | Key    | Xbox/Playstation controller   |
|--------|--------|-------------------------------|
| ↑      | ↑ or W | arrow/joystick	arrow/joystick |
| ←      | ← or A | arrow/joystick	arrow/joystick |
| →      | → or S | arrow/joystick	arrow/joystick |
| ↓      | ↓ or D | arrow/joystick	arrow/joystick |

### User interface

| Action   | Key | Xbox controller | Playstation controller  |
|----------|-----|-----------------|-------------------------|
| Back     | ESC | B               |     O                   |
| Validate | ESC | A               | X                       |
| ↑      | ↑ or W | arrow/joystick	arrow/joystick |
| ←      | ← or A | arrow/joystick	arrow/joystick |
| →      | → or S | arrow/joystick	arrow/joystick |
| ↓      | ↓ or D | arrow/joystick	arrow/joystick |

## Project Architecture

~~~
PhysicalEngine
├── .github
│  ├── workflows
│  │   |── greetings.yml
│  │   |── label.yml
│  │   |── stale.yml
│  │   |── super-linter.yml
│  ├── labeler.yml
│  ├── release.yml
├── Assets
│  ├── *
├── Krita
│  ├── *
├── ProjectSettings
|  ├── *
├── .editorconfig
├── .gitattributes
├── .gitignore
├── README.md
~~~

## GitHub Actions

[![Lint Code Base](https://github.com/Im-Rises/PacManUnity/actions/workflows/super-linter.yml/badge.svg?branch=main)](https://github.com/Im-Rises/PacManUnity/actions/workflows/super-linter.yml)
[![pages-build-deployment](https://github.com/Im-Rises/PacManUnity/actions/workflows/pages/pages-build-deployment/badge.svg?branch=web-version)](https://github.com/Im-Rises/PacManUnity/actions/workflows/pages/pages-build-deployment)

- Lint Code Base: Script to check the code quality for different languages.
- pages-build-deployment: Script to build the project and deploy it to GitHub Pages.

## Documentations

Unity:

<https://unity.com/>

Krita:

<https://krita.org/en/>

Super Linter action:

<https://github.com/github/super-linter>

PacMan movement:

<https://gameinternals.com/understanding-pac-man-ghost-behavior>

Audio files:

<https://www.classicgaming.cc/classics/pac-man/sounds>
<https://www.voicy.network/clips/cHeHx76RLUGh1xsxwb5Xog>

Pac-Man switch mode times:

<https://www.gamedeveloper.com/design/the-pac-man-dossier>

## Authors

Quentin MOREL:

- @Im-Rises
- <https://github.com/Im-Rises>

[![GitHub contributors](https://contrib.rocks/image?repo=Im-Rises/PacManUnity)](https://github.com/Im-Rises/PacManUnity/graphs/contributors)
