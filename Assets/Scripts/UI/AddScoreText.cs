using TMPro;
using UnityEngine;

public class AddScoreText : MonoBehaviour
{
    // テキストのいろいろな値
    [System.Serializable]
    public struct TextValue
    {
        // 出現してから消えるまでの時間       
        public float _lostTime;
        // テキストが透明化していく速さ
        public float _lostSpeed;
        // テキストが浮くスピード
        public Vector3 _moveSpeed;
        // テキストを表示させる際に座標を調整させる値
        public Vector3 _offset;
        // テキストの色
        public Color _initialColor;
    }

    [SerializeField, Header("衝突時の加点")]
    private TextValue _conflict;
    [SerializeField, Header("金星の衝突時の加点")]
    private TextValue _conflictVenus;
    [SerializeField, Header("穴に入った時の加点")]
    private TextValue _holeIn;

    // 出現してから消えるまでの時間
    private float _lostTime;
    // テキストが透明化していく速さ
    private float _lostSpeed;
    // テキストが浮くスピード
    private Vector3 _moveSpeed;
    // テキストを表示させる際に座標を調整させる値
    private Vector3 _offset;
    // テキストの初期の色
    private Color _initialColor;

    // TextMeshPro 型のスクリプトを保持するための変数
    private TextMeshProUGUI _textMeshProUGUI;
    // 表記する点数
    private int _score;
    // 点数の入り方を保持するための変数
    private string _addAcoreType;
    // テキストのアルファ値
    private float _alphaValue;


    // 時間を計るためのタイマー
    private float _timer;

    void Start()
    {
        // 値の初期化
        this._timer = 0.0f;
        _alphaValue = 1.0f;

        // スクリプトの取得
        _textMeshProUGUI = this.GetComponent<TextMeshProUGUI>();

        // 点数をテキストに反映させる
        _textMeshProUGUI.text = "+" + _score.ToString();
        // テキストの座標を調整する
        this.transform.position += _offset;
        // テキストの色を設定する
        this._textMeshProUGUI.color = _initialColor;
    }

    void Update()
    {
        // 経過時間の測定
        this._timer += Time.deltaTime;
        // テキストのアルファ値を更新する
        this._textMeshProUGUI.color = new Color(_initialColor.r, _initialColor.g, _initialColor.b, _alphaValue);
        // テキストを少しずつ上に移動させる
        this.transform.position += _moveSpeed;


        // 指定した時間を過ぎるとどんどん透明になり消える
        if (_lostTime <= this._timer)
        {
            _alphaValue -= _lostSpeed;

            if (_alphaValue <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// テキストの座標や点数、テキストに関する値を設定する関数
    /// 引数：UIManager から受け取るワールド座標の位置、点数の入り方、点数 
    /// </summary>
    /// <param name="Score"></param>
    public void SetTextValue(Vector3 _position, int Score, string _addScoreType)
    {
        // RectTransform 型のコンポーネントを取得する
        RectTransform _rect = GetComponent<RectTransform>();
        // ワールド座標からスクリーンの座標に変換して座標を設定
        Vector3 screenPos = Camera.main.WorldToScreenPoint(_position);
        _rect.position = screenPos;

        // 点数の入り方で値を変える
        SetTextValueOfAddScoreType(_addScoreType);

        // スコアの値を設定
        _score = Score;
    }

    /// <summary>
    /// 点数の入り方によって値を変える
    /// 引数：点数の入り方
    /// </summary>
    private void SetTextValueOfAddScoreType(string _addScoreType)
    {
        // 惑星同氏が衝突していれば
        if (_addScoreType == "Conflict")
        {
            // 衝突用の値をセットする
            _lostTime = _conflict._lostTime;
            _lostSpeed = _conflict._lostSpeed;
            _moveSpeed = _conflict._moveSpeed;
            _offset = _conflict._offset;
            _initialColor = _conflict._initialColor;
        }

        // 金星が衝突していたら
        if (_addScoreType == "ConflictVenus")
        {
            // 金星用の値をセットする
            _lostTime = _conflictVenus._lostTime;
            _lostSpeed = _conflictVenus._lostSpeed;
            _moveSpeed = _conflictVenus._moveSpeed;
            _offset = _conflictVenus._offset;
            _initialColor = _conflictVenus._initialColor;
        }

        // 惑星が穴に入ったら
        if (_addScoreType == "HoleIn")
        {
            // ホールイン用の値をセットする
            _lostTime = _holeIn._lostTime;
            _lostSpeed = _holeIn._lostSpeed;
            _moveSpeed = _holeIn._moveSpeed;
            _offset = _holeIn._offset;
            _initialColor = _holeIn._initialColor;
        }
    }
}
