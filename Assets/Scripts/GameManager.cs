using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int _playerCount;
    private int _matchCount;
    private int _matchesPlayed;
    private List<int> _playerWins;
    public static GameManager Instance;
    public string[] ScenePool;
    public int _sceneIndex = 0;
    private bool _gameEnded = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this) // We destroy other instances if we have one.
            Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        ScenePool = new string[] { "level1", "level2", "level3", "level4" };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu");
        }
        if (!_gameEnded && _matchCount > 0 && _matchesPlayed >= _matchCount)
        {
            _gameEnded = true;
            EndScreen();
        }
    }

    private void EndScreen()
    {
        SceneManager.LoadScene("End Screen");
    }

    public void StartGame(int playerCount, int matchCount)
    {
        _playerCount = playerCount;
        _matchCount = matchCount;
        _matchesPlayed = 0;
        _playerWins = new List<int>();
        _gameEnded = false;
        for (int i = 0; i < _playerCount; i++)
        {
            _playerWins.Add(0);
        }
        ScoreCounter.SetPlayerWins(_playerWins);
        LoadNextLevel();
    }

    public void ResetGame()
    {
        _playerCount = 0;
        _matchCount = 0;
        _matchesPlayed = 0;
        _playerWins = new List<int>();
        ScoreCounter.ResetPlayerWins();
    }

    public int GetPlayerCount()
    {
        return _playerCount;
    }

    public void PlayerWins(int playerIndex)
    {
        if (_playerWins != null)
        {
            _playerWins[playerIndex] += 1;
        }
    }

    public void LoadNextLevel()
    {
        if (_sceneIndex >= ScenePool.Length)
        {
            _sceneIndex = 0;
        }
        SceneManager.LoadScene(ScenePool[_sceneIndex]);
        _sceneIndex++;
    }

    public void MatchEnded()
    {
        _matchesPlayed++;
    }

    public bool GameEnded()
    {
        return _gameEnded;
    }

    public int GetWinnerIndex()
    {
        int max = 0;
        int index = 0;
        int result = -1; ;
        int count = 0;
        foreach (int winCount in _playerWins)
        {
            if (winCount > max)
            {
                max = winCount;
                count = 1;
                result = index;
            }
            else if (winCount == max)
            {
                count++;
                result = -1;
            }
            index++;
        }
        return result;
    }

    public static void ExitGame()
    {
        Application.Quit();
    }
}
