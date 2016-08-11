using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class BombShieldController : HalfMoveMonsterController {
	protected override void Awake () {
		Type = ChessmanType.Bomb_Shield;
		LowerType = ChessmanType.Bomber;
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
		if (ninja.IsInvincible)
			SetStatus (EnumStatus.Dead);
		else {
			ninja.SetStatus (DogeNinjaStatus.StopMove);
			SetStatus (EnumStatus.Low);
		}
	}

	public override bool StopNinja (DogeNinjaController ninja) {
		return !ninja.IsInvincible;
	}
}