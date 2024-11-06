using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FarkleUI : MonoBehaviour
{
    public AudioSource soundEffect; 
    public Image popUpImage; 
    public float displayDuration = 3f;

    private void Start()
    {
        popUpImage.gameObject.SetActive(false);
    }

    public void ShowFarklePopUp()
    {
        StartCoroutine(ShowPopUpCoroutine());
    }

    private IEnumerator ShowPopUpCoroutine()
    {
        popUpImage.gameObject.SetActive(true);
        soundEffect.Play();

        yield return new WaitForSeconds(displayDuration);

        popUpImage.gameObject.SetActive(false);
    }
}
