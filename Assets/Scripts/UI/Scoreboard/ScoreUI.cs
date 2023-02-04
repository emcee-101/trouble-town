using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreUI : MonoBehaviour
{

    public RowUI rowUI;
    public ScoreManager scoreManager;
    private List<RowUI> rowsInstantiatet { get; } = new List<RowUI>();

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

        foreach (RowUI entry in rowsInstantiatet)
        {

            Destroy(entry.gameObject);


        }
        rowsInstantiatet.Clear();

        foreach (Score score in scores)
        {

            //add objects anew to make sure the board is up to date
            RowUI currentRow = Instantiate(rowUI, transform).GetComponent<RowUI>();

           
            //insert UI Values
            currentRow.PlName.text = score.PlayerName;
            currentRow.rank.text = i.ToString();
            currentRow.score.text = score.score.ToString();

            rowsInstantiatet.Add(currentRow);

        }


    }

   
}
