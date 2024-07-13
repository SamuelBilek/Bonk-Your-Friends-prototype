using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _textParent;
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI _text = _textParent.GetComponent<TextMeshProUGUI>();
        int winnerIndex = GameManager.Instance.GetWinnerIndex();
        if (winnerIndex == -1)
        {
            _text.text = "Draw!";
        }
        else
        {
            string playerColor = "";
            if (winnerIndex == 0)
            {
                playerColor = "Blue";
            }
            else if (winnerIndex == 1)
            {
                playerColor = "Red";
            }
            else if (winnerIndex == 2)
            {
                playerColor = "Green";
            }
            else
            {
                playerColor = "Yellow";
            }
            _text.text = playerColor + "\n" + "Wins!";
            _text.color = GameUtils.Instance.GetPlayerColor(winnerIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
