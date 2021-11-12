using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [Header("Mosquito Prefab")]
    [SerializeField]
    GameObject mosquito;
    float time2create = 0f;

    [Header("Start Button")]
    [SerializeField]
    Button startButton;
    bool gameStarted = false;

    [Header("Time Text")]
    [SerializeField]
    TextMeshProUGUI timeTMP;
    int initialTime, countTime;

    [Header("Life Bar")]
    [SerializeField]
    Image lifeBar;
    public static float life = 100;

    [Header("Mosquito Creation")]
    [SerializeField]
    float minCreateRange;
    [SerializeField]
    float maxCreateRange;


    void Start() {
        UnityMainThreadDispatcher.Instance().Enqueue(Blink());
    }

    void Update() {
        if (gameStarted) {
            countTime = (int)Time.realtimeSinceStartup - initialTime;
            UnityMainThreadDispatcher.Instance().Enqueue(() => timeTMP.text = $"Time: {countTime}s");
            UnityMainThreadDispatcher.Instance().Enqueue(() => lifeBar.fillAmount = life / 100);

            if (time2create < Time.realtimeSinceStartup) {
                CreateMosquito();
                time2create = Time.realtimeSinceStartup + Random.Range(minCreateRange, maxCreateRange);
            }

            // Detect end game
            if (life <= 0) {
                print("End Game");
            }
        }
    }

    void CreateMosquito() {
        Instantiate(mosquito, transform);
    }

    public void OnClick() {
        if (!gameStarted) {
            gameStarted = true;

            StopCoroutine(Blink());
            startButton.gameObject.SetActive(false);

            initialTime = (int)Time.realtimeSinceStartup;
        }
    }

    IEnumerator Blink() {
        Color color = startButton.image.color;
        while(true) {
            color.a = .2f + Mathf.PingPong(Time.time, .8f);
            startButton.image.color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
