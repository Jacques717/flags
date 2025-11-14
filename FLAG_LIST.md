# Complete Flag List

This document lists all flags that have been added to the game. You'll need to download the flag images and save them with the exact filenames shown below.

## File Naming Convention
- All lowercase
- No spaces (use single word or combine words)
- Use `.png` extension
- Example: `United States` → `unitedstates.png`

## Download Sources
- **Flagpedia.net**: https://flagpedia.net/download
- **FlagLane.com**: https://www.flaglane.com/

## Complete Flag List (70 Total)

### Original 10 Flags
1. **United States** → `unitedstates.png`
   - Accepted: united states, usa, america, us

2. **United Kingdom** → `unitedkingdom.png`
   - Accepted: united kingdom, uk, britain, england

3. **France** → `france.png`
   - Accepted: france

4. **Germany** → `germany.png`
   - Accepted: germany, deutschland

5. **Japan** → `japan.png`
   - Accepted: japan

6. **Canada** → `canada.png`
   - Accepted: canada

7. **Australia** → `australia.png`
   - Accepted: australia

8. **Brazil** → `brazil.png`
   - Accepted: brazil, brasil

9. **India** → `india.png`
   - Accepted: india

10. **Italy** → `italy.png`
    - Accepted: italy, italia

### European Countries (18 additional)
11. **Spain** → `spain.png`
    - Accepted: spain, espana

12. **Russia** → `russia.png`
    - Accepted: russia, russian federation

13. **Netherlands** → `netherlands.png`
    - Accepted: netherlands, holland

14. **Sweden** → `sweden.png`
    - Accepted: sweden, sverige

15. **Norway** → `norway.png`
    - Accepted: norway, norge

16. **Denmark** → `denmark.png`
    - Accepted: denmark, danmark

17. **Switzerland** → `switzerland.png`
    - Accepted: switzerland, schweiz

18. **Poland** → `poland.png`
    - Accepted: poland, polska

19. **Greece** → `greece.png`
    - Accepted: greece, hellas

20. **Portugal** → `portugal.png`
    - Accepted: portugal

21. **Ireland** → `ireland.png`
    - Accepted: ireland, eire

22. **Belgium** → `belgium.png`
    - Accepted: belgium, belgie

23. **Austria** → `austria.png`
    - Accepted: austria, osterreich

24. **Finland** → `finland.png`
    - Accepted: finland, suomi

25. **Czech Republic** → `czechrepublic.png`
    - Accepted: czech republic, czechia, czech

26. **Romania** → `romania.png`
    - Accepted: romania

27. **Hungary** → `hungary.png`
    - Accepted: hungary, magyarorszag

28. **Turkey** → `turkey.png`
    - Accepted: turkey, turkiye

### Asian Countries (16 additional)
29. **China** → `china.png`
    - Accepted: china, peoples republic of china

30. **South Korea** → `southkorea.png`
    - Accepted: south korea, korea, republic of korea

31. **Thailand** → `thailand.png`
    - Accepted: thailand, siam

32. **Vietnam** → `vietnam.png`
    - Accepted: vietnam, viet nam

33. **Indonesia** → `indonesia.png`
    - Accepted: indonesia

34. **Philippines** → `philippines.png`
    - Accepted: philippines, philippine

35. **Malaysia** → `malaysia.png`
    - Accepted: malaysia

36. **Singapore** → `singapore.png`
    - Accepted: singapore

37. **Pakistan** → `pakistan.png`
    - Accepted: pakistan

38. **Bangladesh** → `bangladesh.png`
    - Accepted: bangladesh

39. **Sri Lanka** → `srilanka.png`
    - Accepted: sri lanka, ceylon

40. **Israel** → `israel.png`
    - Accepted: israel

41. **Saudi Arabia** → `saudiarabia.png`
    - Accepted: saudi arabia, saudi

42. **United Arab Emirates** → `unitedarabemirates.png`
    - Accepted: united arab emirates, uae, emirates

43. **Iran** → `iran.png`
    - Accepted: iran, persia

44. **Iraq** → `iraq.png`
    - Accepted: iraq

### South American Countries (8 additional)
45. **Argentina** → `argentina.png`
    - Accepted: argentina

46. **Chile** → `chile.png`
    - Accepted: chile

47. **Colombia** → `colombia.png`
    - Accepted: colombia

48. **Peru** → `peru.png`
    - Accepted: peru

49. **Venezuela** → `venezuela.png`
    - Accepted: venezuela

50. **Ecuador** → `ecuador.png`
    - Accepted: ecuador

51. **Uruguay** → `uruguay.png`
    - Accepted: uruguay

52. **Paraguay** → `paraguay.png`
    - Accepted: paraguay

### African Countries (8 additional)
53. **South Africa** → `southafrica.png`
    - Accepted: south africa

54. **Egypt** → `egypt.png`
    - Accepted: egypt, egyptian arab republic

55. **Nigeria** → `nigeria.png`
    - Accepted: nigeria

56. **Kenya** → `kenya.png`
    - Accepted: kenya

57. **Morocco** → `morocco.png`
    - Accepted: morocco, maroc

58. **Ethiopia** → `ethiopia.png`
    - Accepted: ethiopia

59. **Ghana** → `ghana.png`
    - Accepted: ghana

60. **Tanzania** → `tanzania.png`
    - Accepted: tanzania

### North American Countries (4 additional)
61. **Mexico** → `mexico.png`
    - Accepted: mexico

62. **Cuba** → `cuba.png`
    - Accepted: cuba

63. **Jamaica** → `jamaica.png`
    - Accepted: jamaica

64. **Costa Rica** → `costarica.png`
    - Accepted: costa rica

### Oceania Countries (2 additional)
65. **New Zealand** → `newzealand.png`
    - Accepted: new zealand, aotearoa

66. **Fiji** → `fiji.png`
    - Accepted: fiji

### Additional Countries (4)
67. **Ukraine** → `ukraine.png`
    - Accepted: ukraine

68. **South Sudan** → `southsudan.png`
    - Accepted: south sudan

69. **Myanmar** → `myanmar.png`
    - Accepted: myanmar, burma

70. **Nepal** → `nepal.png`
    - Accepted: nepal

## Installation Instructions

1. **Download all flag images** from Flagpedia.net or FlagLane.com
2. **Rename files** to match the exact filenames above (all lowercase, no spaces)
3. **Copy to Unity project**:
   - Copy all files to `Assets/Sprites/Flags/`
   - Copy all files to `Assets/Resources/Sprites/Flags/`
4. **Import in Unity**:
   - Select all flag images in Unity Project window
   - In Inspector, set **Texture Type**: "Sprite (2D and UI)"
   - Click **Apply**
5. **Update FlagData asset**:
   - In Unity Editor, go to **Game > Setup Flag Data**
   - Click **"Create Default Flag Data Asset"** (this will update with all 70 flags)
6. **Test**: The game will now randomly select from all 70 flags!

## Notes

- The game will automatically load flag sprites from `Assets/Resources/Sprites/Flags/` at runtime
- Make sure filenames match exactly (case-sensitive in some systems)
- If a flag image doesn't load, check the filename matches the expected format
- The game supports multiple accepted answers per country (e.g., "USA" or "United States" both work)

