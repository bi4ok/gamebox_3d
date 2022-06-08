using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class BonusController : MonoBehaviour
{
    [SerializeField]
    private string pathToScoreFile;

    [SerializeField]
    private float bombScoreCoef;

    [SerializeField]
    private float bonusScoreValue;

    private float playerScore;

    private void Start()
    {
        playerScore = 0;
    }

    public void PlayerScoresUp()
    {
        playerScore += bonusScoreValue;
    }

    public void PlayerScoresUp(float monsterScore, string damageFrom)
    {
        if (damageFrom == "Bullet")
        {
            playerScore += monsterScore;
        }
        else if (damageFrom == "Bonus")
        {
            playerScore += monsterScore * bombScoreCoef;
        }
        
    }

    public void WriteString(string info)
    {
        string path = Application.dataPath + "/" + pathToScoreFile;
        if (!File.Exists(path))
        {
            Debug.Log(path + " does not exist.");
            return;
        }
        
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(info);
        Debug.Log($"Запись сделана {info}");
        writer.Close();
    }

    public void WriteScoreRowInTable(string playerName)
    {
        var newScore = $"{playerName} - {playerScore}";
        WriteString(newScore);
    }

    public float GetCurrentScore()
    {
        return playerScore;
    }
}
