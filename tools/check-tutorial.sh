#!/usr/bin/env bash
# Dry-run the tutorial command path from docs/tutorial.
# This opens short-lived MonoGame DesktopGL smoke windows.

set -u

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
fail=0

green() { printf '\033[32m%s\033[0m\n' "$*"; }
red() { printf '\033[31m%s\033[0m\n' "$*"; }
yellow() { printf '\033[33m%s\033[0m\n' "$*"; }

run() {
  local label="$1"
  shift
  printf '\n==> %s\n' "$label"
  (cd "$REPO_ROOT" && "$@")
  local status=$?
  if (( status == 0 )); then
    green "[ok]   $label"
  else
    red "[fail] $label exited $status"
    fail=$((fail + 1))
  fi
}

run_expected_mgcb_failure() {
  local label="Deliberate MGCB failure"
  local output_file
  output_file="$(mktemp "${TMPDIR:-/tmp}/monogame-mgcb-failure.XXXXXX")"

  printf '\n==> %s\n' "$label"
  (
    cd "$REPO_ROOT/experiments/e05-content-pipeline" &&
      dotnet mgcb /quiet /@:docs/broken-content.mgcb
  ) >"$output_file" 2>&1
  local status=$?
  cat "$output_file"

  if (( status == 0 )); then
    red "[fail] $label unexpectedly exited 0"
    fail=$((fail + 1))
  elif ! grep -Eq 'missing_texture\.png|does not exist' "$output_file"; then
    red "[fail] $label did not mention missing_texture.png"
    fail=$((fail + 1))
  else
    green "[ok]   $label"
  fi

  rm -f "$output_file"
}

echo "MonoGame tutorial dry run"
echo "-------------------------"
yellow "[note] GUI smoke checks open short-lived DesktopGL windows."
yellow "[note] macOS may print native TSM/IMK/CSSM messages; they are non-blocking when expected smoke lines appear."

run ".NET version" dotnet --version
run "MonoGame template lookup" dotnet new list mgdesktopgl
run "Environment check" ./tools/check-env.sh
run "Solution build" dotnet build GameDemo.sln --no-restore -m:1

run "E01 game loop smoke" env E01_SMOKE_TOGGLE_AFTER_FRAMES=30 E01_SMOKE_EXIT_AFTER_FRAMES=90 dotnet run --project experiments/e01-game-loop/E01GameLoop.csproj --no-restore
run "E02 rendering smoke" env E02_SMOKE_TOGGLE_AFTER_FRAMES=180 E02_SMOKE_EXIT_AFTER_FRAMES=340 dotnet run --project experiments/e02-2d-rendering/E02Rendering.csproj --no-restore
run "E03 input smoke" env E03_SMOKE_EXIT_AFTER_FRAMES=120 dotnet run --project experiments/e03-input/E03Input.csproj --no-restore
run "E04 audio smoke" env E04_SMOKE_EXIT_AFTER_FRAMES=150 dotnet run --project experiments/e04-audio/E04Audio.csproj --no-restore
run "E05 content pipeline smoke" env E05_SMOKE_EXIT_AFTER_FRAMES=90 dotnet run --project experiments/e05-content-pipeline/E05ContentPipeline.csproj --no-restore
run_expected_mgcb_failure
run "E06 camera and collision smoke" env E06_SMOKE_EXIT_AFTER_FRAMES=180 dotnet run --project experiments/e06-camera-and-collision/E06CameraAndCollision.csproj --no-restore
run "E07 animation smoke" env E07_SMOKE_EXIT_AFTER_FRAMES=170 dotnet run --project experiments/e07-animation/E07Animation.csproj --no-restore
run "E10 publish" dotnet publish experiments/e10-publishing/E10Publishing.csproj -c Release -r osx-x64 --self-contained true -p:PublishReadyToRun=false
run "E10 published smoke" env -i HOME="$HOME" E10_SMOKE_EXIT_AFTER_FRAMES=60 PATH="/usr/bin:/bin" "$REPO_ROOT/experiments/e10-publishing/bin/Release/net10.0/osx-x64/publish/E10Publishing"
run "Integrated demo smoke" env DEMO_SMOKE_EXIT_AFTER_FRAMES=160 dotnet run --project demo/integrated-demo/IntegratedDemo.csproj --no-restore

echo "-------------------------"
if (( fail > 0 )); then
  red "Tutorial dry run failed with ${fail} failure(s)."
  exit 1
fi

green "Tutorial dry run passed."
