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

            properties.Add(new Property("The Old Creek", 60, "Brown", 2, 10, 30, 90, 160, 250));  //2
            properties.Add(new Property("Gangsters Paradise", 60, "Brown", 4, 20, 60, 180, 320, 450)); //4
            properties.Add(new Property("The Angels Delight", 100, "Blue", 6, 30, 90, 270, 400, 550)); //7
            properties.Add(new Property("Potters Avenue", 100, "Blue", 6, 30, 90, 270, 400, 550)); //9
            properties.Add(new Property("Granger Drive", 120, "Blue", 8, 40, 100, 300, 450, 600));  //10
            properties.Add(new Property("Skywalker Drive", 140, "Purple", 10, 50, 150, 450, 625, 750)); //12
            properties.Add(new Property("Wookie Hole", 140, "Purple", 10, 50, 150, 450, 625, 750)); //14
            properties.Add(new Property("Rey Lane", 160, "Purple", 12, 60, 180, 500, 700, 900));    //15
            properties.Add(new Property("Bishop Drive", 180, "Orange", 14, 70, 200, 550, 750, 950));    //17
            properties.Add(new Property("Dunham Street", 180, "Orange", 14, 70, 200, 550, 750, 950));   //19
            properties.Add(new Property("Broyles Lane", 200, "Orange", 16, 80, 220, 600, 800, 1000));    //20
            properties.Add(new Property("Yue Fei Square", 220, "Red", 18, 90, 250, 700, 875, 1050));   //22
            properties.Add(new Property("Mulan Rouge", 220, "Red", 18, 90, 250, 700, 875, 1050));    //24
            properties.Add(new Property("Han Xin Gardens", 240, "Red", 20, 100, 300, 750, 925, 1100));    //25    
            properties.Add(new Property("Shatner Close", 260, "Yellow", 22, 110, 330, 800, 975, 1150));   //27
            properties.Add(new Property("Picard Avenue", 260, "Yellow", 22, 110, 330, 800, 975, 1150));   //28
            properties.Add(new Property("Crusher Creek", 280, "Yellow", 22, 120, 360, 850, 1025, 1200));   //30
            properties.Add(new Property("Sirat Mews", 300, "Green", 26, 130, 390, 900, 1100, 1275));   //32
            properties.Add(new Property("Ghengis Crecent", 300, "Green", 26, 130, 390, 900, 1100, 1275));  //33
            properties.Add(new Property("Ibis Close", 320, "Green", 28, 150, 450, 1000, 1200, 1400));   //35
            properties.Add(new Property("James Webb Way", 350, "DBlue", 35, 175, 500, 1100, 1300, 1500));   //38
            properties.Add(new Property("Turing Heights", 400, "DBlue", 50, 200, 600, 1200, 1400, 1600));   //40

            //this is a hacky fix but i'm really lazy  (*-*)
            int[] tileList = {2,4,7,9,10,12,14,15,17,19,20,22,24,25,27,28,30,32,33,35,38,40};
            int i = 0;
            foreach(Property p in properties)
            {
                p.tileno = tileList[i];
                i+= 1;
            }

        //Check all imported properly
        Property item = properties[5];
                //Debug.Log(item.name);
                //Debug.Log(item.price);
                //Debug.Log(item.tileno);
        
        }

        //Method to get property object from tile
        public Property getTileProperty(int tileno){
            foreach (Property p in properties)
            {
                if (p.tileno == tileno)
                {
                    //Debug.Log("Returned property "+ p.name);
                    return p;
                }
            }
            return null;
        } 

        //Initialises script when game starts 
        void Awake()
        {
            //Persists between scenes
            DontDestroyOnLoad(gameObject);
            //make sure they woke up
            //Debug.Log("Game Manager Online");

            //Initialises all properties
            initialiseProperties();
        }
    }
}