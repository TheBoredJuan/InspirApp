
[System.Serializable]
public class ExerciseModel
{
    private static string EXERCISE_ONE_DESCRIPTION = "(Ejercicio de prueba) \nInspiración nasal, espiración bucal hasta lograr la capacidad residual funcional";
    private static string EXERCISE_TWO_DESCRIPTION = "Inspiración lenta, nasal, hasta alcanzar la capacidad máxima inspiratoria, seguida de una apnea de 3 a 10 segundos, seguida de una espiración lenta por la boca";
    private static string EXERCISE_THREE_DESCRIPTION = "Subdividida en inspiraciones cortas y sucesivas, sin apneas, hasta completar la máxima capacidad inspiratoria";
    private static string EXERCISE_FOUR_DESCRIPTION = "1) Fase: Inspiración suave y profunda, respirando una pequeña cantidad de aire.\n2) Fase: Vuelve a inspirar profundamente a partir del termino de la 1ª, espira nuevamente una pequeña cantidad.\n3) Fase: Vuelve a inspirar profundamente a partir del termino de la 2ª, espirando completamente";
    private static string EXERCISE_FIVE_DESCRIPTION = "Subdividida en dos inspiraciones la primera corta con un periodo de apnea y la segunda hasta completar la máxima capacidad inspiratoria y realizar una apnea antes de espirar";
    private static string EXERCISE_ERROR_DESCRIPTION = "Tipo de ejercicio incorrecto";

    public int exerciseType;
    public int repNumber;
    public int seriesNumber;

    public ExerciseModel(int exerciseType, int repNumber, int seriesNumber)
    {
        this.exerciseType = exerciseType;
        this.repNumber = repNumber;
        this.seriesNumber = seriesNumber;
    }

    public static string getExerciseDescription(int exerciseType) {
        switch (exerciseType) {
            case 0:
                return EXERCISE_ONE_DESCRIPTION;
            case 1:
                return EXERCISE_TWO_DESCRIPTION;
            case 2:
                return EXERCISE_THREE_DESCRIPTION;
            case 3:
                return EXERCISE_FOUR_DESCRIPTION;
            case 4:
                return EXERCISE_FIVE_DESCRIPTION;
            default:
                return EXERCISE_ERROR_DESCRIPTION;
        }
    }
}
