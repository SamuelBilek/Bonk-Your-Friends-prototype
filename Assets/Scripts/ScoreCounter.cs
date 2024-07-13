using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    private List<string> _colors = new List<string>() { "Blue", "Red", "Green", "Yellow"};
    private static List<int> _wins;
    private TextMeshProUGUI _score;
    // Start is called before the first frame update
    void Start()
    {
        _score = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_wins != null)
        {
            string result = "";
            for (int i = 0; i < _wins.Count; i++)
            {
                result += _colors[i] + ": " + _wins[i] + "\n";
            }
            _score.text = result;
        }
    }

    public static void SetPlayerWins(List<int> wins)
    {
        _wins = wins;
    }

    public static void ResetPlayerWins()
    {
        _wins = null;
    }
}
