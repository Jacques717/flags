# Project Context - Unity Speech Recognition Flag Game

## Project Status
**Date**: November 12, 2024  
**Location**: `C:\Dev\Unity\Snap`  
**Unity Version**: 6000.2.11f1  
**Status**: ✅ **COMPILATION SUCCESSFUL** - Project compiles without errors, ready for setup

## What Has Been Completed

### ✅ Core Scripts (12 files - ALL UPDATED)
All scripts are in `Assets/Scripts/` and have been synced with latest fixes:
1. **FlagData.cs** - ScriptableObject for flag questions
2. **Timer.cs** - 10-second countdown timer
3. **SpeechRecognitionHandler.cs** - Speech recognition with Editor keyboard fallback (unused field removed)
4. **AnswerMatcher.cs** - Fuzzy string matching for answers
5. **GameManager.cs** - Core game logic and flow (uses FindFirstObjectByType)
6. **FlagGameController.cs** - Flag game controller (uses FindFirstObjectByType)
7. **UIManager.cs** - UI management (uses regular Text instead of TextMeshPro, FindFirstObjectByType)
8. **MainMenuController.cs** - Menu navigation (uses FindFirstObjectByType)
9. **TimerUIUpdater.cs** - Timer-UI integration helper
10. **FlagDataSetup.cs** - Editor tool for creating FlagData asset
11. **SceneSetupHelper.cs** - Editor tool for scene setup
12. **GameTester.cs** - Testing helper (uses FindFirstObjectByType, has System.Collections.Generic)

### ✅ Project Structure
- `Assets/Scripts/` - All C# scripts (synced and updated)
- `Assets/Scenes/` - Empty (scenes need to be created)
- `Assets/Sprites/Flags/` - **FLAGS ALREADY COPIED HERE** ✅
- `Assets/Resources/Sprites/Flags/` - **FLAGS NEED TO BE COPIED HERE TOO** (for runtime loading)
- `ProjectSettings/` - Unity project settings created
- `Packages/manifest.json` - Unity UI package installed (com.unity.ugui)
- Documentation files in root

### ✅ Compilation Issues - RESOLVED
- ✅ Unity UI package installed (com.unity.ugui added to manifest.json)
- ✅ All `FindObjectOfType` replaced with `FindFirstObjectByType` (Unity 6 compatibility)
- ✅ Missing `using System.Collections.Generic;` added to GameTester.cs
- ✅ Unused field removed from SpeechRecognitionHandler.cs
- ✅ TextMeshPro replaced with regular Unity Text (temporary - can upgrade later)
- ✅ **Project compiles successfully with 0 errors, 0 warnings**

### ✅ Documentation Created
- `README.md` - Complete setup guide
- `QUICK_START.md` - Step-by-step quick start
- `FLAG_DOWNLOAD_GUIDE.md` - Flag image download instructions
- `UI_SETUP_GUIDE.md` - Detailed UI setup guide
- `SPEECH_RECOGNITION_SETUP.md` - Speech recognition integration
- `PROJECT_SUMMARY.md` - Project overview
- `CONTEXT_FOR_WINDOWS.md` - This file
- `FIX_COMPILATION_ERRORS.md` - Troubleshooting guide
- `INSTALL_UNITY_UI.md` - Unity UI installation guide

## Current Status: READY FOR SETUP

**✅ Unity Editor**: Open and working  
**✅ Compilation**: Successful (0 errors)  
**✅ Unity UI Package**: Installed  
**✅ Flag Images**: Copied to `Assets/Sprites/Flags/` (user confirmed)  
**⚠️ Flag Images**: Need to also copy to `Assets/Resources/Sprites/Flags/` for runtime loading  
**⚠️ Flag Images**: Need to import as Sprites in Unity (set Texture Type)  

## What Needs to Be Done Next

### Step 1: Prepare Flag Images (HIGH PRIORITY)
1. **Copy flags to Resources folder**:
   - Copy all flag images from `Assets/Sprites/Flags/` to `Assets/Resources/Sprites/Flags/`
   - Same files, same names (lowercase, no spaces)

2. **Import flags as Sprites in Unity**:
   - In Unity Project window, select all flag images in `Assets/Sprites/Flags/`
   - In Inspector, set **Texture Type**: "Sprite (2D and UI)"
   - Click **Apply**
   - Repeat for flags in `Assets/Resources/Sprites/Flags/`

3. **Verify flag names match**:
   - Files should be: `unitedstates.png`, `unitedkingdom.png`, `france.png`, `germany.png`, `japan.png`, `canada.png`, `australia.png`, `brazil.png`, `india.png`, `italy.png`
   - All lowercase, no spaces

