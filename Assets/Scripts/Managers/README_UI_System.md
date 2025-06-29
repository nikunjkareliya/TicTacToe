# UI Navigation System Documentation

## Overview
This modular UI navigation system provides event-driven communication between game states and UI screens, with support for popups and smooth transitions.

## Core Components

### 1. GameStateManager
- **Purpose**: Manages game states and raises events when states change
- **Features**: 
  - Singleton pattern with auto-initialization
  - Event-driven state changes
  - Persists across scene loads
- **Usage**: `GameStateManager.Instance.SetState(GameState.MainMenu)`

### 2. UINavigationManager
- **Purpose**: Handles UI screen navigation based on game states
- **Features**:
  - Each GameState maps to a UI screen
  - Uses CanvasGroup for smooth transitions
  - Auto-hides all screens on start
- **Setup**: Add UIScreenData entries in inspector with GameState and CanvasGroup references

### 3. PopupManager
- **Purpose**: Manages popup windows that overlay on any screen
- **Features**:
  - Separate from game state system
  - Multiple popups can be active simultaneously
  - Event-driven show/hide operations
- **Setup**: Add PopupData entries in inspector with popup names and CanvasGroup references

### 4. TransitionManager
- **Purpose**: Handles screen transitions with different effects
- **Transition Types**:
  - Instant: Immediate show/hide
  - Fade: Alpha-based transition
  - Slide: Position-based transition
  - Scale: Scale-based transition
- **Usage**: Automatically used by UINavigationManager and PopupManager

### 5. SceneManager
- **Purpose**: Handles scene loading/unloading with progress tracking
- **Features**:
  - Loading screen with progress indicators
  - Minimum loading time support
  - Event-driven scene operations
- **Usage**: `SceneManager.Instance.LoadScene("GameScene")`

## Event System

### Game State Events
```csharp
// Change game state (automatically shows corresponding UI screen)
GameEvents.RaiseGameStateChanged(GameState.MainMenu);

// Request specific transition type
GameEvents.RaiseScreenTransitionRequested(GameState.Gameplay, TransitionType.Slide);
```

### Popup Events
```csharp
// Show popup
GameEvents.RaiseShowPopup("SettingsPopup");

// Hide popup
GameEvents.RaiseHidePopup("SettingsPopup");

// Hide all popups
GameEvents.RaiseHideAllPopups();
```

### Scene Events
```csharp
// Load scene
GameEvents.RaiseLoadScene("GameScene");

// Track loading progress
GameEvents.OnLoadingProgressUpdated += (progress) => {
    // Update progress bar
};
```

## Setup Instructions

### 1. Scene Structure
```
RootUI (GameObject with UINavigationManager)
├── MainMenuScreen (GameObject with Canvas + CanvasGroup)
├── GameplayScreen (GameObject with Canvas + CanvasGroup)
├── MatchMakingScreen (GameObject with Canvas + CanvasGroup)
├── ResultScreen (GameObject with Canvas + CanvasGroup)
└── PopupContainer (GameObject)
    ├── SettingsPopup (GameObject with CanvasGroup)
    ├── PausePopup (GameObject with CanvasGroup)
    └── OtherPopups...
```

### 2. UINavigationManager Setup
1. Add UINavigationManager to RootUI GameObject
2. In inspector, add UIScreenData entries:
   - GameState: MainMenu → CanvasGroup: MainMenuScreen's CanvasGroup
   - GameState: Gameplay → CanvasGroup: GameplayScreen's CanvasGroup
   - etc.

### 3. PopupManager Setup
1. Add PopupManager to RootUI GameObject
2. In inspector, add PopupData entries:
   - PopupName: "SettingsPopup" → CanvasGroup: SettingsPopup's CanvasGroup
   - PopupName: "PausePopup" → CanvasGroup: PausePopup's CanvasGroup
   - etc.

### 4. Scene Setup
1. Create Loading scene with progress UI
2. Create Game scene with all UI screens
3. Add scenes to Build Settings
4. Configure SceneManager with scene names

## Usage Examples

### Basic Game Flow
```csharp
// Start game
GameStateManager.Instance.SetState(GameState.MainMenu);

// Start matchmaking
GameStateManager.Instance.SetState(GameState.MatchMaking);

// Start gameplay
GameStateManager.Instance.SetState(GameState.Gameplay);

// Show pause popup (doesn't change game state)
GameEvents.RaiseShowPopup("PausePopup");

// Hide pause popup
GameEvents.RaiseHidePopup("PausePopup");

// Show result
GameStateManager.Instance.SetState(GameState.Result);
```

### Custom Transitions
```csharp
// Request specific transition
GameEvents.RaiseScreenTransitionRequested(GameState.Gameplay, TransitionType.Slide);

// Or use manager directly
UINavigationManager.Instance.ShowScreenForState(GameState.MainMenu, TransitionType.Scale, 0.8f);
```

### Scene Loading
```csharp
// Load game scene from loading scene
SceneManager.Instance.LoadGameScene();

// Load with custom loading screen
SceneManager.Instance.LoadScene("GameScene", true);
```

## Auto-Initialization
All managers support auto-initialization for quick testing:
- If you play the Game scene directly, missing managers will be auto-created
- This allows for quick testing without going through the loading scene
- Managers will log when they're auto-created

## Best Practices
1. **Use CanvasGroup**: Always use CanvasGroup for UI screens and popups
2. **Event-Driven**: Use events for communication between systems
3. **Loose Coupling**: Systems communicate through events, not direct references
4. **Single Responsibility**: Each manager has a specific purpose
5. **Consistent Naming**: Use consistent naming for popups and screens

## Troubleshooting
- **Screen not showing**: Check CanvasGroup references in UINavigationManager
- **Popup not showing**: Check popup name and CanvasGroup in PopupManager
- **Transition not working**: Ensure TransitionManager is present in scene
- **Events not firing**: Check that managers are properly initialized 