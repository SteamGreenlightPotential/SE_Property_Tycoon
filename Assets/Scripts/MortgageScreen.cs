using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace PropertyTycoon
{
    public class MortgageScreen : MonoBehaviour
    {
        public boardPlayer currentPlayer;
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

        //Render then make visible screen
        public void mortgageCall(boardPlayer bplayer){
            //Name buttons right stuff
            currentPlayer=bplayer;
            int counter = 0;
            foreach(Property p in bplayer.OwnedProperties){
                buttonList[counter].GetComponentInChildren<Text>().text=p.name;
                counter++;
            }
            for(int i=0;i!=counter;i++){
                buttonList[i].gameObject.SetActive(true);
            }
            activeCount=counter;
            gameObject.SetActive(true);
        }

        public void buttonPressed(int buttonNo){
            buttonNo-=1;
            Property property = currentPlayer.OwnedProperties[buttonNo];
            //If mortgage is successful give player money, if unmortgaged then take away money 
            if (property.toggleMortgage()==true){
                currentPlayer.balance+= property.price/2;
                Debug.Log("Mortgaged property "+property.name);
            }
            else{
                if (property.price/2 > currentPlayer.balance){
                    Debug.Log("Too broke to unmortgage "+ property.name);
                }
                else{
                currentPlayer.balance-= property.price/2;
                 Debug.Log("Mortgaged property "+property.name);
                }
            }
        }
        public void DoneButton(){
            for(int i=0;i!=activeCount;i++){
                buttonList[i].gameObject.SetActive(false);
            }
            gameObject.SetActive(false);

        }
        
        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
