using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerReadyButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;
    private Image _buttonColor;
    private TextMeshProUGUI _textMeshPro;
    private static int _playerCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ToggleButton);
        _buttonColor = _button.GetComponent<Image>();
        _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

        _buttonColor.color = Color.white;
        _textMeshPro.color = Color.black;
        _textMeshPro.text = "Not Ready";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleButton()
    {
        
        if (_buttonColor != null)
        {
            if (_buttonColor.color == Color.white)
            {
                _buttonColor.color = GameUtils.Instance.GetPlayerColor(_playerCount);
                _textMeshPro.color = Color.white;
                _textMeshPro.text = "Ready";
                _playerCount++;
            }
            else
            {
                _buttonColor.color = Color.white;
                _textMeshPro.color = Color.black;
                _textMeshPro.text = "Not Ready";
                _playerCount--;
            }
        }
    }
    public static int GetPlayerCount()
    {
        return _playerCount;
    }

    public static void ResetPlayerCount()
    {
        _playerCount = 0;
    }
}
