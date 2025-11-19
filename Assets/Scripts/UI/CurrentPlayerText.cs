using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerText : MonoBehaviour
{
    [SerializeField]Text _player1;
    [SerializeField]Text _player2;
    [SerializeField] Color _selectColor;
    [SerializeField] Color _defaultColor;

    /// <summary>
    /// 選択されているプレイヤーのテキストの色を変える
    /// </summary>
    public void SelectingPlayer(Player player)
    {
        if(player.PlayerNumber == Player.Number.Player1)
        {
            _player1.color = _selectColor;
            _player2.color = _defaultColor;
        }
        else
        {
            _player1.color = _defaultColor;
            _player2.color = _selectColor;
        }
    }
}
