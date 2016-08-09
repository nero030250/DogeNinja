using UnityEngine;
using System.Collections;

public class SystemDelegate {
	public delegate void VoidDelegate ();
	public delegate void DirectionDelegate (MoveDirection dir);
	public delegate void OnItemInitilaze (GameObject obj, int index);
	public delegate bool BoolIsTarget (GameObject obj);
	public delegate bool BoolIsTarget<T> (T obj);
	public delegate void ColliderDelegate (Collider e);

	public delegate void PosDelegate (Vector3 pos);
}