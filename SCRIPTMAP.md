# Script Map for Pufferfish Commando

The following is a map outlining the general structure of the scripts in the game, organized by the scenes in which they are active. Each scene includes its relevant scripts, followed by a section for scriptable objects and enumerations used throughout the project. The structure represents the typical flow as the player progresses through the game on their first playthrough.

## Scene: MainMenu
- **MenuManager.cs**  
  Handles main menu UI elements and dynamic background.
  - **StoreManager.cs**  
    Handles the game store UI for paid items.
    - **InAppPurchaseManager.cs**  
      Manages in-app purchase orders with the App Store Developer account.
- **TutorialManager.cs** *(DontDestroyOnLoad)*  
  Manages the tutorial system across all scenes.
  - **TutorialSkipButton.cs**  
    Adds functionality for skipping the tutorial while ensuring it resets correctly for the next start.
- **MusicController.cs** *(DontDestroyOnLoad)*  
  Controls game music and sound effects across all scenes, including playback and looping.

## Scene: InventoryScene
- **InventoryManager.cs**  
  Handles the inventory UI elements and equips shells, weapons, and costumes for the GameScene.
  - **StoreManager.cs** *(Duplicate Instance)*  
    Duplicate of StoreManager.cs from MainMenu.
    - **InAppPurchaseManager.cs** *(Duplicate Instance)*  
      Duplicate of InAppPurchaseManager.cs from MainMenu.
- **DataMove.cs**  
  Transfers weapons data from InventoryScene to GameScene using a custom class with a static instance.

## Scene: GameScene
- **GetInventory.cs**  
  Accesses the DataMove instance to instantiate weapon prefabs matching the transferred data.
- **Pufferfish.cs**  
  Manages the main pufferfish's collision and health logic, including reacting to enemy attacks and handling game over scenarios.
  - **TouchRotate.cs**  
    Implements iOS touch screen logic for rotating the pufferfish by dragging.
  - **PentTarget.cs**  
    Tracks enemy positions to trigger retaliatory attacks and animations.
    - **Projectile.cs** *(Some Cases)*  
      Contains additional logic for verifying specialized achievements, attached to specific projectiles.
    - **ProjectileScripts/** *(Some Cases)*  
      Contains scripts for specific projectile behaviors; script names match the projectile prefabs.
      - **Explosion.cs** *(Some Cases)*  
        Simulates an explosion effect and damages enemies after a delay when no collisions occur.
- **ScoreManager.cs**  
  Manages score calculation and timekeeping.
  - **CoinControl.cs**  
    Spins the coin prefab and detects when the player collects it.
  - **XpControl.cs**  
    Controls the XP prefab's spinning and falling, similar to CoinControl but for XP.
- **EnemyManager.cs**  
  Spawns individual enemies and squads in waves, increasing in difficulty with each tier.
  - **TargetControl.cs**  
    Detects pufferfish attacks and calculates enemy damage.
  - **EnemyScripts/**  
    Contains unique scripts for each enemy’s movement, animation, and attack patterns; script names match enemy prefabs.
- **AchievementManager.cs**  
  Verifies completion of active achievements during gameplay.
- **PauseManager.cs**  
  Manages the pause button and pause menu UI.

## Scene: SettingsScene
- **SettingsManager.cs**  
  Controls settings UI elements and adjusts music and sound effects volume based on user input.

## Scene: AwardsScene
- **AchButtonSelect.cs**  
  Populates achievement and statistics pages based on button selections.

## Scriptable Objects
- **Achievement.cs**  
  Stores information about achievements.
- **PentInfo.cs**  
  Stores weapon information.
- **TutorialObject.cs**  
  Stores tutorial section text for easy recall by the TutorialManager.

## Other
- **Enums.cs**  
  Contains enumerations used throughout the project.
