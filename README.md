# Unofficial Patch for Disco Elysium

This mod fixes some outstanding bugs in Disco Elysium - The Final Cut, v2023-03-16.

There are separate mods available for the GOG and Steam releases.

## How to install

- Download the zip for your version from the "Releases" page.
  - For GOG, download "FuriousTareGOG.zip"
  - For Steam, download "FuriousTareSteam.zip"
- Unzip the files into your game directory
  - E.g. `Steam\steamapps\common\Disco Elysium`

## Mod features

### Bugs fixed

- The wrong voice over clip would play when the dialogue entry has "alternative" (conditional) lines, if the dialogue sets a
  variable which changes the "alternative" line to use. Examples:
  - Attempting the Saviour Faire jump to get your RCM coat, the white check always plays the voice-over as if you've
    already attempted a jump.
  - "Talking" to the hanging corpse, comparing it to a harlequin, and ending the "chat", the text mentions "amuse
    yourself with my harlequin features", but the voice-over mentions "*memento mori*".
