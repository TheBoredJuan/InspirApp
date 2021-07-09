using UnityEngine;
using TMPro;

public class FadeOutTextScript : MonoBehaviour
{
    private static float TREE_SECONDS = 3;
    public TMP_Text instructionText;
    private float timer = 0f;
    private float alphaDecrement = 0.01f;

    void Update()
    {
        if (timer > TREE_SECONDS) {
            instructionText.alpha -= alphaDecrement;
        }
        timer += Time.deltaTime;
    }
}
