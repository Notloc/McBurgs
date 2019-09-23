﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBurgerComponent : IGameObject
{
    /// <summary>
    /// Attaches the component to the given node and returns another node from the component if available
    /// </summary>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    BurgerNode AttachTo(BurgerNode targetNode);
}