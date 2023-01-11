using JetBrains.Annotations;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum moveDir
{
    none = 0,
    right,
    left,
    up,
    down,
}

public class GateBehaviourScript : MonoBehaviour
{
    public GameObject RedGateLock, GreenGateLock, BlueGateLock;

    private GameObject[] _GateLocks = new GameObject[9];
    private GameObject[] _NewGateLocks;
    private moveDir _moveDir = moveDir.none;
    private float _speed = 0.2f;
    private int _steps = 0;

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] gateLockPos = new Vector3[9];

        for (int i = 0; i < 3; ++i)
        {
            gateLockPos[i] = transform.position;
            gateLockPos[i].x += 0.8f * i;
            gateLockPos[i].y += 0.8f;

            gateLockPos[i + 3] = transform.position;
            gateLockPos[i + 3].x += 0.8f * i;
            
            gateLockPos[i + 6] = transform.position;
            gateLockPos[i + 6].x += 0.8f * i;
            gateLockPos[i + 6].y -= 0.8f;

            _GateLocks[i] = Instantiate(RedGateLock, gateLockPos[i], Quaternion.identity);
            _GateLocks[i + 3] = Instantiate(GreenGateLock, gateLockPos[i + 3], Quaternion.identity);
            _GateLocks[i + 6] = Instantiate(BlueGateLock, gateLockPos[i + 6], Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] newGateLocksPos = new Vector3[3];

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (int n = 0; n < 3; ++n)
            {
                newGateLocksPos[n] = _GateLocks[n * 3].transform.position;
                newGateLocksPos[n].x -= 0.8f;
            }

            _NewGateLocks = new GameObject[]
            { 
                Instantiate(RedGateLock, newGateLocksPos[0], Quaternion.identity),
                Instantiate(GreenGateLock, newGateLocksPos[1], Quaternion.identity),
                Instantiate(BlueGateLock, newGateLocksPos[2], Quaternion.identity)
            };

            _moveDir = moveDir.right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            for (int n = 0; n < 3; ++n)
            {
                newGateLocksPos[n] = _GateLocks[n * 3].transform.position;
                newGateLocksPos[n].x += 0.8f;
            }

            _NewGateLocks = new GameObject[]
            {
                Instantiate(RedGateLock, newGateLocksPos[0], Quaternion.identity),
                Instantiate(GreenGateLock, newGateLocksPos[1], Quaternion.identity),
                Instantiate(BlueGateLock, newGateLocksPos[2], Quaternion.identity)
            };

            _moveDir = moveDir.left;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _NewGateLocks = new GameObject[3];

            for (int n = 0; n < 3; ++n)
            {
                newGateLocksPos[n] = _GateLocks[n].transform.position;
                newGateLocksPos[n].y += 0.8f;

                _NewGateLocks[n] = Instantiate(BlueGateLock, newGateLocksPos[n], Quaternion.identity);
            }

            _moveDir = moveDir.down;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _NewGateLocks = new GameObject[3];

            for (int n = 0; n < 3; ++n)
            {
                newGateLocksPos[n] = _GateLocks[n].transform.position;
                newGateLocksPos[n].y -= 0.8f;

                _NewGateLocks[n] = Instantiate(RedGateLock, newGateLocksPos[n], Quaternion.identity);
            }

            _moveDir = moveDir.up;
        }
    }

    private void FixedUpdate()
    {
        switch (_moveDir)
        {
            case moveDir.right:
                for (int m = 0; m < 9; ++m)
                {
                    _GateLocks[m].transform.Translate(Vector3.right * _speed);
                }

                for (int m = 0; m < 3; ++m)
                {
                    _NewGateLocks[m].transform.Translate(Vector3.right * _speed);
                }

                if (_steps == 4)
                {
                    for (int d = 2; d < 9; d += 3)
                    {
                        Destroy(_GateLocks[d]);
                    }

                    for (int m = 8; m >= 0; --m)
                    {
                        if (m % 3 == 0) // 6, 3, 0
                        {
                            _GateLocks[m] = _NewGateLocks[m / 3];
                            Destroy(_NewGateLocks[m / 3]);
                        }
                        else
                        {
                            _GateLocks[m] = _GateLocks[m - 1];
                        }
                    }
                }
                
                break;

            case moveDir.left:
                for (int m = 0; m < 9; ++m)
                {
                    _GateLocks[m].transform.Translate(Vector3.left * _speed);
                }

                for (int m = 0; m < 3; ++m)
                {
                    _GateLocks[m].transform.Translate(Vector3.left * _speed);
                }

                if (_steps == 4)
                {
                    for (int d = 0; d < 7; d += 3)
                    {
                        Destroy(_GateLocks[d]);
                    }

                    for (int m = 0; m < 9; ++m)
                    {
                        if (m % 3 == 1) // 2, 5, 8
                        {
                            _GateLocks[m] = _NewGateLocks[((m + 1) - 3) / 3];
                        }
                        else
                        {
                            _GateLocks[m] = _GateLocks[m + 1];
                        }
                    }
                }

                break;

            case moveDir.up:
                for (int m = 0; m < 9; ++m)
                {
                    _GateLocks[m].transform.Translate(Vector3.up * _speed);
                }

                for (int m = 0; m < 3; ++m)
                {
                    _GateLocks[m].transform.Translate(Vector3.up * _speed);
                }

                if (_steps == 4)
                {
                    for (int d = 0; d < 3; ++d)
                    {
                        Destroy(_GateLocks[d]);
                    }

                    for (int m = 0; m < 9; ++m)
                    {
                        if (m >= 6 && m <= 8) // 0, 1, 2
                        {
                            _GateLocks[m] = _NewGateLocks[m - 6];
                        }
                        else
                        {
                            _GateLocks[m] = _GateLocks[m + 3];
                        }
                    }
                }

                break;

            case moveDir.down:
                for (int m = 0; m < 9; ++m)
                {
                    _GateLocks[m].transform.Translate(Vector3.down * _speed);
                }

                for (int m = 0; m < 3; ++m)
                {
                    _GateLocks[m].transform.Translate(Vector3.down * _speed);
                }

                if (_steps == 4)
                {
                    for (int d = 6; d < 9; ++d)
                    {
                        Destroy(_GateLocks[d]);
                    }

                    for (int m = 8; m >= 0; --m)
                    {
                        if (m <= 2)
                        {
                            _GateLocks[m] = _NewGateLocks[m];
                        }
                        else
                        {
                            _GateLocks[m] = _GateLocks[m - 3];
                        }
                    }
                }

                break;

            default:
                break;
        }

        if (_moveDir != moveDir.none)
        {
            ++_steps;

            if (_steps > 4)
            {
                _moveDir = moveDir.none;
                _steps = 0;
            }
        }
    }
}
