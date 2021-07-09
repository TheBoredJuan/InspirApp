using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks;
using UnityEngine.Events;

public class PatientDataManager : MonoBehaviour
{
    [Header("Register inputs")]
    private GenderEnum gender;
    public Toggle otherGenderToggle;
    public TMP_InputField height;
    public TMP_InputField age;
    public TMP_InputField patientName;
    public TMP_InputField document;
    public TMP_InputField username;
    public TMP_InputField password;
    [Header("Login inputs")]
    public TMP_InputField loginUsername;
    public TMP_InputField loginPassword;
    [Header("Register buttons")]
    public Button signInTwoNextButton;
    public Button signInThreeNextButton;
    public Button signInFourNextButton;
    [Header("Menus for interaction for Login and register")]
    public GameObject startMenu;
    public GameObject loginMenu;
    public GameObject registerMenu;
    public GameObject mainMenu;
    [Header("Exercise buttons")]
    public Button exerciseTwoButton;
    public Button exerciseThreeButton;
    public Button exerciseFourButton;
    public Button exerciseFiveButton;
    [Header("Exercise texts")]
    public TMP_Text exerciseDescription;
    public TMP_Text exerciseSeries;
    public TMP_Text exerciseReps;
    [Header("Username exists event")]
    public UnityEvent userExistsEvent = new UnityEvent();
    [Header("Patient data")]
    private PatientModel patient;
    private PrescriptionModel prescription;
    private Dictionary<int, ExerciseModel> exerciseMap;
    private SaveLoadSystem saveLoadSystem;

