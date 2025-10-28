//using UnityEngine;

//public class StretchableRope : MonoBehaviour
//{
//    private Vector2 mouseStartPos;  // The starting position of the mouse
//    private Vector2 mouseCurrentPos;  // The current position of the mouse
//    private Vector2 initialPosition;  // The initial position of the rope (to keep it centered)
//    private Vector2 initialScale;  // The initial scale of the rope

//    private void Start()
//    {
//        // Save the initial scale of the rope (square) at the start
//        initialScale = transform.localScale;
//        initialPosition = transform.position;

//        // Force the object to stay at Z = 0 when the game starts
//        transform.position = new Vector3(initialPosition.x, initialPosition.y, 0f);
//    }

//    private void Update()
//    {
//        // Detect if the left mouse button is pressed
//        if (Input.GetMouseButtonDown(0))  // Mouse Button 0 is the left button
//        {
//            // Record the starting position of the mouse
//            mouseStartPos = Input.mousePosition;
//        }
//        else if (Input.GetMouseButton(0))  // Mouse Button 0 is being held down
//        {
//            // Update the current mouse position
//            mouseCurrentPos = Input.mousePosition;

//            // Calculate the horizontal distance the mouse has moved (in pixels)
//            float horizontalDistance = mouseCurrentPos.x - mouseStartPos.x;

//            // Scale the rope's length based on the horizontal distance (only X-axis)
//            float scaleFactor = horizontalDistance / 100f; // Adjust 100f for desired scaling speed

//            // Scale the rope symmetrically along the X-axis
//            transform.localScale = new Vector3(scaleFactor, initialScale.y, 1f);

//            // Adjust the position to keep the rope centered
//            // Ensure Z position remains 0 for 2D purposes
//            transform.position = new Vector3(initialPosition.x - scaleFactor / 2f, initialPosition.y, 0f);

//            // Rotate the rope to face the mouse cursor
//            Vector2 direction = mouseCurrentPos - (Vector2)Camera.main.WorldToScreenPoint(transform.position);
//            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
//        }
//        else if (!Input.GetMouseButton(0))  // Mouse Button 0 is being held down
//        {
//            transform.localScale = new Vector3(initialScale.x, initialScale.y, 1f);
//        }
//    }
//}
using UnityEngine;

public class StretchableRope : MonoBehaviour
{
    public Transform anchor;  // The anchor point (fixed position) of the rope
    private Vector2 mouseStartPos;  // The starting position of the mouse
    private SpriteRenderer spriteRenderer;  // SpriteRenderer component of the rope
    private Vector2 initialScale;  // Initial scale of the sprite
    private Vector2 direction;  // Direction to the mouse
    private float scalingFactor = 1f;  // Factor to slow down the scaling (adjust this as needed)

    private void Start()
    {
        // Get the SpriteRenderer component of the rope
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Save the initial scale of the rope
        initialScale = spriteRenderer.transform.localScale;

        // Ensure the rope starts at the anchor position
        transform.position = anchor.position;
    }

    private void Update()
    {
        // Detect if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))  // Mouse Button 0 is the left button
        {
            // Record the starting position of the mouse
            mouseStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))  // Mouse Button 0 is being held down
        {
            // Update the current mouse position
            Vector2 mouseCurrentPos = Input.mousePosition;

            // Calculate the direction from the anchor to the mouse position
            direction = mouseCurrentPos - (Vector2)anchor.position;

            // Calculate the distance between the anchor and the mouse (this will control the rope's length)
            float distance = direction.magnitude;

            // Apply scaling factor to the distance (this controls how fast the rope stretches)
            float scaledDistance = distance * scalingFactor; // Adjust scalingFactor for more/less stretching speed

            // Stretch the rope's sprite based on the scaled distance (stretch along x-axis)
            spriteRenderer.transform.localScale = new Vector3(scaledDistance, initialScale.y, 1f); // Only stretch on x-axis

            // Keep the anchor position fixed (set the rope position to the anchor's position)
            transform.position = anchor.position;

            // Calculate the angle to rotate the rope to face the mouse cursor
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply the rotation to the rope
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else if (!Input.GetMouseButton(0))  // Mouse Button 0 is not being held down
        {
            // Reset the rope's scale when the mouse button is released
            spriteRenderer.transform.localScale = new Vector3(initialScale.x, initialScale.y, 1f);

            // Reset the rotation
            transform.rotation = Quaternion.identity;
        }
    }
}





