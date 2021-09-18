using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [SerializeField]
    Button startButton;
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
            gameStarted = true;

            StopCoroutine(BlinkText());
            ClickHereTMP.gameObject.SetActive(false);
            startButton.gameObject.SetActive(false);

            initialTime = (int)Time.realtimeSinceStartup;
        }

        score++; // TODO: Move this implementation to Mosquito
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
