using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonoBehaviour
{
    GameObject gameObject { get; }
    Transform transform { get; }
}
