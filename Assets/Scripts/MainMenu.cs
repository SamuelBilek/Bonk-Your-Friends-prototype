using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ResetGame();
        PlayerReadyButton.ResetPlayerCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
