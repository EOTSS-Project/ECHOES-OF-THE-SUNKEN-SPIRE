using System;
using UnityEngine;

// Game-wide events: hazards, water, checkpoints and the exit announce what happened;
// the HUD, the water and the player listen.
public static class GameEvents
{
    public static event Action<string> PlayerDied;        // "drowned" or "burned"
    public static event Action<Vector3> PlayerRespawned;  // feet position of the checkpoint
    public static event Action CheckpointReached;
    public static event Action CrystalDeposited;          // a carried crystal was left at a green sanctuary
    public static event Action CrystalLost;               // a dropped crystal sank under the water
    public static event Action<string> Hint;              // story text when a new section begins
    public static event Action FinalRush;                 // the water speeds up for the last climb
    public static event Action Won;

    public static void RaisePlayerDied(string reason) => PlayerDied?.Invoke(reason);
    public static void RaisePlayerRespawned(Vector3 feet) => PlayerRespawned?.Invoke(feet);
    public static void RaiseCheckpointReached() => CheckpointReached?.Invoke();
    public static void RaiseCrystalDeposited() => CrystalDeposited?.Invoke();
    public static void RaiseCrystalLost() => CrystalLost?.Invoke();
    public static void RaiseHint(string text) => Hint?.Invoke(text);
    public static void RaiseFinalRush() => FinalRush?.Invoke();
    public static void RaiseWon() => Won?.Invoke();
}
