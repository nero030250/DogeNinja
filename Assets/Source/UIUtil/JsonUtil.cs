using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public class JsonValue {
	private Dictionary <string, object> values = new Dictionary<string, object> ();

	public JsonValue () {}

	public JsonValue (string str) {
		values = JsonConvert.DeserializeObject <Dictionary<string, object>> (str);
	}

	public int GetInt (string key) {
		return JsonConvert.DeserializeObject<int> (values [key].ToString ());
	}

	public List<int> GetIntList (string key) {
		return JsonConvert.DeserializeObject<List<int>> (values [key].ToString ());
	}

	public T Get <T> (string key) where T : class {
		return values [key] as T;
	}

	public List<T> GetList <T> (string key) where T : class {
		return JsonConvert.DeserializeObject<List<T>> (values [key].ToString ());
	}

//	public void Add (string key, int value) {
//		values.Add (key, value);
//	}
//
//	public void Add (string key, double value) {
//		values.Add (key, value);
//	}
//
//	public void Add (string key, string value) {
//		values.Add (key, value);
//	}
//
//	public void Add <T> (string key, List<T> value) {
//		values.Add (key, value);
//	}

	public void Add (string key, object value) {
		values.Add (key, value);
	}

	public string ToJson  () {
		return JsonConvert.SerializeObject (values);
	}

	public override string ToString () {
		return JsonConvert.SerializeObject (values);
	}
}