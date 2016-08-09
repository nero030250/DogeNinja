using UnityEngine;
using System.Collections;

public class MonsterController : EnemyController {
	protected override void Awake () {
		base.Awake ();
		Type = ChessmanType.Monster;
		Direction = MoveDirection.None;
	}

	public override void OnNinjaComeIn (DogeNinjaController ninja) {
		SetStatus (EnumStatus.Dead);
		ninja.SetStatus (DogeNinjaStatus.Invincible);
	}
}