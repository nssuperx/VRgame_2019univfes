using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
 
public class ReStart : MonoBehaviour {
 
	//　トータル制限時間
	private float totalTime;
	//　制限時間（分）
	[SerializeField]
	private int minute;
	//　制限時間（秒）
	[SerializeField]
	private float seconds;
	//　前回Update時の秒数
	private float oldSeconds;
 
	void Start () {
		totalTime = minute * 60 + seconds;
		oldSeconds = 0f;
	}
 
	void Update () {
		//　制限時間が0秒以下なら何もしない
		if (totalTime <= 1f) {
			return;
		}
		//　一旦トータルの制限時間を計測；
		totalTime = minute * 60 + seconds;
		totalTime -= Time.deltaTime;
 
		//　再設定
		minute = (int) totalTime / 60;
		seconds = totalTime - minute * 60;
 
		oldSeconds = seconds;
		//　制限時間以下になったら結果発表を行う
		if(totalTime <= 1f) {
			SceneManager.LoadScene("level selection");
			Destroy(GameObject.Find("Plane"));
		}
	}
}
 