using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public enum EnumStatus {
	None,
	Dead,
	Low,
	Stop,
}

public class EnemyController : ChessmanController {

	public static EnemyController AddEnemyController (GameObject obj, ChessmanType type) {
		if (type == ChessmanType.Monster)
			obj.AddComponent <MonsterController> ();
		else if (type == ChessmanType.Bomb)
			obj.AddComponent <BombController> ();
		else if (type == ChessmanType.Bomber)
			obj.AddComponent<BomberController> ();
		else if (type == ChessmanType.Weapon)
			obj.AddComponent <WeaponController> ();
		else if (type == ChessmanType.Shield)
			obj.AddComponent <ShieldController> ();
		else if (type == ChessmanType.Bomb_Shield)
			obj.AddComponent <BombShieldController> ();
		else if (type == ChessmanType.Weapon_Shield)
			obj.AddComponent <WeaponShieldController> ();
		return obj.GetComponent <EnemyController> ();
	}

	public ChessmanType LowerType = ChessmanType.Empty;

	public override void Clear () {
		status = EnumStatus.None;
	}

	public override void RefreshStatus () {
		if (BoardPos == BoardPanelController.Instance.DogeNinja.BoardPos)
			OnNinjaComeIn (BoardPanelController.Instance.DogeNinja);
	}

	public override void Billing () {
		if (status == EnumStatus.Low && LowerType == ChessmanType.Empty)
			status = EnumStatus.Dead;
		if (status == EnumStatus.None)
			return;
		if (status == EnumStatus.Low)
			ToLow ();
		if (status == EnumStatus.Dead)
			KillSelf ();
	}

	protected virtual void ToLow () {
		KillSelf ();
		if (LowerType != ChessmanType.Empty) {
			EnemyController ctrl = BoardPanelController.Instance.CreateEnemy (BoardPos, LowerType);
			ctrl.Direction = Direction;
		}
	}

	protected virtual void KillSelf () {
		IsAlive = false;
		if (status == EnumStatus.Dead) {
			if (Type == ChessmanType.Bomb)
				SoundManager.Instance.Play ("Boom");
			else
				SoundManager.Instance.Play ("Kill");
			BoardPanelController.Instance.AddScore (BoardManager.Instance.GetScore (Type));
			BoardPanelController.Instance.PlayBoomEffect (BoardPos);
		}
		if (status == EnumStatus.Low)
			BoardPanelController.Instance.AddScore (BoardManager.Instance.GetScore (Type, LowerType));

		if (BoardPanelController.Instance != null)
			BoardPanelController.Instance.RemoveEnemy (this);
		Destroy (gameObject);
	}

	public virtual void OnNinjaComeIn (DogeNinjaController ninja) {}

	public virtual bool StopNinja (DogeNinjaController ninja) {
		return false;
	}

	protected EnumStatus status;
	public void SetStatus (EnumStatus status) {
		this.status = status;
		if (status == EnumStatus.Dead)
			widget.alpha = 0;
	}
}