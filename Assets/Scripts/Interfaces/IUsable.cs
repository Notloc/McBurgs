using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable : IGrabbable
{
    void EnableUse();
    void DisableUse();
}
