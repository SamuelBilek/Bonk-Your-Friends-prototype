using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayButton : MonoBehaviour
{
    private Button _button;
    private Image _buttonColor;
    private TextMeshProUGUI _textMeshPro;
    [SerializeField]
    private Color _readyColor;
    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _buttonColor = _button.GetComponent<Image>();
        _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        _button.onClick.AddListener(StartGame);

        _textMeshPro.text = "Waiting for players";
        _buttonColor.color = Color.white;
        _textMeshPro.color = Color.black;
        _button.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerReadyButton.GetPlayerCount() >= 2)
        {
            _textMeshPro.text = "PLAY";
            _buttonColor.color = Color.green;
            _textMeshPro.color = Color.white;
            _button.enabled = true;
        }
        else
        {
            _textMeshPro.text = "Waiting for players";
            _buttonColor.color = Color.white;
            _textMeshPro.color = Color.black;
            _button.enabled = false;
        }
    }

    private void StartGame()
    {
        GameManager.Instance.StartGame(PlayerReadyButton.GetPlayerCount(), RoundCounter.GetRoundsCount());
    }
}
