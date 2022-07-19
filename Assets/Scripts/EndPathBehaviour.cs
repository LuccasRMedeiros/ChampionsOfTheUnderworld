using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPathBehaviour : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D deamon)
    {
        Destroy(deamon.gameObject);
    }
}
