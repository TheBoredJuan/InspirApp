using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultCalculation : MonoBehaviour
{
    private int vitalCapacityGoalPercentage;
    private float maxCapacityGoal;
    private MovementScript movementScript;

    public TMP_Text seriesAndRepsText;
    public TMP_Text percentageText;
    public TMP_Text goalVitalCapacityText;
    public TMP_Text maxCapacityObtainedText;

    //Graph section

    public TMP_Text seriesText;
    public Button continueButton;
    private List<GameObject> gameObjects = new List<GameObject>();
    private int serieNumber = 1;

    void Start()
    {
        movementScript = FindObjectOfType<MovementScript>();
        vitalCapacityGoalPercentage = GameData.patientPrescription.vitalCapacityGoal;
        maxCapacityGoal = GameData.patient.vitalCapacity * (float)GameData.patientPrescription.vitalCapacityGoal / 100;
    }

    public void GetResults(int numSeries, int numReps) {
        seriesAndRepsText.text = "Número de Series: " + numSeries + "\nNúmero de Repeticiones: " + numReps;
        percentageText.text = "Porcentaje de capacidad vital objetivo: " + vitalCapacityGoalPercentage + "%";
        goalVitalCapacityText.text = "Capacidad vital objetivo: " + maxCapacityGoal + "ml";
        maxCapacityObtainedText.text = "Máxima capacidad obtenida: " + (movementScript.maxMl + 1100) + "ml";
    }

    public void GetGraphResults() {
        if (serieNumber <= GameData.exercise.SeriesNum)
        {
            continueButton.GetComponentInChildren<TMP_Text>().text = (serieNumber == GameData.exercise.SeriesNum) ? "Volver" : "Continuar";
            GetGraphResults(serieNumber);
            serieNumber++;
        }
        else {
            SceneManagerUtil.LoadScene(0);
        }
    }
    private void GetGraphResults(int serieNumber) {
        foreach (GameObject gameObject in gameObjects) {
            Destroy(gameObject);
        }
        if (serieNumber <= GameData.exercise.SeriesNum) {
            gameObjects = movementScript.GetSerieGraph(serieNumber);
            seriesText.text = "Serie :" + serieNumber + "/" + GameData.exercise.SeriesNum;
        }
    }
}
