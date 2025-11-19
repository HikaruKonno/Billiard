using UnityEngine;

public class PlayerBall : Planet
{
    //------------------------------------------------------------------------------------
    // Unityライフサイクル
    //------------------------------------------------------------------------------------
    #region LifeCycle
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pocket"))
        {
            base.OnTriggerEnter(other);                         // 共通の落ちた処理をする
            GameManager.Instance.CurrentPlayer.IsFoul = true;   // 現在のPlayerのファールフラグをtrueにする
        }
    }
    #endregion

    //------------------------------------------------------------------------------------
    // Public関数
    //------------------------------------------------------------------------------------
    #region PublicFunction
    /// <summary>
    /// PlayerBall の物理判定のアクティブをセットする関数
    /// </summary>
    /// <param name="isActive">trueだったら非アクティブ</param>
    public void SetActiveRigidbody(bool isActive)
    {
        // PlayerBall の物理判定を非アクティブにする
        BallMove.Rb.isKinematic = isActive;
        Sphere.isTrigger = isActive;
    }
    #endregion

    //------------------------------------------------------------------------------------
    // Protected関数
    //------------------------------------------------------------------------------------
    #region ProtectedFunction
    // 惑星の処理
    protected override void Ability(Collision collision)
    {
        // 最初に当たったのがMoonだったら処理
        if ((collision.GetType() == typeof(Moon)))
        {
            // プレイヤーを加速させる
            BallMove.Rb.velocity *= 2.0f;
            // Moon能力のフラグをfalseにする
            //EventManager.EventMng.AbilityFlag.Moon = false;
        }
    }
    #endregion
}
