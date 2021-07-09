using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    
    private static float ONE_SECOND = 1;
    // Delay time between the end of a serie and the rest message
    private static int REST_DELAY_TIME = 3;
    // Show time of the instructions message
    private static int INSTRUCTIONS_SHOW_TIME = 3;
    // Rest time
    public static int REST_TIME = 10;

    public GameObject obstacle;
    public GameObject restText;
    public GameObject pivots;
    public TMP_Text restTimeText;
    public TMP_Text seriesText;
    public TMP_Text repsText;

    private float timer = 0;
    private float exerciseTimeCounter = -1;
    private int actualSerie = 1;
    private int actualRep = 1;
    private bool serieInCourse = true;
    private bool isInDelayTime = false;
    private bool isStartingExercise = true;
    private int restDelayCounter = 0;
    private int instructionsTimeCounter = 0;
    private int timeWaited = 0;

    public GameObject resultMenu;
    private ResultCalculation resultCalculation;

    public GameObject windowGraphGameObject;
    private WindowGraph windowGraph;
    private List<int> valueListPrescriptionExercise = new List<int>();

    public GameObject player;
    private MovementScript movementScript;
    private int serieCount = 1;

    private bool firstObstacle = true;

    private void Start()
    {
        resultCalculation = FindObjectOfType<ResultCalculation>();
        windowGraph = FindObjectOfType<WindowGraph>();
        movementScript = FindObjectOfType<MovementScript>();
        setSeriesText(actualSerie, GameData.exercise.SeriesNum);
        setRepsText(actualRep, GameData.exercise.RepsNum);
        pivots.SetActive(false);
        StartCoroutine(LateStart());
        StartCoroutine(GetValueListPrescription());        
    }

    IEnumerator LateStart() {
        yield return new WaitForSeconds(0.02f);
        windowGraphGameObject.SetActive(false);
    }

    IEnumerator GetValueListPrescription() {
        IExercise exercise = GameData.exercise;
        //Fist value, time = 0
        valueListPrescriptionExercise.Add((int)exercise.GetPositionToSpawn(0));
        for (int i = 0; i < exercise.RepsNum; i++)
        {
            for (int j = 1; j <= exercise.ExercisePeriod; j++)
            {
                valueListPrescriptionExercise.Add((int)exercise.GetPositionToSpawn(j));
                yield return null;
            }
        }
    }
    void Update()
    {
        if (timer > ONE_SECOND) {
            timer = 0;
            if (isStartingExercise) {
                //If the exercise is starting it shows the initial message to avoid obstacles
                instructionsTimeCounter++;
                if (instructionsTimeCounter > INSTRUCTIONS_SHOW_TIME)
                {
                    pivots.SetActive(true);
                    isStartingExercise = false;
                }
            }
            else if (serieInCourse)
            {
                GameObject newObstacle = Instantiate(obstacle);
                exerciseTimeCounter = (exerciseTimeCounter + 1) % GameData.exercise.ExercisePeriod;
                newObstacle.transform.position = transform.position + new Vector3(0, GameData.GetAbsolutPositionFromMl(GameData.exercise.GetPositionToSpawn(exerciseTimeCounter)), 0);
                Destroy(newObstacle, 3.5f);
                timer = 0;
                if (firstObstacle) {
                    firstObstacle = false;
                } 
                else if (exerciseTimeCounter == 0)
                {
                    actualRep++;
                    setRepsText(actualRep, GameData.exercise.RepsNum);
                    if (actualRep > GameData.exercise.RepsNum)
                    {
                        isInDelayTime = true;
                        serieInCourse = false;
                    }
                }
            }
            else if (isInDelayTime) {
                restDelayCounter++;
                if (restDelayCounter > REST_DELAY_TIME) {
                    restDelayCounter = 0;
                    actualSerie++;
                    actualRep = 1;
                    setSeriesText(actualSerie, GameData.exercise.SeriesNum);
                    setRepsText(actualRep, GameData.exercise.RepsNum);
                    if (actualSerie > GameData.exercise.SeriesNum) getExerciseResults();
                    else showWaitMenu(true);
                    isInDelayTime = false;
                }
            }
            else
            {
                timeWaited++;
                getWaitedTime(timeWaited);
                if (timeWaited > REST_TIME)
                {
                    showWaitMenu(false);
                    exerciseTimeCounter = -1;
                    firstObstacle = true;
                    serieInCourse = true;
                    timeWaited = 0;
                }
            }
            
        }
        timer += Time.deltaTime;
    }
    private void showWaitMenu(bool isActive) {
        pivots.SetActive(!isActive);
        restText.SetActive(isActive);
        //Get the graph for the actual exercise
        windowGraphGameObject.SetActive(isActive);
        if (isActive) {
            windowGraph.ShowGraph(valueListPrescriptionExercise, WindowGraph.EXERCISE_PRESCRIPTION_SERIE_COLOR);
            movementScript.GetPlayerGraph(serieCount);
            serieCount++;
        }

        restTimeText.text = REST_TIME.ToString();
    }

    private void getWaitedTime(int timeWaited) {
        restTimeText.text = (REST_TIME - timeWaited).ToString();
    }

    private void setSeriesText(int actualSerie, int totalSeries)
    {
        if (actualSerie <= totalSeries) {
            seriesText.text = "Serie:" + actualSerie + "/" + totalSeries;
        }
    }
    private void setRepsText(int actualRep, int totalReps)
    {
        if (actualRep <= totalReps) {
            repsText.text = "Reps:" + actualRep + "/" + totalReps;
        }
    }

    private void getExerciseResults() {
        Time.timeScale = 0f;
        actualSerie = 1;
        resultMenu.SetActive(true);
        movementScript.SavePlayerGraph(serieCount);
        windowGraph.ShowGraph(valueListPrescriptionExercise, WindowGraph.EXERCISE_PRESCRIPTION_SERIE_COLOR);
        resultCalculation.GetResults(GameData.exercise.SeriesNum, GameData.exercise.RepsNum);
    }
}
