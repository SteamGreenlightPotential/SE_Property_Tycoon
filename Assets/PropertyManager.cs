using UnityEngine;
using System.Collections.Generic;

namespace PropertyTycoon
{
    public class PropertyManager : MonoBehaviour
    {
        public List<Property> properties = new List<Property>(); //Holds all properties

        //Initialises all properties. Hardcoded based on database files given by client
        public void initialiseProperties()
        {

            properties.Add(new Property("The Old Creek", 60, "Brown", 2));
            properties.Add(new Property("Gangsters Paradise", 60, "Brown", 4));
            properties.Add(new Property("The Angels Delight", 100, "Blue", 6));
            properties.Add(new Property("Potters Avenue", 100, "Blue", 6));
            properties.Add(new Property("Granger Drive", 120, "Blue", 8));
            properties.Add(new Property("Skywalker Drive", 140, "Purple", 10));
            properties.Add(new Property("Wookie Hole", 140, "Purple", 10));
            properties.Add(new Property("Rey Lane", 160, "Purple", 12));
            properties.Add(new Property("Bishop Drive", 180, "Orange", 14));
            properties.Add(new Property("Dunham Street", 180, "Orange", 14));
            properties.Add(new Property("Broyles Lane", 200, "Orange", 16));
            properties.Add(new Property("Yue Fei Square", 220, "Red", 18));
            properties.Add(new Property("Mulan Rouge", 220, "Red", 18));
            properties.Add(new Property("Han Xin Gardens", 240, "Red", 20));
            properties.Add(new Property("Shatner Close", 260, "Yellow", 22));
            properties.Add(new Property("Picard Avenue", 260, "Yellow", 22));
            properties.Add(new Property("Crusher Creek", 280, "Yellow", 22));
            properties.Add(new Property("Sirat Mews", 300, "Green", 26));
            properties.Add(new Property("Ghengis Crecent", 300, "Green", 26));
            properties.Add(new Property("Ibis Close", 320, "Green", 28));
            properties.Add(new Property("James Webb Way", 350, "DBlue", 35));
            properties.Add(new Property("Turing Heights", 400, "DBlue", 50));

            //this is a hacky fix but i'm really lazy
            int[] tileList = {2,4,7,9,10,12,14,15,17,19,20,22,24,25,27,28,30,32,33,35,38,40};
            int i = 0;
            foreach(Property p in properties){
                p.tileno = tileList[i];
                i+= 1;
            }

        //Check all imported properly
        Property item = properties[5];
                Debug.Log(item.name);
                Debug.Log(item.price);
                Debug.Log(item.tileno);
        
        }

        //Method to get property object from tile
        public Property getTileProperty(int tileno){
            foreach (Property p in properties){
                if (p.tileno == tileno){
                    return p;
                }
            }
            Debug.Log("No property with tile number " + tileno + "!");
            return null;
        } 

        //Initialises script when game starts 
        void Awake()
        {
            //Persists between scenes
            DontDestroyOnLoad(gameObject);
            //make sure they woke up
            Debug.Log("Game Manager Online");

            //Initialises all properties
            initialiseProperties();
        }




    }
}