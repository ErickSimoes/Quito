using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    RectTransform circle;
    RectTransform canvas;

    [SerializeField]
    TextMeshProUGUI ClickHereTMP;
    bool gameStarted = false;

    [SerializeField]
    TextMeshProUGUI scoreTMP;
    int score = 0;

    [SerializeField]
    TextMeshProUGUI timeTMP;
    [SerializeField]
    int limitTime = 20;
    int initialTime, countTime;

    void Awake() {
        canvas = GetComponent<RectTransform>();
    }

    void Start() {
        UnityMainThreadDispatcher.Instance().Enqueue(BlinkText());
        
        timeTMP.text = $"Time: {limitTime}";
    }

    void Update() {
        if (gameStarted) {
            countTime = limitTime - ((int)Time.realtimeSinceStartup - initialTime);
            UnityMainThreadDispatcher.Instance().Enqueue(() => timeTMP.text = $"Time: {countTime}");

            if (countTime <= 0) {
                print("End Game"); // TODO: End game
            }
        }
    }

    public void OnClick() {
        if (!gameStarted) {
            StopCoroutine(BlinkText());
            ClickHereTMP.gameObject.SetActive(false);
            gameStarted = true;

            initialTime = (int)Time.realtimeSinceStartup;
        }
        
        float x = (canvas.rect.width / 2) - circle.rect.width;
        float y = (canvas.rect.height / 2) - circle.rect.height;
        circle.localPosition = new Vector3(Random.Range(-x, x), Random.Range(-y, y));

        score++;
        UnityMainThreadDispatcher.Instance().Enqueue(() => scoreTMP.text = $"Score: {score}");
    }

    IEnumerator BlinkText() {
        Color color = ClickHereTMP.color;
        while(true) {
            color.a = .2f + Mathf.PingPong(Time.time, .8f);
            ClickHereTMP.color = color;
            yield return new WaitForSeconds(0.05f);
        }
        
    }
}
