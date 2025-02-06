using System.Collections;
using UnityEngine;

public class Grid_Movement : MonoBehaviour
{
    private bool isMoving; //Prevent multiple movements at once
    private Vector3 origPos, targetPos;
    private float TimeToMove = 0.2f;
    private int TileCount = 0; //Check which tile the player is on

    //Board boundaries
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -5f;
    public float maxY = 5f;

    void Update()
    {
        if (TileCount == 40) //Loops the board
            TileCount = 0;

        if (Input.GetKeyDown(KeyCode.Space) && !isMoving) //Temporary roll with space
        {
            int RandNum = RollDice();
            Debug.Log("Roll: " + RandNum);

            StartCoroutine(ProcessMovements(RandNum)); //Starts the movement function
        }
    }

    //Process movement one by one
    private IEnumerator ProcessMovements(int steps) 
    {
        for (int i = 0; i < steps; i++)
        {
            Vector3 direction = NextDir(); 
            yield return StartCoroutine(MovePlayer(direction)); //Wait for this movement to finish
        }
    }
    //Get direction based on TileCount
    private Vector3 NextDir() 
    {
        Vector3 direction = Vector3.zero;

        if (TileCount >= 0 && TileCount < 10) //Top row (0-9)
        {
            direction = Vector3.right;
        }
        else if (TileCount >= 10 && TileCount < 20) //Right row (10-19)
        {
            direction = Vector3.down;
        }
        else if (TileCount >= 20 && TileCount < 30) //Bottom row (20-29)
        {
            direction = Vector3.left;
        }
        else if (TileCount >= 30 && TileCount < 40) //Left row (30-39)
        {
            direction = Vector3.up;
        }
        else
        {
            TileCount = 0; //Resets if TileCount exceeds 39
            direction = Vector3.right;
        }

        TileCount += 1; //Increment TileCount after getting direction
        return direction;
    }

    //Player movement function
    private IEnumerator MovePlayer(Vector3 direction) 
    {
        isMoving = true; //Lets program know a movement is in prograss

        float elapsedTime = 0;
        
        origPos = transform.position;
        targetPos = origPos + direction;

        //Check if the target position is in the boundaries
        if (targetPos.x < minX) targetPos.x = minX;
        if (targetPos.x > maxX) targetPos.x = maxX;
        if (targetPos.y < minY) targetPos.y = minY;
        if (targetPos.y > maxY) targetPos.y = maxY;

        while (elapsedTime < TimeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / TimeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false; //Lets program know movement has ended
    }

    //Function to roll a dice )
    private int RollDice()
    {
        return Random.Range(1, 7); //Rolls 1-6
    }
}