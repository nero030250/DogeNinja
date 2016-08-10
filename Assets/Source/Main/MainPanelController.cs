using UnityEngine;
using System.Collections;

public class MainPanelController : UISingleton <MainPanelController> {

	private UILabel scoreLabel;
	private UILabel roundLabel;

	private UIButton restartBtn;

	public GameObject BoardAnchor { get; private set; }

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
		restartBtn = GetButton ("RestartBtn");

		EventDelegate.Add (restartBtn.onClick, Init);

		BoardAnchor = GetCtrl ("BoardAnchor");
	}

	public void Init () {
		BoardPanelController.Init ();
	}
}