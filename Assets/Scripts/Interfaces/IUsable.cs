using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable : IGrabbable
{
    Vector3 UseOffset { get; }
    void EnableUse();
    void DisableUse();
}
