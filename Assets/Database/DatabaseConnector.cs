using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* ------- NOT FULLY IMPLEMENTED ------- 
 * At the moment only local database connection works.
 * 
 * and only data downloading and not uploading
 * 
 */
public class DatabaseConnector : MonoBehaviour 
{
	public string[] values;
	private IEnumerator coroutine;
	private WWW connection;
	private Text leveldatatext, namedatatext, timedatatext;

	private void Start()
	{
	}

	public void GetReferences()
	{
		leveldatatext = GameObject.Find("TextLevelValues").GetComponent<Text>();
		namedatatext = GameObject.Find("TextNameValues").GetComponent<Text>();
		timedatatext = GameObject.Find("TextTimeValues").GetComponent<Text>();
	}

	public void StartConnection()
	{
		StartCoroutine(Connector());
	}

	private IEnumerator Connector()
	{
		connection = new WWW("http://localhost/index.php");
		yield return connection;
		PostData();
	}

	private void PostData()
	{
		for (int i = 0; i < 3; i++)
		{
			leveldatatext.text += ConvertData(GetData(i), "level") + "\n";
			namedatatext.text += ConvertData(GetData(i), "name") + "\n";
			timedatatext.text += ConvertData(GetData(i), "time") + "\n";
		}
		Debug.Log(ConvertData(GetData(0), "name") + " (" + ConvertData(GetData(0), "level") + "): " + ConvertData(GetData(0), "time"));
	}

	private string GetData(int index)
	{
		string data = connection.text;
		values = data.Split(';');

		return values[index];
	}


	private string ConvertData(string data, string index)
	{
		string value = data.Substring(data.IndexOf(index) + index.Length);
		if (value.Contains("|"))
			value = value.Remove(value.IndexOf("|"));
		return value;
	}
}
