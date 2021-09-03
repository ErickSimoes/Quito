using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    RectTransform circle;
    RectTransform canvas;

    void Awake() {
        canvas = GetComponent<RectTransform>();
    }

    public void OnClick() {
        float x = (canvas.rect.width / 2) - circle.rect.width;
        float y = (canvas.rect.height / 2) - circle.rect.height;
        circle.localPosition = new Vector3(Random.Range(-x, x), Random.Range(-y, y));
    }
}
