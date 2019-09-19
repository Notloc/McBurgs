using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InterfaceUtil
{
    public static bool IsNull(IGameObject inter)
    {
        if (inter == null)
            return true;

        return !(inter as MonoBehaviour);
    }
}
