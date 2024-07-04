using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimeContext
{
    public float DeltaTime { get; }
    public float FixedDeltaTime { get; }
}
