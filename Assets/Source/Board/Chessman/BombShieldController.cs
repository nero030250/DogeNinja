using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class BombShieldController : HalfMoveMonsterController {
	protected override void Awake () {
		Type = ChessmanType.Bomb_Shield;
		LowerType = ChessmanType.Bomber;
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