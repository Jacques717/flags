# Quick Start Guide

## Step 1: Open Project in Unity

1. Open Unity Hub
2. Click "Add" and select the `/home/jacques/Unity/Snap` folder
3. Open the project (Unity 2020.3 LTS or later recommended)

## Step 2: Install TextMeshPro (if prompted)

When Unity opens, you may be prompted to install TextMeshPro. Click "Import TMP Essentials".

## Step 3: Download Flag Images

See `FLAG_DOWNLOAD_GUIDE.md` for detailed instructions.

**Quick version:**
1. Download 10 flags from https://flagpedia.net/download
2. Save to `Assets/Sprites/Flags/` with names like:
   - `unitedstates.png`
   - `unitedkingdom.png`
   - `france.png`
   - etc. (see FLAG_DOWNLOAD_GUIDE.md for full list)

3. In Unity, select each image and set Texture Type to "Sprite (2D and UI)", then Apply

## Step 4: Copy Flags to Resources Folder

**Important**: For runtime loading, flags need to be in `Assets/Resources/Sprites/Flags/`

1. Copy all flag PNG files from `Assets/Sprites/Flags/` to `Assets/Resources/Sprites/Flags/`
2. Or use Unity's menu: Right-click flag → Move to Resources/Sprites/Flags/

## Step 5: Create Flag Data Asset

1. In Unity Editor, go to **Game > Setup Flag Data**
2. Click **Create Default Flag Data Asset**
3. This creates `Assets/Resources/FlagData.asset`

## Step 6: Set Up Scenes

### Option A: Use Scene Setup Helper (Recommended)

1. Go to **Game > Setup Scenes**
2. Click **Setup MainMenu Scene**
3. Click **Setup GameScene**

### Option B: Manual Setup

See `README.md` for detailed manual setup instructions.

## Step 7: Configure Build Settings

1. File > Build Settings
2. Add both scenes:
   - MainMenu (index 0)
   - GameScene (index 1)
3. Select your target platform (iOS or Android)

## Step 8: Test in Editor

1. Open MainMenu scene
2. Press Play
3. Click "Flags" category
4. Click "Single Player"
5. **In Editor**: Type country name and press Enter to simulate speech recognition
6. Game should progress through 10 flags

## Step 9: Test on Device (Optional)

For actual speech recognition testing:
1. Build and deploy to iOS/Android device
2. Grant microphone permissions when prompted
3. Test speech recognition functionality

## Troubleshooting

**Flags not showing?**
- Check that flags are in `Assets/Resources/Sprites/Flags/`
- Verify filenames match exactly (lowercase, no spaces)
- Check that sprites are set to "Sprite (2D and UI)" type

**Speech recognition not working in Editor?**
- This is normal! Editor uses keyboard input
- Type the country name and press Enter
- Real speech recognition only works on iOS/Android devices

**Scene not found errors?**
- Make sure both scenes are added to Build Settings
- Verify scene files exist in `Assets/Scenes/`

**Missing references in Inspector?**
- Use the Scene Setup Helper to create basic structure
- Manually assign UI references in UIManager component
- See README.md for detailed UI setup

## Next Steps

- Set up UI Canvas and assign all references
- Customize colors and styling
- Add sound effects
- Test on actual devices
- Integrate UnitySpeechToText plugin for production speech recognition

