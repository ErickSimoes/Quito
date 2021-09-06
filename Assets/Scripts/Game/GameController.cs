using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    RectTransform circle;
    RectTransform canvas;

    [SerializeField]
    TextMeshProUGUI ClickHere;
    bool blinkStoped = false;

    [SerializeField]
    TextMeshProUGUI scoreTMP;
    int score = 0;

    void Awake() {
        canvas = GetComponent<RectTransform>();
    }

    void Start() {
        UnityMainThreadDispatcher.Instance().Enqueue(BlinkText());
    }

    public void OnClick() {
        if (!blinkStoped) {
            StopCoroutine(BlinkText());
            ClickHere.gameObject.SetActive(false);
            blinkStoped = true;
        }
        
        float x = (canvas.rect.width / 2) - circle.rect.width;
        float y = (canvas.rect.height / 2) - circle.rect.height;
        circle.localPosition = new Vector3(Random.Range(-x, x), Random.Range(-y, y));

        score++;
        UnityMainThreadDispatcher.Instance().Enqueue(() => scoreTMP.text = $"Score: {score}");
    }

    IEnumerator BlinkText() {
        Color color = ClickHere.color;
        while(true) {
            color.a = .2f + Mathf.PingPong(Time.time, .8f);
            ClickHere.color = color;
            yield return new WaitForSeconds(0.05f);
        }
        
    }
}
