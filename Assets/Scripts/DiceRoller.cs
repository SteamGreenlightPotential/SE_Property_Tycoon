using UnityEngine;
using System.Collections;

public class DiceRoller : MonoBehaviour
{
    public float spinSpeed = 720f;          // Spin speed (degrees per second)
    public float rotationDuration = 0.5f;   // Duration for smooth rotation correction
    public bool isSpinning = true;         // Flag to indicate if dice is spinning
    public bool hide = true;
    public bool IsWaitingForDiceRoll = false; // Detect if dice is currently rolling
    private string nameOfObject;

    // Store the result of the dice roll
    public int Result;

    // Flag to check if the dice has been rolled
    public bool HasRolled { get; private set; }

    public Quaternion[] faceRotations = new Quaternion[6]
    {
        Quaternion.Euler(0f, 0f, 0f),      // Face 1
        Quaternion.Euler(0f, 90f, 0f),     // Face 2
        Quaternion.Euler(-90f, 0f, 0f),     // Face 3
        Quaternion.Euler(90f, 0f, 0f),    // Face 4
        Quaternion.Euler(0f, -90f, 0f),    // Face 5
        Quaternion.Euler(180f, 0f, 0f)     // Face 6
    };

    private Vector3 diagonalAxis = new Vector3(1f, 1f, 1f).normalized;

    void Update()
    {
        // Spin the dice 
        if (isSpinning)
        {
            transform.Rotate(diagonalAxis, spinSpeed * Time.deltaTime, Space.World);
        }

        nameOfObject = gameObject.name;
        if (hide==true && nameOfObject == "dice1")
        {
           transform.position = new Vector3(1f,0f,4f);
        }
        else if (hide==false && nameOfObject == "dice1")
        {
           transform.position = new Vector3(1f,0f,-4f);
        }
        else if (hide==true && nameOfObject == "dice2")
        {
           transform.position = new Vector3(-1f,0f,4f);
        }
        else if (hide==false && nameOfObject == "dice2")
        {
           transform.position = new Vector3(-1f,0f,-4f);
        }
    }

    // Coroutine to rotate to the dice over a bit of time
    IEnumerator StopDiceRotation(Quaternion targetRotation, float duration)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Emake rotation exact
        transform.rotation = targetRotation;
        yield return new WaitForSeconds(1);
        hide = true;
    }

    public void PrepareRoll()
    {
        //Show the dice and start spinning it
        hide = false;
        isSpinning = true;
        HasRolled = false;
    }

    public IEnumerator CompleteRoll()
    {
        //Stop spinning the dice and output result
        isSpinning = false;
        Result = Random.Range(1, 7);
        Debug.Log("Dice Rolled: " + Result);

        Quaternion targetRotation = faceRotations[Result - 1];
        yield return StartCoroutine(StopDiceRotation(targetRotation, rotationDuration));
        HasRolled = true;
    }
}