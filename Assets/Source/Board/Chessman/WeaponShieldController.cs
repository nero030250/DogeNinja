using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class WeaponShieldController : MoveMonsterController {
	protected override void Awake () {
		Type = ChessmanType.Weapon_Shield;
		LowerType = ChessmanType.Weapon;
		base.Awake ();
	}

	public override void OnNinjaComeIn (DogeNinjaController ninja) {
		if (ninja.IsInvincible)
			SetStatus (EnumStatus.Dead);
		else {
			// 不移动了, 失去攻击性
			if (!IsMoveable) {
				SetStatus (EnumStatus.Low);
				ninja.SetStatus (DogeNinjaStatus.StopMove);
			}
			else 
				ninja.SetStatus (DogeNinjaStatus.Dead);
		}
	}

	public override bool StopNinja (DogeNinjaController ninja) {
		return !ninja.IsInvincible;
	}
}