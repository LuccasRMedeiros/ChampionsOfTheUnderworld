using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    private bool _canSummonDaemon = false;
    private float _timer = 0.0f;
    private string[] _beatKey = { "a", "s", "d" };
    private DaemonBehaviour _daemonProps = null;
    
    public GameObject Daemon;
    public int manyDaemons = 0;

    // Update is called once per frame
    void Update()
    {
        int nextBeatKey = 0;

        // When there aren't any daemons anymore, tells to summoner that it can summon new ones
        if (!_canSummonDaemon && manyDaemons == 0)
            _canSummonDaemon = false;

        if (_canSummonDaemon)
        {
            _timer += Time.deltaTime;

            if (_timer > 0.5f) // Wait half second to spaw a new daemon
            {
                _timer = 0.0f;

                var newBornDaemon = Instantiate(Daemon, transform.position, Quaternion.identity);
                nextBeatKey = Random.Range(0, 3);

                _daemonProps = newBornDaemon.GetComponent<DaemonBehaviour>();
                _daemonProps.beatKey = _beatKey[nextBeatKey];
                _daemonProps.spawnerProps = this;

                manyDaemons++;

                Debug.Log(_daemonProps.beatKey);
            }
        }

        // When three daemons were spawned, tell the summoner it can't summon
        if (manyDaemons == 3)
            _canSummonDaemon = false;
    }
}
