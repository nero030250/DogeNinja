using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;

public class TexturePackerWindow : EditorWindow {
	[MenuItem ("Build/TexturePacker %&t", false, 10)]
	public static void ShowWindow () {
		EditorWindow.GetWindow (typeof(TexturePackerWindow), false, "图集打包");
	}

	private string atlasName = "main";
	void OnGUI () {
		atlasName = EditorGUILayout.TextField ("图集文件夹", atlasName);
		if (GUILayout.Button ("开始打包", GUILayout.Width (200))) {
			Excute ();
		}
	}

	void Excute () {
		string path = string.Format ("{0}/../Res/TexturePackerBat.bat", Application.dataPath);
		Process process = new Process ();
		ProcessStartInfo startInfo = new ProcessStartInfo (path, atlasName);
		startInfo.UseShellExecute = false;
		startInfo.CreateNoWindow = false;
		startInfo.RedirectStandardInput = true;
		startInfo.RedirectStandardOutput = true;
		process.StartInfo = startInfo;
		Process.Start (path, atlasName);
	}
}