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

enum selectorDir
{
    none = 0,
    horizontal,
    vertical,
}

public class GateBehaviourScript : MonoBehaviour
{
    public GameObject RedGateLock, GreenGateLock, BlueGateLock, HorizontalSelector, VerticalSelector;

    private GameObject[,] _GateLocks = new GameObject[3, 3];
    private GameObject _NewLock;
    private GameObject _Selector;
    private moveDir _moveDir = moveDir.none;
    private selectorDir _selectorDir = selectorDir.horizontal;
    private float _speed = 0.2f;
    private float _traveled = 0;
    private int _steps = 0;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 selectorPos = transform.position;

        selectorPos.x += 0.8f;
        selectorPos.y += 0.8f;
        selectorPos.z += 1;
        _Selector = Instantiate(HorizontalSelector, selectorPos, Quaternion.identity);
        
        for (int i = 0; i < 3; ++i)
        {
            Vector3 gateLockPos = transform.position;

            gateLockPos.x += 0.8f * i;
            gateLockPos.y += 0.8f;
            _GateLocks[i, 0] = Instantiate(RedGateLock, gateLockPos, Quaternion.identity);

            gateLockPos.y -= 0.8f;
            _GateLocks[i, 1] = Instantiate(GreenGateLock, gateLockPos, Quaternion.identity);

            gateLockPos.y -= 0.8f;
            _GateLocks[i, 2] = Instantiate(BlueGateLock, gateLockPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_selectorDir == selectorDir.horizontal)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_steps == 2)
                {
                    _Selector.transform.Translate(Vector3.up * 1.6f);
                    _steps = 0;
                }
                else
                {
                    _Selector.transform.Translate(Vector3.down * 0.8f);
                    ++_steps;
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_steps == 0)
                {
                    _Selector.transform.Translate(Vector3.down * 1.6f);
                    _steps = 2;
                }
                else
                {
                    _Selector.transform.Translate(Vector3.up * 0.8f);
                    --_steps;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Vector3 newLockPos = _GateLocks[0, _steps].transform.position;
                
                newLockPos.x -= 0.8f;
                _NewLock = Instantiate(_GateLocks[2, _steps], newLockPos, Quaternion.identity);
                _moveDir = moveDir.right;
                _selectorDir = selectorDir.none;
                
                Destroy(_Selector);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Vector3 newLockPos = _GateLocks[2, _steps].transform.position;

                newLockPos.x += 0.8f;
                _NewLock = Instantiate(_GateLocks[0, _steps], newLockPos, Quaternion.identity);
                _moveDir = moveDir.left;
                _selectorDir = selectorDir.none;

                Destroy(_Selector);
            }
        }
        else if (_selectorDir == selectorDir.vertical)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_steps == 2)
                {
                    _Selector.transform.Translate(Vector3.left * 1.6f);
                    _steps = 0;
                }
                else
                {
                    _Selector.transform.Translate(Vector3.right * 0.8f);
                    ++_steps;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_steps == 0)
                {
                    _Selector.transform.Translate(Vector3.right * 1.6f);
                    _steps = 2;
                }
                else
                {
                    _Selector.transform.Translate(Vector3.left * 0.8f);
                    --_steps;
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Vector3 newLockPos = _GateLocks[_steps, 2].transform.position;

                newLockPos.y -= 0.8f;
                _NewLock = Instantiate(_GateLocks[_steps, 0], newLockPos, Quaternion.identity);
                _moveDir = moveDir.up;
                _selectorDir = selectorDir.none;

                Destroy(_Selector);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Vector3 newLockPos = _GateLocks[_steps, 0].transform.position;

                newLockPos.y += 0.8f;
                _NewLock = Instantiate(_GateLocks[_steps, 2], newLockPos, Quaternion.identity);
                _moveDir = moveDir.down;
                _selectorDir = selectorDir.none;

                Destroy(_Selector);
            }
        }
    }

    private void FixedUpdate()
    {
        switch (_moveDir)
        {
            case moveDir.right:
                for (int i = 0; i < 3; ++i)
                {
                    _GateLocks[i, _steps].transform.Translate(Vector3.right * _speed);
                }
                _NewLock.transform.Translate(Vector3.right * _speed);
                _traveled += _speed;

                if (_traveled == 0.8f)
                {
                    Destroy(_GateLocks[2, _steps]);

                    for (int i = 2; i > 0; --i)
                    {
                        _GateLocks[i, _steps] = _GateLocks[i - 1, _steps];
                    }
                    _GateLocks[0, _steps] = _NewLock;
                }

                break;

            case moveDir.left:
                for (int i = 0; i < 3; ++i)
                {
                    _GateLocks[i, _steps].transform.Translate(Vector3.left * _speed);
                }
                _NewLock.transform.Translate(Vector3.left * _speed);
                _traveled += _speed;

                if (_traveled == 0.8f)
                {
                    Destroy(_GateLocks[0, _steps]);

                    for (int i = 0; i < 2; ++i)
                    {
                        _GateLocks[i, _steps] = _GateLocks[i + 1, _steps];
                    }
                    _GateLocks[2, _steps] = _NewLock;
                }

                break;

            case moveDir.up:
                for (int i = 0; i < 3; ++i)
                {
                    _GateLocks[_steps, i].transform.Translate(Vector3.up * _speed);
                }
                _NewLock.transform.Translate(Vector3.up * _speed);
                _traveled += _speed;

                if (_traveled == 0.8f)
                {
                    Destroy(_GateLocks[_steps, 0]);

                    for (int i = 0; i < 2; ++i)
                    {
                        _GateLocks[_steps, i] = _GateLocks[_steps, i + 1];
                    }
                    _GateLocks[_steps, 2] = _NewLock;
                }

                break;

            case moveDir.down:
                for (int i = 0; i < 3; ++i)
                {
                    _GateLocks[_steps, i].transform.Translate(Vector3.down * _speed);
                }
                _NewLock.transform.Translate(Vector3.down * _speed);
                _traveled += _speed;

                if (_traveled == 0.8f)
                {
                    Destroy(_GateLocks[_steps, 2]);

                    for (int i = 2; i > 0; --i)
                    {
                        _GateLocks[_steps, i] = _GateLocks[_steps, i - 1];
                    }
                    _GateLocks[_steps, 0] = _NewLock;
                }

                break;

            default:
                break;
        }

        if (_traveled == 0.8f)
        {
            if (_moveDir == moveDir.right || _moveDir == moveDir.left)
            {
                Vector3 newSelectorPos = _GateLocks[_steps, 1].transform.position;

                newSelectorPos.z += 1;

                _Selector = Instantiate(VerticalSelector, newSelectorPos, Quaternion.identity);
                _selectorDir = selectorDir.vertical;
            }
            else if (_moveDir == moveDir.up || _moveDir == moveDir.down)
            {
                Vector3 newSelectorPos = _GateLocks[1, _steps].transform.position;

                newSelectorPos.z += 1;

                _Selector = Instantiate(HorizontalSelector, newSelectorPos, Quaternion.identity);
                _selectorDir = selectorDir.horizontal;
            }

            _moveDir = moveDir.none;
            _traveled = 0;
        }
    }
}
