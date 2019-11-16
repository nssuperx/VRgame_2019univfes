using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour {

    // スコアを表示する
    public Text scoreText;
    // ハイスコアを表示する
    public Text highScoreText;

    private int score;

    private int highScore;

    // PlayerPrefsで保存するためのキー
    private string highScoreKey = "highScore";

    void Start ()
    {
        score = GameObject.Find("Plane").GetComponent<GameScore>().Score;
        highScore = GameObject.Find("Plane").GetComponent<GameScore>().HighScore;
    }

    void Update ()
    {

        // スコア・ハイスコアを表示する
        scoreText.text = "Score:" + score.ToString ();
        highScoreText.text = "HighScore:" + highScore.ToString ();
    }

    private void Initialize ()
    {
        // スコアを0に戻す
        score = 0;

        // ハイスコアを取得する。保存されてなければ0を取得する。
        highScore = PlayerPrefs.GetInt (highScoreKey, 0);
    }

    public void Save ()
    {
        // ハイスコアを保存する
        PlayerPrefs.SetInt (highScoreKey, highScore);
        PlayerPrefs.Save ();

        // ゲーム開始前の状態に戻す
        Initialize ();
    }


}