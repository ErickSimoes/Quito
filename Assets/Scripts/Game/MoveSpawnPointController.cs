using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpawnPointController : MonoBehaviour {

    enum SlideDirection {
        HORIZONTAL,
        VERTICAL
    }

    [SerializeField]
    SlideDirection direction;
    RectTransform myRectTransform;
    static RectTransform gameCanvasRectTransform;

    const float SPEED = 350;
    Vector2 minPosition, maxPosition, targetPosition;

    void Start() {
        if (!gameCanvasRectTransform) {
            gameCanvasRectTransform = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<RectTransform>();
        }

        float width = gameCanvasRectTransform.rect.width;
        float height = gameCanvasRectTransform.rect.height;
        
        myRectTransform = GetComponent<RectTransform>();
        if (direction == SlideDirection.HORIZONTAL) {
            minPosition = new Vector2(-(width / 2), myRectTransform.localPosition.y);
            maxPosition = new Vector2(width / 2, myRectTransform.localPosition.y);
        } else {
            minPosition = new Vector2(myRectTransform.localPosition.x, - (height / 2));
            maxPosition = new Vector2(myRectTransform.localPosition.x, height / 2);
        }
        targetPosition = minPosition;
    }

    
    void Update() {
        myRectTransform.localPosition = Vector2.MoveTowards(myRectTransform.localPosition, targetPosition, SPEED * Time.deltaTime);
        
        if (myRectTransform.localPosition.Equals(minPosition)) {
            targetPosition = maxPosition;
        } else if (myRectTransform.localPosition.Equals(maxPosition)) {
            targetPosition = minPosition;
        }
    }
}
