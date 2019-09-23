using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public interface IUsable : IGrabbable
{
    Vector3 UseOffset { get; }

    void EnableUse();
    void DisableUse();

    UnityAction OnEnableEvent { get; set; }
    UnityAction OnDisableEvent { get; set; }
}
