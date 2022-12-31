using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 5.0f;
    private Vector3 position = Vector3.zero;
    private Quaternion rotation = Quaternion.identity;
    private Vector3 direction = Vector3.zero;
    private bool doLookAtMouse;

    [SerializeField]
    private Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LookAtMouse());
    }
    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position + (direction * speed) * Time.deltaTime;
        gameObject.transform.position = position;
    }

    public void OnMove(InputAction.CallbackContext obj)
    {
        direction = obj.ReadValue<Vector2>().normalized;
    }

    private void RotateTowardsMouse()
    {
        // Get Mouse position in world
        var mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Get difference between current position and mousePosition
        Vector3 direction = mousePos - transform.position;

        // Set target to face direction
        transform.up = new Vector2(direction.x, direction.y);


        // ------------- alternative -------------- 


        // normalize to only get direction
        //diff.Normalize();

        //// Calculate angle
        //float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        //Debug.Log("diff " + diff);
    }

    private IEnumerator LookAtMouse()
    {
        doLookAtMouse = true;
        while(doLookAtMouse)
        {
            RotateTowardsMouse();
            yield return new WaitForFixedUpdate();
        }
    }
}
