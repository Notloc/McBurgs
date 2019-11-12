using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICuttable : INetworkedObject
{
    void Cut(Collision collision);
}
