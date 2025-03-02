using System.Collections;
using UnityEngine;

public class Grid_Movement : MonoBehaviour
{
    private Vector3 origPos, targetPos;
    private float TimeToMove = 0.2f;
    private int TileCount = 0;

    public void Move(int steps)  // Called from Turn_Script to trigger movement for 1 turn
    {
        StartCoroutine(ProcessMovements(steps));
    }

    private IEnumerator ProcessMovements(int steps)
    {
        for (int i = 0; i < steps; i++) // For each tile crossed check direction and move player
        {
            Vector3 direction = NextDir();
            yield return StartCoroutine(MovePlayer(direction));
        }
    }

    private Vector3 NextDir()
    {
        Vector3 direction = Vector3.zero;
        if (TileCount >= 0 && TileCount < 10)
        {
            direction = Vector3.right; // Move price right
            transform.eulerAngles = new Vector3(180, 0, 270); // Rotate to face right
        }
        else if (TileCount >= 10 && TileCount < 20)
        {
            direction = Vector3.down; // Move price down 
            transform.eulerAngles = new Vector3(180, 0, 0); // Rotate to face down
        }
        else if (TileCount >= 20 && TileCount < 30)
        {
            direction = Vector3.left; // Move price left
            transform.eulerAngles = new Vector3(180, 0, 90); // Rotate to face left
        }
        else if (TileCount >= 30 && TileCount < 40)
        {
            direction = Vector3.up; // Move price up
            transform.eulerAngles = new Vector3(180, 0, 180); // Rotate to face up
        }
        else
        {
            TileCount = 0; // Reset TileCount to loop board
            direction = Vector3.right; // Reset direction to right
        }

        TileCount += 1; // Increment TileCount for each tile moved across
        return direction;
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        float elapsedTime = 0;
        origPos = transform.position; // Store current position
        targetPos = origPos + direction; // Store target position

        while (elapsedTime < TimeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / TimeToMove)); // Move piece to target position over time
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos; // Make sure piece actually reaches destination
    }
}