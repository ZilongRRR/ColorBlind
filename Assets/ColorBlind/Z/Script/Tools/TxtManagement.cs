using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class TxtSetting {
	public string TxtName;
	public List<string> TxtContent = new List<string> ();
}

public class TxtManagement : Z_MonoBase {
	public TxtSetting Z_Txt;
	public List<string> TxtContent = new List<string> ();

	public void ReadTxt () {
		TxtContent = ReadTxt (Z_Txt.TxtName);
	}

	public List<string> ReadTxt (string txtname) {
		List<string> contentlist = new List<string> ();
		StreamReader sr = new StreamReader ("Txt/" + txtname + ".txt");
		string content;
		while ((content = sr.ReadLine ()) != null) {
			contentlist.Add (content);
		}
		sr.Close ();
		return contentlist;
	}

	public void WriteTxt (string txtname, string[] data) {
		StreamWriter sr = new StreamWriter ("Txt/" + txtname + ".txt");
		foreach (string s in data) {
			sr.WriteLine (s);
		}
		sr.Close ();
	}

	public void WriteTxt (string txtname, List<string> data) {
		StreamWriter sr = new StreamWriter ("Txt/" + txtname + ".txt");
		foreach (string s in data) {
			sr.WriteLine (s);
		}
		sr.Close ();
	}
}