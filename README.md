# Unity Speech Recognition Flag Game

A cross-platform Unity game that uses speech recognition to identify country names from displayed flags.

## Phase 1 Features

- Menu system with category selection (currently Flags)
- Single player and multiplayer modes (multiplayer coming soon)
- 10 randomized flags per game
- 10-second timer per question
- Speech recognition using device microphone
- Answer validation with fuzzy matching
- Score tracking and feedback

## Setup Instructions

### 1. Unity Project Setup

1. Open this project in Unity (2020.3 LTS or later recommended)
2. Install TextMeshPro if prompted (Window > TextMeshPro > Import TMP Essential Resources)

### 2. Download Flag Images

Download flag images from one of these free sources:
- **Flagpedia.net**: https://flagpedia.net/download
- **FlagLane.com**: https://www.flaglane.com/

**Required flags (10 total):**
- United States
- United Kingdom
- France
- Germany
- Japan
- Canada
- Australia
- Brazil
- India
- Italy

### 3. Import Flag Images

1. Save downloaded flag images to `Assets/Sprites/Flags/`
2. Name files using lowercase, no spaces:
   - `unitedstates.png`
   - `unitedkingdom.png`
   - `france.png`
   - `germany.png`
   - `japan.png`
   - `canada.png`
   - `australia.png`
   - `brazil.png`
   - `india.png`
   - `italy.png`

3. In Unity, select each image and set:
   - **Texture Type**: Sprite (2D and UI)
   - **Max Size**: 512 or 1024
   - Click **Apply**

### 4. Create Flag Data Asset

1. In Unity Editor, go to **Game > Setup Flag Data**
2. Click **Create Default Flag Data Asset**
3. This creates `Assets/Resources/FlagData.asset`
4. Optionally assign flag sprites manually in the Inspector

### 5. Set Up Scenes

#### MainMenu Scene
1. Create new scene: `Assets/Scenes/MainMenu.unity`
2. Add empty GameObject named "GameManager"
3. Add `GameManager` component
4. Add empty GameObject named "UIManager"
5. Add `UIManager` component
6. Add empty GameObject named "MainMenuController"
7. Add `MainMenuController` component
8. Set up UI Canvas with buttons (see UI Setup below)

#### GameScene
1. Create new scene: `Assets/Scenes/GameScene.unity`
2. Add empty GameObject named "GameManager" (or use DontDestroyOnLoad from menu)
3. Add `GameManager` component
4. Add empty GameObject named "FlagGameController"
5. Add `FlagGameController` component
6. Add empty GameObject named "Timer"
7. Add `Timer` component
8. Add empty GameObject named "SpeechRecognitionHandler"
9. Add `SpeechRecognitionHandler` component
10. Add empty GameObject named "UIManager"
11. Add `UIManager` component
12. Set up UI Canvas (see UI Setup below)

### 6. UI Setup

#### MainMenu UI
- Canvas with:
  - Title text: "Flag Game"
  - Button: "Flags" category
  - Button: "Single Player" (initially hidden, shown after category selection)
  - Button: "Multiplayer" (initially hidden, shown after category selection)

#### GameScene UI
- Canvas with:
  - Image: Flag display (assign to UIManager.flagImage)
  - Text: Question number (assign to UIManager.questionNumberText)
  - Text: Timer (assign to UIManager.timerText)
  - Text: Score (assign to UIManager.scoreText)
  - Text: "Listening..." indicator (assign to UIManager.listeningText)
  - Panel: Feedback panel with text (assign to UIManager.feedbackPanel and feedbackText)

#### Score Screen UI
- Panel with:
  - Text: Final score (assign to UIManager.finalScoreText)
  - Button: Return to Menu (assign to UIManager.returnToMenuButton)
  - Button: Play Again (assign to UIManager.playAgainButton)

### 7. Configure Components

#### FlagGameController
- Assign `FlagData` asset (from Resources/FlagData)
- Assign `Timer` component reference
- Assign `SpeechRecognitionHandler` component reference
- Assign `UIManager` component reference

#### GameManager
- Assign `FlagGameController` reference (or it will find it automatically)
- Set `questionsPerGame` to 10

#### UIManager
- Assign all UI element references in Inspector
- Set colors for correct/incorrect feedback

### 8. Build Settings

1. File > Build Settings
2. Add scenes: MainMenu, GameScene
3. Set MainMenu as first scene (index 0)
4. Select platform: iOS or Android
5. Configure platform-specific settings:
   - **iOS**: Set Bundle Identifier, enable microphone usage description
   - **Android**: Set Package Name, add microphone permission

## Testing

### Editor Testing (Local)
- The game uses keyboard input in the Editor for testing
- Type the country name and press Enter to simulate speech recognition
- Full game flow can be tested without deploying to device

### Device Testing
- Speech recognition requires actual iOS/Android device
- Build and deploy to test microphone functionality
- Ensure microphone permissions are granted

## Speech Recognition Plugin

For production, you'll need to integrate the UnitySpeechToText plugin:
- GitHub: https://github.com/yasirkula/UnitySpeechToText
- Follow plugin setup instructions for iOS/Android

The current implementation includes:
- Editor fallback (keyboard input)
- Android speech recognition structure
- iOS speech recognition placeholder

## Future Enhancements

- Additional categories (capitals, landmarks, etc.)
- Multiplayer mode implementation
- Sound effects and animations
- Leaderboards
- Difficulty levels
- More flag variations

## Notes

- Answer matching uses fuzzy matching to handle variations (e.g., "USA", "United States", "America")
- Timer automatically stops when answer is detected
- Questions are randomized each game
- Score is calculated as correct answers / total questions

