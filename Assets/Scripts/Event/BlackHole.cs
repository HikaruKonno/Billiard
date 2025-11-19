using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BlackHole
{
    //------------------------------------------------------------------------------------
    // メンバ変数
    //------------------------------------------------------------------------------------
    #region Field
    [SerializeField] 
    private float _blackHoleTime;      // ブラックホールの出現時間
    [SerializeField] 
    private float _vacuumPower;        // 吸い込む力
    [SerializeField] 
    private float _explosionForce;     // 超新星爆発の際の力
    #endregion

    //------------------------------------------------------------------------------------
    // Public関数
    //------------------------------------------------------------------------------------
    #region PublicFunction

    /// <summary>
    /// ブラックホールに吸い込まれる関数 
    /// </summary>
    public void SuckedBlackHole(List<Planet> planets, GameObject blackHole)
    {
        for (int i = 0; i < planets.Count; i++)
        {
            // 落ちたボールは処理しない
            if (planets[i].IsFall == false)
            {

                Vector3 direction;                                      // 力を加える方向
                Vector3 _blackHolePos = blackHole.transform.position;   // ブラックホールの座標
                Vector3 thisPosition = planets[i].transform.position;   // 自分自身の位置情報を取得する

                // ブラックホール方向のベクトルを取得
                direction = _blackHolePos - thisPosition;
                // BlackHole の方向に対して力を加える
                planets[i].BallMove.Rb.velocity = direction * _vacuumPower;
            }
        }
    }

    /// <summary>
    /// 超新星爆発が起きる（大号令）
    /// </summary>
    public void Explosion(List<Planet> planets)
    {
        for(int i = 0; i < planets.Count; ++i)
        {
            // 落ちたボールは処理しない
            if (planets[i].IsFall == false)
            {
                // 力を加える方向ベクトル
                float X = Random.Range(-1.0f, 1.0f);
                float Z = Random.Range(-1.0f, 1.0f);
                Vector3 Vec3 = new Vector3(X, 0.0f, Z);

                // 力を加える
                planets[i].BallMove.Rb.velocity = Vec3 * _explosionForce;
            }
        }
    }
    #endregion
}
