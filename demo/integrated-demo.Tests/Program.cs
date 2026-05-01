using IntegratedDemo;
using Microsoft.Xna.Framework;

static void AssertTrue(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

var demo = DemoState.CreateDefault();
AssertTrue(demo.Phase == DemoPhase.Start, "Demo should start on the start screen.");

var result = demo.Update(new DemoInput(Vector2.Zero, StartPressed: true, RestartPressed: false), elapsedSeconds: 1f / 60f);
AssertTrue(result.Started, "Start press should begin the run.");
AssertTrue(demo.Phase == DemoPhase.Playing, "Demo should enter play after start.");

for (var i = 0; i < 130 && demo.Phase == DemoPhase.Playing; i++)
{
    result = demo.Update(new DemoInput(new Vector2(1f, 0f), StartPressed: false, RestartPressed: false), elapsedSeconds: 1f / 60f);
}

AssertTrue(result.Won, "Moving right through all pickups should win.");
AssertTrue(demo.Phase == DemoPhase.Won, "Demo should enter win state.");
AssertTrue(demo.Score == DemoState.TargetScore, "Score should equal target score after collecting all pickups.");

result = demo.Update(new DemoInput(Vector2.Zero, StartPressed: false, RestartPressed: true), elapsedSeconds: 1f / 60f);
AssertTrue(result.Restarted, "Restart should reset after win.");
AssertTrue(demo.Phase == DemoPhase.Playing, "Restart should immediately begin a new run.");
AssertTrue(demo.Score == 0, "Restart should clear score.");

var lossDemo = DemoState.CreateDefault();
lossDemo.Update(new DemoInput(Vector2.Zero, StartPressed: true, RestartPressed: false), elapsedSeconds: 1f / 60f);
for (var i = 0; i < 80 && lossDemo.Phase == DemoPhase.Playing; i++)
{
    result = lossDemo.Update(new DemoInput(new Vector2(0f, 1f), StartPressed: false, RestartPressed: false), elapsedSeconds: 1f / 60f);
}

AssertTrue(result.Lost, "Moving down into the hazard should lose.");
AssertTrue(lossDemo.Phase == DemoPhase.Lost, "Demo should enter lost state.");

var smoke = DemoSmokeSettings.FromValues("160");
if (smoke is null)
{
    throw new InvalidOperationException("Smoke settings should parse a valid exit frame.");
}

AssertTrue(smoke.InputForFrame(1).StartPressed, "Smoke should start on frame 1.");
AssertTrue(smoke.InputForFrame(60).MoveAxis.X > 0f, "Smoke should move right through pickups.");
AssertTrue(smoke.InputForFrame(140).RestartPressed, "Smoke should restart after the win path.");
AssertTrue(!smoke.ShouldExit(159), "Smoke should not exit before the configured frame.");
AssertTrue(smoke.ShouldExit(160), "Smoke should exit at the configured frame.");

Console.WriteLine("Integrated demo tests passed.");
