using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardPanelController : UISingleton <BoardPanelController> {
	private const string UI_PATH = "Prefab/BoardPanel";

	public static void Init () {
		if (Instance)
			Destroy (Instance.gameObject);
		Instance = null;
		UIHelper.Create (MainPanelController.Instance.BoardAnchor, UI_PATH);
	}

	public int GridWidth = 141;
	public int GridHeight = 138;

	public DogeNinjaController DogeNinja { get; private set; }
	private GameObject enemyItem;

	private List<EnemyController> enemyList = new List<EnemyController> ();

	public bool IsOver = false;
	private bool isInRound = false;

	protected override void Awake () {
		base.Awake ();
		DogeNinja = GetChildComponent <DogeNinjaController> ("DogeNinja");
		enemyItem = GetCtrl ("EnemyItem");
		enemyItem.SetActive (false);

		InputM.Instance.OnMouseDown += OnBirthPlaceSelect;
		InputM.Instance.OnDirection += OnDirection;
	}

	void Start () {
		DogeNinja.Hide ();
		MainPanelController.Instance.Round = 0;
		MainPanelController.Instance.Score = 0;
		nextKeyRound = 0;
		RollEnemy ();
	}

	protected override void OnDestroy () {
		base.OnDestroy ();
		if (InputM.Instance != null) {
			InputM.Instance.OnMouseDown -= OnBirthPlaceSelect;
			InputM.Instance.OnDirection -= OnDirection;
		}
	}

	public void AddEnemy (EnemyController enemy) {
		enemyList.Add (enemy);
	}

	public void RemoveEnemy (EnemyController enemy) {
		enemyList.Remove (enemy);
		roundMoveEnemy.Remove (enemy);
	}

	void OnBirthPlaceSelect (Vector3 worldPos) {
		if (IsOver || isInRound || !DogeNinja.IsHide)
			return;
		Vector3 localPosition = transform.InverseTransformPoint (worldPos);
		Vector3 roundPosition = RoundPosition (localPosition);
		if (IsInBoard (roundPosition) && GetEnemy (roundPosition) == null)
			DogeNinja.SetPosition (roundPosition);
	}

	private Vector2 RoundPosition (Vector3 position) {
		int x = Mathf.RoundToInt (position.x / GridWidth);
		int y = Mathf.RoundToInt (position.y / GridHeight);
		return new Vector2 (x, y);
	}

	public Vector3 TransFromBoardPos (Vector2 boardPos) {
		return new Vector3 (boardPos.x * GridWidth, boardPos.y * GridHeight, 0);
	}

	void OnDirection (MoveDirection dir) {
		if (IsOver || isInRound)
			return;
		if (DogeNinja.IsHide)
			return;
		if (dir == MoveDirection.None) {
			DogeNinja.Hide ();
			return;
		}
		DogeNinja.SetMoveDirection (dir);
		RoundStart ();
	}

	private int nextKeyRound;
	private int firstWeightCount;
	private Dictionary <ChessmanType, int> firstWeight = new Dictionary<ChessmanType, int> ();
	private int secondWeightCount;
	private Dictionary <ChessmanType, int> secoudWeight = new Dictionary<ChessmanType, int> ();
	private void RefreshWeight () {
		firstWeight = BoardManager.Instance.GetWeight (MainPanelController.Instance.Round, 1);
		firstWeightCount = 0;
		foreach (int weight in firstWeight.Values)
			firstWeightCount += weight;
		
		secoudWeight = BoardManager.Instance.GetWeight (MainPanelController.Instance.Round, 2);
		secondWeightCount = 0;
		foreach (int weight in secoudWeight.Values)
			secondWeightCount += weight;
	}

	private void RollEnemy () {
		if (nextKeyRound == MainPanelController.Instance.Round)
			RefreshWeight ();
		List<Vector2> freePos = new List<Vector2> ();
		for (int x = -2; x <= 2; x++) {
			for (int y = -2; y <= 2; y++)
				freePos.Add (new Vector2 (x, y));
		}
		foreach (EnemyController enemy in enemyList) {
			if (enemy.IsAlive)
				freePos.Remove (enemy.BoardPos);
		}
		if (freePos.Count <= 2) {
			// 结束
			return;
		}
		int posRound = Random.Range (0, freePos.Count);
		CreateEnemy (freePos [posRound], GetRoundType (firstWeightCount, firstWeight));
		freePos.RemoveAt (posRound);
		posRound = Random.Range (0, freePos.Count);
		CreateEnemy (freePos [posRound], GetRoundType (secondWeightCount, secoudWeight));
	}

	private ChessmanType GetRoundType (int count, Dictionary <ChessmanType, int> weights) {
		int round = Random.Range (0, count);
		int weightSum = 0;
		foreach (ChessmanType type in weights.Keys) {
			weightSum += weights [type];
			if (weightSum > round)
				return type;
		}
		return ChessmanType.Empty;
	}
		
	public EnemyController CreateEnemy (Vector2 boardPos, ChessmanType type) {
		Debug.LogWarning (string.Format ("Create {0} at {1}", type, boardPos));
		GameObject obj = NGUITools.AddChild (gameObject, enemyItem);
		obj.name = type.ToString ();
		obj.SetActive (true);
		EnemyController ctrl = EnemyController.AddEnemyController (obj, type);
		ctrl.SetPosition (boardPos);
		return ctrl;
	}

	private int BOARD_BOTTOM = -2;
	private int BOARD_TOP = 2;
	private int BOARD_LEFT = -2;
	private int BOARD_RIGHT = 2;

	public bool IsInBoard (Vector2 pos) {
		return BOARD_LEFT <= pos.x && pos.x <= BOARD_RIGHT && BOARD_BOTTOM <= pos.y && pos.y <= BOARD_TOP;
	}

	#region                                      回合逻辑

	private int preRoundScore;
	private int roundScore;
	// 当前回合可移动棋子, 每回合开始刷新, 每步之后都在减少
	private List<EnemyController> roundMoveEnemy = new List<EnemyController> ();
	private void RoundStart () {
		MainPanelController.Instance.Round++;
		preRoundScore = MainPanelController.Instance.Score;
		roundScore = 1;
		roundMoveEnemy.Clear ();
		roundMoveEnemy.AddRange (enemyList);
		roundMoveEnemy.RemoveAll ((chessman) => chessman.Direction == MoveDirection.None);

		isInRound = true;
		StepStart ();
	}

	private int chessmanMoveCount;
	private void StepStart () {
		if (IsOver) {
			RoundOver ();
			return;
		}
		// 被占用坐标
		List<Vector2> occupiedPos = new List<Vector2> ();
		// 该回合不可移动的所有怪物
		foreach (EnemyController enemy in enemyList) {
			if (!roundMoveEnemy.Contains (enemy) && enemy.IsAlive)
				occupiedPos.Add (enemy.BoardPos);
		}
		bool isBreak = false;
		// 通过循环删除之前可动, 当前陷入不可动状态的棋子, 如果当前循环没有剔除新的棋子, 就表示剩下的棋子都是可动的, 开始下一步
		while (!isBreak) {
			isBreak = true;
			for (int index = roundMoveEnemy.Count - 1; index >= 0; index--) {
				EnemyController enemy = roundMoveEnemy [index];
				Vector2 nextPos = enemy.CalcNextPosition ();
				// 下个位置出界
				// 下个位置已经被占用
				// 怪物带盾且下个位置被主角占用
				bool moveable = true;
				if (!IsInBoard (nextPos)
				    || occupiedPos.Contains (nextPos)
				    || (nextPos == DogeNinja.CalcTrueNextPosition () && (enemy.Type == ChessmanType.Bomb_Shield))) {
					moveable = false;
				}
				EnemyController other = GetEnemy (nextPos);
				// 处理相邻的穿过BUG
				if (other != null && other.CalcNextPosition () == enemy.BoardPos)
					moveable = false;
				if (!moveable) {
					enemy.IsMoveable = false;
					occupiedPos.Add (enemy.BoardPos);
					roundMoveEnemy.RemoveAt (index);
					isBreak = false;
				}
			}
		}
		roundMoveEnemy.Sort ((x, y) => {
			if (x.BoardPos.x != y.BoardPos.x)
				return (int)(x.BoardPos.y - y.BoardPos.y);
			return (int)(y.BoardPos.x - x.BoardPos.x);
		});

		for (int index = roundMoveEnemy.Count - 1; index >= 0; index--) {
			EnemyController enemy = roundMoveEnemy [index];
			Vector2 nextPos = enemy.CalcNextPosition ();
			if (occupiedPos.Contains (nextPos)) {
				occupiedPos.Add (enemy.BoardPos);
				enemy.IsMoveable = false;
				roundMoveEnemy.RemoveAt (index);
			} else
				occupiedPos.Add (nextPos);
		}

		if (roundMoveEnemy.Count > 0 || DogeNinja.IsMoveable) {
			chessmanMoveCount = roundMoveEnemy.Count + 1;
			foreach (EnemyController enemy in roundMoveEnemy)
				enemy.Move ();
			DogeNinja.Move ();
		} else
			RoundOver ();
	}

	public void OnChessmanMoveOver () {
		chessmanMoveCount--;
		if (chessmanMoveCount == 0)
			StepOver ();
	}

	private void StepOver () {
		List <EnemyController> stepEnemyList = new List<EnemyController> ();
		stepEnemyList.AddRange (enemyList);
		foreach (EnemyController enemy in stepEnemyList)
			enemy.RefreshStatus ();
		DogeNinja.RefreshStatus ();
		foreach (EnemyController enemy in stepEnemyList)
			enemy.Billing ();
		DogeNinja.Billing ();

		StepStart ();
	}

	private void RoundOver () {
		if (IsOver) {
			ResultPanelController.Create (MainPanelController.Instance.Score);
			return;
		}

		foreach (EnemyController enemy in enemyList)
			enemy.Clear ();
		DogeNinja.Clear ();
		isInRound = false;
		if (!DogeNinja.IsAlive) {
		} else {
			RollEnemy ();
		}
	}

	public EnemyController GetEnemy (Vector2 boardPos) {
		foreach (EnemyController other in enemyList) {
			if (other.BoardPos == boardPos)
				return other;
		}
		return null;
	}

	public EnemyController GetOtherEnemy (EnemyController enemy) {
		foreach (EnemyController other in enemyList) {
			if (other != enemy && other.BoardPos == enemy.BoardPos)
				return other;
		}
		return null;
	}

	public void AddScore (int score) {
		roundScore *= score;
		MainPanelController.Instance.Score = preRoundScore + roundScore;
	}

	#endregion
}