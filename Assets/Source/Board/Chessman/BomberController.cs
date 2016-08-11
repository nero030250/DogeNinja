using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class BomberController : HalfMoveMonsterController {
	protected override void Awake () {
		Type = ChessmanType.Bomber;
		base.Awake ();
	}

	private Vector2 lastPosition;

	public override void Move () {
		lastPosition = BoardPos;
		base.Move ();
	}

	protected override void OnHalfStep () {
		if (BoardPanelController.Instance.StepTimes == 0)
			BoardPanelController.Instance.CreateEnemy (lastPosition, ChessmanType.Bomb);
	}

	public override void OnNinjaComeIn (DogeNinjaController ninja) {
		SetStatus (EnumStatus.Dead);
		ninja.SetStatus (DogeNinjaStatus.Invincible);
	}
}