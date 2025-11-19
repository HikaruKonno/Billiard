using UnityEngine;
using UnityEngine.UI;

public class ScoreUIViewer : MonoBehaviour
{
    [SerializeField]
    private Text _playerScore1; // プレイヤーのスコアを表示するテキスト
    [SerializeField]
    private Text _playerScore2;

    // UIの更新
    public void UpdateScore(Player.Number number,int score)
    {
        if(number == Player.Number.Player1)
        {
            _playerScore1.text = $"{number} : {score}";
        }
        else
        {
            _playerScore2.text = $"{number} : {score}";
        }
    }
}
