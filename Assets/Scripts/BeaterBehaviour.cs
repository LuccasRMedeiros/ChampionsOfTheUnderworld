using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaterBehaviour : MonoBehaviour
{
    private DaemonBehaviour _daemonBehaviour = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        _daemonBehaviour = other.GetComponent<DaemonBehaviour>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(_daemonBehaviour.beatKey))
            Destroy(other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        _daemonBehaviour = null;
    }
}
