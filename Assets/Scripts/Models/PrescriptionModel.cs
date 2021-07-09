using System.Collections.Generic;

[System.Serializable]
public class PrescriptionModel
{
    public string username;
    public int vitalCapacityGoal;
    public List<ExerciseModel> exercises;

    public PrescriptionModel(string username, int vitalCapacityGoal, List<ExerciseModel> exercises) {
        this.username = username;
        this.vitalCapacityGoal = vitalCapacityGoal;
        this.exercises = exercises;
    }

    public static PrescriptionModel getDefaultPrescription(string username)
    {
        List<ExerciseModel> exercises = new List<ExerciseModel>();
        exercises.Add(new ExerciseModel(1, 1, 3));
        exercises.Add(new ExerciseModel(2, 1, 3));
        exercises.Add(new ExerciseModel(3, 1, 3));
        exercises.Add(new ExerciseModel(4, 1, 3));
        return new PrescriptionModel(username, 80, exercises);
    }
}
