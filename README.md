# Clipboard Masking (Windows)

Small Windows tray app that automatically masks sensitive clipboard text: emails, phone numbers, IPv4, credit cards, SSNs, URLs, plus custom patterns.

## Features
- Runs in the system tray; double‑click to toggle monitoring
- Built‑in maskers: `[EMAIL]`, `[PHONE]`, `[IP_ADDRESS]`, `[CARD_NUMBER]`, `[SSN]`, `[URL]`
- Custom names and regex patterns via Settings
- Fast precompiled regex with timeouts; reliable monitoring via `AddClipboardFormatListener`

## Requirements
- Windows 10/11
- .NET SDK 8.0+ (for development) or .NET Desktop Runtime 8 for framework‑dependent publish

## Build and run
- Visual Studio: open `ClipboardMaskingWindows.sln` and press F5
- CLI (on Windows):
  ```powershell
  dotnet build ClipboardMasking.Win.csproj
  dotnet run --project ClipboardMasking.Win.csproj
  ```

## Publish for users
- Single executable (no .NET install):
  ```powershell
  dotnet publish ClipboardMasking.Win.csproj -c Release -r win-x64 --self-contained true \
    -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None \
    -o .\publish\win-x64
  ```
- Smaller download (requires .NET Desktop Runtime 8):
  ```powershell
  dotnet publish ClipboardMasking.Win.csproj -c Release -r win-x64 --self-contained false \
    -p:PublishSingleFile=true -o .\publish\net8-win-x64
  ```

## Use
- Lives in the system tray; if hidden, click the `^` overflow
- Tray menu: Start/Pause, Settings (manage maskers and custom patterns), Restore Last Content, Quit

## Troubleshooting
- No tray icon: Windows Settings → Personalization → Taskbar → Other system tray icons → enable “Clipboard Masking”
- Nothing masked: try Notepad; ensure both apps run at the same elevation (normal vs Administrator)

Notes: Windows‑only build (WinForms). Building on non‑Windows hosts is not supported by the .NET SDK.
