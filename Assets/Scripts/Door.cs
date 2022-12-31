using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    float openRotation;

    [SerializeField]
    float closeRotation;

    float currentRotation = 0;

    [SerializeField]
    private float speed;

    private bool isPassive = true;
    private bool isOpen = false;
    // Update is called once per frame

    private void OnEnable()
    {
        EventManager.onObjectTriggerEvent += HandleTrigger;
    }


    private void HandleTrigger()
    {
        if(isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if(collision.tag == "Player")
        {
            Open();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if(collision.tag == "Player")
        {
            Close();
        }
    }

    public void Open()
    {
        if(isPassive)
        {
            isOpen = true;
            StartCoroutine(OpenOrClose(openRotation));
        }
    }

    public void Close()
    {
        if(isPassive)
        {
            isOpen = false;
            StartCoroutine(OpenOrClose(closeRotation));
        }
    }

    private IEnumerator OpenOrClose(float targetRotation)
    {
        Debug.Log("opening or closing");
        isPassive = false;
        float t = 0;
        float rotationTime = 0.2f;
        float startRotation = currentRotation;
        float endRotation = targetRotation;

        while (t < rotationTime)
        {
            float rotation = Mathf.Lerp(startRotation, endRotation, t/rotationTime);
            currentRotation = rotation;
            transform.rotation = Quaternion.Euler(0, 0, rotation);

            t += Time.deltaTime;
            yield return null;
        }
        isPassive = true;
    }

    private IEnumerator ClosingTimer(float timeUntilClose)
    {
        while (timeUntilClose > 0)
        {
            timeUntilClose -= 1f;
            yield return new WaitForSeconds(1);
        }
        Close();
    }
}
