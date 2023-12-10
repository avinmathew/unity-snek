using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake2 : MonoBehaviour
{
    public GameObject SnakeSegmentPrefab;
    public AudioSource AudioSource;
    public AudioClip EatClip;
    public AudioClip DieClip;

    private Vector2 _input = Vector2.zero;
    private Vector2 _direction;
    private List<GameObject> _segments = new List<GameObject>();

    private void Start()
    {
        ResetSnake();
        transform.position = new Vector3(1, 0, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && _direction != Vector2.down)
        {
            _input = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && _direction != Vector2.up)
        {
            _input = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && _direction != Vector2.right)
        {
            _input = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && _direction != Vector2.left)
        {
            _input = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        _direction = _input;

        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].transform.position = _segments[i - 1].transform.position;
        }

        float x = Mathf.Round(transform.position.x) + _direction.x;
        float y = Mathf.Round(transform.position.y) + _direction.y;
        transform.position = new Vector3(x, y, 0);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            GameManager.Instance.AddScore2();
            AudioSource.PlayOneShot(EatClip);
            Grow();
        }
        else if (collision.tag == "Obstacle")
        {
            GameManager.Instance.ResetScore2();
            AudioSource.PlayOneShot(DieClip);
            ResetSnake();
        }
    }

    private void Grow()
    {
        GameObject newSegment = Instantiate(SnakeSegmentPrefab);
        newSegment.transform.position = _segments.LastOrDefault().transform.position;
        _segments.Add(newSegment);
    }

    private void ResetSnake()
    {
        // Initial direction
        _direction = Vector2.right;

        // Remove any previous segments if we are restarting the game
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i]);
        }

        _segments.Clear();

        // First segment is the head
        _segments.Add(gameObject);

        // Reset position if we are restarting the game
        gameObject.transform.position = Vector3.zero;
    }

    public void SetActive(bool value)
    {
        foreach (var segment in _segments)
        {
            segment.gameObject.SetActive(value);
        }
    }

    public bool Occupies(float x, float y)
    {
        foreach (var segment in _segments)
        {
            if (segment.transform.position.x == x && segment.transform.position.y == y)
            {
                return true;
            }
        }
        return false;
    }
}
