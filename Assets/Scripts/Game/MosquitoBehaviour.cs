using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MosquitoBehaviour : MonoBehaviour {

    static GameObject[] spawnPoints;
    static float xRange, yRange;
    RectTransform myRectTransform;

    [Header("Swig Size")]
    public float swig;

    [Header("Speed")]
    [SerializeField]
    public float speedReference = 200;
    float speed;

    [Header("Animator")]
    [SerializeField]
    Animator animator;

    [Header("Sounds")]
    [SerializeField] AudioClip flying;
    [SerializeField] AudioClip swallowing;
    [SerializeField] AudioClip[] smashed;
    AudioSource audioSource;

    Vector3 targetPosition;
    bool isAlive = true, isSting = false;
    static RectTransform deadPool;
    Coroutine suckBlood;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        myRectTransform = GetComponent<RectTransform>();
        xRange = (GameController.gameCanvasWidth / 2) - myRectTransform.rect.width;
        yRange = (GameController.gameCanvasHeight / 2) - myRectTransform.rect.height;

        if (spawnPoints == null || spawnPoints.Length == 0) {
            spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        }

        myRectTransform.localPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].GetComponent<RectTransform>().localPosition;

        if (!deadPool) {
            deadPool = GameObject.FindGameObjectWithTag("DeadPool").GetComponent<RectTransform>();
        }

        ChoosePosition();
        InvokeRepeating(nameof(ChoosePosition), 1f, Random.Range(1f, 3f));

        audioSource.clip = flying;
        audioSource.Play();
    }

    void Update() {
        if (isAlive && !isSting) {
            myRectTransform.localPosition = Vector2.MoveTowards(myRectTransform.localPosition, targetPosition, speed * Time.deltaTime);

            if (myRectTransform.localPosition.x < targetPosition.x) {
                myRectTransform.localScale = new Vector3(-1, 1, 1);
            } else {
                myRectTransform.localScale = Vector3.one;
            }

            if (myRectTransform.localPosition == targetPosition) {
                ChooseBehaviour();
            }
        }

    }

    void ChooseBehaviour() {
        if (Random.value <= .8) {
            Sting();
        } else {
            ChoosePosition();
        }
    }

    void Sting() {
        isSting = true;
        animator.SetBool("IsSting", true);
        CancelInvoke(nameof(ChoosePosition));

        audioSource.clip = swallowing;
        audioSource.Play();

        suckBlood = StartCoroutine(SuckBlood());
    }

    IEnumerator SuckBlood() {
        while(true) {
            GameController.life -= swig;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void ChoosePosition() {
        speed = Random.value >= .5 ? speedReference * 4 : speedReference;

        targetPosition = new Vector3(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange));
    }

    public void OnClick() {
        isAlive = false;
        CancelInvoke(nameof(ChoosePosition));
        if (suckBlood != null) {
            StopCoroutine(suckBlood);
        }

        GameController.totalMosquitoDead++;

        animator.SetBool("IsDead", true);

        audioSource.clip = smashed[Random.Range(0, smashed.Length)];
        audioSource.loop = false;
        audioSource.Play();

        GetComponent<Image>().raycastTarget = false;

        transform.SetParent(deadPool, false);
        myRectTransform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }

    public void DestroyMe() {
        Destroy(this.gameObject);
    }

}
