using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class HalfMoveMonsterController : MoveMonsterController {

	public override void Move () {
		Vector2 pos = CalcNextPosition ();
		DogeNinjaController ninja = BoardPanelController.Instance.DogeNinja;

		// 不会在半路对撞
		if (ninja.BoardPos != pos || MoveDirectionHelper.GetReverseDirection (ninja.Direction) != Direction) {
			Sequence seq = new Sequence ();
			seq.Append (HOTween.To (transform, MOVE_DURATION / 2, new TweenParms ()
				.Prop ("localPosition", (BoardPanelController.Instance.TransFromBoardPos (pos) + transform.localPosition) / 2)
				.OnComplete (OnHalfStep)));
			seq.Append (HOTween.To (transform, MOVE_DURATION / 2, new TweenParms ()
				.Prop ("localPosition", BoardPanelController.Instance.TransFromBoardPos (pos))
				.OnComplete (() => OnMoveCompleted ())));
			seq.PlayForward ();
			BoardPos = pos;
		} else {
			Sequence seq = new Sequence ();
			seq.Append (HOTween.To (transform, MOVE_DURATION / 2, new TweenParms ()
				.Prop ("localPosition", (BoardPanelController.Instance.TransFromBoardPos (pos) + transform.localPosition) / 2)
				.OnComplete (() => {
				OnHalfStep ();
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

	protected virtual void OnHalfStep () {}
}