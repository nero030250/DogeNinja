using UnityEngine;
using System.Collections;

public class ScoreController : UICollectController {

	private UISprite levelSpr;
	private UIGridEx scoreGrid;

	private string scoreStr;

	protected override void Awake () {
	}

	public void Init (int score, int level) {
		base.Awake ();
		levelSpr = GetSprite ("Level");
		scoreGrid = GetGridEx ("ScoreGrid");
		scoreGrid.OnInitializeItem = OnInitializeScoreItem;
		scoreStr = string.Format ("+{0}", score);
		scoreGrid.Resize (scoreStr.Length);
		if (level == 0)
			levelSpr.gameObject.SetActive (false);
		else if (level == 1)
			levelSpr.spriteName = "good";
		else if (level == 2)
			levelSpr.spriteName = "great";
		else
			levelSpr.spriteName = "perfect";
		gameObject.SetActive (true);
		foreach (UISprite sprite in gameObject.GetComponentsInChildren<UISprite> ())
			sprite.depth = 10 + level;
		GetComponent <UITable> ().Reposition ();
	}

	private void OnInitializeScoreItem (GameObject obj, int index) {
		obj.GetComponent<UISprite> ().spriteName = scoreStr [index].ToString ();
	}
}