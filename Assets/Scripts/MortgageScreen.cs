using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace PropertyTycoon
{
    public class MortgageScreen : MonoBehaviour
    {
        private int activeCount = 0;
       
        public Button[] buttonList = new Button[20];            

        

        private void Start()
        {

            // Ensure the mortgage panel is hidden when not in use 
            gameObject.SetActive(false);

            //hide all unused buttons
            foreach (Button b in buttonList){
                b.gameObject.SetActive(false);
            }
            
        }

        public void mortgageCall(boardPlayer bplayer){
            //Name buttons right stuff
            int counter = 0;
            foreach(Property p in bplayer.OwnedProperties){
                buttonList[counter].GetComponentInChildren<Text>().text=p.name;
                counter++;
            }
            for(int i=0;i!=counter;i++){
                buttonList[i].gameObject.SetActive(true);
            }
            activeCount=counter;
        }

        private void buttonPressed(int buttonNo){

        }
        
        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
