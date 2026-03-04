using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EventChoiceData
{
    [TextArea(2, 4)]
    public string buttonText;

    [TextArea(3, 6)]
    public string resultText;

    [Header("State Changes")]
    public List<string> setFlags = new List<string>();
    public List<string> clearFlags = new List<string>();
}