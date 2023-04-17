using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public static Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    void Start()
    {
        ScoreManager.Instance.ResetScore();
    }

    
    public static void UpdateText(int score)
    {
       text.text= score.ToString();
    }

    public static void ScaleText(Vector3 targetScale)
    {
        text.rectTransform.localScale = targetScale;
    }
}
