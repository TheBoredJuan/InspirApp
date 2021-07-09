using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseFour : IExercise
{
    public int ExercisePeriod { get; set; }
    public int RepsNum { get; set; }
    public int SeriesNum { get; set; }

    private float maxCapacity;

    public ExerciseFour(int repsNum, int seriesNum, float maxCapacity)
    {
        RepsNum = repsNum;
        SeriesNum = seriesNum;
        ExercisePeriod = 13;
        this.maxCapacity = maxCapacity;
    }

    float IExercise.GetPositionToSpawn(float timeCounter)
    {
        float positionInMl = 1100;
        float decreaseFactor = (maxCapacity - positionInMl) / 6f;
        float increaseFactor = (maxCapacity - positionInMl) / 5f;
        if (timeCounter <= 1)
        {
            positionInMl += timeCounter * increaseFactor;
        }
        else if (timeCounter <= 3)
        {
            positionInMl += increaseFactor + timeCounter * (increaseFactor / 2);
        }
        else if (timeCounter == 4)
        {
            positionInMl += 3 * increaseFactor;
        }
        else if (timeCounter <= 6)
        {
            positionInMl += 3 * increaseFactor + (timeCounter - 4) * (increaseFactor / 2);
        }
        else if (timeCounter == 7) 
        {
            positionInMl = maxCapacity;
        }
        else
        {
            positionInMl = maxCapacity - (timeCounter - 7) * decreaseFactor;
        }
        return positionInMl;
    }
}