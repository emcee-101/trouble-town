using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreEndUI : MonoBehaviour
{

    public RowEndUI rowUI;
    public ScoreEndManager scoreManager;
    private List<RowEndUI> rowsInstantiatet { get; } = new List<RowEndUI>();

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
        List<ScoreEnd> scores = scoreManager.scores;

        int i = 0;

        foreach (RowEndUI entry in rowsInstantiatet)
        {

            Destroy(entry.gameObject);


        }
        rowsInstantiatet.Clear();

        foreach (ScoreEnd score in scores)
        {
            i++;

            //add objects anew to make sure the board is up to date
            RowEndUI currentRow = Instantiate(rowUI, transform).GetComponent<RowEndUI>();

           
            //insert UI Values
            currentRow.PlName.text = score.PlayerName;
            currentRow.rank.text = i.ToString();
            currentRow.score.text = score.score.ToString();

            rowsInstantiatet.Add(currentRow);

        }


    }

   
}
