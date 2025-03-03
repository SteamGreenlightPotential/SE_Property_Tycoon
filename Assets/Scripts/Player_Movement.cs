using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using PropertyTycoon;

namespace PropertyTycoon
{
    public class boardPlayer : MonoBehaviour
    {
        public List<Property> OwnedProperties = new List<Property>(); // Create a list  to store owned properties
        private Vector3 origPos, targetPos;
        private float TimeToMove = 0.2f;
        public int TileCount = 0;

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

        // temporary wallet example

        private int balance = 1500;
        //private int amount = 0;

        public void taxCheck()
        {
            if (TileCount == 4)
            {
                balance -= 100;
                Debug.Log("Tax tile. New balance is " + balance);
            }

            if (TileCount == 38)
            {
                balance -= 100;
                Debug.Log("Tax tile. New balance is " + balance);
            }
        }


        //Checks the given property against the properties owned by the player to see if they have a monopoly
        public bool monopolyCheck(Player player, Property property)
        {
            Dictionary<string, int> colours = new Dictionary<string, int>();
            foreach (Property p in OwnedProperties){
                if (colours.ContainsKey(p.colour)==false){
                    colours.Add(p.colour,1);
                }
                else{
                    colours[p.colour]++;
                }
            }
            if (property.colour == "Brown" || property.colour == "DBlue"){
                return colours[property.colour] >1;
            } 
            else{
                return colours[property.colour] >2;
            }
            

        }
        
    
    public void BuyTile(Property property)
        {
            int cost = property.price;
            if (balance >= cost)
            {
                balance -= cost;
                OwnedProperties.Add(property);
                Debug.Log("Tile " + property.name + " purchased. New balance is " + balance);
            }
            else
            {
                Debug.Log("Not enough money to purchase tile " + property.name + ". Balance: " + balance);
            }
        }

        public void PayRent(Player owner,boardPlayer bplayer ,int rent,Property p)
        {
            //checks for a monopoly for passed property. If true, double rent if there aren't any houses
            if (monopolyCheck(owner,p)==true && p.houses == 0 && p.hotel == false){
                rent = rent * 2;}
            
            if (balance >= rent)
            {
                balance -= rent;
                bplayer.ReceiveRent(rent);
                Debug.Log("Paid rent of £" + rent + ". New balance is " + balance);
            }
            else
            {
                Debug.Log("Not enough money to pay rent. Current balance: " + balance);
            }
        }

        public void ReceiveRent(int rent)
        {
            balance += rent;
            Debug.Log("Received £" + rent + " of rent. New balance is " + balance);
        }
    }
}
