using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DaemonBehaviour : MonoBehaviour
{
    private float Speed = 0.1f;
    public char BeatKey = 'a';
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.left * Speed);
    }

    void OnBecameInvisible()
    {
        Destroy(this);
    }
}
