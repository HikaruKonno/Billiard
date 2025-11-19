using UnityEngine;

public class AddScoreTextManager : MonoBehaviour
{
    //------------------------------------------------------------------------------------
    // メンバ変数
    //------------------------------------------------------------------------------------
    #region Field
    [SerializeField]
    private Transform _addScoreTextCanvas;  // 点数加算演出のテキストを映すためのキャンバス(親オブジェクト)
    [SerializeField]
    private GameObject _addScoreEffectText; // 点数加算演出のテキスト
    #endregion

    //------------------------------------------------------------------------------------
    // Public関数
    //------------------------------------------------------------------------------------
    #region PublicFunction
    /// <summary>
    /// 点数加算演出のテキストを受け渡された点数で出す関数
    /// 引数：ボール同士が衝突したときのボールの位置 、点数の入り方 、加算される点数 
    /// </summary>
    /// <param name="_point"></param>
    public void AddScoreEffect(Vector3 _position, int _point, string _addScoreType)
    {
        // Canvas の子オブジェクトとしてインスタンス化する
        GameObject _addScoreTextObj = Instantiate(_addScoreEffectText, Vector3.zero, Quaternion.identity, _addScoreTextCanvas);

        // テキストに引数を受け渡す 
        AddScoreText _addScoreText = _addScoreTextObj.GetComponent<AddScoreText>();
        _addScoreText.SetTextValue(_position, _point,_addScoreType);
    }
    #endregion
}
