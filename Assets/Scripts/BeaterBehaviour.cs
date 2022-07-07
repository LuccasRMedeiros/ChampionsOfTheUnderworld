using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaterBehaviour : MonoBehaviour
{
    public GameObject BeatDaemon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollsionEnter2D(Collision2D other)
    {
        BeatDaemon = other;
    }
}
