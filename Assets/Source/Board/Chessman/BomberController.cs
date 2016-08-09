using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class BomberController : HalfMoveMonsterController {
	protected override void Awake () {
		Type = ChessmanType.Bomber;
		base.Awake ();
	}

	public override void Clear () {
		base.Clear ();
		stepTimes = 0;
	}

	private int stepTimes = 0;
	private Vector2 lastPosition;

	public override void Move () {
		lastPosition = BoardPos;
		base.Move ();
	}

	protected override void OnHalfStep () {
		if (stepTimes == 0)
			BoardPanelController.Instance.CreateEnemy (lastPosition, ChessmanType.Bomb);
		stepTimes++;
	}

	public override void OnNinjaComeIn (DogeNinjaController ninja) {
		SetStatus (EnumStatus.Dead);
		ninja.SetStatus (DogeNinjaStatus.Invincible);
	}
}