[System.Serializable] 

//Abstract class representing a property, initalised by gameManager
public class Property {
    public string name;
    
    public int price; //Cost to purchase 
    public bool owned; 
    public Player owner; //Current owner. Starts at null (owned by bank)
    public string colour; //Can be Brown, Blue, Purple, Orange, Yellow, Green, DBlue
    public int baseRent; //initial rent
    public int houses; //Max of 4
    public bool hotel; //If hotel is true, there must be no houses

    public Property(string newname, int newprice, string newcolour, int newbaseRent){
        name = newname;
        price = newprice;
        colour = newcolour;
        baseRent = newbaseRent;
        owned = false;
        owner = null;
        houses = 0;
        hotel = false;
    }

    //function to add 1 house to property
    public void addHouse(){
        if (houses + 1 != 5 && hotel == false){
            houses += 1;
        }
    }
    //Adds hotel, gets rid of all houses
    public void addHotel(){
        if (houses == 4){
            houses = 0;
            hotel = true;
        }
    }

    //function to switch owners. If removing an owner, use removeOwner
    public void switchOwner(Player newOwner){
        owner = newOwner;
    
    }
    
    //removes owner. Used when selling property
    public void removeOwner(){
        owner = null;
        houses = 0;
        hotel = false;
    }


}