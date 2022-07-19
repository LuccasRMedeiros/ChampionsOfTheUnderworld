using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DaemonBehaviour : MonoBehaviour
{
    private float _speed = 0.1f;
    public string beatKey;
    
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
        transform.Translate(Vector3.left * _speed);
    }

    void OnBecameInvisible()
    {
        Destroy(this);
    }
}
