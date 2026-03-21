using System;
using UnityEngine;

[Serializable]
public class EventOutcome
{
    public int coinDelta;
    public int reputationDelta;
    public int timeDelta;

    public string[] setFlags;
    public string[] clearFlags;
}