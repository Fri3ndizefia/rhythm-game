using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Play music
    public AudioSource theMusic;

    public bool startPlaying;

    public BeatScroller theBS;

    public static GameManager instance;

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    // Score prize ...
    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    public int combo;

    public Text scoreText;
    public Text multiText;
    public Text comboText;

    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missHits;

    public GameObject resultScreen;
    public Text percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        scoreText.text = "Score: 0";
        currentMultiplier = 1;
        comboText.text = "Combo: 0";

        totalNotes = FindObjectsOfType<NoteObject>().Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                theBS.hasStarted = true;

                theMusic.Play();
            }
        }
        else
        {
            if (!theMusic.isPlaying && !resultScreen.activeInHierarchy)
            {
                resultScreen.SetActive(true);

                normalsText.text = normalHits.ToString();
                goodsText.text = goodHits.ToString();
                perfectsText.text = perfectHits.ToString();
                missesText.text = missHits.ToString();

                float totalHit = normalHits + goodHits + perfectHits;
                float percentHit = (totalHit / totalNotes) * 100f;

                percentHitText.text = percentHit.ToString("F2") + "%"; // float data type with one decimal.

                string rankVal = "F";

                if (percentHit == 100)
                {
                    rankVal = "Full Combo";
                }else if (percentHit > 98)
                {
                    rankVal = "EX+";
                }
                else if (percentHit > 95)
                {
                    rankVal = "EX";
                }
                else if (percentHit > 90)
                {
                    rankVal = "A";
                }
                else if (percentHit > 80)
                {
                    rankVal = "B";
                }
                else if (percentHit > 60)
                {
                    rankVal = "C";
                }else if (percentHit > 40)
                {
                    rankVal = "D";
                }

                rankText.text = rankVal;

                finalScoreText.text = currentScore.ToString();
            }
        }
    }
    public void NoteHit()
    {
        Debug.Log("Hit On Time");

        if(currentMultiplier -1 < multiplierThresholds.Length)
        {
            multiplierTracker++;

            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }

        multiText.text = "Multiplier: x" + currentMultiplier;

        //currentScore += scorePerNote * currentMultiplier;
        scoreText.text = "Score: " + currentScore;
        
        combo ++;
        comboText.text = "Combo: " + combo;
    }

    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();

        normalHits++;
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();

        goodHits++;
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();

        perfectHits++;
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");
        currentMultiplier = 1;
        multiplierTracker = 0;
        multiText.text = "Multiplier: x" + currentMultiplier;

        combo = 0;
        comboText.text = "Combo: 0";

        missHits++;
    }
}
