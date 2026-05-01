# E10 Publishing

## How to run

Development run:

```bash
dotnet run --project experiments/e10-publishing/E10Publishing.csproj
```

Self-contained publish:

```bash
dotnet publish experiments/e10-publishing/E10Publishing.csproj -c Release -r osx-x64 --self-contained true
```

Published smoke without `dotnet` on `PATH`:

```bash
env -i HOME="$HOME" E10_SMOKE_EXIT_AFTER_FRAMES=60 PATH="/usr/bin:/bin" experiments/e10-publishing/bin/Release/net10.0/osx-x64/publish/E10Publishing
```

## What you should see

The development run opens a small DesktopGL window with a colored bar animation. The published smoke opens the same window, logs that it rendered 30 update frames, then exits automatically.

Controls:

- `Escape` exits.

## What was learned

For this macOS DesktopGL project, `dotnet publish -c Release -r osx-x64 --self-contained true` produces a runnable executable under `bin/Release/net10.0/osx-x64/publish/`. Running that executable with a minimal `PATH` proves it is not relying on `dotnet` from the SDK being available on `PATH`.
