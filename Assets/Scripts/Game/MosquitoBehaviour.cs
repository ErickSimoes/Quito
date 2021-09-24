using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MosquitoBehaviour : MonoBehaviour {

    static GameObject[] spawnPoints;
    static RectTransform gameCanvas;
    static float xRange, yRange;
    RectTransform myRectTransform;

    [SerializeField]
    float speedReference = 150;
    float speed;
    [SerializeField]
    Sprite deadSprite;
    Image image;

    Vector3 targetPosition;
    bool isAlive = true;
    static RectTransform deadPool;
    
    void Start() {
        if (!gameCanvas) {
            gameCanvas = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<RectTransform>();
        }

        myRectTransform = GetComponent<RectTransform>();
        xRange = (gameCanvas.rect.width / 2) - myRectTransform.rect.width;
        yRange = (gameCanvas.rect.height / 2) - myRectTransform.rect.height;

        if (spawnPoints == null || spawnPoints.Length == 0) {
            spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        }

        image = GetComponent<Image>();

        myRectTransform.localPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].GetComponent<RectTransform>().localPosition;

        if (!deadPool) {
            deadPool = GameObject.FindGameObjectWithTag("DeadPool").GetComponent<RectTransform>();
        }

        InvokeRepeating(nameof(ChoosePosition), 1f, Random.Range(1f, 3f));
    }

    void Update() {
        if (isAlive) {
            myRectTransform.localPosition = Vector2.MoveTowards(myRectTransform.localPosition, targetPosition, speed * Time.deltaTime);

            if (myRectTransform.localPosition.x < targetPosition.x) {
                myRectTransform.localScale = new Vector3(-1, 1, 1);
            } else {
                myRectTransform.localScale = Vector3.one;
            }
        }

        if (myRectTransform.localPosition == targetPosition) {
            ChoosePosition();
        }
    }

    void ChoosePosition() {
        speed = Random.value >= .5 ? speedReference * 3 : speedReference;

        targetPosition = new Vector3(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange));
    }

    public void OnClick() {
        isAlive = false;
        GameController.score++;
        CancelInvoke(nameof(ChoosePosition));

        Destroy(GetComponent<Animator>());
        image.sprite = deadSprite;
        image.raycastTarget = false;
        transform.SetParent(deadPool, false);
        myRectTransform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }
}
