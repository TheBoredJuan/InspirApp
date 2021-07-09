using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseOne : IExercise
{
    public int ExercisePeriod { get ; set; }
    public int RepsNum { get; set; }
    public int SeriesNum { get; set; }

    public ExerciseOne(int repsNum, int seriesNum)
    {
        RepsNum = repsNum;
        SeriesNum = seriesNum;
        ExercisePeriod = 9;
    }

    float IExercise.GetPositionToSpawn(float timeCounter)
    {
        float decreaseFactor = 500f / 6f;
        float increaseFactor = 500f / 3f;
        float positionInMl = GameData.minCapacity;
        if (timeCounter <= 3)
        {
            positionInMl += timeCounter * increaseFactor;
        }
        else
        {
            positionInMl += 500f - (timeCounter - 3) * decreaseFactor;
        }
        return positionInMl;
    }
}
