using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class StartMenuUI : MonoBehaviour
{
    public static StartMenuUI saveScript;

    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI bestScoreText;

    public string playerName;
    public string displayPlayerName;
    public string bestPlayerNameAndScore;
    private GameObject gameManager;

    private void Awake()
    {
        if (saveScript == null)
        {
            saveScript = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (bestPlayerNameAndScore != null)
        {
            Debug.Log("Load name");
            LoadName();
            bestScoreText.text = bestPlayerNameAndScore;
        }
    }

    public void FindGameManager()
    {
        gameManager = GameObject.Find("MainManager");
        if (gameManager != null)
        {
            MainManager gameManagerScript = gameManager.GetComponent<MainManager>();
            bestPlayerNameAndScore = gameManagerScript.BestScoreText();
            SaveName();
        }
    }

    void StartNew()
    {
        playerName = inputField.text;
        SaveName();
        SceneManager.LoadScene(1);
    }

    void Exit()
    {
#if UNITY_EDITOR
EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    void LoadNameValueChange()
    {
        LoadName();
        inputField.text = playerName;
        
       
    }

    void LoadBestScore()
    {
        bestScoreText.text = "Best Score: " + playerName;
    }
    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public string bestPlayerNameAndScore;
    }

    public void SaveName()
    {
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.bestPlayerNameAndScore = bestPlayerNameAndScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savejson.json", json);
    }

    public void LoadName()
    {
        string path = Application.persistentDataPath + "/savejson.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerName = data.playerName;
            bestPlayerNameAndScore = data.bestPlayerNameAndScore;
        }
    }
}
