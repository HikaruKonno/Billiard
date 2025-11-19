using UnityEngine;

public class BilliardCue : MonoBehaviour
{
    //------------------------------------------------------------------------------------
    // メンバ変数
    //------------------------------------------------------------------------------------
    #region Field
    private Rigidbody _rb;
    private Vector3 _velocity;
    #endregion

    //------------------------------------------------------------------------------------
    // Unityライフサイクル
    //------------------------------------------------------------------------------------
    #region LifeCycle

    // 最初の１フレームに処理される関数
    private void Start()
    {
        // Rigidbodyを取得
        MyDebug.Assert(TryGetComponent<Rigidbody>(out _rb));
    }

    // 当たった時の処理
    private void OnTriggerEnter(Collider other)
    {
        // PlayerBallに当たったら
        if (other.TryGetComponent<PlayerBall>(out var ball))
        {
            MyDebug.Assert(other.TryGetComponent<Rigidbody>(out var rb));
            Player player = GameManager.Instance.CurrentPlayer;
            var shoot = player.GetPlayerOperate().GetBallShooter();
            shoot.ShootBall(ball.BallMove.Rb);          // スピードを渡す
            player.IsShoot = true;                      // PlayerBallを打ったフラグを true に
            ball.IsStop = false;                        // PlayerBallの止まっているフラグをfalseにする
            _rb.Sleep();                                // 物理挙動を停止
            _rb.isKinematic = true;                     // 物理演算の影響をなくす
            gameObject.SetActive(false);                // このオブジェクトを非アクティブにする
        }
    }
    #endregion

    //------------------------------------------------------------------------------------
    // Private関数
    //------------------------------------------------------------------------------------
    #region PrivateFunction

    #endregion
}
