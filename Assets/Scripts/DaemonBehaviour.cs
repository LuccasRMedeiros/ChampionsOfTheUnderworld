using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DaemonBehaviour : MonoBehaviour
{
    private float _speed = 0.1f;
    
    public string beatKey;
    public SpawnerBehaviour spawnerProps;

    void FixedUpdate()
    {
        transform.Translate(Vector3.left * _speed);
    }

    void OnDisable()
    {
        spawnerProps.manyDaemons--;
    }
}
