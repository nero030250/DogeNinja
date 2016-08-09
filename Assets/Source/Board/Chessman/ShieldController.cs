using UnityEngine;
using System.Collections;

public class ShieldController : EnemyController {
	protected override void Awake () {
		base.Awake ();
		Type = ChessmanType.Shield;
		Direction = MoveDirection.None;
		LowerType = ChessmanType.Monster;
	}

	public override void OnNinjaComeIn (DogeNinjaController ninja) {
		Debug.LogWarning ("OnNinjaCome");
		if (ninja.IsInvincible)
			SetStatus (EnumStatus.Dead);
		else {
			SetStatus (EnumStatus.Low);
			ninja.SetStatus (DogeNinjaStatus.StopMove);
		}
	}

	public override bool StopNinja (DogeNinjaController ninja) {
		return !ninja.IsInvincible;
	}
}