using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Money : NetworkBehaviour, IInteractable
{
    [SerializeField] float value = 1f;
    public float Value { get { return value; } }

    public void Interact()
    {
        ClientController.Instance.IncreaseMoney(value);
        Destroy(this.gameObject);
    }

    public void SetValue(float val)
    {
        this.value = val;
    }
}
