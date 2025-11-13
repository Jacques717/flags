# Moving Project to Windows C:\ Drive

## Why Move to Windows?

Unity and Unreal Engine work best with native Windows paths. WSL paths (`\\wsl.localhost\...`) can cause:
- Path resolution issues
- Slower file I/O performance
- Permission problems
- Unity Hub compatibility issues

## Recommended Location

Move your project to:
```
C:\Users\YourName\UnityProjects\Snap
```

Or:
```
C:\UnityProjects\Snap
```

## Steps to Move

### Option 1: Copy from WSL to Windows (Recommended)

1. **In Windows File Explorer:**
   - Navigate to: `\\wsl.localhost\Ubuntu-22.04\home\jacques\Unity\Snap`
   - Select all files and folders
   - Copy (Ctrl+C)

2. **Create new folder in Windows:**
   - Go to `C:\Users\YourName\` (or `C:\`)
   - Create folder: `UnityProjects`
   - Create subfolder: `Snap`

3. **Paste files:**
   - Paste all files into `C:\Users\YourName\UnityProjects\Snap`

4. **In Unity Hub:**
   - Remove the old project (WSL path)
   - Click "Add" → Select the new Windows path
   - Open the project

### Option 2: Use Windows Terminal/PowerShell

```powershell
# In PowerShell (Windows)
xcopy "\\wsl.localhost\Ubuntu-22.04\home\jacques\Unity\Snap" "C:\Users\YourName\UnityProjects\Snap" /E /I /H
```

### Option 3: Use WSL Command

```bash
# In WSL terminal
cp -r /home/jacques/Unity/Snap /mnt/c/Users/YourName/UnityProjects/Snap
```

## After Moving

1. **Remove old project from Unity Hub**
2. **Add new project** from Windows path
3. **Open project** - should work smoothly now!

## Future Development

For game development:
- ✅ Use Windows paths: `C:\UnityProjects\...`
- ✅ Use Windows-native tools: Unity Editor, Visual Studio, etc.
- ✅ Keep WSL for: Linux-specific development, servers, Docker, etc.

## Note

You can keep the WSL copy as a backup, or delete it after confirming the Windows copy works.

