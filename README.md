# Cosmicrafts2019
Original 2D Cosmicrafts

## Overview
Cosmicrafts2019 is a Unity-based 2D space strategy game that has been modified for WebGL compatibility and includes a comprehensive development bypass mode for offline testing.

## WebGL Compatibility Changes

### Dev Bypass Mode
The game includes a comprehensive development bypass mode that allows full offline functionality for WebGL builds where HTTP requests are not available.

**Key Features:**
- Automatic login with dummy credentials (`devuser`/`devpass`)
- Populated player data with 8 default units
- Complete tutorial system functionality
- All game modes accessible without server connection

**Implementation:**
- Located in `Assets/Scripts/Connections/Scr_Database.cs`
- Controlled by `DEV_BYPASS = true` flag
- Automatically populates player stats, units, and achievements

### Database Connection Bypass
All server calls are bypassed in dev mode through `scr_BDUpdate.cs`:
- Network requests are skipped with debug logging
- Local data persistence maintained
- All game functionality preserved

### Language System Fix
**Issue:** Tutorial text was empty in English
**Solution:** Added missing English translations to `Assets/Resources/XML/UIStrings.xml`
- Fixed all tutorial strings (`txt_tutorial_03` to `txt_tutorial_22`)
- Fixed menu tutorial strings (`txt_menu_tut_01` to `txt_menu_tut_16`)
- Now displays proper English text in tutorial popups

### Drag & Drop System Fix
**Issue:** Skill cards (U_Skill_02) couldn't be dragged in tutorial
**Solution:** Fixed logic in `Assets/Scripts/Interfaze/scr_DragUnit.cs`
- Skills now properly set `okPosition = true`
- Non-skills use spawn area validation
- Skills can be dropped anywhere on the map

## File Structure

### Core Scripts Modified
- `Assets/Scripts/Connections/Scr_Database.cs` - Dev bypass implementation
- `Assets/Scripts/Connections/scr_BDUpdate.cs` - Network call bypassing
- `Assets/Scripts/Interfaze/scr_DragUnit.cs` - Drag & drop fix
- `Assets/Resources/XML/UIStrings.xml` - Language strings fix

### Key Components
- **Login System:** Automatic dev login with dummy data
- **Tutorial System:** Fully functional with proper text display
- **Card System:** Drag & drop working for all unit types
- **Language System:** English translations complete

## Development Setup

### Enabling Dev Mode
1. Set `DEV_BYPASS = true` in `Scr_Database.cs`
2. Game will automatically login with dev credentials
3. All server calls are bypassed with debug logging

### Disabling Dev Mode
1. Set `DEV_BYPASS = false` in `Scr_Database.cs`
2. Game will use normal server authentication
3. Requires valid server connection

## Build Configuration

### WebGL Settings
- Target platform: WebGL
- Compression format: Disabled (for faster loading)
- Development build recommended for testing

### Required Assets
- All XML files in `Assets/Resources/XML/`
- Language strings properly configured
- Tutorial assets and prefabs

## Known Issues & Solutions

### Tutorial Text Empty
**Problem:** Tutorial popups showed empty text
**Solution:** Added missing English translations to UIStrings.xml

### Skill Drag & Drop Not Working
**Problem:** U_Skill_02 couldn't be dragged in tutorial
**Solution:** Fixed skill positioning logic in scr_DragUnit.cs

### Server Connection Required
**Problem:** Game requires server for normal operation
**Solution:** Implemented comprehensive dev bypass mode

## Future Development

### Potential Improvements
- Add more dev units for testing
- Implement save/load for dev progress
- Add configuration options for dev mode
- Expand tutorial system

### Maintenance Notes
- Keep DEV_BYPASS flag synchronized across scripts
- Maintain language string completeness
- Test drag & drop functionality regularly
- Verify tutorial flow in dev mode

## Credits
Original Cosmicrafts game with WebGL compatibility modifications for offline development and testing.
