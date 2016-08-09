using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class WeaponShieldController : MoveMonsterController {
	protected override void Awake () {
		Type = ChessmanType.Weapon_Shield;
		LowerType = ChessmanType.Weapon;
		Attack = true;
		base.Awake ();
	}

	public override void OnNinjaComeIn (DogeNinjaController ninja) {
		if (ninja.IsInvincible)
			SetStatus (EnumStatus.Dead);
		else {
			// 表示 ninja是从背后捅的刀子
			if (ninja.Direction == direction && ninja.isStepMove)
				SetStatus (EnumStatus.Low);
			else 
				ninja.SetStatus (DogeNinjaStatus.Dead);
		}
	}
}