using System;
using UnityEngine;

[Serializable]
public struct Transition
{
    public Decision decision;
    public State trueState;
    public State falseState;
}
