using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BoardManager : UISingleton <BoardManager> {
	private const string CONFIG_PATH = "NinjaConfig";

	public List<int> KeyRoundList { get; private set; }
	private Dictionary<ChessmanType, ChessmanConfig> configDic = new Dictionary<ChessmanType, ChessmanConfig> ();

	public int BestScore {
		get { return PlayerPrefs.GetInt ("BestScore"); }
		set { 
			if (value > BestScore)
				PlayerPrefs.SetInt ("BestScore", value);
		}
	}

	protected override void Awake () {
		base.Awake ();
	}

	void Start () {
		Random.seed = (int)System.DateTime.UtcNow.Ticks;
		InitConfig ();
		MainPanelController.Instance.Init ();
	}

	private void InitConfig () {
		string content = ResourcesManager.Instance.ReadConfigFile (CONFIG_PATH);
		JsonValue json = new JsonValue (content);
		KeyRoundList = json.GetIntList ("KeyRound");
		List<ChessmanConfig> configList = json.GetList <ChessmanConfig> ("ChessmanNode");
		foreach (ChessmanConfig config in configList) {
			Debug.LogWarning (config.Type);
			configDic.Add (config.Type, config);
		}
	}

	public ChessmanConfig GetConfig (ChessmanType type) {
		return configDic [type];
	}

	public List<ChessmanConfig> GetNodeConfigs () {
		return configDic.Values.ToList ();
	}

	public int NextKeyRound (int round) {
		int result = 0;
		foreach (int keyRound in KeyRoundList) {
			result = keyRound;
			if (result > round)
				break;
		}
		return result;
	}

	public Dictionary <ChessmanType, int> GetWeight (int round, int times) {
		Dictionary <ChessmanType, int> result = new Dictionary<ChessmanType, int> ();
		int index = KeyRoundList.IndexOf (round);
		if (times == 1) {
			foreach (ChessmanConfig config in configDic.Values) {
				int weight = config.FirstWeight [Mathf.Min (index, config.FirstWeight.Count - 1)];
				if (weight != 0)
					result.Add (config.Type, weight);
			}
		} else if (times == 2) {
			foreach (ChessmanConfig config in configDic.Values) {
				int weight = config.SecondWeight [Mathf.Min (index, config.SecondWeight.Count - 1)];
				if (weight != 0)
					result.Add (config.Type, weight);
			}
		}
		return result;
	}

	public int GetScore (ChessmanType type, ChessmanType lowType = ChessmanType.Empty) {
		int score = GetConfig (type).Score;
		if (lowType == ChessmanType.Empty)
			return score;
		return score - GetConfig (lowType).Score;
	}
}