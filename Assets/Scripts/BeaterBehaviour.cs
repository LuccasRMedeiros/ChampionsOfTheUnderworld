using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaterBehaviour : MonoBehaviour
{
    public GameObject BeatDaemon = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            print("'A' key was pressed");
            Destroy(BeatDaemon);
        }
    }

    void OnCollsionEnter2D(Collision2D other)
    {
        print("Collision Detected");
        BeatDaemon = other.gameObject;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        print("End of collision");
        BeatDaemon = null;
    }
}
