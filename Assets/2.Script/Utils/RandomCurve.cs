using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomCurve : MonoBehaviour
{

    // The start position of the game object
    [SerializeField]Transform start;

    // The end position of the game object
    [SerializeField]Transform end;

    // The number of steps to use for the curve
    int numSteps = 50;

    // The current position of the game object
    Vector3 currentPosition;

    void Start()
    {
        // Set the initial position of the game object
        transform.localPosition = start.localPosition;

        // Set the current position of the game object
        //currentPosition = start.localPosition;
        //currentPosition = new Vector3(Random.RandomRange(-150,150), start.localPosition.y,start.localPosition.z);

        StartCoroutine(StartCurve());
    }

    IEnumerator StartCurve()
    {
        while(true)
        {
            if (currentPosition != end.localPosition)
            {
                // Calculate the next position along the curve
                Vector2 nextPosition = Vector2.Lerp(currentPosition, end.localPosition, 1f/ numSteps);
                // Set the position of the game object
                transform.localPosition = nextPosition;

                // Set the current position to the next position
                currentPosition = nextPosition;

                yield return new WaitForEndOfFrame();

            }
            if ((int)currentPosition.y >= (int)end.localPosition.y)
                break;
        }
        // If the current position is not equal to the end position, move the game object along the curve

        Managers.Resource.Destroy(gameObject);

        yield return null;

    }

    private void Reset()
    {
        // Set the initial position of the game object
        transform.localPosition = start.localPosition;

        // Set the current position of the game object
        //currentPosition = start.localPosition;
        //currentPosition = new Vector3(Random.RandomRange(-150, 150), start.localPosition.y, start.localPosition.z);
    }

}
