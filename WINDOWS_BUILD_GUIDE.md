# Windows Build Guide

## Building for Windows

### Step 1: Switch Build Platform

1. In Unity, go to **File → Build Settings** (or press `Ctrl+Shift+B`)
2. Select **PC, Mac & Linux Standalone**
3. Select **Windows** as the target platform
4. Click **Switch Platform** (if needed)
5. Wait for Unity to reimport assets

### Step 2: Configure Player Settings

1. Click **Player Settings** in Build Settings window
2. Configure:
   - **Company Name**: Your company name
   - **Product Name**: "Flag Game" (or your preferred name)
   - **Default Icon**: Set a game icon if desired
   - **Resolution and Presentation**:
     - **Default Screen Width**: 1920
     - **Default Screen Height**: 1080
     - **Fullscreen Mode**: Windowed (for testing)

### Step 3: Enable Microphone Capability (Optional)

**Note:** For Windows Standalone builds, the Microphone capability checkbox may not be visible in Unity 6. This is normal - `KeywordRecognizer` works with Windows system permissions.

**If you see the Microphone option:**
1. In Unity, go to **Edit → Project Settings → Player**
2. Select the **Windows** tab (under "Publishing Settings" or "PC, Mac & Linux Standalone")
3. Look for **Capabilities** section
4. Check **Microphone** if available

**If you don't see it:** That's okay! Just make sure Windows microphone permissions are enabled (see Step 5 below).

**Note:** The game uses Unity's built-in `KeywordRecognizer` which works out of the box on Windows builds. No additional plugins needed!

### Step 4: Build the Game

1. In **Build Profiles** window, click the **Build** button (bottom right)
2. Choose a folder for your build (e.g., create a `Builds` folder in your project)
3. Unity will create an `.exe` file and a `_Data` folder
4. Wait for the build to complete

### Step 5: Enable Windows Microphone Permissions

Before testing, ensure Windows allows microphone access:

1. **Open Windows Settings** (Windows Key + I)
2. Go to **Privacy → Microphone**
3. Make sure **"Allow apps to access your microphone"** is ON
4. **Make sure "Allow desktop apps to access your microphone"** is ON (this is important!)
5. Test your microphone:
   - Right-click the speaker icon in system tray
   - Select **Sounds → Recording**
   - Speak into your microphone and check if the green bars move

### Step 6: Test the Build

1. Navigate to your build folder
2. Double-click the `.exe` file to run the game
3. Test speech recognition:
   - Start a game
   - When you see "Listening..." appear, speak a country name clearly
   - Try saying: "United States", "Canada", "France", "Germany", etc.
   - The game should recognize your voice!

## Windows Speech Recognition Setup

Before testing, ensure Windows Speech Recognition is enabled:

1. **Open Windows Settings** (Windows Key + I)
2. Go to **Privacy → Microphone**
3. Make sure **"Allow apps to access your microphone"** is ON
4. **Allow desktop apps to access your microphone** should also be ON
5. Test your microphone:
   - Right-click the speaker icon in system tray
   - Select **Sounds → Recording**
   - Speak into your microphone and check if the green bars move

**How it works:**
- The game uses Unity's `KeywordRecognizer` which recognizes predefined keywords
- Supported country names: "United States", "USA", "US", "America", "United Kingdom", "UK", "Britain", "England", "France", "Germany", "Japan", "Canada", "Australia", "Brazil", "India", "Italy"
- Also recognizes variations: "Deutschland", "Brasil", "Italia"
- Speak clearly and the game will recognize your voice!

## Troubleshooting

### "KeywordRecognizer not found" Error

**Solution:** Make sure you're building for Windows Standalone (not UWP)
- In Build Settings, select **PC, Mac & Linux Standalone**
- Platform should be **Windows**
- The `KeywordRecognizer` is only available in Windows Standalone builds

### Microphone Not Working

1. **Check Windows Privacy Settings**:
   - Settings → Privacy → Microphone
   - Ensure microphone access is allowed

2. **Check Microphone Permissions**:
   - The game should request microphone permission on first run
   - If not, check Windows Privacy settings

3. **Test Microphone in Windows**:
   - Settings → System → Sound → Test your microphone

4. **Check Console Logs**:
   - Look for "Windows Speech Recognition initialized with X keywords"
   - Look for "Windows Speech Recognition started - speak a country name"
   - If you see errors, check the specific error message

### Speech Not Recognized

1. **Speak Clearly**: Say country names clearly and at a normal pace
2. **Check Grammar**: The game recognizes:
   - "United States", "USA", "US", "America"
   - "United Kingdom", "UK", "Britain", "England"
   - "France", "Germany", "Japan", "Canada", "Australia", "Brazil", "India", "Italy"

3. **Check Console**: Look for recognition messages in the game's console output

4. **Enable Online Recognition** (optional):
   - Windows Settings → Privacy → Speech
   - Enable "Online speech recognition" for better accuracy

### Build Errors

1. **Missing Scenes**: Ensure both MainMenu and GameScene are in Build Settings
2. **Missing Assets**: Check that all flag sprites are in Resources folder
3. **Script Errors**: Fix any compilation errors before building

## Testing Checklist

- [ ] Game builds successfully
- [ ] Game launches without errors
- [ ] Main menu appears correctly
- [ ] Can start a game
- [ ] Flags display correctly
- [ ] Microphone permission is requested
- [ ] Speech recognition initializes
- [ ] Can recognize spoken country names
- [ ] Game flow works (questions, timer, scoring)

## Next Steps

After confirming Windows build works:

1. Test on different Windows versions (Windows 10, Windows 11)
2. Test with different microphones
3. Fine-tune speech recognition grammar if needed
4. Consider adding more country name variations
5. Add visual feedback when listening (microphone icon, etc.)

