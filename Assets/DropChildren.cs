using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the attached GameObject unparent all of its children on game start.
/// </summary>
public class DropChildren : MonoBehaviour
{
    [SerializeField] bool deleteObject = true;

    private void Awake()
    {
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            children[i] = transform.GetChild(i);

        foreach (Transform child in children)
            Drop(child);

        if (deleteObject)
            Destroy(this.gameObject);
    }

    private void Drop(Transform child)
    {
        child.SetParent(null);
    }
}
