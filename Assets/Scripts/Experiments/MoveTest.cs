using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    [SerializeField] Rigidbody r;
    [SerializeField] float speed = 0.35f;

    // Update is called once per frame
    void FixedUpdate()
    {
        float input = Input.GetAxis("Horizontal");


        r.MovePosition(r.position + Vector3.right * input * speed);
    }
}