    private void Reset()
    {
        saveLoadSystem = FindObjectOfType<SaveLoadSystem>();
    }
    public void Start()
    {
        if (PlayerPrefs.HasKey("patient") && PlayerPrefs.HasKey("prescription")) {
            patient = JsonUtility.FromJson<PatientModel>(PlayerPrefs.GetString("patient"));
            prescription = JsonUtility.FromJson<PrescriptionModel>(PlayerPrefs.GetString("prescription"));
            MapExcercises();
            startMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
        saveLoadSystem = FindObjectOfType<SaveLoadSystem>();
        ClearRegisterInputs();
        ClearLoginInputs();
    }

    #region Register
    public void GenderSelection(int selection) {
        if (selection == 0)
        {
            if (otherGenderToggle.isOn)
            {
                gender = GenderEnum.OTHER_M;
            }
            else
            {
                gender = GenderEnum.MALE;
            }
        }
        else
        {
            if (otherGenderToggle.isOn)
            {
                gender = GenderEnum.OTHER_F;
            }
            else
            {
                gender = GenderEnum.FEMALE;
            }
        }
    }

    public void ValidateInputsSignInTwo() {
        signInTwoNextButton.interactable = (height.text.Length > 1 && height.text.Length <= 3) &&
                                            (age.text.Length > 0 && age.text.Length <= 3);
    }

    public void ValidateInputsSignInThree()
    {
        signInThreeNextButton.interactable = (document.text.Length >= 6 && document.text.Length <= 10) && 
                                                (patientName.text.Length > 0);
    }

    public void ValidateInputsSignInFour()
    {
        signInFourNextButton.interactable = password.text.Length > 3 && 
                                            username.text.Length > 3 && username.text.Length <= 16;
    }

    public void Register() {
        StartCoroutine(CheckIfPatientExistsCorutine());
    }

    private IEnumerator CheckIfPatientExistsCorutine() {
        Task<bool> patientExists = saveLoadSystem.PatientExists(username.text);
        yield return new WaitUntil(() => patientExists.IsCompleted);
        if (patientExists.Result)
        {
            username.text = "";
            userExistsEvent.Invoke();
        }
        else {
            RegisterPatientCorutine();
        }
    }

    private void RegisterPatientCorutine() {
        long documentLong = Convert.ToInt64(document.text);
        int ageInt = Convert.ToInt32(age.text);
        int heightInt = Convert.ToInt32(height.text);
        int vitalCapacity = getVitalCapacity((int)gender, ageInt, heightInt);
        PatientModel newPatient = new PatientModel(username.text, password.text, patientName.text, documentLong, (int)gender, ageInt, heightInt, vitalCapacity);
        
        saveLoadSystem.SavePatient(newPatient);
        registerMenu.SetActive(false);
        mainMenu.SetActive(true);
        this.patient = newPatient;

        StartCoroutine(LoadPatientPrescriptionCorutine());
    }
    #endregion

    #region Login
    public void Login() {
        StartCoroutine(LoadPatientCorutine());    }

    private IEnumerator LoadPatientCorutine() {
        Task<PatientModel> patientTask = saveLoadSystem.LoadPatient(loginUsername.text);
        yield return new WaitUntil(() => patientTask.IsCompleted);
        if (patientTask.Result != null && Equals(patientTask.Result.password, loginPassword.text))
        {
            loginMenu.SetActive(false);
            mainMenu.SetActive(true);
            this.patient = patientTask.Result;
            StartCoroutine(LoadPatientPrescriptionCorutine());
        }
    }

    private IEnumerator LoadPatientPrescriptionCorutine() {
        Task<PrescriptionModel> prescriptionTask = saveLoadSystem.LoadPatientPrescription(patient.username);
        yield return new WaitUntil(() => prescriptionTask.IsCompleted);
        if (prescriptionTask.Result != null) {
            this.prescription = prescriptionTask.Result;
            MapExcercises();
        }
    }
    #endregion

    private void MapExcercises() {
        exerciseMap = new Dictionary<int, ExerciseModel>();
        foreach (ExerciseModel exercise in this.prescription.exercises) {
            exerciseMap.Add(exercise.exerciseType, exercise);
        }
    }
    public void ValidateExercisesForPrescription() {
        exerciseTwoButton.interactable = exerciseThreeButton.interactable = exerciseFourButton.interactable = exerciseFiveButton.interactable = false;
        
        if (exerciseMap.ContainsKey(1)) {
            exerciseTwoButton.interactable = true;
        }
        if (exerciseMap.ContainsKey(2))
        {
            exerciseThreeButton.interactable = true;
        }
        if (exerciseMap.ContainsKey(3))
        {
            exerciseFourButton.interactable = true;
        }
        if (exerciseMap.ContainsKey(4))
        {
            exerciseFiveButton.interactable = true;
        }
    }

    public void FillExerciseInfo(int exerciseType) {
        exerciseDescription.text = ExerciseModel.getExerciseDescription(exerciseType);
        PlayerPrefs.SetInt("exerciseType", exerciseType);
        if (exerciseType == 0)
        {
            exerciseSeries.text = "2";
            exerciseReps.text = "1";
        }
        else {
            exerciseSeries.text = exerciseMap[exerciseType].seriesNumber.ToString();
            exerciseReps.text = exerciseMap[exerciseType].repNumber.ToString();
        }
    }

    public void SavePatientDataInPlayerPrefs() {
        PlayerPrefs.SetString("patient", JsonUtility.ToJson(patient));
        PlayerPrefs.SetString("prescription", JsonUtility.ToJson(prescription));
        SceneManagerUtil.LoadScene(1);
    }

    #region Utils

    public void ClearPlayerPrefabs() {
        PlayerPrefs.DeleteAll();
    }
    public void ClearRegisterInputs() {
        height.text = "";
        age.text = "";
        patientName.text = "";
        document.text = "";
        username.text = "";
        password.text = "";
        signInTwoNextButton.interactable = false;
        signInThreeNextButton.interactable = false;
        signInFourNextButton.interactable = false;
    }

    private int getVitalCapacity(int gender, int age, int height) {
        float result;
        if (gender == (int)GenderEnum.MALE || gender == (int)GenderEnum.OTHER_M)
        {
            result = (27.63f - 0.112f * age) * height;
        }
        else {
            result = (21.78f - 0.101f * age) * height;
        }
        return (int) result;
    }

    public void ClearLoginInputs() {
        loginPassword.text = "";
        loginUsername.text = "";
    }
    private enum GenderEnum { 
        MALE, FEMALE, OTHER_M, OTHER_F
    }
    #endregion
}
