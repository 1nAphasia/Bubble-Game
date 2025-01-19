using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeImageSlideshow : MonoBehaviour
{
    public float picTimer;

    void Start()
    {
        Time.timeScale = 1;
        Animator animator = GetComponent<Animator>();
        animator.SetBool("Start", true);
        picTimer = 5f;
    }

    private void Update()
    {
        picTimer -= Time.deltaTime;
        Debug.Log(picTimer);
        if (picTimer <= 0)
        {
            SceneManager.LoadScene("Test1");
            picTimer = 5f;
        }
    }
}
