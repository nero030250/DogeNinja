using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class MoveMonsterController : EnemyController {
	
	protected override void Awake () {
		base.Awake ();
		Direction = (MoveDirection)Random.Range (1, 5);
	}

	public override void Clear () {
		base.Clear ();
		Direction = MoveDirectionHelper.GetReverseDirection (Direction);
	}

	public override void Move () {
		Vector2 pos = CalcNextPosition ();
		DogeNinjaController ninja = BoardPanelController.Instance.DogeNinja;
		// 不会在半路对撞
		if (ninja.BoardPos != pos || MoveDirectionHelper.GetReverseDirection (ninja.Direction) != Direction) {
			HOTween.To (transform, MOVE_DURATION, new TweenParms ().Prop ("localPosition", BoardPanelController.Instance.TransFromBoardPos (pos)).OnComplete (() => OnMoveCompleted ()));
			BoardPos = pos;
		} else {
			Sequence seq = new Sequence ();
			seq.Append (HOTween.To (transform, MOVE_DURATION / 2, new TweenParms ()
				.Prop ("localPosition", (BoardPanelController.Instance.TransFromBoardPos (pos) + transform.localPosition) / 2)
				.OnComplete (() => {
				OnNinjaComeIn (ninja);
				if (status != EnumStatus.Dead)
					BoardPos = pos;
			})));
			seq.Append (HOTween.To (transform, MOVE_DURATION / 2, new TweenParms ()
				.Prop ("localPosition", BoardPanelController.Instance.TransFromBoardPos (pos))
				.OnComplete (() => OnMoveCompleted ())));
			seq.PlayForward ();
		}
	}
}