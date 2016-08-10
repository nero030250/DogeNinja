using UnityEngine;
using System.Collections;

public class ResultPanelController : UISingleton <ResultPanelController> {
	private const string UI_PATH = "Prefab/ResultPanel";

	public static void Create (int score) {
		GameObject obj = UIHelper.Create (UI_PATH);
		obj.GetComponent<ResultPanelController> ().Init (score);
	}

	private UILabel scoreLabel;
	private UIButton restartBtn;
	private GameObject bestObj;

	protected override void Awake () {
		base.Awake ();
		scoreLabel = GetLabel ("ScoreLabel");
		restartBtn = GetButton ("RestartBtn");
		bestObj = GetCtrl ("Best");

		EventDelegate.Add (restartBtn.onClick, OnRestartClick);
	}

	public void Init (int score) {
		scoreLabel.text = string.Format ("得分:{0}", score);
		bestObj.SetActive (score > BoardManager.Instance.BestScore);
		BoardManager.Instance.BestScore = score;
	}

	private void OnRestartClick () {
		Destroy (gameObject);
		MainPanelController.Instance.Init ();
	}
}