﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface IGameObject
{
    GameObject gameObject { get; }
    Transform transform { get; }
}
