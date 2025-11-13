# Install Unity UI Package - Step by Step

## The Problem
Unity can't find `Image`, `Text`, and `Button` because the Unity UI package isn't installed.

## Solution: Install Unity UI Package

### Method 1: Via Package Manager (Recommended)

1. **Open Package Manager**:
   - Window > Package Manager (or press Ctrl+9)

2. **Switch to Unity Registry**:
   - At the top-left of Package Manager, click the dropdown that says "In Project"
   - Select **"Unity Registry"**

3. **Search for Unity UI**:
   - In the search bar at the top, type: **"Unity UI"** or **"com.unity.ugui"**
   - Look for the package named **"Unity UI"** (by Unity Technologies)
   - It should show version 1.0.0 or similar

4. **Install the Package**:
   - Click the **"Install"** button on the Unity UI package
   - Wait for installation to complete

5. **Verify**:
   - The errors should automatically clear
   - Check the Console - errors should be gone

### Method 2: Via manifest.json (If Package Manager doesn't show it)

1. **Close Unity Editor**

2. **Edit the manifest file**:
   - Navigate to: `C:\Dev\Unity\Snap\Packages\manifest.json`
   - Open it in a text editor

3. **Add Unity UI dependency**:
   - Add this line to the "dependencies" section:
   ```json
   "com.unity.ugui": "1.0.0",
   ```

4. **Save and reopen Unity**

### Method 3: Check if it's already installed but not enabled

1. **In Package Manager**:
   - Switch to "In Project" view
   - Look for "Unity UI" in the list
   - If it's there but shows errors, try:
     - Right-click > Reimport
     - Or remove and reinstall

## After Installation

Once Unity UI is installed:
- All `Image`, `Text`, and `Button` errors should disappear
- The project should compile successfully
- You can exit Safe Mode

## Note

In Unity 6, some packages that were built-in before might need to be explicitly installed. Unity UI is one of them.

