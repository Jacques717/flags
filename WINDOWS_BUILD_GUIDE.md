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

### Step 3: Add System.Speech Reference (Important!)

Windows Speech Recognition requires the `System.Speech` assembly. To add it:

1. In Unity, go to **Edit → Project Settings → Player**
2. Expand **Other Settings**
3. Under **Configuration**, find **Api Compatibility Level**
4. Set it to **.NET Framework** (not .NET Standard 2.1)
5. If you don't see this option, you may need to:
   - Close Unity
   - Edit `ProjectSettings/ProjectSettings.asset`
   - Find `apiCompatibilityLevel` and set it to `6` (.NET Framework)
   - Or use Unity Hub to change the project's .NET version

**Alternative: Manual Assembly Reference**

If System.Speech is not available, you may need to:

1. Create a file `Assets/mcs.rsp` (if it doesn't exist)
2. Add this line: `-r:System.Speech.dll`
3. Note: System.Speech.dll is usually located at:
   `C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Speech.dll`

### Step 4: Build the Game

1. In **Build Settings**, click **Build**
2. Choose a folder for your build (e.g., `Builds/Windows`)
3. Unity will create an `.exe` file and a `_Data` folder
4. Wait for the build to complete

### Step 5: Test the Build

1. Navigate to your build folder
2. Double-click the `.exe` file to run the game
3. Test microphone input:
   - Start a game
   - When prompted, speak a country name clearly
   - The game should recognize your speech

## Windows Speech Recognition Setup

Before testing, ensure Windows Speech Recognition is enabled:

1. **Open Windows Settings** (Windows Key + I)
2. Go to **Privacy → Microphone**
3. Make sure **"Allow apps to access your microphone"** is ON
4. Go to **Time & Language → Speech**
5. Make sure **"Online speech recognition"** is enabled (optional, for better accuracy)
6. Test your microphone:
   - Right-click the speaker icon in system tray
   - Select **Sounds → Recording**
   - Speak into your microphone and check if the green bars move

## Troubleshooting

### "System.Speech not found" Error

**Solution 1: Change API Compatibility**
- Set API Compatibility Level to **.NET Framework** (see Step 3 above)

**Solution 2: Add Assembly Reference**
- Create `Assets/mcs.rsp` with: `-r:System.Speech.dll`
- Restart Unity

**Solution 3: Use Alternative Library**
- Consider using a Unity-compatible speech recognition plugin
- Or use Windows Runtime APIs (requires UWP build)

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
   - Look for "Windows Speech Recognition initialized successfully"
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

