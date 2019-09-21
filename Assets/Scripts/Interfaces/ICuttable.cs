using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICuttable : IGameObject
{
    void Cut(Collision collision);
}
