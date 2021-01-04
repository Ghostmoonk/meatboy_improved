using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keybind : MonoBehaviour
{

    private Dictionary<string, KeyCode> keys;

    [SerializeField] TextStringBinds[] textBinds;
    Dictionary<string, Text> textBindsDico;

    [SerializeField] Text left, right, jump;
    private string currentKeyName = "";

    // Start is called before the first frame update
    void Start()
    {
        keys = new Dictionary<string, KeyCode>();
        textBindsDico = new Dictionary<string, Text>();
        keys.Add("Gauche", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Gauche", "Q")));
        keys.Add("Droite", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Droite", "D")));
        keys.Add("Saut", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Saut", "Space")));

        left.text = keys["Gauche"].ToString();
        right.text = keys["Droite"].ToString();
        jump.text = keys["Saut"].ToString();

        foreach (TextStringBinds item in textBinds)
        {
            textBindsDico.Add(item.inputName, item.textObject);
        }
    }

    void OnGUI()
    {
        if (currentKeyName != "")
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keys[currentKeyName] = e.keyCode;
                Debug.Log(textBindsDico[currentKeyName].text);
                textBindsDico[currentKeyName].text = e.keyCode.ToString();
                Debug.Log(keys[currentKeyName]);
                currentKeyName = "";
            }
        }
    }

    public void ChangeKey(string clickedOn)
    {
        currentKeyName = clickedOn;
    }

    public void SaveKeys()
    {
        foreach (var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
        PlayerPrefs.Save();
    }

}

[System.Serializable]
class TextStringBinds
{
    public Text textObject;
    public string inputName;
}