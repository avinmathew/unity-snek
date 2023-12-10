using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI Title;
    public TextMeshProUGUI Label1P;
    public TextMeshProUGUI Label2P;
    public TextMeshProUGUI Instructions1P;
    public TextMeshProUGUI Instructions2P;
    public TextMeshProUGUI PlayerSelector;
    public GameObject Snake1;
    public GameObject Snake2;
    public GameObject Food;
    public TextMeshProUGUI Score1;
    public TextMeshProUGUI Best1;
    public TextMeshProUGUI Score2;
    public TextMeshProUGUI Best2;
    public AudioSource AudioSource;
    public AudioClip PlayerSelectorSound;

    private int _numberOfPlayers = 1;
    private bool _isGameStarted = false;
    private int _score1 = 0;
    private int _best1 = 0;
    private int _score2 = 0;
    private int _best2 = 0;

    private void Start()
    {
        GameManager.Instance = this;

        Instructions2P.enabled = false;
        Score1.enabled = false;
        Best1.enabled = false;
        Score2.enabled = false;
        Best2.enabled = false;
    }

    public void Update()
    {
        // Quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isGameStarted)
            {
                StopGame();
            }
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }
        }

        if (_isGameStarted)
        {
            return;
        }
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && _numberOfPlayers == 1)
        {
            _numberOfPlayers = 2;
            Instructions1P.enabled = false;
            Instructions2P.enabled = true;
            PlayerSelector.rectTransform.position = new Vector3(-10.86f, 3.58f, 90.00f);
            AudioSource.PlayOneShot(PlayerSelectorSound);
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && _numberOfPlayers == 2)
        {
            _numberOfPlayers = 1;
            Instructions1P.enabled = true;
            Instructions2P.enabled = false;
            PlayerSelector.rectTransform.position = new Vector3(-10.86f, 6.47f, 90.00f);
            AudioSource.PlayOneShot(PlayerSelectorSound);
        }
        else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (_isGameStarted)
            return;

        _isGameStarted = true;

        // UI
        Title.enabled = false;
        Label1P.enabled = false;
        Label2P.enabled = false;
        PlayerSelector.enabled = false;
        Instructions1P.enabled = false;
        Instructions2P.enabled = false;

        // Game
        Snake1.SetActive(true);
        Food.SetActive(true);
        Score1.enabled = true;
        Best1.enabled = true;
        // Snake body
        Snake snake = GameObject.FindObjectOfType<Snake>();
        snake.SetActive(true);
        if (_numberOfPlayers == 2)
        {
            Snake2.SetActive(true);
            Score2.enabled = true;
            Best2.enabled = true;

            // Snake body
            Snake2 snake2 = GameObject.FindObjectOfType<Snake2>();
            snake2.SetActive(true);
        }
    }

    public void StopGame()
    {
        if (!_isGameStarted)
            return;

        _isGameStarted = false;

        // UI
        Title.enabled = true;
        Label1P.enabled = true;
        Label2P.enabled = true;
        PlayerSelector.enabled = true;
        Instructions1P.enabled = true;
        Instructions2P.enabled = true;

        // Game
        // Hide snake body before head, otherwise FindObjectByType returns null
        Snake snake = GameObject.FindObjectOfType<Snake>();
        snake.SetActive(false);
        Snake1.SetActive(false);
        Food.SetActive(false);
        Score1.enabled = false;
        Best1.enabled = false;
        if (_numberOfPlayers == 2)
        {
            Snake2 snake2 = GameObject.FindObjectOfType<Snake2>();
            snake2.SetActive(false);
            Snake2.SetActive(false);
            Score2.enabled = false;
            Best2.enabled = false;
        }
    }

    public void AddScore1()
    {
        _score1++;
        UpdateScoreUI();
    }

    public void AddScore2()
    {
        _score2++;
        UpdateScoreUI();
    }

    public void ResetScore1()
    {
        if (_score1 > _best1)
        {
            _best1 = _score1;
        }
        _score1 = 0;
        UpdateScoreUI();
    }

    public void ResetScore2()
    {
        if (_score2 > _best2)
        {
            _best2 = _score2;
        }
        _score2 = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        Score1.text = $"Score: {_score1.ToString()}";
        Best1.text = $"Best: {_best1.ToString()}";
        Score2.text = $"Score: {_score2.ToString()}";
        Best2.text = $"Best: {_best2.ToString()}";
    }
}
