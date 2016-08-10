using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class ChessmanController : UICollectController {

	public static float MOVE_DURATION = 0.2f;

	public ChessmanType Type { get; protected set; }
	protected MoveDirection direction;
	public MoveDirection Direction {
		get { return direction; }
		protected set {
			direction = value;
			SetPerform ();
		}
	}

	public Vector2 BoardPos { get; protected set; }

	public bool IsAlive { get; protected set; }

	protected UISprite performSprite;
	protected UIWidget widget;

	protected override void Awake () {
		base.Awake ();
		performSprite = GetSprite ("PerformSprite");
		widget = GetComponent <UIWidget> ();
		IsAlive = true;
	}

	public virtual void Clear () {}

	public virtual void SetPosition (Vector2 pos) {
		BoardPos = pos;
		transform.localPosition = BoardPanelController.Instance.TransFromBoardPos (BoardPos);
	}

	public Vector2 CalcNextPosition () {
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
		return BoardPos + movePos;
	}

	public virtual void Move () {
	}

	protected virtual void OnMoveCompleted () {
		BoardPanelController.Instance.OnChessmanMoveOver ();
	}

	// 全体移动结束后 (StepOver)调用, 根据重叠格子刷新状态
	public virtual void RefreshStatus () {}

	// 全体RefreshStatus之后调用, 结算当前Step
	public virtual void Billing () {}

	protected virtual void SetPerform () {
		if (Direction == MoveDirection.None) {
			performSprite.spriteName = Type.ToString ().ToLower ();
			performSprite.flip = UIBasicSprite.Flip.Nothing;
		} else if (Direction == MoveDirection.Left) {
			performSprite.spriteName = string.Format ("{0}_r", Type).ToLower ();
			performSprite.flip = UIBasicSprite.Flip.Horizontally;
		} else if (Direction == MoveDirection.Right) {
			performSprite.spriteName = string.Format ("{0}_r", Type).ToLower ();
			performSprite.flip = UIBasicSprite.Flip.Nothing;
		} else if (Direction == MoveDirection.Back) {
			performSprite.spriteName = string.Format ("{0}_b", Type).ToLower ();
			performSprite.flip = UIBasicSprite.Flip.Nothing;
		} else if (Direction == MoveDirection.Forward) {
			performSprite.spriteName = string.Format ("{0}_f", Type).ToLower ();
			performSprite.flip = UIBasicSprite.Flip.Nothing;
		}
	}
}