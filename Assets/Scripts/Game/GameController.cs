using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [SerializeField]
    GameObject mosquito;
    float time2create = 0f;

    [SerializeField]
    Button startButton;
    bool gameStarted = false;

    [SerializeField]
    TextMeshProUGUI scoreTMP;
    public static int score = 0;

    [SerializeField]
    TextMeshProUGUI timeTMP;
    int initialTime, countTime;

    void Start() {
        UnityMainThreadDispatcher.Instance().Enqueue(BlinkText());
    }

    void Update() {
        if (gameStarted) {
            countTime = (int)Time.realtimeSinceStartup - initialTime;
            UnityMainThreadDispatcher.Instance().Enqueue(() => scoreTMP.text = $"Score: {score}");
            UnityMainThreadDispatcher.Instance().Enqueue(() => timeTMP.text = $"Time: {countTime}s");

            if (time2create < Time.realtimeSinceStartup) {
                CreateMosquito();
                time2create = Time.realtimeSinceStartup + Random.Range(0.5f, 4f);
            }
        }
    }

    void CreateMosquito() {
        Instantiate(mosquito, transform);
    }

    public void OnClick() {
        if (!gameStarted) {
            gameStarted = true;

            StopCoroutine(BlinkText());
            startButton.gameObject.SetActive(false);

            initialTime = (int)Time.realtimeSinceStartup;
        }
    }

    IEnumerator BlinkText() {
        Color color = startButton.image.color;
        while(true) {
            color.a = .2f + Mathf.PingPong(Time.time, .8f);
            startButton.image.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
