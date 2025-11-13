# UI Setup Guide

This guide helps you set up the UI elements in both MainMenu and GameScene.

## MainMenu Scene UI Setup

### 1. Create Canvas
- Right-click in Hierarchy → UI → Canvas
- Set Canvas Scaler: Scale With Screen Size
- Reference Resolution: 1920x1080

### 2. Create Menu Panel
- Right-click Canvas → UI → Panel
- Name it "MenuPanel"
- Set color/background as desired

### 3. Create Title
- Right-click MenuPanel → UI → Text - TextMeshPro
- Name it "TitleText"
- Text: "Flag Game"
- Center align, large font size

### 4. Create Category Button
- Right-click MenuPanel → UI → Button - TextMeshPro
- Name it "FlagsButton"
- Text: "Flags"
- Position below title

### 5. Create Game Mode Buttons
- Right-click MenuPanel → UI → Button - TextMeshPro
- Name it "SinglePlayerButton"
- Text: "Single Player"
- Position below Flags button
- **Initially set to inactive** (uncheck in Inspector)

- Right-click MenuPanel → UI → Button - TextMeshPro
- Name it "MultiplayerButton"
- Text: "Multiplayer"
- Position below Single Player button
- **Initially set to inactive**

### 6. Assign References to UIManager
- Select UIManager GameObject
- In Inspector, drag references:
  - MenuPanel → menuPanel
  - FlagsButton → flagsButton
  - SinglePlayerButton → singlePlayerButton
  - MultiplayerButton → multiplayerButton

## GameScene UI Setup

### 1. Create Canvas (if not exists)
- Right-click in Hierarchy → UI → Canvas
- Set Canvas Scaler: Scale With Screen Size

### 2. Create Game Panel
- Right-click Canvas → UI → Panel
- Name it "GamePanel"
- This will contain all game UI

### 3. Create Flag Display
- Right-click GamePanel → UI → Image
- Name it "FlagImage"
- Set size: 400x300 (or desired)
- Center it on screen
- **Important**: Remove the Source Image (it will be set at runtime)

### 4. Create Question Number Text
- Right-click GamePanel → UI → Text - TextMeshPro
- Name it "QuestionNumberText"
- Text: "Question 1/10"
- Position at top center
- Font size: 24

### 5. Create Timer Text
- Right-click GamePanel → UI → Text - TextMeshPro
- Name it "TimerText"
- Text: "10"
- Position below question number
- Font size: 48, bold
- Color: Red (for urgency)

### 6. Create Score Text
- Right-click GamePanel → UI → Text - TextMeshPro
- Name it "ScoreText"
- Text: "Score: 0/10"
- Position at top right
- Font size: 20

### 7. Create Listening Indicator
- Right-click GamePanel → UI → Text - TextMeshPro
- Name it "ListeningText"
- Text: "Listening..."
- Position below flag image
- Font size: 18
- Color: Blue
- **Initially set to inactive**

### 8. Create Feedback Panel
- Right-click Canvas → UI → Panel
- Name it "FeedbackPanel"
- Set semi-transparent background
- **Initially set to inactive**

### 9. Create Feedback Text
- Right-click FeedbackPanel → UI → Text - TextMeshPro
- Name it "FeedbackText"
- Text: ""
- Center align
- Font size: 32, bold
- **Initially set to inactive**

### 10. Create Score Screen Panel
- Right-click Canvas → UI → Panel
- Name it "ScoreScreenPanel"
- Set background color
- **Initially set to inactive**

### 11. Create Final Score Text
- Right-click ScoreScreenPanel → UI → Text - TextMeshPro
- Name it "FinalScoreText"
- Text: "Final Score: 0/10"
- Center align
- Font size: 48

### 12. Create Return to Menu Button
- Right-click ScoreScreenPanel → UI → Button - TextMeshPro
- Name it "ReturnToMenuButton"
- Text: "Return to Menu"
- Position at bottom left

### 13. Create Play Again Button
- Right-click ScoreScreenPanel → UI → Button - TextMeshPro
- Name it "PlayAgainButton"
- Text: "Play Again"
- Position at bottom right

### 14. Assign All References to UIManager
- Select UIManager GameObject
- In Inspector, drag references:

**Menu UI:**
- MenuPanel → menuPanel
- FlagsButton → flagsButton
- SinglePlayerButton → singlePlayerButton
- MultiplayerButton → multiplayerButton

**Game UI:**
- GamePanel → gamePanel
- FlagImage → flagImage
- QuestionNumberText → questionNumberText
- TimerText → timerText
- ScoreText → scoreText
- ListeningText → listeningText

**Feedback UI:**
- FeedbackPanel → feedbackPanel
- FeedbackText → feedbackText
- Set correctColor (green)
- Set incorrectColor (red)

**Score Screen UI:**
- ScoreScreenPanel → scoreScreenPanel
- FinalScoreText → finalScoreText
- ReturnToMenuButton → returnToMenuButton
- PlayAgainButton → playAgainButton

## Layout Suggestions

### MainMenu Layout
```
┌─────────────────────┐
│   Flag Game (Title) │
│                     │
│   [Flags Button]    │
│                     │
│ [Single Player] ← Initially hidden
│ [Multiplayer]   ← Initially hidden
└─────────────────────┘
```

### GameScene Layout
```
┌─────────────────────────────┐
│ Question 1/10    Score: 0/10│
│                             │
│           10 (Timer)        │
│                             │
│      [Flag Image]           │
│                             │
│      Listening...            │
└─────────────────────────────┘
```

### Feedback Overlay
```
┌─────────────────────────────┐
│                             │
│      Correct!               │
│   (or Incorrect message)    │
│                             │
└─────────────────────────────┘
```

### Score Screen Layout
```
┌─────────────────────────────┐
│                             │
│   Final Score: 8/10         │
│        80%                   │
│                             │
│ [Return to Menu] [Play Again]│
└─────────────────────────────┘
```

## Testing UI

1. **Test Menu Navigation**
   - Click Flags button → Single Player and Multiplayer should appear
   - Click Single Player → Should load GameScene

2. **Test Game UI**
   - Flag should display
   - Timer should count down from 10
   - Score should update
   - Listening text should appear

3. **Test Feedback**
   - After answering, feedback should show
   - Correct = green, Incorrect = red

4. **Test Score Screen**
   - After 10 questions, score screen should appear
   - Buttons should work

## Tips

- Use RectTransform anchors for responsive layouts
- Test on different screen sizes
- Use Unity's UI Builder for complex layouts (optional)
- Keep UI elements organized in hierarchy
- Use consistent spacing and sizing

## Common Issues

**UI not showing:**
- Check Canvas is set to Screen Space - Overlay
- Verify UI elements are children of Canvas
- Check that panels are active

**References not working:**
- Ensure all GameObjects are named exactly as specified
- Drag references in Inspector, don't type manually
- Save scene after assigning references

**Text not updating:**
- Verify TextMeshPro is imported
- Check that references are assigned in UIManager
- Look for errors in Console

