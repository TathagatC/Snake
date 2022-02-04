using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private List<Transform> _segments = new List<Transform>();
    public Transform bodyPrefab;
    public int intialSize = 3;

    private void Awake()
    {
        gridPosition = new Vector2Int(10,10);
        gridMoveTimerMax = .10f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1, 0);
    }

    void Start()
    {
        ResetState();
    }

    private void Update()
    {
        KeyInputs();
        GridMovement();
    }

    // void FixedUpdate()
    // {
    //     for(int i = _segments.Count - 1; i > 0 ; i--)
    //     {
    //         _segments[i].position = _segments[i - 1].position;    
    //     }
    // }

    private void KeyInputs()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(gridMoveDirection.y != -1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = +1;
            }
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(gridMoveDirection.y != +1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -1;
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(gridMoveDirection.x != +1)
            {
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;
            }
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(gridMoveDirection.x != -1)
            {
                gridMoveDirection.x = +1;
                gridMoveDirection.y = 0;
            }
        }
    }

    private void GridMovement()
    {
        gridMoveTimer += Time.deltaTime;

        if(gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            for(int i = _segments.Count - 1; i > 0 ; i--)
        {
            _segments[i].position = _segments[i - 1].position;    
        }

            gridPosition += gridMoveDirection;

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90);
        }
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.bodyPrefab);
        segment.position = _segments[_segments.Count -1].position;

        _segments.Add(segment);
    }

    private void ResetState()
    {
        for(int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);    
        }

        _segments.Clear();
        _segments.Add(this.transform);

        for(int i = 1; i < this.intialSize; i++)
        {
            _segments.Add(Instantiate(this.bodyPrefab));    
        }

        this.transform.position = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Food")
        {
            Grow();   
        } else if( other.tag == "Obstacle")
        {
            ResetState();
        }
    }
}
