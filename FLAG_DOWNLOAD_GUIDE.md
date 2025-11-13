# Flag Image Download Guide

## Quick Download Links

### Option 1: Flagpedia.net (Recommended)
1. Visit: https://flagpedia.net/download
2. Download individual flags or the complete set
3. Recommended format: PNG, 512x512 or 1024x1024 pixels

### Option 2: FlagLane.com
1. Visit: https://www.flaglane.com/
2. Browse country flags
3. Download in PNG format

## Required Flags for Phase 1

Download these 10 flags and save them with these exact filenames (lowercase, no spaces):

1. **United States** → `unitedstates.png`
   - Alternative names: USA, America, US
   
2. **United Kingdom** → `unitedkingdom.png`
   - Alternative names: UK, Britain, England
   
3. **France** → `france.png`
   
4. **Germany** → `germany.png`
   - Alternative names: Deutschland
   
5. **Japan** → `japan.png`
   
6. **Canada** → `canada.png`
   
7. **Australia** → `australia.png`
   
8. **Brazil** → `brazil.png`
   - Alternative names: Brasil
   
9. **India** → `india.png`
   
10. **Italy** → `italy.png`
    - Alternative names: Italia

## File Naming Convention

- Use lowercase letters only
- Remove all spaces
- Use `.png` extension
- Examples:
  - ✅ `unitedstates.png`
  - ✅ `unitedkingdom.png`
  - ❌ `United States.png` (has space and capital)
  - ❌ `united_states.png` (has underscore)

## Importing to Unity

1. Create folder: `Assets/Sprites/Flags/` (if it doesn't exist)
2. Copy all flag PNG files to this folder
3. In Unity Project window, select all flag images
4. In Inspector, set:
   - **Texture Type**: Sprite (2D and UI)
   - **Max Size**: 512 or 1024 (depending on source quality)
   - **Filter Mode**: Bilinear
   - **Compression**: None (for best quality) or High Quality
5. Click **Apply**
6. Verify sprites appear correctly in the Project window

## Verification

After importing, verify:
- All 10 flag sprites are visible in `Assets/Sprites/Flags/`
- Each sprite can be previewed in the Inspector
- Sprite names match the expected format (lowercase, no spaces)

## Troubleshooting

**Problem**: Flag sprite not loading in game
- **Solution**: Check that filename matches exactly (case-sensitive on some systems)
- Verify sprite is set to "Sprite (2D and UI)" type
- Check that sprite is in `Assets/Sprites/Flags/` folder

**Problem**: Flag image is blurry
- **Solution**: Increase Max Size in import settings (512 → 1024)
- Use higher resolution source images

**Problem**: Flag not found error
- **Solution**: Ensure filename matches country name format (e.g., `unitedstates.png` not `usa.png`)
- Check Resources folder structure is correct

