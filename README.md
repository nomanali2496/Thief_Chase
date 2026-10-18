# 🚔 Thief Chase — High-Speed Open World Pursuit Simulator

[![Unity 2022.3 LTS](https://img.shields.io/badge/Unity-2022.3%20LTS-black.svg?style=flat&logo=unity)](https://unity.com/)
[![Language: C#](https://img.shields.io/badge/Language-C%23-blue.svg)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Physics](https://img.shields.io/badge/Physics-RCC%20Pro%20Vehicle%20Physics-orange.svg)]()
[![Platform](https://img.shields.io/badge/Platform-PC%20%7C%20Android-brightgreen.svg)]()
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

An adrenaline-pumping 3D urban vehicular pursuit game built in Unity (URP). Players take the role of an undercover getaway driver outrunning intelligent police interceptors, navigating traffic systems, dynamic roadblock spawners, and multi-tier city escape routes.

---

## 🌟 Key Gameplay & Technical Highlights

- **Advanced Vehicle Dynamics:**
  - Integrated with Realistic Car Controller (RCC) for authentic vehicle suspension, drift traction, tire screech audio, downforce, and high-speed cornering physics.
  - Multi-camera chase angles with dynamic FOV speed effects and cinematic replay triggers.
- **Intelligent Police Pursuit AI:**
  - Flanking, PIT maneuver attempts, and aggressive roadblocks orchestrated by AI behavior trees.
  - Multi-level wanted system with escalating police response (patrol cruisers, high-speed interceptors, and tactical barriers).
- **Urban Traffic & Waypoint Simulation:**
  - Dynamic traffic spawner utilizing spatial partitioning to simulate active city streets with minimal draw calls and memory overhead.
  - Waypoint-based civilian navigation with collision avoidance and emergency braking logic.
- **Dynamic Minimap & Navigation Radar:**
  - Real-time 2D/3D radar tracking suspect vehicles, pursuit units, and extraction checkpoints.
- **AI-Powered Dialogue & Radio Dispatch:**
  - Integrated Gemini API assistant (`GeminiManager.cs`) capable of generating dynamic police radio chatter and contextual heist dialogue based on pursuit intensity.

---

## 🏗️ Core Script Architecture

```
Assets/Scripts/
├── AI_TrafficController.cs     # Autonomous civilian traffic steering and pathing
├── TrafficSpawner.cs           # Spatial object pooling for city traffic density
├── DistanceUI.cs               # Real-time extraction point distance & pursuit meter
├── GameSceneManager.cs         # Scene transitions, level loading, and stage flow
├── MainGameRun.cs              # Game loop manager, win/loss triggers & timer
├── GeminiManager.cs            # Contextual AI dialogue & tactical radio chatter
├── CarAutoRotate.cs            # Showroom 3D turntable viewer mechanics
└── Cyberpunk/                  # Custom neon shaders, visual post-processing effects
```

---

## 🛠️ Tech Stack & Requirements

- **Engine:** Unity 2022.3 LTS (URP - Universal Render Pipeline)
- **Physics:** Rigidbody physics + RCC Vehicle Simulation
- **Scripting:** C# (.NET Standard 2.1)
- **Target Platforms:** PC (Standalone Windows) & Android Mobile

---

## 👨‍💻 Developed by

**Noman Ali** — Unity Game Developer & 3D Technical Artist  
- **Email:** [noman.haydevs@gmail.com](mailto:noman.haydevs@gmail.com)  
- **GitHub:** [@nomanali2496](https://github.com/nomanali2496)  
- **Upwork:** [Noman Ali Profile](https://www.upwork.com/freelancers/~01bcadb303a1879813)  
- **Fiverr:** [Noman Ali Services](https://www.fiverr.com/s/L3327XY)  
