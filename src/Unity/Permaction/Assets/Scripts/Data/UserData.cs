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
    public static List<int> selected_element_ids;
    public static List<Element> selected_elements = new List<Element>();
    // Terrain
    public static string[][] terrain_heightmap;
    public static bool terrain_loaded = false;
    public static PlacementReply reply;
    public static bool elements_loaded = false;
    public static GraphicalElement selected_element = null;
    // Additional data
    public static BinaryInteractions binary_interactions = null;
    public static List<PhysicalElement> physical_elements = new List<PhysicalElement>();
}
