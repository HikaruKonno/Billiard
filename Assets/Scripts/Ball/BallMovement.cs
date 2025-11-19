using UnityEngine;

[System.Serializable]
public class BallMovement
{
    //------------------------------------------------------
    // ビルダー
    //------------------------------------------------------
    #region Builder

    public class Builder
    {
        public Builder() { }

        public Planet planet { get; private set; }
        public LayerMask groundMask { get; private set; }
        public float rayLength { get; private set; }

        public BallMovement Build()
        {
            return new BallMovement(this);
        }
        public Builder SetPlanet(Planet planet)
        {
            this.planet = planet;
            return this;
        }
        public Builder SetLayer(LayerMask layer)
        {
            groundMask = layer;
            return this;
        }
        public Builder SetRayLength(float length)
        {
            rayLength = length;
            return this;
        }
    }

    #endregion

    //------------------------------------------------------------------------------------
    // メンバ変数
    //------------------------------------------------------------------------------------
    #region Field

    private Planet _planet;                 // 惑星クラス
    private Rigidbody _rb;                  // 物理クラス
    private LayerMask _groundMask;          // 地面のレイヤー
    private float _rayLength;               // 飛ばすレイの長さ
    private const float _stopSpeed = 3f;    // スピードの下限

    #endregion

    //------------------------------------------------------------------------------------
    // プロパティ
    //------------------------------------------------------------------------------------
    #region Property

    public Rigidbody Rb { get => _rb; set => _rb = value; }

    #endregion

    //------------------------------------------------------------------------------------
    // コンストラクタ
    //------------------------------------------------------------------------------------
    public BallMovement(Builder builder)
    {
        _planet = builder.planet;
        _rayLength = builder.rayLength;
        _groundMask = builder.groundMask;
    }

    //------------------------------------------------------------------------------------
    // Public関数
    //------------------------------------------------------------------------------------
    #region PublicFunction

    /// <summary>
    /// ボールが止まっているか判断する
    /// </summary>
    /// <returns>止まっていたらtrue</returns>
    public bool TryStop()
    {
        MyDebug.Assert(_rb != null);
        MyDebug.Assert(_planet != null);

        // 地面に接してなかったら止まってない判定を返す
        if (!Physics.Raycast(_planet.transform.position, Vector3.down, out _, _rayLength, _groundMask))
        {
            return false;
        }

        // 指定したスピードを下回ったら止める
        if (_rb.velocity.magnitude < _stopSpeed)
        {
            // ボールを止める
            _rb.Sleep();

            //ボールが止まっているか念のため確認  
            if (_rb.IsSleeping())
            {
                return true;
            }
        }

        return false;
    }
    #endregion
}
