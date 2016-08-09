using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class FileUtility {
	private const string CONFIG_PATH = "Config";

	public static string GetConfigPath () {
		return Application.dataPath + "/../" + CONFIG_PATH ;
	}

	private static string GetFullPath (string dir, string configFile) {
		if (!Directory.Exists (dir))
			Directory.CreateDirectory (dir);
		string fullPath = string.Format ("{0}/{1}", dir, configFile);
		return fullPath;
	}

	// 没有就创建新的
	public static string ReadConfigFile (string configFile) {
		string fullPath = GetFullPath (GetConfigPath (), configFile);
		FileStream stream = File.Open (fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		stream.Close ();
		return File.ReadAllText (fullPath);
	}

	public static void WriteConfigFile (string configFile, string content) {
		string fullPath = GetFullPath (GetConfigPath (), configFile);
		File.WriteAllText (fullPath, content);
	}
}