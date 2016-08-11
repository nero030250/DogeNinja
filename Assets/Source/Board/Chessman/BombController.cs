using UnityEngine;
using System.Collections;

public class BombController : EnemyController {
	protected override void Awake () {
		base.Awake ();
		Type = ChessmanType.Bomb;
		Direction = MoveDirection.None;
	}

	public override void OnNinjaComeIn (DogeNinjaController ninja) {
		SetStatus (EnumStatus.Dead);
		if (!ninja.IsInvincible) {
			ninja.SetStatus (DogeNinjaStatus.Dead);
		}
	}
}