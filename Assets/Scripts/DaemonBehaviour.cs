using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaemonBehaviour : MonoBehaviour
{
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
        transform.position += new Vector3(0.2, 0, 0);
    }
}