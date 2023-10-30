using JetBrains.Annotations;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

enum gateState
{
    preparing,
    closed,
    deciphering,
    opening,
    open,
}


enum moveDir
{
    none,
    right,
    left,
    up,
    down,
}

enum selectorDir
{
    horizontal,
    vertical,
}


public class GateBehaviourScript : MonoBehaviour
{
    public GameObject GateLock, Daemon, HorizontalSelector, VerticalSelector;

    private GameObject[,] _GateLocks = new GameObject[3, 3];
    private GameObject _NewLock;
    private GameObject _Selector;
    private gateState _gateState = gateState.preparing;
    private moveDir _moveDir = moveDir.none;
    private selectorDir _selectorDir = selectorDir.horizontal;
    private float _speed = 0.2f;
    private float _traveled = 0;
    private float _spawnTimer = 0.0f;
    private float _closeTimer = 0.0f;
    private int _steps = 0;
    private int _manyDaemons = 0;
    private char[,] _matrix;
    private string[] _beatKey = { "a", "s", "d" };
    private DaemonBehaviour _daemonProps = null;

    private char[,] createNewMatrix()
    {
        int shuffleFactor = Random.Range(1, 8);
        char[] shuffle = { 'r', 'r', 'r', 'g', 'g', 'g', 'b', 'b', 'b' };
        int i = 0;

        while (shuffleFactor > 0)
        {
            char aux = shuffle[i];
            shuffle[i] = shuffle[i + shuffleFactor];
            shuffle[i + shuffleFactor] = aux;

            ++i;
            if (i % shuffleFactor == 0)
                i += shuffleFactor;

            if (i + shuffleFactor >= 8)
            {
                char[] auxArray = new char[9];

                for (int axi = 0; axi < 9; ++axi)
                {
                    auxArray[axi] = shuffle[i];
                    ++i;

                    if (i > 8)
                        i = 0;
                }

                shuffle = auxArray;
                --shuffleFactor;
                i = 0;
            }
        }

        char[,] ret = new char[3, 3];
        
        for (int ri = 0; ri < 3; ++ri)
        {
            ret[ri, 0] = shuffle[ri * 3];
            ret[ri, 1] = shuffle[(ri * 3) + 1];
            ret[ri, 2] = shuffle[(ri * 3) + 2];
        }

        return ret;
    }

    private Color selectColor(char rep)
    {
        switch (rep)
        {
            case 'r':
                return Color.red;

            case 'g':
                return Color.green;

            default:
                return Color.blue;
        }
    }

    private GameObject createGateLock(Vector3 lockPos, char lockColor)
    {
        GameObject ret = Instantiate(GateLock, lockPos, Quaternion.identity);
        ret.GetComponent<SpriteRenderer>().color = selectColor(lockColor);

        return ret;
    }

    private void RestartGate()
    {
        _matrix = createNewMatrix();

        for (int i = 0; i < 3; ++i)
        {
            Vector3 gateLockPos = transform.position;

            gateLockPos.x += 0.8f * i;
            gateLockPos.y += 0.8f;
            _GateLocks[i, 0] = createGateLock(gateLockPos, _matrix[i, 0]);

            gateLockPos.y -= 0.8f;
            _GateLocks[i, 1] = createGateLock(gateLockPos, _matrix[i, 1]);

            gateLockPos.y -= 0.8f;
            _GateLocks[i, 2] = createGateLock(gateLockPos, _matrix[i, 2]);
        }

        _gateState = gateState.closed;
    }
    private void SpawnDaemons()
    {
        int nextBeatKey = 0;

        _spawnTimer += Time.deltaTime;

        if (_spawnTimer > 0.5f) // Wait half second to spaw a new daemon
        {
            _spawnTimer = 0.0f;

            var newBornDaemon = Instantiate(Daemon, transform.position, Quaternion.identity);
            nextBeatKey = Random.Range(0, 3);

            _daemonProps = newBornDaemon.GetComponent<DaemonBehaviour>();
            _daemonProps.beatKey = _beatKey[nextBeatKey];
            _manyDaemons++;

            if (_manyDaemons == 3)
            {
                _manyDaemons = 0;
                _gateState = gateState.preparing;
            }

            Debug.Log(_daemonProps.beatKey);
        }
    }

    // Tells if there was suficient delay to start opening the gate
    private void WaitToDecipher()
    {
        _closeTimer += Time.deltaTime;

        if (_closeTimer >= 3.0f)
        {
            Vector3 selectorPos = transform.position;
            
            selectorPos.x += 0.8f;
            selectorPos.y += 0.8f;
            selectorPos.z += 1;

            _Selector = Instantiate(HorizontalSelector, selectorPos, Quaternion.identity);
            _selectorDir = selectorDir.horizontal;
            _steps = 0;

            _closeTimer = 0.0f;
            _gateState = gateState.deciphering;
        }
    }

