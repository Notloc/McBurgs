using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveTooltip : IGameObject
{
    Vector3 DisplayOffset { get; }
}
