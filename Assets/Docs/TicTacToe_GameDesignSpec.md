# 🎮 Tic-Tac-Toe Classic: Duel Mode

## 🧩 Game Design Specification

### 🔍 Game Concept Overview

A timeless, turn-based game of logic and strategy. Players aim to align three of their marks—X or O—horizontally, vertically, or diagonally on a 3x3 grid. The game supports both **offline AI bot matches** and **real-time online multiplayer via Photon PUN 2**, offering both casual solo play and competitive duels.

Designed for mobile and desktop platforms with cross-platform consistency and minimal UI.

---

## 🎯 Core Gameplay Loop

Game Start → Mode Selection → Match Start → Turn-by-Turn Play → Win/Draw Check → Game Over → Replay / Exit

---

## 🧠 Game Modes

| Mode                | Description                                                                 |
|---------------------|-----------------------------------------------------------------------------|
| **1. Player vs AI Bot** | Single-player offline mode where the player competes against a computer opponent |
| **2. PvP Online**        | Real-time multiplayer mode where players are matched using Photon rooms       |

### Mode Comparison

| Feature               | Player vs AI Bot         | PvP (Online via Photon)          |
|-----------------------|--------------------------|----------------------------------|
| Internet Required     | ❌                        | ✅                                |
| Turn Sync             | Local                    | Real-time sync via Photon        |
| Time Limit (per turn) | Optional                 | Recommended (e.g., 10s per move) |
| Rematch Option        | Immediate                | Requires consent from both users |
| Player Identity       | Single Profile           | Display nickname of both players |
| Bot Difficulty        | Easy / Medium / Hard     | N/A                              |

---

## 🧱 Gameplay Rules

| Element         | Details                                                       |
|------------------|---------------------------------------------------------------|
| Grid Size        | 3x3 matrix                                                    |
| Player Symbols   | X or O (assigned randomly or selectable before match)         |
| Win Condition    | 3 same symbols in a row (horizontal, vertical, or diagonal)   |
| Draw Condition   | All 9 cells filled with no winner                             |
| Rematch Option   | Available post-match, optionally with swapped symbols         |
| Turn Indicator   | Visual highlight or label (e.g., “Your Turn”, “Opponent Turn”)|

---

## 🎨 Visual Style & UX Goals

- **Minimalist** layout with clear tap areas and animated feedback
- **Consistent UI across mobile and desktop**: responsive design
- **Winning Line Animation**: Highlight the winning row
- **Dark/Light Theme Toggle** (optional)
- **Custom Symbols** (unlockable via settings – optional)

---

## 🖼️ UI Screen Flow

### 📱 Primary Screens

| Screen             | Description                                                              |
|--------------------|--------------------------------------------------------------------------|
| **Splash Screen**   | Optional branding                                                       |
| **Main Menu**       | Game logo + two options: [Play vs AI] / [Play Online]                   |
| **Match Setup (AI)**| Choose difficulty (Easy/Medium/Hard), Pick symbol, Start match          |
| **Match Setup (PvP)**| Auto-match or join room with code, wait for opponent                   |
| **Gameplay Screen** | Interactive 3x3 board, turn indicator, player info, exit button         |
| **Result Screen**   | “You Win / You Lose / Draw”, option to Replay or Exit                   |
| **Settings**        | Sound/music toggle, vibration, color scheme                             |
| **Credits/About**   | (Optional)                                                              |

### 🔁 UI Flow Diagram

Splash → Main Menu  
Play vs AI → AI Setup → Gameplay Screen → Result Screen → [Replay] or [Exit]  
Play Online → Matchmaking / Room Join → Gameplay Screen → Result Screen → [Replay] or [Exit]

---

## 🧠 AI Bot Design (Game Design Level)

| Difficulty Level | Behavior Logic                                                            |
|------------------|-----------------------------------------------------------------------------|
| **Easy**          | Random cell selection from available spots                                 |
| **Medium**        | Attempts to block player wins and make basic winning moves                 |
| **Hard**          | Strategic: Blocks + plans ahead (simplified Minimax logic or heuristics)   |

AI moves will include a **brief delay (0.5–1s)** to feel natural.

---

## 🌐 Online PvP (Photon PUN 2 - Design Perspective)

| Feature              | Design Note                                                                 |
|----------------------|-----------------------------------------------------------------------------|
| Matchmaking          | Auto match or room code-based manual join                                  |
| Turn Syncing         | Game logic updates across both clients after every valid move              |
| Disconnect Handling  | Graceful exit + declare remaining player winner or cancel match            |
| Turn Timer           | Optional per-move timer (e.g., 10 seconds per turn)                        |
| Player Display       | Show nickname or default “Player 1” and “Player 2”                         |
| Rematch Consent      | Both players must opt-in to rematch (UI prompt at result screen)           |

---

## 📦 Extra Features (Optional/Future Expansion)

| Feature              | Description                                                                 |
|----------------------|-----------------------------------------------------------------------------|
| Symbol Skins         | Alternate visuals for X/O (e.g., emojis, themed icons)                      |
| Stats Tracking       | Win/Loss history stored locally or via cloud                                |
| Emojis/Quick Chat    | Allow quick emoji reactions during online match (e.g., “👍”, “😅”)           |
| Leaderboards         | Daily/Weekly wins leaderboard (for online mode)                             |
| Unlockables          | Cosmetic unlocks tied to win milestones                                     |

---

## 📱 Platform-Specific Considerations

| Platform   | UI / UX Focus                                                                    |
|------------|----------------------------------------------------------------------------------|
| **Mobile** | Large tap zones, vibration feedback, portrait orientation, quick matches         |
| **Standalone** | Mouse click interaction, optional keyboard shortcuts (R for restart, Esc to quit) |

---

## ✅ Design Goals Summary

- Deliver fast, intuitive gameplay sessions
- Make it fun for both casual solo play and real-time duels
- Encourage replays with minimal friction
- Provide clarity and fairness through visual feedback
- Ensure experience parity across platforms (mobile & PC)