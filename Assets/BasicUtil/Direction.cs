using UnityEngine;
using System.Collections;

public enum MoveDirection {
	None = 0,
	Left = 1,
	Right = 2,
	Forward = 3,
	Back = 4,
}

public static class MoveDirectionHelper {
	public static MoveDirection GetDirection (Vector3 startPos, Vector3 endPos) {
		float xOffset = Mathf.Abs (endPos.x - startPos.x);
		float yOffset = Mathf.Abs (endPos.y - startPos.y);
		if (xOffset > yOffset) {
			if (endPos.x - startPos.x > InputM.Instance.MIN_MOVE)
				return MoveDirection.Right;
			if (startPos.x - endPos.x > InputM.Instance.MIN_MOVE)
				return MoveDirection.Left;
		} else {
			if (endPos.y - startPos.y > InputM.Instance.MIN_MOVE)
				return MoveDirection.Back;
			if (startPos.y - endPos.y > InputM.Instance.MIN_MOVE)
				return MoveDirection.Forward;
		}
		return MoveDirection.None;
	}

	public static MoveDirection GetReverseDirection (MoveDirection dir) {
		if (dir == MoveDirection.Left)
			return MoveDirection.Right;
		else if (dir == MoveDirection.Right)
			return MoveDirection.Left;
		else if (dir == MoveDirection.Back)
			return MoveDirection.Forward;
		else if (dir == MoveDirection.Forward)
			return MoveDirection.Back;
		return MoveDirection.None;
	}
}