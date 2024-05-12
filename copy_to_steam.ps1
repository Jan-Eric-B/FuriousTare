Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Copy-Item -Path "FuriousTareSteam\bin\Release\net6.0\FuriousTareSteam.dll" -Destination "F:\Games\Steam\steamapps\common\Disco Elysium\BepInEx\plugins\FuriousTareSteam"

echo "Copied!"
