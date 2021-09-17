using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MosquitoBehaviour : MonoBehaviour {

    static RectTransform gameCanvas;
    static float xRange, yRange;
    RectTransform myRectTransform;

    [SerializeField]
    float speed = 150;
    [SerializeField]
    Sprite deadSprite;
    Image image;

    Vector3 targetPosition;
    bool isAlive = true;
    
    void Start() {
        if (!gameCanvas) {
            gameCanvas = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<RectTransform>();
            myRectTransform = GetComponent<RectTransform>();
            xRange = (gameCanvas.rect.width / 2) - myRectTransform.rect.width;
            yRange = (gameCanvas.rect.height / 2) - myRectTransform.rect.height;
        }

        image = GetComponent<Image>();

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
        targetPosition = new Vector3(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange));
    }

    public void OnClick() {
        isAlive = false;
        CancelInvoke(nameof(ChoosePosition));

        GetComponent<Animator>().enabled = false;

        image.sprite = deadSprite;
    }
}
