using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PivotScript : MonoBehaviour
{
    private TMP_Text pivotText;
    public float ml;
    void Start()
    {
        pivotText = GetComponent<TMP_Text>();
        StartCoroutine(LateStart(0.2f));
    }
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        transform.position = new Vector3(transform.position.x, GameData.GetAbsolutPositionFromMl(ml + 1100), 0);
        if (transform.position.y > GameData.ROOF_POSITION) gameObject.SetActive(false);
        pivotText.text = ml.ToString();
    }
}
