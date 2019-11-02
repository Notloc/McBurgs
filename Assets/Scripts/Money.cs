using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour, IInteractable
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
