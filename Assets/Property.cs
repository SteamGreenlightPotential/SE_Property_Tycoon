[System.Serializable] 

//Abstract class representing a property, initalised by gameManager
public class Property {
    public string name;
    
    public int price; //Cost to purchase 
    public bool owned;
    public Player owner;
    public string colour; //Can be Brown, Blue, Purple, Orange, Yellow, Green, DBlue
    public int baseRent;
    public int houses;
    public bool hotel;

    public Property(string name, int price, string colour, int baseRent){
        owned = false;
        owner = null;
        name = name;
        price = price;
        colour = colour;
        baseRent = baseRent;
        houses = 0;
        hotel = false;
    }


}