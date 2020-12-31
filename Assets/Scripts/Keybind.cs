using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keybind : MonoBehaviour
{

    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public Text left, right, jump;
    private GameObject currentKey;

    // Start is called before the first frame update
    void Start()
    {
        keys.Add("Gauche", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Gauche", "Q")));
        keys.Add("Droite", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Droite", "D")));
        keys.Add("Saut", (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Saut", "Space")));

        left.text = keys["Gauche"].ToString();
        right.text = keys["Droite"].ToString();
        jump.text = keys["Saut"].ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if(currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                currentKey = null;
            }
        }
    }

    public void ChangeKey(GameObject clickedOn)
    {
        currentKey = clickedOn;
    }

    public void SaveKeys()
    {
        foreach(var key in keys)
        {
            PlayerPrefs.SetString(key.Key,key.Value.ToString());
        }
        PlayerPrefs.Save();
    }

}
