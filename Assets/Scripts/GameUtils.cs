using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameUtils : MonoBehaviour
{
    public static GameUtils Instance { get; private set; }
    [SerializeField]
    private int _maxPlayerCount;
    [SerializeField]
    public float BounceFactor;
    [SerializeField]
    public int MaxAttackStrength;
    [SerializeField]
    public float cameraSpeed;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private Color Player1Color;
    [SerializeField]
    private Color Player2Color;
    [SerializeField]
    private Color Player3Color;
    [SerializeField]
    private Color Player4Color;
    private List<Color> _playerColors;

    public static int playerCount = 0;

    public float sceneWidth = 20.0f;
    public float sceneHeight = 20.0f;

    private int _currentPlayerLayer;
    private int _currentPlayerWeaponLayer;

    public float playerMaxDistance;

    private List<GameObject> _players = new List<GameObject>();
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this) // We destroy other instances if we have one.
            Destroy(this.gameObject);
        _playerColors = new List<Color>() { Player1Color, Player2Color, Player3Color, Player4Color };
    }

    public float minX() { return -sceneWidth / 2.0f; }
    public float maxX() { return sceneWidth / 2.0f; }
    public float minZ() { return -sceneHeight / 2.0f; }
    public float maxZ() { return sceneHeight / 2.0f; }
    public Vector3 positionClippedIntoGameArea(Vector3 pos)
    {
        Vector3 result = pos;
        result.x = result.x < minX() ? minX() : result.x;
        result.x = result.x > maxX() ? maxX() : result.x;
        result.z = result.z < minZ() ? minZ() : result.z;
        result.z = result.z > maxZ() ? maxZ() : result.z;
        return result;
    }
    public bool isOutsideGameArea(Vector3 pos)
    {
        return pos.x < minX() || pos.x > maxX() || pos.z < minZ() || pos.z >
        maxZ();
    }

    public static Vector3 ComputeEulerStep(Vector3 x0, Vector3 dx_dt, float delta_t)
    {
        return x0 + delta_t * dx_dt;
    }



    // Start is called before the first frame update
    void Start()
    {
        //ResetLayers();
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.F6))
        {
            SpawnPlayer();
        }*/

        if (Input.GetKeyDown(KeyCode.F5))
        {
            KillLastPlayer();
        }
    }

    public Color GetPlayerColor(int playerIndex)
    {
        return _playerColors[playerIndex];
    }
    public void SpawnPlayer(Vector3 pos)
    {
        if (playerCount < _maxPlayerCount)
        {
            GameObject playerGO = Instantiate(playerPrefab, pos, Quaternion.identity);
            _currentPlayerLayer++;
            _currentPlayerWeaponLayer++;
            playerGO.GetComponent<PlayerController>().SetLayers(_currentPlayerLayer, _currentPlayerWeaponLayer);
            _players.Add(playerGO);
        }
        else
        {
            Debug.Log("Maximum Player Count Reached!");
        }
    }

    public void KillPlayer(int playerIndex)
    {
        if (playerCount > 0)
        {
            Debug.Log("Killing player index: " + playerIndex + " Total number of players: " + _players.Count);
            GameObject killedPlayer = _players.Where(x => x.GetComponent<PlayerController>().GetPlayerIndex() == playerIndex).First();
            _players = _players.Where(x => x.GetComponent<PlayerController>().GetPlayerIndex() != playerIndex).ToList();
            Destroy(killedPlayer);
        }
        else
        {
            Debug.Log("No more players in game!");
        }
    }

    public void KillLastPlayer()
    {
        if (playerCount > 0)
        {
            _currentPlayerLayer--;
            _currentPlayerWeaponLayer--;
            Destroy(_players[_players.Count - 1]);
            _players.RemoveAt(_players.Count - 1);
        }
        else
        {
            Debug.Log("No more players in game!");
        }
    }
    public void ResetLayers()
    {
        _currentPlayerLayer = LayerMask.NameToLayer("Player1") - 1;
        _currentPlayerWeaponLayer = LayerMask.NameToLayer("Player1Weapon") - 1;
    }

    public int GetPlayerCount()
    {
        return _players.Count;
    }

    public void ClearPlayerList()
    {
        _players.Clear();
    }

    public PlayerController GetPlayerController(int index)
    {
        return _players[index].GetComponent<PlayerController>();
    }

    public Vector3 GetCenterBetweenPlayers()
    {
        if (_players.Count == 0)
        {
            return Vector3.zero;
        }
        Vector3 min = _players[0].transform.position;
        Vector3 max = _players[0].transform.position;
        for (int i = 1; i < _players.Count; i++)
        {
            GameObject player = _players[i];
            if (player != null)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (player.transform.position[j] < min[j])
                    {
                        min[j] = player.transform.position[j];
                    }
                    if (player.transform.position[j] > max[j])
                    {
                        max[j] = player.transform.position[j];
                    }
                }
            }
        }
        playerMaxDistance = (max - min).magnitude;
        return Vector3.Lerp(min, max, 0.5f);
    }

    public List<GameObject> getPlayers()
    {
        return _players;
    }
}
