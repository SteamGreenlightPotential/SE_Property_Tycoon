using System;

namespace PropertyTycoon
{
    [Serializable]
    public class Property
    {
        public string name;             // Name of the property
        public int price;               // Cost to purchase
        public string colour;           // Property group (e.g., "Brown", "Blue", etc.)
        public bool owned;              // Ownership status
        public Player owner;            // Current owner (null if owned by the bank)
        public int baseRent;            // Rent for unimproved property
        public int rentWith1House;      // Rent with 1 house
        public int rentWith2Houses;     // Rent with 2 houses
        public int rentWith3Houses;     // Rent with 3 houses
        public int rentWith4Houses;     // Rent with 4 houses
        public int rentWithHotel;       // Rent with a hotel
        public int houses;              // Number of houses (0-4)
        public bool hotel;              // True if the property has a hotel
        public int houseCost;           // Cost to build a house
        public int hotelCost;           // Cost to build a hotel
        public int tileno { get; set; } // Tile number for property location tracking
        public bool mortgaged=false;    //Toggle fort mortgaged state

        // Constructor for properties with full rent details
        public Property(string name, int price, string colour, int baseRent, int rent1, int rent2, int rent3, int rent4, int hotelRent)
        {
            this.name = name;
            this.price = price;
            this.colour = colour;
            this.baseRent = baseRent;
            this.rentWith1House = rent1;
            this.rentWith2Houses = rent2;
            this.rentWith3Houses = rent3;
            this.rentWith4Houses = rent4;
            this.rentWithHotel = hotelRent;
            this.houses = 0;
            this.hotel = false;
            this.owned = false;
            this.owner = null;
        }

        // Constructor for simplified properties (if needed)
        public Property(string name, int price, string colour, int baseRent)
        {
            this.name = name;
            this.price = price;
            this.colour = colour;
            this.baseRent = baseRent;
            this.houses = 0;
            this.hotel = false;
            this.owned = false;
            this.owner = null;
        }

        // Method to add a house to the property
        public void addHouse()
        {
            if (houses < 4 && !hotel)
            {
                houses++;
            }
        }

        // Method to add a hotel (requires 4 houses)
        public void addHotel()
        {
            if (houses == 4)
            {
                houses = 0; // Reset houses
                hotel = true;
            }
        }

        // Method to switch ownership to a new player
        public void SwitchOwner(Player newOwner)
        {
            owner = newOwner;
            owned = true;
        }

        // Method to remove ownership
        public void RemoveOwner()
        {
            owner = null;
            owned = false;
            houses = 0;
            hotel = false;
        }

        //Toggle mortgage, return true or false depending on whether mortgaging or unmorgaging
        public bool toggleMortgage(){
            if (mortgaged){
                mortgaged=false;
                Turn_Script.Instance.CheckBankruptcy(owner);
                return false;
            }
            else{
                mortgaged=true;
                Turn_Script.Instance.CheckBankruptcy(owner);
                return true;
            }
        }

        // Check if the property can have a house added
        public bool CanAddHouse(Player owner)
        {
            return owner.OwnsAllPropertiesInColorGroup(colour) && !hotel && houses < 4;
        }

        // Check if the property can have a hotel added
        public bool CanAddHotel(Player owner)
        {
            return owner.OwnsAllPropertiesInColorGroup(colour) && houses == 4 && !hotel;
        }

    }
}