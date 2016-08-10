using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public enum DogeNinjaStatus {
	None,
	StopMove,
	Invincible,
	Dead,
}

public class DogeNinjaController : ChessmanController {

	public bool IsHide { get; private set; }
	public bool IsInvincible { get; private set; }

	private UISpriteAnimation spriteAnimation;

	protected override void Awake () {
		base.Awake ();
		IsMoveable = true;
		spriteAnimation = performSprite.GetComponent <UISpriteAnimation> ();
	}

	public override void Clear () {
		IsInvincible = false;
		IsMoveable = true;
		Direction = MoveDirection.None;
		status = DogeNinjaStatus.None;
		PlayHideAnimation ();
	}

	public void Hide () {
		widget.alpha = 0f;
		IsHide = true;
	}

	public override void SetPosition (Vector2 pos) {
		IsHide = false;
		BoardPos = pos;
		transform.localPosition = BoardPanelController.Instance.TransFromBoardPos (BoardPos);
		widget.alpha = 0.5f;
		SetSpriteDepth ();
		SetPerform ();
	}

	public void SetMoveDirection (MoveDirection direction) {
		widget.alpha = 1f;
		Direction = direction;
		SetPerform ();
	}

	private void MoveStop () {
		IsMoveable = false;
		IsInvincible = false;
		status = DogeNinjaStatus.None;
		SetPerform ();
		OnMoveCompleted ();
	}

	public bool isStepMove = false;
	public override void Move () {
		isStepMove = false;
		if (Direction == MoveDirection.None) {
			MoveStop ();
			return;
		}
		Vector2 pos = CalcNextPosition ();
		if (!BoardPanelController.Instance.IsInBoard (pos)) {
			MoveStop ();
		} else {
			isStepMove = true;
			EnemyController ctrl = BoardPanelController.Instance.GetEnemy (pos);
			// 逻辑上比较特殊, 因为要提前判定
			if ( ctrl != null && ctrl.StopNinja (this)) {
				Sequence seq = new Sequence (new SequenceParms ().OnComplete (() => {
					ctrl.OnNinjaComeIn (this);
					OnMoveCompleted ();
				}));
				seq.Append (HOTween.To (transform, MOVE_DURATION / 2, new TweenParms ().Prop ("localPosition", (BoardPanelController.Instance.TransFromBoardPos (pos) + transform.localPosition) / 2)));
				seq.Append (HOTween.To (transform, MOVE_DURATION / 2, new TweenParms ().Prop ("localPosition", transform.localPosition)));
				seq.PlayForward ();
			} else {
				HOTween.To (transform, MOVE_DURATION, new TweenParms ().Prop ("localPosition", BoardPanelController.Instance.TransFromBoardPos (pos)).Ease (EaseType.EaseOutQuad).OnComplete (() => OnMoveCompleted ()));
				BoardPos = pos;
				SetSpriteDepth ();
			}
		}
	}

	protected override void SetPerform () {
		spriteAnimation.framesPerSecond = 5;
		spriteAnimation.loop = true;
		if (Direction == MoveDirection.None || !IsMoveable) {
			spriteAnimation.enabled = false;
			performSprite.spriteName = "ninja_idle";
			performSprite.flip = UIBasicSprite.Flip.Nothing;
			performSprite.MakePixelPerfect ();
		} else if (Direction == MoveDirection.Left) {
			spriteAnimation.enabled = true;
			spriteAnimation.namePrefix = "ninja_r_";
			performSprite.flip = UIBasicSprite.Flip.Horizontally;
			spriteAnimation.Play ();
		} else if (Direction == MoveDirection.Right) {
			spriteAnimation.enabled = true;
			spriteAnimation.namePrefix = "ninja_r_";
			performSprite.flip = UIBasicSprite.Flip.Nothing;
			spriteAnimation.Play ();
		} else if (Direction == MoveDirection.Back) {
			spriteAnimation.enabled = true;
			spriteAnimation.namePrefix = "ninja_b_";
			performSprite.flip = UIBasicSprite.Flip.Nothing;
			spriteAnimation.Play ();
		} else if (Direction == MoveDirection.Forward) {
			spriteAnimation.enabled = true;
			spriteAnimation.namePrefix = "ninja_f_";
			performSprite.flip = UIBasicSprite.Flip.Nothing;
			spriteAnimation.Play ();
		}
	}

	// 状态有优先级
	private DogeNinjaStatus status; 
	public void SetStatus (DogeNinjaStatus status) {
		if (this.status == DogeNinjaStatus.Invincible)
			return;
		if (status == DogeNinjaStatus.Dead)
			this.status = DogeNinjaStatus.Dead;
		else 
			this.status = status;
		if (status != DogeNinjaStatus.Invincible)
			IsMoveable = false;
	}

	// 由EnemyController.OnNinjaComeIn 修改状态
	public override void RefreshStatus () {
		base.RefreshStatus ();
	}

	public override void Billing () {
		if (status == DogeNinjaStatus.Dead) {
			BoardPanelController.Instance.IsOver = true;
			IsAlive = false;
			Hide ();
		} else if (status == DogeNinjaStatus.Invincible) {
			IsInvincible = true;
		} else if (status == DogeNinjaStatus.StopMove) {
			Direction = MoveDirection.None;
		}
	}

	public Vector2 CalcTrueNextPosition () {
		Vector2 movePos = Vector2.zero;
		switch (Direction) {
		case MoveDirection.Left:
			movePos = Vector2.left;
			break;
		case MoveDirection.Right:
			movePos = Vector2.right;
			break;
		case MoveDirection.Forward:
			movePos = Vector2.down;
			break;
		case MoveDirection.Back:
			movePos = Vector2.up;
			break;
		}
		if (!BoardPanelController.Instance.IsInBoard (BoardPos + movePos))
			return BoardPos;
		EnemyController enemy = BoardPanelController.Instance.GetEnemy (BoardPos + movePos);
		if (enemy == null || enemy.IsMoveable || (enemy.Type != ChessmanType.Shield && enemy.Type != ChessmanType.Bomb_Shield && enemy.Type != ChessmanType.Weapon_Shield))
			return BoardPos + movePos;
		return BoardPos;
	}

	public void PlayHideAnimation () {
		IsHide = true;
		spriteAnimation.enabled = true;
		spriteAnimation.loop = false;
		spriteAnimation.framesPerSecond = 30;
		spriteAnimation.namePrefix = "ninja_hide";
		spriteAnimation.Play ();
	}
}