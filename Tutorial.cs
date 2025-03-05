using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject tutorialBox;
    [SerializeField] GameObject tutorialEnemy;

    //[SerializeField] private Camera cam;

    bool isTutorial;

    void Start()
    {
        StartCoroutine(StartTutorial());
    }

    void Update()
    {
        if (isTutorial)
            PlayerHUDManager.instance.SetTutorialImage(Camera.main, tutorialBox.transform.position, tutorialEnemy.transform.position);

        if (isTutorial && Input.GetMouseButton(0))
            ToggleTutorial(false);
    }
    
    IEnumerator StartTutorial()
    {
        while (PlayerHUDManager.instance == null)
        {
            yield return null;
        }

        ToggleTutorial(true);
    }

    void ToggleTutorial(bool state)
    {
        PlayerHUDManager.instance.ToggleTutorialUI(state);
        isTutorial = state;

        Time.timeScale = state ? 0f : 1f;
    }
}
