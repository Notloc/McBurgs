using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public interface INetworkedObject : IGameObject
{
    NetworkInstanceId netId { get; }
}
