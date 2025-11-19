using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    private void Start()
    {
        if(GameManager.Instance.GetWinnerPlayer() == null)
        {
            _text.text = "Draw";
            return;
        }

        _text.text = GameManager.Instance.GetWinnerPlayer().PlayerNumber.ToString() + "\n Winner";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
            Application.Quit();//ゲームプレイ終了
#endif
        }
        else if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Title");
        }
        
    }
}
