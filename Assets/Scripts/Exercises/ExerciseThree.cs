using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseThree : IExercise
{
    public int ExercisePeriod { get; set; }
    public int RepsNum { get; set; }
    public int SeriesNum { get; set; }

    private float maxCapacity;

    public ExerciseThree(int repsNum, int seriesNum, float maxCapacity)
    {
        RepsNum = repsNum;
        SeriesNum = seriesNum;
        ExercisePeriod = 14;
        this.maxCapacity = maxCapacity;
    }

    float IExercise.GetPositionToSpawn(float timeCounter)
    {
        float positionInMl = 1100;
        float decreaseFactor = (maxCapacity - positionInMl) / 6f;
        float increaseFactor = (maxCapacity - positionInMl) / 8f;
        if (timeCounter <= 8)
        {
            positionInMl += timeCounter * increaseFactor;
        }
        else
        {
            positionInMl = maxCapacity - (timeCounter - 8) * decreaseFactor;
        }
        return positionInMl;
    }
}