    private void MoveSelector()
    {
        Vector3 newLockPos;

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
                newLockPos = _GateLocks[0, _steps].transform.position;

                newLockPos.x -= 0.8f;
                _NewLock = Instantiate(_GateLocks[2, _steps], newLockPos, Quaternion.identity);
                _moveDir = moveDir.right;

                Destroy(_Selector);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                newLockPos = _GateLocks[2, _steps].transform.position;

                newLockPos.x += 0.8f;
                _NewLock = Instantiate(_GateLocks[0, _steps], newLockPos, Quaternion.identity);
                _moveDir = moveDir.left;

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
                newLockPos = _GateLocks[_steps, 2].transform.position;

                newLockPos.y -= 0.8f;
                _NewLock = Instantiate(_GateLocks[_steps, 0], newLockPos, Quaternion.identity);
                _moveDir = moveDir.up;

                Destroy(_Selector);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                newLockPos = _GateLocks[_steps, 0].transform.position;

                newLockPos.y += 0.8f;
                _NewLock = Instantiate(_GateLocks[_steps, 2], newLockPos, Quaternion.identity);
                _moveDir = moveDir.down;

                Destroy(_Selector);
            }
        }
    }
    private void CheckMatrix()
    {
        bool isMatrixOrdened = false;
        // First check if all columns are color aligned
        for (int i = 0; i < 3; ++i)
        {
            isMatrixOrdened = _matrix[i, 0] == _matrix[i, 1] && _matrix[i, 1] == _matrix[i, 2];

            if (!isMatrixOrdened)
                break;
        }
   
        // If that failed, tries to check if are lines that are color aligned
        if (!isMatrixOrdened)
        {
            for (int i = 0; i < 3; ++i)
            {
                isMatrixOrdened = _matrix[0, i] == _matrix[1, i] && _matrix[1, i] == _matrix[2, i];

                if (!isMatrixOrdened)
                    break;
            }
        }

        // If the matrix is ordened change the gate state to opening
        if (isMatrixOrdened)
            _gateState = gateState.opening;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_gateState == gateState.preparing)
        {
            RestartGate();
            _gateState = gateState.closed;
        }
    }


    // Update is called once per frame
    // Here is built the gate state machine
    void Update()
    {
        switch (_gateState)
        {
            case gateState.preparing:
                RestartGate();
                break;

            case gateState.closed:
                WaitToDecipher();
                break;

            case gateState.deciphering:
                MoveSelector();
                break;

            case gateState.opening:
                for (int i = 0; i < 3; ++i)
                {
                    Destroy(_GateLocks[i, 0]);
                    Destroy(_GateLocks[i, 1]);
                    Destroy(_GateLocks[i, 2]);
                    Destroy(_Selector);
                }

                _gateState = gateState.open;
                break;

            case gateState.open:
                SpawnDaemons();
                break;
        }
    }

    // Here we make the movementation to happen
    private void FixedUpdate()
    {
        char holder; // Hold the last moving char in matrix

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
                    holder = _matrix[2, _steps];

                    Destroy(_GateLocks[2, _steps]);

                    for (int i = 2; i > 0; --i)
                    {
                        _GateLocks[i, _steps] = _GateLocks[i - 1, _steps];
                        _matrix[i, _steps] = _matrix[i - 1, _steps];
                    }
                    _GateLocks[0, _steps] = _NewLock;
                    _matrix[0, _steps] = holder;
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
                    holder = _matrix[0, _steps];

                    Destroy(_GateLocks[0, _steps]);

                    for (int i = 0; i < 2; ++i)
                    {
                        _GateLocks[i, _steps] = _GateLocks[i + 1, _steps];
                        _matrix[i, _steps] = _matrix[i + 1, _steps];
                    }
                    _GateLocks[2, _steps] = _NewLock;
                    _matrix[2, _steps] = holder;
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
                    holder = _matrix[_steps, 0];

                    Destroy(_GateLocks[_steps, 0]);

                    for (int i = 0; i < 2; ++i)
                    {
                        _GateLocks[_steps, i] = _GateLocks[_steps, i + 1];
                        _matrix[_steps, i] = _matrix[_steps, i + 1];
                    }
                    _GateLocks[_steps, 2] = _NewLock;
                    _matrix[_steps, 2] = holder;
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
                    holder = _matrix[_steps, 2];

                    Destroy(_GateLocks[_steps, 2]);

                    for (int i = 2; i > 0; --i)
                    {
                        _GateLocks[_steps, i] = _GateLocks[_steps, i - 1];
                        _matrix[_steps, i] = _matrix[_steps, i - 1];
                    }
                    _GateLocks[_steps, 0] = _NewLock;
                    _matrix[_steps, 0] = holder;
                }

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

            // Check matrix only after preparing the next movement
            CheckMatrix();
        }
    }
}
