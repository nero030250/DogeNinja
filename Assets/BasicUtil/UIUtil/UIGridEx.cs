using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (UIGrid))]
public class UIGridEx : MonoBehaviour{

	private UIGrid _grid;
	public UIGrid Grid {
		get {
			if (_grid == null)
				_grid = gameObject.GetComponent <UIGrid> ();
			return _grid;
		}
	}
	public GameObject itemPrefab;
	public UIScrollView scrollView;

	public SystemDelegate.OnItemInitilaze OnInitializeItem;
	private List<GameObject> itemList = new List<GameObject> ();

	public int Count;

	public void Awake () {
		Grid.hideInactive = true;
	}

	public void Resize (int count, bool repostion = false) {
		Count = count;
		for (int index = 0; index < count; index ++) {
			if (itemList.Count <= index) {
				GameObject obj = NGUITools.AddChild (gameObject, itemPrefab);
				obj.name = index.ToString ("D2");
				itemList.Add (obj);
			}
			itemList [index].SetActive (true);
			if (OnInitializeItem != null)
				OnInitializeItem (itemList [index], index);
		}
		itemPrefab.SetActive (false);
		for (int index = count; index < itemList.Count; index ++)
			itemList [index].SetActive (false);
		if (repostion) {
			Grid.Reposition ();
			if (scrollView != null)
				scrollView.ResetPosition ();
		}
	}

	public T GetItem <T> (int index) where T : MonoBehaviour {
		if (index < itemList.Count)
			return itemList[index].GetComponent<T> ();
		return null;		
	}

	public List<T> GetItems <T> (SystemDelegate.BoolIsTarget<T> condition) where T : MonoBehaviour {
		List<T> list = new List<T> ();
		foreach (GameObject t in itemList) {
			if (!t.activeInHierarchy)
				continue;
			T tScrpit = t.GetComponent<T> ();
			if (condition (tScrpit))
				list.Add (tScrpit);
		}
		return list;
	}

	public T GetItem <T> (SystemDelegate.BoolIsTarget<T> condition) where T : MonoBehaviour {
		foreach (GameObject t in itemList) {
			if (!t.activeInHierarchy)
				continue;
			T tScrpit = t.GetComponent<T> ();
			if (condition (tScrpit))
				return tScrpit;
		}
		return null;
	}

	[ContextMenu ("PosTest")]
	public void PosTest () {
		if (itemList != null) {
			for (int index = 0; index < itemList.Count; index++)
				DestroyImmediate (itemList [index]);
			itemList.Clear ();
		}
		Resize (Count, true);
	}
}