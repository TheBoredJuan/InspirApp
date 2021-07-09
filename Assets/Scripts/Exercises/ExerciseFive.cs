using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseFive : IExercise
{
    public int ExercisePeriod { get; set; }
    public int RepsNum { get; set; }
    public int SeriesNum { get; set; }

    private float maxCapacity;

    public ExerciseFive(int repsNum, int seriesNum, float maxCapacity)
    {
        RepsNum = repsNum;
        SeriesNum = seriesNum;
        ExercisePeriod = 16;
        this.maxCapacity = maxCapacity;
    }

    float IExercise.GetPositionToSpawn(float timeCounter)
    {
        float positionInMl = 1100;
        //Number of times variable means if is fractionated in two, three or four times.
        float numberOfTimes = 2f;
        float decreaseFactor = (maxCapacity - positionInMl) / 6f;
        float increaseFactor = ((maxCapacity - positionInMl) / numberOfTimes) / 2f;
        // In the (3,5], (7,10] ranges the patient do apnea.
        if (timeCounter <= 2)
        {
            positionInMl += timeCounter * increaseFactor;
        }
        else if (timeCounter <= 5)
        {
            positionInMl += 2 * increaseFactor;
        }
        else if (timeCounter <= 7)
        {
            positionInMl += 2 * increaseFactor + (timeCounter - 5) * increaseFactor;
        }
        else if (timeCounter <= 9) {
            positionInMl += 4 * increaseFactor;
        }
        else
        {
            positionInMl = maxCapacity - (timeCounter - 10) * decreaseFactor;
        }
        return positionInMl;
    }
}
