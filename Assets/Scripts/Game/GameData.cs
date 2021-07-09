using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static readonly float ROOF_POSITION = 0.8f;
    public static readonly float FLOOR_POSITION = -0.8f;

    public static PatientModel patient;
    public static PrescriptionModel patientPrescription;
    private ExerciseModel exerciseModel;
    public static float maxCapacity;
    public static float minCapacity = 1100;
    private static float slopeConstant;
    public static IExercise exercise;
    void Start()
    {
        slopeConstant = (maxCapacity - minCapacity) / (ROOF_POSITION - FLOOR_POSITION);
    }
    private void OnEnable()
    {
        retrivePatientInfo();
    }
    private void retrivePatientInfo()
    {
        patient = JsonUtility.FromJson<PatientModel>(PlayerPrefs.GetString("patient"));
        patientPrescription = JsonUtility.FromJson<PrescriptionModel>(PlayerPrefs.GetString("prescription"));
        int exerciseSelected = PlayerPrefs.GetInt("exerciseType");
        if (exerciseSelected != 0)
        {
            foreach (ExerciseModel exerciseModel in patientPrescription.exercises)
            {
                if (exerciseModel.exerciseType == exerciseSelected)
                {
                    this.exerciseModel = exerciseModel;
                }
            }
        }
        else
        {
            this.exerciseModel = new ExerciseModel(0, 1, 2);
        }
        maxCapacity = patient.vitalCapacity * (float)patientPrescription.vitalCapacityGoal / 100;
        switch (exerciseSelected)
        {
            case 0:
                exercise = new ExerciseOne(this.exerciseModel.repNumber, this.exerciseModel.seriesNumber);
                break;
            case 1:
                exercise = new ExerciseTwo(this.exerciseModel.repNumber, this.exerciseModel.seriesNumber, maxCapacity);
                break;
            case 2:
                exercise = new ExerciseThree(this.exerciseModel.repNumber, this.exerciseModel.seriesNumber, maxCapacity);
                break;
            case 3:
                exercise = new ExerciseFour(this.exerciseModel.repNumber, this.exerciseModel.seriesNumber, maxCapacity);
                break;
            case 4:
                exercise = new ExerciseFive(this.exerciseModel.repNumber, this.exerciseModel.seriesNumber, maxCapacity);
                break;
        }
    }
    public static float GetAbsolutPositionFromMl(float positionInMl)
    {
        return (positionInMl - minCapacity + slopeConstant * FLOOR_POSITION) / slopeConstant;
    }

    public static int GetMlPositionFromAbsolut(float position)
    {
        return (int)(slopeConstant * (position - FLOOR_POSITION) + minCapacity) - (int)minCapacity;
    }
}
