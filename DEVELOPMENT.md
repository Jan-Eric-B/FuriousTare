# Development setup

## Steam

- Install .NET SDK v6
  - https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.421-windows-x64-installer
  - This version was required to generate the plugin from a template, but perhaps a different version will work for you.
- Download BepInEx v6 (4901521, build date 2024-02-10T05:53:59), for IL2CPP, Windows, x64
  - https://builds.bepinex.dev/projects/bepinex_be 
  - https://builds.bepinex.dev/projects/bepinex_be/688/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.688%2B4901521.zip
- Extract BepInEx into the game directory (in `Steam\steamapps\common\Disco Elysium`)
- Run the game once. You should see the BepInEx console.
- From the directory `Disco Elysium\BepInEx\interop`, copy the files `Assembly-CSharp.dll`, `DialogueSystem.dll`, and
  `IL2Cppmscorlib.dll`.
- Create the directory `lib\` in project `FuriousTareSteam\`. Paste the DLL files.
- - Open a console in `FuriousTareSteam`
- Install dependencies: `dotnet restore`
- Build the plugin DLL: `dotnet build`.
- Copy the built DLL from `bin\Debug\net6.0\FuriousTareSteam.dll"`.
- Paste the built DLL into `Disco Elysium\BepInEx\plugins\FuriousTareSteam`
- Run the game and test your changes!

## GOG

- Download BepInEx v6 for Mono, Windows, x64
  - https://builds.bepinex.dev/projects/bepinex_be/688/BepInEx-Unity.Mono-win-x64-6.0.0-be.688%2B4901521.zip
- Extract BepInEx into the game directory
- Run the game once. You should see the BepInEx console.
- From the directory `Disco Elysium\Disco Elysium_data\Managed`, copy the files `Assembly-CSharp.dll` and `DialogueSystem.dll`.
- Create the directory `lib\` in project `FuriousTareGOG\`. Paste the DLL files.
- Open a console in `FuriousTareGOG`
- Install dependencies: `dotnet restore`
- Build the plugin DLL: `dotnet build`.
- Copy the built DLL from `bin\Debug\netstandard2.0\FuriousTareGOG.dll"`.
- Paste the built DLL into `Disco Elysium\BepInEx\plugins\FuriousTareGOG`
- Run the game and test your changes!
