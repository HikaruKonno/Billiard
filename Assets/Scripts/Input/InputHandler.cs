#define DEBUG
using UnityEngine;

// 入力の内容を設定
public static class InputHandler
{
    public static InputData GetInput()
    {
        return new InputData
        {
            DecidePressed   = Input.GetKeyDown(KeyCode.Space),
            MenuPressed     = Input.GetKeyDown(KeyCode.Escape),
            RightPressed    = ( (Input.GetKey(KeyCode.RightArrow))) || (Input.GetKey(KeyCode.D) ),
            LeftPressed     = ( (Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)) ),
            UpPressed       = ( (Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.UpArrow)) ),
            DownPressed     = ( (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.DownArrow)) ),
            AnyPressed      = Input.anyKey
        };
    }
}

