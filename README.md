# Unofficial Patch for Disco Elysium

This mod fixes some outstanding bugs in Disco Elysium - The Final Cut, v2024-04-23.

The mod has been tested on the GOG and Steam releases for Windows. Older versions of the game are not officially
supported (Steam might work, GOG will not, due to the game files being built in a different way).

## How to install

- Download the zip for your version from the "Releases" page.
  - For GOG or Steam, download "FuriousTareIL2CPP.zip"
- Unzip the files into your game directory
  - E.g. `Steam\steamapps\common\Disco Elysium`

## Configuration

You can disable specific patch categories via a config file.

Run the game once to generate the config file, then edit it. For example, to disable the change in flashlight behaviour,
open `Disco Elysium\BepInEx\config\FuriousTareIL2CPP.cfg`, and change the value like so:

```toml
[Patches]
StopWavingThatFlashlight = false
```

The patch names are listed in the `Bugs fixed` section.

Some patches have additional configuration:

```toml
[ThoughtCabinetScrolling]

## Change the scroll sensitivity in the thought cabinet description panels. Originally 1, defaults to 10.
# Setting type: Int32
# Default value: 10
ScrollSensitivity = 10
```

## Mod features

### Bugs fixed

Some patches for dialogue fixes are marked with the "Articy ID", representing the unique node in the dialogue graph, e.g. `0x0100004500009218`.

- `AbilityLabelOverflow`: The ability label number and pips would overflow onto the next line when you have 10 or more
  points in an ability. This can happen if you have a Physique of 4, internalise "Revacholian Nationhood", and drink
  three different alcohols.
- `DialoguePathFixes`: `0x0100004500009218`: When asking Joyce for 10,000 reals, choosing "Hydrodynamique E40? Sounds fast." would trigger
  the correct response, _and also_ the response for the other dialogue choice ("I like high fidelity *anything*").
- `HandHud`: The hand HUD icons wouldn't respond to mouseover or clicks, except for a small area below the icons.
  - This was because the clock's "rectangle" actually overlapped and blocked the icons.
- `HandHudReplaceHeldItem`: When swapping a held consumable item (booze, smokes) with another, the new item would not be usable
  from the HUD until you un-equip & re-equip it.
- `MuzzleKimsBark`: When attempting to open the locked apartment door, Kim's "bark" voice-over clip would trigger multiple
  times, making it much louder.
- `SkipIncorrectVoiceOver`: `0x0100005800001E34`: The wrong voice-over clip plays when Cindy the Skull looks at Joyce.
  - This is a data problem - the voice-over clips for this dialogue entry were, perhaps, not recorded or imported into
    the game - and the correct clips do not exist in the game files. For now, we just skip playing the voice-over. 
- `StopWavingThatFlashlight`: When entering a conversation while holding a flashlight, the flashlight is supposed to stay still, but you could still
  wave it around.
- `TakeASwig`: Consuming a held substance sometimes doesn't play an animation when you're standing still, but does when
  you're moving, or if you switch to the inventory screen.
- `ThoughtCabinetScrolling`: The Thought Cabinet description panel has very low scrolling sensitivity.
  - The original value is 1. The new default value is 10. The sensitivity is configurable.
- `ThrowAGunLoseAGun`: When throwing a gun, if you had both Villiers and Ruby guns, you would lose the Villiers even if you threw Ruby's.
- `TweakHudWhiteSpace`: The bottom-right HUD UI white space could be improved:
  - The clock's right edge is too close to the day text.
  - The space between hand icons should align with the colon in the clock.
  - The joystick icons are snug against the hand icons.
- `VoiceOverFixAlternatives`: The wrong voice over clip would play when the dialogue entry has "alternative" (conditional) lines, if the dialogue sets a
  variable which changes the "alternative" line to use. Examples:
  - `0x0100004C00004BC7`: Attempting the Saviour Faire jump to get your RCM coat, the white check always plays the
    voice-over as if you've already attempted a jump.
  - `0x010000580001C11B`: "Talking" to the hanging corpse, comparing it to a harlequin, and ending the "chat", the text
    reads "Humour yourself with my harlequin features," but the voice-over is "Amuse yourself with my frank manners
    and my *memento mori* features."
