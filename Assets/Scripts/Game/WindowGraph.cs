using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    public static Color EXERCISE_PRESCRIPTION_SERIE_COLOR = new Color(0.486f, 0.99f, 0f);
    public static Color EXERCISE_PATIENT_SERIE_COLOR = new Color(0f, 0.545f, 0.545f);
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform maxCapacityGoalTemplate;

    private void Awake()
    {
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        maxCapacityGoalTemplate = graphContainer.Find("MaxCapacityGoalTemplate").GetComponent<RectTransform>();
    }

    private void Start()
    {
        float yMaximum = (GameData.exercise is ExerciseOne) ? (GameData.minCapacity + 800f) * 1.2f : GameData.maxCapacity * 1.2f;
        float yMininum = GameData.minCapacity - 200f;
        float graphHeight = graphContainer.sizeDelta.y;

        RectTransform maxCapacityGoal = Instantiate(maxCapacityGoalTemplate);
        maxCapacityGoal.SetParent(graphContainer, false);
        maxCapacityGoal.gameObject.SetActive(true);
        float maxCapacityGoalAnchoredPosition = (GameData.exercise is ExerciseOne) ? GameData.minCapacity + 500f : yMaximum / 1.2f;
        maxCapacityGoal.anchoredPosition = new Vector2(0f, ((maxCapacityGoalAnchoredPosition - yMininum) / (yMaximum - yMininum)) * graphHeight);

    }

    public List<GameObject> ShowGraph(List<int> valueList, Color color) {
        List<GameObject> resultGameObjects = new List<GameObject>();
        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMaximum = (GameData.exercise is ExerciseOne) ? (GameData.minCapacity + 800f) * 1.2f : GameData.maxCapacity * 1.2f;
        float yMininum = GameData.minCapacity - 200f;
        
        float xSize = graphWidth / (valueList.Count + 1);
        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++) {
            float xPosition = xSize + i * xSize;
            //Debug.Log(valueList[i]);
            float yPosition = ((valueList[i] - yMininum) / (yMaximum - yMininum)) * graphHeight;
            //Debug.Log(yPosition);
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), color);
            resultGameObjects.Add(circleGameObject);
            if (lastCircleGameObject != null) {
                GameObject dotConnection = CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition, color);
                resultGameObjects.Add(dotConnection);
            }
            lastCircleGameObject = circleGameObject;
        }
        return resultGameObjects;
    }

    private GameObject CreateCircle(Vector2 anchoredPosition, Color color)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        Destroy(gameObject, GameManager.REST_TIME + 3);
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.GetComponent<Image>().color = color;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        return gameObject;
    }

    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, Color color) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        Destroy(gameObject, GameManager.REST_TIME + 3);
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = color;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.sizeDelta = new Vector2(distance, 3f); 
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
        return gameObject;

    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
