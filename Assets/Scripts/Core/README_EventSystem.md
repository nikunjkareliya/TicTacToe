# 🎮 Event System Documentation

## Overview

The Tic-Tac-Toe game uses a modular event system built with C# events and Action delegates to achieve loose coupling between all game systems. This approach allows different components to communicate without direct dependencies.

## 🏗️ Architecture

### Core Components

1. **`GameEvents`** - Static class containing all game events
2. **`GameSettings`** - ScriptableObject for configuration data
3. **`EventSystemExample`** - Example component showing proper usage

### Key Principles

- **Loose Coupling**: Components communicate through events, not direct references
- **Single Responsibility**: Each component handles its own specific functionality
- **Event-Driven**: All game state changes are communicated through events
- **Memory Safe**: Proper subscription/unsubscription to prevent memory leaks

## 📋 Available Events

### Game State Events
- `OnGameStateChanged(GameState)` - Game state changes
- `OnGameStarted()` - New game begins
- `OnGameEnded(GameResult)` - Game ends (win/lose/draw)
- `OnPlayerWon(PlayerType)` - Player wins
- `OnGameDraw()` - Game ends in draw

### Turn Management Events
- `OnPlayerTurnChanged(PlayerType)` - Turn switches to different player
- `OnPlayerMoved(PlayerType, int, int)` - Player makes a move
- `OnInvalidMove(string)` - Invalid move attempted

### Board Events
- `OnBoardReset()` - Board is cleared
- `OnCellMarked(int, int, PlayerType)` - Cell is marked
- `OnWinningLineFound(Vector2Int, Vector2Int)` - Winning line detected

### AI Events
- `OnAIThinkingStarted()` - AI begins thinking
- `OnAIMoveCompleted(int, int)` - AI makes a move
- `OnAIDifficultyChanged(AIDifficulty)` - AI difficulty changes

### UI Events
- `OnUIUpdateRequired()` - UI needs updating
- `OnButtonClicked(string)` - UI button is clicked
- `OnSettingsChanged(GameSettings)` - Settings are modified

### Online Multiplayer Events
- `OnConnectingToOnline()` - Connecting to online services
- `OnConnectedToOnline()` - Successfully connected
- `OnOnlineConnectionFailed(string)` - Connection failed
- `OnPlayerJoinedRoom(string)` - Player joins room
- `OnPlayerLeftRoom(string)` - Player leaves room
- `OnRoomReady()` - Room ready to start

## 🚀 Usage Examples

### Subscribing to Events

```csharp
using TicTacToe;

public class MyComponent : MonoBehaviour
{
    private void OnEnable()
    {
        // Subscribe to events
        GameEvents.OnCellMarked += HandleCellMarked;
        GameEvents.OnGameEnded += HandleGameEnded;
    }
    
    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        GameEvents.OnCellMarked -= HandleCellMarked;
        GameEvents.OnGameEnded -= HandleGameEnded;
    }
    
    private void HandleCellMarked(int row, int col, PlayerType player)
    {
        Debug.Log($"Cell ({row}, {col}) marked by {player}");
    }
    
    private void HandleGameEnded(GameResult result)
    {
        Debug.Log($"Game ended: {result}");
    }
}
```

### Raising Events

```csharp
using TicTacToe;

// Raise a cell marked event
GameEvents.RaiseCellMarked(1, 1, PlayerType.X);

// Raise a game start event
GameEvents.RaiseGameStarted();

// Raise a player won event
GameEvents.RaisePlayerWon(PlayerType.X);
```

### Using GameSettings

```csharp
using TicTacToe;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    
    public void SetAIDifficulty(AIDifficulty difficulty)
    {
        // Note: GameSettings properties are read-only, 
        // changes should be made through the ScriptableObject asset
        Debug.Log($"AI Difficulty: {_gameSettings.DefaultAIDifficulty}");
    }
    
    public void ResetToDefaults()
    {
        _gameSettings.ResetToDefaults();
    }
}
```

## 🔧 Best Practices

### 1. Always Unsubscribe
```csharp
private void OnDisable()
{
    // Always unsubscribe in OnDisable to prevent memory leaks
    GameEvents.OnSomeEvent -= HandleSomeEvent;
}
```

### 2. Use Proper Event Handlers
```csharp
// Good: Specific event handler
private void HandleCellMarked(int row, int col, PlayerType player)
{
    // Handle the specific event
}

// Avoid: Generic event handler that handles multiple events
private void HandleGenericEvent(object data)
{
    // This makes debugging harder
}
```

### 3. Raise Events Appropriately
```csharp
// Good: Raise events when state actually changes
public void MakeMove(int row, int col, PlayerType player)
{
    if (IsValidMove(row, col))
    {
        _board[row, col] = player;
        GameEvents.RaiseCellMarked(row, col, player); // Raise after successful move
    }
    else
    {
        GameEvents.RaiseInvalidMove("Invalid move"); // Raise for invalid moves
    }
}
```

### 4. Use Debug Logging
```csharp
// The event system includes built-in debug logging
GameEvents.RaiseCellMarked(1, 1, PlayerType.X);
// This automatically logs: "Cell (1, 1) marked by X"
```

## 🧪 Testing the Event System

### Using EventSystemExample

1. Add `EventSystemExample` component to any GameObject in your scene
2. Enable "Enable Logging" in the inspector
3. Use the "Toggle Logging" method to test events
4. Check the console for event logs

### Manual Testing

```csharp
using TicTacToe;

// Test specific events
GameEvents.RaiseGameStarted();
GameEvents.RaiseCellMarked(0, 0, PlayerType.X);
GameEvents.RaisePlayerTurnChanged(PlayerType.O);
```

## 🔄 Event Flow Examples

### Game Start Flow
1. `OnGameStarted` → UI updates
2. `OnGameStateChanged(Playing)` → UI shows gameplay screen
3. `OnPlayerTurnChanged(PlayerType.X)` → UI shows current player

### Move Flow
1. `OnPlayerMoved(PlayerType.X, 1, 1)` → Board updates
2. `OnCellMarked(1, 1, PlayerType.X)` → Visual feedback
3. `OnPlayerTurnChanged(PlayerType.O)` → Turn indicator updates

### Win Flow
1. `OnWinningLineFound(start, end)` → Visual highlight
2. `OnPlayerWon(PlayerType.X)` → Win celebration
3. `OnGameEnded(GameResult.PlayerXWon)` → Game over state

## 📁 File Structure

```
Assets/Scripts/
├── Core/
│   ├── GameEvents.cs              # Main event system
│   ├── EventSystemExample.cs      # Usage example
│   └── README_EventSystem.md      # This documentation
└── Data/
    └── GameSettings.cs            # Configuration data
```

## 🎯 Namespace

All event system components use the `TicTacToe` namespace:

```csharp
using TicTacToe;
```

This ensures clean organization and prevents naming conflicts with other Unity assets or third-party packages.