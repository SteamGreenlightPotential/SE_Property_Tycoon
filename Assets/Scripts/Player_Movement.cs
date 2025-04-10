using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace PropertyTycoon
{
    public class boardPlayer : MonoBehaviour
    {
        public List<Property> OwnedProperties = new List<Property>(); // Create a list  to store owned properties
        private Vector3 origPos, targetPos;
        private float TimeToMove = 0.2f;
        
        public int TileCount = 1; //Starts at 1 to match the excel spreadsheet tile counts (makes life easier i promise)
        public bool goPassed = false;
        public bool inJail = false; //Player is in jail
        public int jailTurns = 0; //Number of turns player has been in jail
        private string nameOfObject; //Get the name of the object using the script

        public IEnumerator Move(int steps)  // Called from Turn_Script to trigger movement for 1 turn
        {
            yield return StartCoroutine(ProcessMovements(steps));
        }

        IEnumerator ProcessMovements(int steps)
        {
            for (int i = 0; i < steps; i++) // For each tile crossed check direction and move player
            {
                Vector3 direction = NextDir();
                yield return StartCoroutine(MovePlayer(direction));
            }
        }

        //like moving but FASTER. Assumes the timescale has been set to 100 before being initiated
        public IEnumerator ProcessTeleport(int steps,float originalTimeScale)
        {
            for (int i = 0; i < steps; i++) // For each tile crossed check direction and move player
            {
                Vector3 direction = NextDir();
                yield return StartCoroutine(TeleportPlayer(direction));

            }
            Time.timeScale = originalTimeScale; //reset timescale
            yield return null;
        }

        public Vector3 NextDir()
        {
            nameOfObject = gameObject.name;
            Vector3 direction = Vector3.zero;

            if (nameOfObject == "car")
            {
                if (TileCount >= 1 && TileCount < 11)
                {
                    direction = Vector3.up;
                    transform.eulerAngles = new Vector3(180, 0, 0);
                }
                else if (TileCount >= 11 && TileCount < 21)
                {
                    direction = Vector3.right;
                    transform.eulerAngles = new Vector3(180, 0, 90);
                }
                else if (TileCount >= 21 && TileCount < 31)
                {
                    direction = Vector3.down;
                    transform.eulerAngles = new Vector3(180, 0, 180);
                }
                else if (TileCount >= 31 && TileCount < 41)
                {
                    direction = Vector3.left;
                    transform.eulerAngles = new Vector3(180, 0, 270);
                }
                else
                {
                    TileCount = 1;
                    direction = Vector3.up;
                    goPassed = true;
                }
            }
            else if (nameOfObject == "iron")
            {
                if (TileCount >= 1 && TileCount < 11)
                {
                    direction = Vector3.up;
                    transform.eulerAngles = new Vector3(0, -90, 90);
                }
                else if (TileCount >= 11 && TileCount < 21)
                {
                    direction = Vector3.right;
                    transform.eulerAngles = new Vector3(90, -90, 90);
                }
                else if (TileCount >= 21 && TileCount < 31)
                {
                    direction = Vector3.down;
                    transform.eulerAngles = new Vector3(0, -90, 90);
                }
                else if (TileCount >= 31 && TileCount < 41)
                {
                    direction = Vector3.left;
                    transform.eulerAngles = new Vector3(90, -90, 90);
                }
                else
                {
                    TileCount = 1;
                    direction = Vector3.up;
                    goPassed = true;
                }
            }
            else if (nameOfObject == "smartphone")
            {
                if (TileCount >= 1 && TileCount < 11)
                {
                    direction = Vector3.up;
                    transform.eulerAngles = new Vector3(90, 90, 0);
                }
                else if (TileCount >= 11 && TileCount < 21)
                {
                    direction = Vector3.right;
                    transform.eulerAngles = new Vector3(180, 90, 0);
                }
                else if (TileCount >= 21 && TileCount < 31)
                {
                    direction = Vector3.down;
                    transform.eulerAngles = new Vector3(90, 90, 0);
                }
                else if (TileCount >= 31 && TileCount < 41)
                {
                    direction = Vector3.left;
                    transform.eulerAngles = new Vector3(180, 90, 0);
                }
                else
                {
                    TileCount = 1;
                    direction = Vector3.up;
                    goPassed = true;
                }
            }
            else
            {
                // Default behavior for other objects
                if (TileCount >= 1 && TileCount < 11)
                {
                    direction = Vector3.up;
                    transform.eulerAngles = new Vector3(180, 0, 180);
                }
                else if (TileCount >= 11 && TileCount < 21)
                {
                    direction = Vector3.right;
                    transform.eulerAngles = new Vector3(180, 0, 270);
                }
                else if (TileCount >= 21 && TileCount < 31)
                {
                    direction = Vector3.down;
                    transform.eulerAngles = new Vector3(180, 0, 0);
                }
                else if (TileCount >= 31 && TileCount < 41)
                {
                    direction = Vector3.left;
                    transform.eulerAngles = new Vector3(180, 0, 90);
                }
                else
                {
                    TileCount = 1;
                    direction = Vector3.up;
                    goPassed = true;
                }
            }

            TileCount += 1;
            return direction;
        }


        private IEnumerator MovePlayer(Vector3 direction)
        {
            origPos = transform.position; // Store current position
            targetPos = origPos + direction; // Store target position

            float startTime = Time.realtimeSinceStartup;
            float elapsed = 0;
            while (elapsed < TimeToMove)
            {
                elapsed = Time.realtimeSinceStartup - startTime;
                float t = Mathf.Clamp01(elapsed / TimeToMove);
                transform.position = Vector3.Lerp(origPos, targetPos, t);
                yield return null;
            }

            transform.position = targetPos; // Make sure piece actually reaches destination
        }
        private IEnumerator TeleportPlayer(Vector3 direction)
        {
            origPos = transform.position; // Store current position
            targetPos = origPos + direction; // Store target position

            float startTime = Time.time;
            float elapsed = 0;
            while (elapsed < TimeToMove)
            {
            elapsed = Time.time - startTime;
            float t = Mathf.Clamp01(elapsed / TimeToMove);
            transform.position = Vector3.Lerp(origPos, targetPos, t);
            yield return null;
            }

            transform.position = targetPos; // Make sure piece actually reaches destination
        }

        



        public int balance = 1500;

        public void taxCheck()
        {
            if (TileCount == 5)
            {
                balance -= 100;
                Debug.Log("Tax tile. New balance is " + balance);
            }

            if (TileCount == 39)
            {
                balance -= 100;
                Debug.Log("Tax tile. New balance is " + balance);
            }
        }


        //Checks the given property against the properties owned by the player to see if they have a monopoly
        public bool monopolyCheck(Player player, Property property)
        {
            Dictionary<string, int> colours = new Dictionary<string, int>();
            foreach (Property p in player.bPlayer.OwnedProperties){
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
        
    

    public void BuyTile(Property property, Player owner) //Buys property, assigns it to owner
        {
            int cost = property.price;
            if (balance >= cost)
            {
                balance -= cost;
                OwnedProperties.Add(property);
                property.owner = owner;
                property.owned = true;
                Debug.Log("Tile " + property.name + " purchased. New balance is " + balance);
            }
            else
            {
                Debug.Log("Not enough money to purchase tile " + property.name + ". Balance: " + balance);
            }
        }

        public void PayRent(int rent,Property p)
        {
            //Get owner of property
            Player owner = p.owner;
            boardPlayer bplayer = owner.bPlayer;
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
    
        public IEnumerator toJail(){
        
        // set timescale to 100 to save me time making a player teleport
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 100f; //make piece "teleport" to jail
        int jailDistance = 40 - TileCount + 11; // distance to jail
        yield return StartCoroutine(ProcessTeleport(jailDistance,originalTimeScale)); // move player to jail
        Time.timeScale = originalTimeScale; //reset timescale
        }
    }
}
