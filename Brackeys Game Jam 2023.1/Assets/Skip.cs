using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skip : MonoBehaviour
{
    [SerializeField] private GameObject skipText;
    private bool pressed;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (pressed)
            {
                GameManager.Instance.LoadScene("Menu");
            }
            pressed = true;
            skipText.SetActive(true);
            StartCoroutine(ChangePressed());
        }        
    }

    private IEnumerator ChangePressed()
    {
        yield return new WaitForSeconds(2);
        pressed = false;
        skipText.SetActive(false);
    }
}
