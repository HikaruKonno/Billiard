using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DerectionGaugeControl : MonoBehaviour
{
    //------------------------------------------------------------------------------------
    // Unityライフサイクル
    //------------------------------------------------------------------------------------
    #region LifeCycle
    void Start()
    {
        BallShooter.DirectionUI = OnDirectionGaugeUI;
    }
    #endregion

    //------------------------------------------------------------------------------------
    // Public関数
    //------------------------------------------------------------------------------------
    #region PublicFunction
    /// <summary>
    /// 方向ゲージの角度を反映する
    /// </summary>
    public void OnDirectionGaugeUI(float _rotationZ)
    {
        transform.rotation = Quaternion.Euler(90f, 0f, _rotationZ);
    }
    #endregion
}
