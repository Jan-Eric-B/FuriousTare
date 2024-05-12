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

Dialogue fixes are marked with the "Articy ID", representing the unique node in the dialogue graph.

- The wrong voice over clip would play when the dialogue entry has "alternative" (conditional) lines, if the dialogue sets a
  variable which changes the "alternative" line to use. Examples:
  - `0x0100004C00004BC7`: Attempting the Saviour Faire jump to get your RCM coat, the white check always plays the
    voice-over as if you've already attempted a jump.
  - `0x010000580001C11B`: "Talking" to the hanging corpse, comparing it to a harlequin, and ending the "chat", the text
    reads "Humour yourself with my harlequin features," but the voice-over is "Amuse yourself with my frank manners
    and my *memento mori* features."
- `0x0100005800001E34`: The wrong voice-over clip plays when Cindy the Skull looks at Joyce.
  - This is a data problem - the voice-over clips for this dialogue entry were, perhaps, not recorded or imported into
    the game - and the correct clips do not exist in the game files. For now, we just skip playing the voice-over. 
- `0x0100004500009218`: When asking Joyce for 10,000 reals, choosing "Hydrodynamique E40? Sounds fast." would trigger 
  the correct response, _and also_ the response for the other dialogue choice ("I like high fidelity *anything*").
- When entering a conversation while holding a flashlight, the flashlight is supposed to stay still, but you could still
  wave it around.
