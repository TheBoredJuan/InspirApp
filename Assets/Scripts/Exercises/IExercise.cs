using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExercise
{
    int ExercisePeriod { get; set; }
    int RepsNum { get; set; }
    int SeriesNum { get; set; }

    float GetPositionToSpawn(float timeCounter);
}
