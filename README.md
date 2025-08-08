# Clipboard Masking (Windows)

A lightweight Windows Forms tray application that monitors the clipboard and automatically masks sensitive data you copy (emails, phone numbers, IPs, credit cards, SSNs, URLs, and custom patterns).

## Features
- Runs in the system tray; minimal UI.
- Start/Pause monitoring from the tray menu; double‑click tray icon to toggle.
- Balloon notifications when monitoring starts/stops and when text is masked.
- Built‑in masking for:
  - Emails → `[EMAIL]`
  - Phone numbers → `[PHONE]`
  - IPv4 → `[IP_ADDRESS]`
  - Credit cards → `[CARD_NUMBER]`
  - SSNs → `[SSN]`
  - URLs (optional) → `[URL]`
  - Custom names and custom regex patterns
- Fast and safe masking using precompiled regex with timeouts.
- Reliable clipboard monitoring via `AddClipboardFormatListener`.

## Requirements
- Windows 10/11
- .NET SDK 8.0+ (9.x SDK also works for building 8.0 targets)
- PowerShell or Visual Studio/VS Code

## Get started
```powershell
# Clone
# git clone https://github.com/<you>/ClipboardMaskingWindows.git
# cd ClipboardMaskingWindows/ClipboardMaskingWindows

# Build & run (from the project folder)
dotnet build ClipboardMasking.Win.csproj
dotnet run --project ClipboardMasking.Win.csproj
```

Or open `ClipboardMaskingWindows.sln` in Visual Studio and press F5.

## Using the app
- The app starts in the tray (bottom‑right). If you don’t see it, click the `^` overflow arrow.
- Tray menu:
  - `▶ Start Monitoring` / `⏸ Pause Monitoring`
  - `Settings…` (enable/disable maskers, add custom names/patterns)
  - `Show Status` (quick counters)
  - `Restore Last Content` (appears when masking occurred)
  - `Quit`
- Double‑click the tray icon to toggle monitoring.

## Quick test
Paste into Notepad (use plain text):
- `test@example.com` → `[EMAIL]`
- `(555) 123-4567` → `[PHONE]`
- `192.168.1.100` → `[IP_ADDRESS]`
- `1234-5678-9012-3456` → `[CARD_NUMBER]`
- `123-45-6789` → `[SSN]`
- `https://example.com` → `[URL]` (if enabled)

See `test_data.txt` for more examples.

## Monitor/Control from PowerShell
```powershell
# Start
cd "$env:USERPROFILE\OneDrive\Documents\GitHub\ClipboardMaskingWindows\ClipboardMaskingWindows"
dotnet run --project ClipboardMasking.Win.csproj

# Check status (once)
Get-Process ClipboardMasking.Win -ErrorAction SilentlyContinue | Format-Table Name,Id,StartTime,CPU,WS -AutoSize

# Live monitor (every 5s)
while ($true) { $p=Get-Process ClipboardMasking.Win -ErrorAction SilentlyContinue; if ($p){$p|Format-Table Name,Id,CPU,WS -AutoSize}else{'Not running'}; Start-Sleep 5; Clear-Host }

# Stop
Get-Process ClipboardMasking.Win -ErrorAction SilentlyContinue | Stop-Process -Force

# Quick clipboard check
Set-Clipboard 'test@example.com'; Start-Sleep 1; Get-Clipboard
```

## Build, clean, publish
```powershell
# Clean/restore/build
dotnet clean ClipboardMasking.Win.csproj
dotnet restore ClipboardMasking.Win.csproj
dotnet build ClipboardMasking.Win.csproj

# Publish portable folder (Release)
dotnet publish ClipboardMasking.Win.csproj -c Release -r win-x64 --self-contained false -o .\publish
```

## Distribute to end users (GitHub Releases)
### Option A: Single EXE (no .NET install required)
Produces one executable that runs on any Win10/11 x64 machine.
```powershell
dotnet publish ClipboardMasking.Win.csproj -c Release -r win-x64 --self-contained true \
  -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None \
  -o .\publish\win-x64
```
Then:
- Zip the folder `.\publish\win-x64` and upload the ZIP to a GitHub Release.
- Release notes (for users):
  - Download ZIP → Right‑click → Properties → Unblock (if present) → OK
  - Extract all → Double‑click `ClipboardMasking.Win.exe`
  - If SmartScreen appears: “More info” → “Run anyway”

### Option B: Smaller download (requires .NET Desktop Runtime)
```powershell
dotnet publish ClipboardMasking.Win.csproj -c Release -r win-x64 --self-contained false \
  -p:PublishSingleFile=true -o .\publish\net8-win-x64
```
Tell users to install the .NET 8 Desktop Runtime first: `https://dotnet.microsoft.com/en-us/download/dotnet/8.0/runtime`.

### Option C: Installer (MSI/EXE)
Use Inno Setup, WiX, or the “Visual Studio Installer Projects” extension to package `publish` output into an installer (add Start Menu shortcut, auto‑start, etc.).

### Distribution tips
- Code‑sign binaries to avoid SmartScreen warnings.
- Avoid publish trimming for WinForms.
- For updates, create a new GitHub Release with a new ZIP.

## Troubleshooting
- Tray icon not visible
  - Click the `^` arrow to show hidden icons.
  - Windows Settings → Personalization → Taskbar → Other system tray icons → enable “Clipboard Masking”.
- Nothing is masked in the browser
  - Try Notepad first; some web inputs prevent external clipboard changes.
  - Run both the browser and this app at the same elevation (both normal or both as Administrator).
- Build fails with “file in use”/locked exe
  - Stop the app before building:
    ```powershell
    Get-Process ClipboardMasking.Win -ErrorAction SilentlyContinue | Stop-Process -Force
    ```
- Resource error: `MainForm.resources` not found
  - The app no longer depends on satellite resource files; the icon is set directly in code. Rebuild and run.
- Icon path issues
  - The project expects `app_icon.ico` to be in the project folder. It already exists in this repo.

## Project structure (main)
- `Program.cs` – app entry
- `Forms/MainForm.*` – tray UI and menu
- `Clipboard/ClipboardMonitor.cs` – clipboard event hook (AddClipboardFormatListener)
- `Clipboard/ClipboardAnonymizer.cs` – masking logic
- `Data/*` & `Services/*` – settings and persistence

## Notes
- Masking happens in‑memory by replacing clipboard text; the original last masked content can be restored from the tray menu.
- Custom patterns are regular expressions. Invalid patterns are ignored safely.
