using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class TurnProgress
{
    private TurnUIText _turnUIText;
    private int _remainingTurns;
    private event Action<ChangeStateRequest> _onChangeState;

    public TurnProgress(int initialTurns, TurnUIText turnUIText,Action<ChangeStateRequest> action)
    {
        _remainingTurns = initialTurns;
        _turnUIText = turnUIText;
        _onChangeState = action;

        // ターン数をセット
        _turnUIText.UpdateText(_remainingTurns);
    }

    /// <summary>
    /// ターン経過の処理
    /// </summary>
    public void Progress()
    {
        // ゲームが終わったらリザルトに飛ぶ
        if (_remainingTurns <= 0)
        {
            StateChangeBus.UnSubscribe(_onChangeState);
            SceneManager.LoadScene("Result");
        }
        _remainingTurns--;
        _turnUIText?.UpdateText(_remainingTurns);
    }
}

