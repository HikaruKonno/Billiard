using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnUIText:MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _turnText;

    public void UpdateText(int turn)
    {
        _turnText.text = turn.ToString();
    }
}
