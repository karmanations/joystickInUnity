using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour {
    public float speed = 5.0f;
    public Transform player;

    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;

    public Transform circle;
    public Transform outerCircle;

	void Update () 
    {
        if(Input.GetMouseButtonDown(0)) // a finger is placed down. Returns true only during frame where the finger is placed down.
        {
            // Transforms a point from screen space (where the finger is on the screen) into world space coordinates.
            // We are getting the main camera (Camera.main) and converting the finger coordinates to coordinates in the world space.
            pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z)); // get the finger position on screen.

            // We can now place the joystick visuals where the finger layed down at first with pointA
            // We multiply by -1 because the camera and the GUI look in the opposite direction, so opposite coordinates.
            // For example, if you use LookAt with a camera to look at a UIText, the text will show up reversed.
            // https://answers.unity.com/questions/912465/uitext-inverted-when-looking-at-camera.html
            circle.transform.position = pointA * -1; 
            outerCircle.transform.position = pointA * -1;

            // Showing the joystick only when moving, so when not moving we can see more of the screen.
            circle.GetComponent<SpriteRenderer>().enabled = true;
            outerCircle.GetComponent<SpriteRenderer>().enabled = true;
        }
        if(Input.GetMouseButton(0)) // a finger is placed down
        {
            touchStart = true; // a finger is placed on the screen, signal that will be used in the FixedUpdate

            // Getting the coordinates where our finger actually is.
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }
        else
        {
            touchStart = false;
        }
        
	}
	private void FixedUpdate() 
    {
    // Movement calculations in the FixedUpdate. We're not doing it in the Update function because else our player would move only each frame, 
    // and if you have a low framerate it becomes unplayable. 

        if(touchStart) // a finger is placed down. Returns true each frame while the finger is still down.
        { 
            Vector2 offset = pointB - pointA; // offset of original and actual position of finger
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f); // clamping makes our direction be in a scale of -1 to 1
            moveCharacter(direction * -1); // -1 because the camera and the GUI use opposite coordinates, so we convert them

            circle.transform.position = new Vector2(pointA.x + direction.x, pointA.y + direction.y) * -1;
        }
        else
        {
            // Not moving, so we hide the joystick.
            circle.GetComponent<SpriteRenderer>().enabled = false;
            outerCircle.GetComponent<SpriteRenderer>().enabled = false;
        }

	}

	void moveCharacter(Vector2 direction)
    {
        // We're not modifiying the transform.position becazuse it's like teleporting the player and it could ead to numerous glitches.
        // We instead translate from the original to new coordinates.
        player.Translate(direction * speed * Time.deltaTime);
    }
}