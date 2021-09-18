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

    [SerializeField]
    TextMeshProUGUI clickHereTMP;
    bool gameStarted = false;

    [SerializeField]
    TextMeshProUGUI scoreTMP;
    int score = 0;

    [SerializeField]
    TextMeshProUGUI timeTMP;
    [SerializeField]
    int limitTime = 20;
    int initialTime, countTime;

    void Start() {
        UnityMainThreadDispatcher.Instance().Enqueue(BlinkText());
        
        timeTMP.text = $"Time: {limitTime}";
    }

    void Update() {
        if (gameStarted) {
            countTime = limitTime - ((int)Time.realtimeSinceStartup - initialTime);
            UnityMainThreadDispatcher.Instance().Enqueue(() => timeTMP.text = $"Time: {countTime}");

            if (time2create < Time.realtimeSinceStartup) {
                CreateMosquito();

                time2create = Time.realtimeSinceStartup + Random.Range(0.5f, 4f);
            }

            if (countTime <= 0) {
                print("End Game"); // TODO: End game
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
            clickHereTMP.gameObject.SetActive(false);
            startButton.gameObject.SetActive(false);

            initialTime = (int)Time.realtimeSinceStartup;
        }

        score++; // TODO: Move this implementation to Mosquito
        UnityMainThreadDispatcher.Instance().Enqueue(() => scoreTMP.text = $"Score: {score}");
    }

    IEnumerator BlinkText() {
        Color color = clickHereTMP.color;
        while(true) {
            color.a = .2f + Mathf.PingPong(Time.time, .8f);
            clickHereTMP.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
