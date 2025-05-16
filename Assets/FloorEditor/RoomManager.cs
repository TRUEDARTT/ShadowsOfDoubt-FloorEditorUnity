using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Range(0, 0)]
    public int selectedRoomForPainting = 0;
    public List<Room> rooms = new List<Room>();
}

[Serializable]
public class Room
{
    public int id = -1;
    public Color color;
    public string roomPreset; // RoomTypePreset string reference
}

public static class RoomPresets
{
    public static readonly string[] RoomPreset = new[]
    {
        "Alley",
        "Atrium",
        "Backstreet",
        "Ballroom",
        "BasementLobby",
        "Bathroom",
        "Bedroom",
        "ControlRoom",
        "CorporateCorridoor",
        "CorporateLobby",
        "Cupboard",
        "Den",
        "Diner",
        "DiningRoom",
        "Eatery",
        "FathomsLobby",
        "FathomsYard",
        "Hallway",
        "HotelLobby",
        "Industrial",
        "IndustrialLobby",
        "Joiner",
        "Kitchen",
        "LivingRoom",
        "Lobby",
        "Null",
        "OfficeSpace",
        "Park",
        "Path",
        "PowerRoom",
        "PrivateOffice",
        "PublicBathroom",
        "PublicBathroomEatery",
        "Reception",
        "RetailSpace",
        "Rooftop",
        "RooftopBar",
        "RooftopVent",
        "ShowerRoom",
        "SpareBedroom",
        "Storeroom",
        "Street",
        "StreetFrontage",
        "Study",
        "Utility",
        "Warehouse",
        "Yard"
    };
}