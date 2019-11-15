using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour {

    // スコアを表示する
    public Text scoreText;
    // ハイスコアを表示する
    public Text highScoreText;

    // スコア
    private int score;

    public int Score{
        get{return score;}
        private set{score = value;}
    }

    // ハイスコア
    private int highScore;

    public int HighScore{
        get{return highScore;}
        private set{highScore = value;}
    }

    // PlayerPrefsで保存するためのキー
    private string highScoreKey = "highScore";

    void Start ()
    {
        Initialize ();
        DontDestroyOnLoad(this);
    }

    void Update ()
    {
        // スコアがハイスコアより大きければ
        if (highScore < score) 
        {
            highScore = score;
        }

        // スコア・ハイスコアを表示する
        scoreText.text = "Score:" + score.ToString ();
        highScoreText.text = "HighScore:" + highScore.ToString ();
    }

    // ゲーム開始前の状態に戻す
    private void Initialize ()
    {
        // スコアを0に戻す
        score = 0;

        // ハイスコアを取得する。保存されてなければ0を取得する。
        highScore = PlayerPrefs.GetInt (highScoreKey, 0);
    }

    // ポイントの追加
    public void AddPoint (int point)
    {
        score = score + point;
    }

    // ハイスコアの保存
    public void Save ()
    {
        // ハイスコアを保存する
        PlayerPrefs.SetInt (highScoreKey, highScore);
        PlayerPrefs.Save ();

        // ゲーム開始前の状態に戻す
        Initialize ();
    }

    void OnCollisionEnter(Collision collision) 
    {
    //衝突判定
    if (collision.gameObject.tag == "core") 
        {
            
            //スコア処理を追加
            FindObjectOfType<GameScore>().AddPoint(20);

        }
    }
}