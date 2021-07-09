public class ExerciseTwo : IExercise
{
    public int ExercisePeriod { get; set; }
    public int RepsNum { get; set; }
    public int SeriesNum { get; set; }

    private float maxCapacity;

    public ExerciseTwo(int repsNum, int seriesNum, float maxCapacity)
    {
        RepsNum = repsNum;
        SeriesNum = seriesNum;
        ExercisePeriod = 15;
        this.maxCapacity = maxCapacity;
    }

    float IExercise.GetPositionToSpawn(float timeCounter)
    {
        float positionInMl = GameData.minCapacity;
        float decreaseFactor = (maxCapacity - positionInMl) / 6f;
        float increaseFactor = (maxCapacity - positionInMl) / 3f;
        // In the (3,9] range the patient does apnea.
        if (timeCounter <= 3)
        {
            positionInMl += timeCounter * increaseFactor;
        }
        else if (timeCounter <= 9) {
            positionInMl = maxCapacity;
        } else
        {
            positionInMl = maxCapacity - (timeCounter - 9) * decreaseFactor;
        }
        return positionInMl;
    }
}
