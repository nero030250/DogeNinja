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
	public bool IsMoveable { get { return Direction != MoveDirection.None && BoardPanelController.Instance.IsInBoard (CalcNextPosition ()); } }
	public bool IsInvincible { get; private set; }

	private UISpriteAnimation animation;

	protected override void Awake () {
		base.Awake ();
		animation = performSprite.GetComponent <UISpriteAnimation> ();
	}

	public override void Clear () {
		IsInvincible = false;
		Direction = MoveDirection.None;
		status = DogeNinjaStatus.None;
		SetPerform ();
		Hide ();
	}

	public void Hide () {
		IsHide = true;
		widget.alpha = 0f;
	}

	public override void SetPosition (Vector2 pos) {
		IsHide = false;
		BoardPos = pos;
		transform.localPosition = BoardPanelController.Instance.TransFromBoardPos (BoardPos);
		widget.alpha = 0.5f;
	}

	public void SetMoveDirection (MoveDirection direction) {
		widget.alpha = 1f;
		Direction = direction;
		SetPerform ();
	}

	public bool isStepMove = false;
	public override void Move () {
		isStepMove = false;
		if (Direction == MoveDirection.None) {
			OnMoveCompleted ();
			return;
		}
		Vector2 pos = CalcNextPosition ();
		if (!BoardPanelController.Instance.IsInBoard (pos)) {
			OnMoveCompleted ();
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
			}
		}
	}

	protected override void SetPerform () {
		if (Direction == MoveDirection.None) {
			animation.enabled = false;
			performSprite.spriteName = "ninja_idle";
			performSprite.flip = UIBasicSprite.Flip.Nothing;
			performSprite.MakePixelPerfect ();
		} else if (Direction == MoveDirection.Left) {
			animation.enabled = true;
			animation.namePrefix = "ninja_r_";
			performSprite.flip = UIBasicSprite.Flip.Horizontally;
			animation.Play ();
		} else if (Direction == MoveDirection.Right) {
			animation.enabled = true;
			animation.namePrefix = "ninja_r_";
			performSprite.flip = UIBasicSprite.Flip.Nothing;
			animation.Play ();
		} else if (Direction == MoveDirection.Back) {
			animation.enabled = true;
			animation.namePrefix = "ninja_b_";
			performSprite.flip = UIBasicSprite.Flip.Nothing;
			animation.Play ();
		} else if (Direction == MoveDirection.Forward) {
			animation.enabled = true;
			animation.namePrefix = "ninja_f_";
			performSprite.flip = UIBasicSprite.Flip.Nothing;
			animation.Play ();
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
}