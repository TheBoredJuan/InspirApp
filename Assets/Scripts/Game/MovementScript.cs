using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovementScript : MonoBehaviour
{

    public float velocity;
    public TMP_Text mlText;
    public TMP_Text maxMlText;
    public TMP_Text numberOfHitsText;

    private Rigidbody2D rb;
    private float verticalVelocity;
    private int numberOfHits = 0;
    public int maxMl = 0;

    private bool mousePressed = false;

    public GameObject windowGraphGameObject;
    private WindowGraph windowGraph;
    private List<int> patientExerciseValues = new List<int>();
    private Dictionary<int, List<int>> patientCompleteExerciseDictionary = new Dictionary<int, List<int>>();
    
    void Start() {
        windowGraph = FindObjectOfType<WindowGraph>();
        rb = GetComponent<Rigidbody2D>();
        numberOfHitsText.text = numberOfHits.ToString();
        maxMlText.text = maxMl.ToString();
    }
    
    void Update() {
        //Mouse inputs
        /*if (Input.GetMouseButtonDown(0)) mousePressed = true;
        else if (Input.GetMouseButtonUp(0)) mousePressed = false;
        verticalVelocity = mousePressed ? 1 : -1;*/
        //Touch input
        switch (Input.touchCount) {
            case 0:
                verticalVelocity = 0;
                break;
            case 1:
                verticalVelocity = 1;
                break;
            case 2:
                verticalVelocity = -1;
                break;
            default:
                verticalVelocity = 0;
                break;
        }
        //Ml Update
        int actualMl = GameData.GetMlPositionFromAbsolut(transform.position.y);
        mlText.text = actualMl.ToString();
        if (actualMl > maxMl) {
            maxMl = actualMl;
            maxMlText.text = maxMl.ToString();
        }
    }

    void FixedUpdate() {
        float fixedVelocity = (verticalVelocity > 0) ? velocity : velocity / 2;
        rb.velocity = Vector2.up * verticalVelocity * fixedVelocity;
    }

    public void GetPlayerGraph(int serieCount) {
        windowGraph.ShowGraph(patientExerciseValues, WindowGraph.EXERCISE_PATIENT_SERIE_COLOR);
        patientCompleteExerciseDictionary.Add(serieCount, patientExerciseValues);
        patientExerciseValues = new List<int>();
    }

    public void SavePlayerGraph(int serieCount) {
        patientCompleteExerciseDictionary.Add(serieCount, patientExerciseValues);
        patientExerciseValues = new List<int>();
    }

    public List<GameObject> GetSerieGraph(int serieNumber) {
        return windowGraph.ShowGraph(patientCompleteExerciseDictionary[serieNumber], WindowGraph.EXERCISE_PATIENT_SERIE_COLOR);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            collision.gameObject.SetActive(false);
            numberOfHits++;
            numberOfHitsText.text = numberOfHits.ToString();
        }
        else if (collision.gameObject.CompareTag("Graph")) {
            patientExerciseValues.Add(GameData.GetMlPositionFromAbsolut(transform.position.y) + (int)GameData.minCapacity);
        }
    }
}
