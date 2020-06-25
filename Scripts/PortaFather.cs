using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PortaFather : MonoBehaviour
{
    public GameObject porta0;
    public GameObject porta1;
    public Transform finalPos0;
    public Transform finalPos1;
    public PortaFather portaFather;

    public GameObject interactE;

    public AudioSource audioSource;
    public AudioClip doorSlide;

    public bool isOnRangeToOpen;
    public bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        isOnRangeToOpen = false;
        portaFather = this;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isOnRangeToOpen && !isOpen)
        {
            isOpen = true;
            audioSource.PlayOneShot(doorSlide);
            porta0.transform.DOMove(finalPos0.transform.position, 1);
            porta1.transform.DOMove(finalPos1.transform.position, 1);
            interactE.SetActive(false);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            isOnRangeToOpen = true;

            interactE.SetActive(true);

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOnRangeToOpen = false;

            interactE.SetActive(false);

        }
    }
}
