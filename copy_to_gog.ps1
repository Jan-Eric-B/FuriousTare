Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Copy-Item -Path "FuriousTareIL2CPP\bin\Release\net6.0\FuriousTareIL2CPP.dll" -Destination "F:\Games\GOG\Disco Elysium\BepInEx\plugins\FuriousTare\"

echo "Copied!"
