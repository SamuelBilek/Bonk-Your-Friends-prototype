using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundCounter : MonoBehaviour
{
    [SerializeField]
    private int _maxRoundsCount = 10;
    private static int _roundsCount = 1;
    private TextMeshProUGUI _textMeshProUGUI;
    // Start is called before the first frame update
    void Start()
    {
        _textMeshProUGUI = transform.Find("Rounds").GetComponentInChildren<TextMeshProUGUI>();
        UpdateScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddRound()
    {
        if (_roundsCount < _maxRoundsCount)
        {
            _roundsCount++;
            UpdateScreen();
        }
    }
    public void RemoveRound()
    {
        if ( _roundsCount > 1)
        {
            _roundsCount--;
            UpdateScreen();
        }
    }

    private void UpdateScreen()
    {
        _textMeshProUGUI.text = _roundsCount.ToString();
    }

    public static int GetRoundsCount()
    {
        return _roundsCount;
    }
}
