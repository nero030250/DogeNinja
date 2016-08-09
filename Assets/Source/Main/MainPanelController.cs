using UnityEngine;
using System.Collections;

public class MainPanelController : UISingleton <MainPanelController> {

	private UILabel scoreLabel;
	private UILabel roundLabel;

	private UIButton restartBtn;

	public GameObject BoardAnchor { get; private set; }

	private int score;
	public int Score { get { return score; }
		set {
			score = value;
			scoreLabel.text = score.ToString ();
		}
	}

	private int round;
	public int Round {
		get { return round; }
		set {
			round = value;
			roundLabel.text = round.ToString ();
		}
	}

	protected override void Awake () {
		base.Awake ();
		scoreLabel = GetLabel ("ScoreLabel");
		roundLabel = GetLabel ("RoundLabel");
		restartBtn = GetButton ("RestartBtn");

		EventDelegate.Add (restartBtn.onClick, Init);

		BoardAnchor = GetCtrl ("BoardAnchor");
	}

	public void Init () {
		BoardPanelController.Init ();
	}
}