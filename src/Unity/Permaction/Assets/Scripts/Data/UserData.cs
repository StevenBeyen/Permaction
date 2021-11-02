using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using API;
using Graphical;

public static class UserData
{
    // Initialization
    public static MetaData meta_data = new MetaData();
    public static User user;
    // Menu
    public static List<int> selectedElementIds;
    public static List<Element> selectedElements = new List<Element>();
    // Terrain
    public static string[][] terrain_heightmap;
    public static bool terrain_loaded = false;
    public static PlacementReply reply;
    public static bool elements_loaded = false;
    public static GraphicalElement selectedElement = null;
    // Additional data
    public static BinaryInteractions binaryInteractions = null;
    public static List<PhysicalElement> physicalElements = new List<PhysicalElement>();
}
