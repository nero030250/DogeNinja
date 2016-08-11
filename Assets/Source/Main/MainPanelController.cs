using UnityEngine;
using System.Collections;

public class MainPanelController : UISingleton <MainPanelController> {

	private UILabel scoreLabel;
	private UILabel roundLabel;
	private UILabel bestLabel;

	private UIButton restartBtn;

	public GameObject BoardAnchor { get; private set; }

	private GameObject scoreObj;

	private int score;
	public int Score { get { return score; }
		set { Debug.LogWarning (value);
			score = value;
			scoreLabel.text = string.Format ("得分:{0}", score);
		}
	}

	private int round;
	public int Round {
		get { return round; }
		set {
			round = value;
			roundLabel.text = string.Format ("回合:{0}", round);
		}
	}

	protected override void Awake () {
		base.Awake ();
		scoreLabel = GetLabel ("ScoreLabel");
		roundLabel = GetLabel ("RoundLabel");
		bestLabel = GetLabel ("BestLabel");
		restartBtn = GetButton ("RestartBtn");

		EventDelegate.Add (restartBtn.onClick, Init);

		BoardAnchor = GetCtrl ("BoardAnchor");
		scoreObj = GetCtrl ("ScorePrefab");
	}

	void Start () {
		RefreshBest ();
	}

	private void RefreshBest () {
		bestLabel.text = string.Format ("最高:{0}", BoardManager.Instance.BestScore);
	}

	public void Init () {
		RefreshBest ();
		BoardPanelController.Init ();
	}

	public int AddScore (int score, int level) {
		int result = score * level;
		Score += result;
		return score;
	}

	public void ShowScore (int scoreCount, int level) {
		GameObject obj =NGUITools.AddChild (scoreObj.transform.parent.gameObject, scoreObj);
		obj.GetComponent<ScoreController> ().Init (score, level);
	}
}