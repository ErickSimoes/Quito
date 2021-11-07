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

    [Header("Life Bar")]
    [SerializeField]
    Image lifeBar;
    public static float life = 100;

    [SerializeField]
    TextMeshProUGUI timeTMP;
    int initialTime, countTime;

    void Start() {
        UnityMainThreadDispatcher.Instance().Enqueue(BlinkText());
    }

    void Update() {
        if (gameStarted) {
            countTime = (int)Time.realtimeSinceStartup - initialTime;
            UnityMainThreadDispatcher.Instance().Enqueue(() => timeTMP.text = $"Time: {countTime}s");
            UnityMainThreadDispatcher.Instance().Enqueue(() => lifeBar.fillAmount = life / 100);

            if (time2create < Time.realtimeSinceStartup) {
                CreateMosquito();
                time2create = Time.realtimeSinceStartup + Random.Range(0.5f, 4f);
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
