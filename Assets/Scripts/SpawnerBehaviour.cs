using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    private float _timer = 0.0f;
    private float _waitTill = 0.2f;
    private static string _nextBeatKey = "a";
        
    private DaemonBehaviour _daemonBehaviour = null;
    public GameObject Daemon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _waitTill)
        {
            var newBornDaemon = Instantiate(Daemon, transform.position, Quaternion.identity);
        
            _daemonBehaviour = newBornDaemon.GetComponent<DaemonBehaviour>();
            _daemonBehaviour.beatKey = _nextBeatKey;
            _timer = 0;

            if (_nextBeatKey == "a")
            {
                _nextBeatKey = "s";
                _waitTill = 1.5f;
            }
            else if (_nextBeatKey == "s")
                _nextBeatKey = "d";
            else if (_nextBeatKey == "d")
            {
                _nextBeatKey = "a";
                _waitTill = 3.0f;
            }

            Debug.Log(_daemonBehaviour.beatKey);
        }
    }
}
