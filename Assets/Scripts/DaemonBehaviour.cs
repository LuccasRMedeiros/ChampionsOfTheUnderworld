using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DaemonBehaviour : MonoBehaviour
{
    public float speed = -0.1f;
    public string beatKey;

    void FixedUpdate()
    {
        transform.Translate(Vector3.left * speed);
    }
}
