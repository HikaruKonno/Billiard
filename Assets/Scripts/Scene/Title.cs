using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Title : MonoBehaviour
{
    private const float _waitTime = 0.2f; // 次の入力を受け付けるまで待つ時間

    [SerializeField]
    private TextMeshProUGUI _statText;  // ゲームスタートのテキスト
    [SerializeField]
    private TextMeshProUGUI _quitText;  // ゲーム終了のテキスト
    [SerializeField]
    private Color _selectColor;         // 選択されているときの色
    [SerializeField]
    private Color _defaultColor;        // 通常時の色

    private int _selectIndex;           // 選択されている番号
    private float _timer;               // タイマー

    private void Start()
    {
        _statText.color = _selectColor;
        _selectIndex = 0;
        _timer = 0.0f;
    }

    private void Update()
    {
        // 入力の結果を保持
        InputData input = InputHandler.GetInput();
        _timer += Time.deltaTime;

        // テキストの色変え
        SelectColor(input);

        // 決定ボタンを押されたら
        if (input.DecidePressed)
        {
            // 選択された番号が0番目だったら
            if (_selectIndex == 0)
            {
                // インゲームに飛ぶ
                SceneManager.LoadScene("Billiard");
            }
            else
            {
                // ゲームプレイ終了
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();//ゲームプレイ終了
#endif
            }
        }

    }

    // 選択されたテキストの色を変える
    private void SelectColor(InputData input)
    {
        // 待ち時間分経過していなかったら判定をしない
        if(_timer < _waitTime)
        {
            return;
        }

        // 下を押されたら
        if (input.DownPressed)
        {
            // 現在選択されているものが０番目だったら
            if (_selectIndex == 0)
            {
                _quitText.color = _selectColor;
                _statText.color = _defaultColor;
                _selectIndex = 1;
            }
            else
            {
                _statText.color = _selectColor;
                _quitText.color = _defaultColor;
                _selectIndex = 0;
            }
        }
        // 上を押されたら
        else if (input.UpPressed)
        {
            // 現在選択されているものが０番目だったら
            if (_selectIndex == 0)
            {
                _quitText.color = _selectColor;
                _statText.color = _defaultColor;
                _selectIndex = 1;
            }
            else
            {
                _statText.color = _selectColor;
                _quitText.color = _defaultColor;
                _selectIndex = 0;
            }
        }

        _timer = 0f;
    }
}
