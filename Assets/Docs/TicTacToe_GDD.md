
Game Design Document: Tic Tac Toe  Classic Mini Game

 Game Overview
Title: Tic Tac Toe Classic
Genre: Board / Puzzle / Mini Game
Target Platforms: iOS, Android
Orientation: Portrait
Target Audience: Casual players, ages 6+
Monetization: None (optional: integrate rewarded ads)
Online Requirement:
- Offline for AI Bot Mode
- Internet required for Online Multiplayer Mode

 Core Gameplay Loop
- Player launches game and selects a mode:
  1. Play vs Bot
  2. Play Online vs Player
- Game begins on a 3x3 board.
- Players take alternate turns placing X or O.
- First to form a straight line of three wins.
- If all cells are filled with no winner  Draw.
- End game result screen  Replay or Return to Menu.

 Game Modes
1. Single Player Mode (Offline)  vs AI Bot
- AI Difficulty Levels:
  - Easy: Random moves
  - Medium: Basic logic (block or win)
  - Hard: Minimax (unbeatable)

2. Online Multiplayer (Player vs Player via Photon)
- Matchmaking: Auto-match with a random player in Photon lobby
- Turn-based: Players take turns synced in real-time
- Disconnection Handling:
  - Timeout after X seconds results in opponent win
  - Graceful rematch or return to lobby

 Win Conditions
- A player aligns three of their symbols (X or O) in a row, column, or diagonal
- Game ends in draw if all 9 cells are filled and no player wins

 Grid Logic
- Static 3x3 Matrix
- Each move is validated (turn-based, unoccupied cell only)
- Turn indicator must be synchronized (online and offline)
- Game ends immediately upon valid win or draw detection

 Online Multiplayer (Photon PUN2)
Networking Specs:
- SDK: Photon PUN2
- Matchmaking: Random Room Join/Create
- Syncing: RPCs or RaiseEvent with Cell Index + Player Symbol
- Turn Validation: Server-authoritative (if needed)
- Max Players/Room: 2
- Timeout Handling: Auto-win after 30s inactivity
- Room Reuse: Yes (for rematch)

 Full UI/UX Flow

 Main Menu
- Play vs Bot  leads to Difficulty Selection
- Play Online  leads to Matchmaking Screen
- Settings  Sound / Haptics toggle

 Play vs Bot Mode Flow
Main Menu  Difficulty Selection  Game Board (vs AI)  Result Screen

Screens:
1. Difficulty Selection
2. Game Board (vs AI)
3. Result Screen

 Online Multiplayer Flow
Main Menu  Matchmaking Screen (Connecting)  Opponent Found  Online Game Board  Result Screen

Screens:
1. Matchmaking Screen
2. Game Board (Online)
3. Result Screen

 AI Implementation (Offline)
- Easy: Random moves
- Medium: Win/block logic
- Hard: Minimax (optimal play)

 Visual Style
- Clean, flat minimal UI
- Animated X/O drawing
- Win line highlight
- Optional themes (chalkboard, neon, classic)

 Audio & Feedback
- Tap and place sounds
- Subtle background music (toggleable)
- Win/loss/draw sound cues
- Optional: haptic feedback

 Technical Specs Summary
- Engine: Unity (2021+)
- Input: Tap
- Frame Rate Target: 60 FPS
- Offline Support: Yes (for AI Mode)
- Online Dependency: Yes (for PvP Mode)

 QA Checklist
- Accurate win/draw detection
- Turn logic is respected in both modes
- No duplicate/invalid move bugs
- AI behaves according to difficulty
- Multiplayer sync and timeout work correctly
- Disconnection handling triggers fallback result

 Anti-Cheat & Fair Play
- Server-authoritative logic (Photon)
- Reject moves outside players turn
- Optional: board state verification

 Future Expansions
- Ranked mode with Elo rating
- Cosmetic unlocks (symbol and board skins)
- Timed Blitz mode (5s per turn)
- Avatar selection
- Leaderboard integration (Game Center / Google Play)