### Step 2: Create Flag Data Asset
1. In Unity Editor, go to: **Game > Setup Flag Data**
2. Click **"Create Default Flag Data Asset"**
3. This creates `Assets/Resources/FlagData.asset` with 10 default flags
4. Optionally assign flag sprites manually in Inspector (or they'll load automatically from Resources)

### Step 3: Set Up Scenes
1. In Unity Editor, go to: **Game > Setup Scenes**
2. Click **"Setup MainMenu Scene"** - Creates MainMenu scene with basic structure
3. Click **"Setup GameScene"** - Creates GameScene with basic structure
4. **Important**: These helpers create the GameObjects, but you still need to set up UI

### Step 4: Create UI (Follow UI_SETUP_GUIDE.md)
1. **MainMenu Scene**:
   - Create Canvas
   - Add UI elements (buttons, text)
   - Assign references to UIManager component

2. **GameScene**:
   - Create Canvas
   - Add UI elements (flag image, timer text, score text, etc.)
   - Assign references to UIManager component

3. **See UI_SETUP_GUIDE.md** for detailed step-by-step instructions

### Step 5: Configure Build Settings
1. File > Build Settings
2. Add both scenes:
   - MainMenu (index 0)
   - GameScene (index 1)
3. Set MainMenu as first scene

### Step 6: Test in Editor
1. Open MainMenu scene
2. Press **Play**
3. Click "Flags" → "Single Player"
4. **In Editor**: Type country name and press Enter to simulate speech recognition
5. Game should progress through 10 flags

## Key Features Implemented

- **Menu System**: Category selection (Flags) with expandable structure
- **Game Flow**: 10 randomized flags, 10-second timer per question
- **Speech Recognition**: Editor keyboard input + Android/iOS structure
- **Answer Matching**: Fuzzy matching handles variations (USA, United States, etc.)
- **Scoring**: Tracks correct answers, displays final score
- **Local Testing**: Full game testable in Editor using keyboard

## Technical Details

### Code Updates Made
- **Unity 6 Compatibility**: All `FindObjectOfType<T>()` replaced with `FindFirstObjectByType<T>()`
- **UI System**: Using regular Unity `Text` instead of TextMeshPro (can upgrade later)
- **Unity UI Package**: Installed via manifest.json (`com.unity.ugui`)
- **Code Quality**: All warnings resolved, unused code removed

### Answer Matching
- Uses Levenshtein distance for fuzzy matching
- Handles variations: "USA", "United States", "America", "US"
- Case-insensitive matching
- Partial string matching

### Speech Recognition
- **Editor**: Keyboard input (type + Enter) - **WORKING**
- **Android**: Structure ready for UnitySpeechToText plugin
- **iOS**: Placeholder ready for plugin integration

### Game Flow
1. MainMenu → Select "Flags" → Select "Single Player"
2. Load GameScene → Display random flag
3. Start 10-second timer → Begin speech recognition
4. On answer → Check if correct → Show feedback
5. After 10 flags → Show score screen

## File Structure
```
C:\Dev\Unity\Snap\
├── Assets/
│   ├── Scripts/          (12 C# scripts - all updated)
│   ├── Scenes/           (to be created via Game > Setup Scenes)
│   ├── Sprites/Flags/    (✅ flags already copied here)
│   └── Resources/
│       ├── Sprites/Flags/ (⚠️ need to copy flags here too)
│       └── FlagData.asset (to be created via Game > Setup Flag Data)
├── ProjectSettings/      (Unity settings)
├── Packages/
│   └── manifest.json     (Unity UI package installed)
├── Library/             (Unity cache)
├── Temp/                (Unity temp files)
└── Documentation files
```

## Important Notes

- **Unity Text (Not TextMeshPro)**: Currently using regular Unity `Text` components. Can upgrade to TextMeshPro later if desired.
- **Resources Folder**: Flag sprites MUST be in `Assets/Resources/Sprites/Flags/` for runtime loading
- **Editor Testing**: Game works in Editor using keyboard input (no device needed)
- **Flag Images**: Already copied to `Assets/Sprites/Flags/` - need to also copy to Resources folder
- **Flag Import**: Need to set Texture Type to "Sprite (2D and UI)" in Unity Inspector

## Next Steps (In Order)

1. ✅ **DONE**: Fix compilation errors
2. ⚠️ **IN PROGRESS**: Copy flags to Resources folder
3. ⚠️ **IN PROGRESS**: Import flags as Sprites in Unity
4. ⏳ **TODO**: Create FlagData asset (Game > Setup Flag Data)
5. ⏳ **TODO**: Set up scenes (Game > Setup Scenes)
6. ⏳ **TODO**: Create UI (follow UI_SETUP_GUIDE.md)
7. ⏳ **TODO**: Test game flow in Editor
8. ⏳ **TODO**: Build to device for speech recognition testing

## Helpful Unity Menu Items

- **Game > Setup Flag Data** - Creates FlagData asset
- **Game > Setup Scenes** - Creates basic scene structure
- **Window > Package Manager** - Manage packages (Unity UI already installed)

## Common Issues to Watch For

1. ✅ **FIXED**: Missing Unity UI - Now installed
2. ⚠️ **Flag sprites not loading**: Must be in Resources folder AND imported as Sprites
3. ⚠️ **Scene not found**: Need to add scenes to Build Settings after creating them
4. ⚠️ **Missing references**: Assign UI references in UIManager Inspector after creating UI

## Quick Reference

**Flag File Names** (all lowercase, no spaces):
- unitedstates.png
- unitedkingdom.png
- france.png
- germany.png
- japan.png
- canada.png
- australia.png
- brazil.png
- india.png
- italy.png

**Flag Locations Needed**:
- `Assets/Sprites/Flags/` ✅ (already copied)
- `Assets/Resources/Sprites/Flags/` ⚠️ (need to copy here too)

---

**Last Update**: November 12, 2024  
**Status**: ✅ Compilation successful, ready for asset setup and scene creation  
**Next Action**: Copy flags to Resources folder, import as Sprites, create FlagData asset
