using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    private bool _matchEnded = false;

    [SerializeField]
    private List<GameObject> _spawn = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //_spawn.Add(new List<Vector3>() {new Vector3(-7.5f, 0, 6), new Vector3(7.5f, 0, -6),
        //new Vector3(-7.5f, 0, -6), new Vector3(7.5f, 0, 6) });
        //_spawn.Add(new List<Vector3>() {new Vector3(-7.5f, 0, 6), new Vector3(7.5f, 0, -6),
        //new Vector3(-7.5f, 0, -6), new Vector3(7.5f, 0, 6) });
        //_spawn.Add(new List<Vector3>() {new Vector3(0f, 0, 5f), new Vector3(0, 0, -5f),
        //new Vector3(5, 0, 0), new Vector3(-5, 0, 0) });
        //_spawn.Add(new List<Vector3>() {new Vector3(-2f, 0, 8.75f), new Vector3(2f, 0, -8.75f),
        //new Vector3(8.75f, 0, 2f), new Vector3(-8.75f, 0, -2f) });
        GameUtils.Instance.ResetLayers();
        GameUtils.Instance.ClearPlayerList();
        int playerCount = GameManager.Instance.GetPlayerCount();
        for (int i = 0; i < playerCount; i++)
        {
            GameUtils.Instance.SpawnPlayer(_spawn[i].transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_matchEnded && !GameManager.Instance.GameEnded())
        {
            GameManager.Instance.LoadNextLevel();
        }
        if (!_matchEnded && GameUtils.Instance.GetPlayerCount() == 1)
        {
            PlayerController player = GameUtils.Instance.GetPlayerController(0);
            GameManager.Instance.PlayerWins(player.GetPlayerIndex());
            GameManager.Instance.MatchEnded();
            _matchEnded = true;
        }
        else if (!_matchEnded && GameUtils.Instance.GetPlayerCount() == 0)
        {
            GameManager.Instance.MatchEnded();
            _matchEnded = true;
        }
    }
}
