using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class WeaponController : MoveMonsterController {
	protected override void Awake () {
		Type = ChessmanType.Weapon;
		base.Awake ();
	}

	public override void OnNinjaComeIn (DogeNinjaController ninja) {
		if (ninja.IsInvincible)
			SetStatus (EnumStatus.Dead);
		else {
			// 不移动了, 失去攻击性
			if (!IsMoveable) {
				SetStatus (EnumStatus.Dead);
				ninja.SetStatus (DogeNinjaStatus.Invincible);
			}
			else 
				ninja.SetStatus (DogeNinjaStatus.Dead);
		}
	}
}