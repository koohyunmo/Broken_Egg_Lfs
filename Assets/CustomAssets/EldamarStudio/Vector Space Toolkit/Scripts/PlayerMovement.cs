using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject EnginePS;
    public Camera playerCamera;
    public float movementSpeed = 50;
    public float sprint = 2f;
    public float rotationOffset = 280;

    bool accelerated = false;

    void Update()
    {
        // Call rotate function
        Rotate();

        float x = Input.GetAxisRaw("Horizontal"); //the value will be -1, 0, or 1 (fir left, no input, and right)
        float y = Input.GetAxisRaw("Vertical"); //the value will be -1, 0, or 1 (for down, no input, and up)

        if (x == 0 && y == 0)
        {
            if (EnginePS.activeSelf)
                EnginePS.SetActive(false);

            return;
        }

        // now based on the input we compute a direction vector, and we normalize it to get a unit vector
        Vector2 direction = new Vector2(x, y).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
            accelerated = true;
        else
            accelerated = false;

        // now we call the function that compuse and sets the player`s position
        Move(direction);
    }

    void Move(Vector2 direction)
    {
        //Find the screen limits to the player`s movement (left, right, top and bottom edges of the screen)
        Vector2 min = playerCamera.ViewportToWorldPoint(new Vector2(0, 0)); //this is the bottom-left point (corner) of the screen
        Vector2 max = playerCamera.ViewportToWorldPoint(new Vector2(1, 1)); //this is the top-tight point (corner) of the screen

        max.x = max.x - 0.0225f; // subtract the player sprite half width
        min.x = min.x + 0.0225f; // add the player sprite half wigth

        max.y = max.y - 0.285f; // subtract the player sprite half height
        min.y = min.y + 0.285f; // add the player sprite half height

        //Get the player`s current position
        Vector2 pos = transform.position;

        //Calculate the new position
        if (accelerated)
            pos += direction * (movementSpeed * sprint) * Time.deltaTime;
        else
            pos += direction * movementSpeed * Time.deltaTime;

        //Update the player`s position
        transform.position = pos;

        if (!EnginePS.activeSelf)
            EnginePS.SetActive(true);
    }

    void Rotate()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }
}