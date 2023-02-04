using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{

    public RowUI rowUI;
    public ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {

        // TEST VALUES
        //scoreManager.scores.Add(new Score("Johnson", 23));
        //scoreManager.scores.Add(new Score("Immanuel", 25));
        //scoreManager.SortScores();

        refreshUI();
    }

    public void refreshUI() {

        scoreManager.refreshScore();
        List<Score> scores = scoreManager.scores;

        int i = 0;

        foreach (Score score in scores)
        {
            RowUI row = Instantiate(rowUI, transform).GetComponent<RowUI>();


            //Debug.Log(row.PlName.text);

            row.PlName.text = score.PlayerName;
            row.rank.text = i.ToString();
            row.score.text = score.score.ToString();

        }


    }

   
}
