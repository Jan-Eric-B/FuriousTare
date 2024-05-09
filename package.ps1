Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Remove-Item -Force -Recurse -ErrorAction "Continue" -Path "temp/"
mkdir -Path "temp\package\GOG\BepInEx\config","temp\package\GOG\BepInEx\plugins\FuriousTareGOG","temp\package\Steam\BepInEx\config","temp\package\Steam\BepInEx\plugins\FuriousTareSteam"
Expand-Archive "BepInEx\BepInEx-Unity.Mono-*.zip" "temp\package\GOG"
Expand-Archive "BepInEx\BepInEx-Unity.IL2CPP-*.zip" "temp\package\Steam"

Copy-Item -Path "BepInEx\config\IL2CPP\BepInEx.cfg" -Destination "temp\package\Steam\BepInEx\config\"
Copy-Item -Path "BepInEx\config\Mono\BepInEx.cfg" -Destination "temp\package\GOG\BepInEx\config\"

dotnet build --configuration "Release"

Copy-Item -Path "FuriousTareGOG\bin\Release\netstandard2.0\FuriousTareGOG.dll" -Destination "temp\package\GOG\BepInEx\plugins\FuriousTareGOG"
Copy-Item -Path "FuriousTareSteam\bin\Release\net6.0\FuriousTareSteam.dll" -Destination "temp\package\Steam\BepInEx\plugins\FuriousTareSteam"

Compress-Archive -Path "temp\package\GOG\*" -DestinationPath "temp\FuriousTareGOG.zip"
Compress-Archive -Path "temp\package\Steam\*" -DestinationPath "temp\FuriousTareSteam.zip"
Remove-Item -Force -Recurse -ErrorAction "Continue" -Path "temp/package"

echo "Done!"
