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

    [Header("Dead Step")]
    [SerializeField]
    int deadStep;
    [SerializeField]
    float decrementCreationStep;
    [SerializeField]
    float mosquitoSpeedIncrement;
    int nextDeadStep;
    public static int totalMosquitoDead = 0;

    public static float gameCanvasWidth;
    public static float gameCanvasHeight;


    void Start() {
        RectTransform rectTransform = GetComponent<RectTransform>();
        gameCanvasWidth = rectTransform.rect.width;
        gameCanvasHeight = rectTransform.rect.height;

        UnityMainThreadDispatcher.Instance().Enqueue(Blink());
        nextDeadStep = deadStep;
    }

    void Update() {
        if (gameStarted) {
            countTime = (int)Time.realtimeSinceStartup - initialTime;
            UnityMainThreadDispatcher.Instance().Enqueue(() => timeTMP.text = $"Time: {countTime}s");
            UnityMainThreadDispatcher.Instance().Enqueue(() => lifeBar.fillAmount = life / 100);

            // Increment difficulty
            if (totalMosquitoDead >= nextDeadStep) {
                nextDeadStep += deadStep;

                minCreateRange -= decrementCreationStep;
                maxCreateRange -= decrementCreationStep * 2;
            }

            if (time2create < Time.realtimeSinceStartup) {
                time2create = Time.realtimeSinceStartup + Random.Range(minCreateRange, maxCreateRange);

                CreateMosquito();
            }

            // Detect end game
            if (life <= 0) {
                print("End Game");
            }
        }
    }

    void CreateMosquito() {
        GameObject newMosquito = Instantiate(mosquito, transform);
        newMosquito.GetComponent<MosquitoBehaviour>().speedReference += mosquitoSpeedIncrement;
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
