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
    // Scenes
    public static AsyncOperation DEMO_MENU_ASYNC_SCENE;
    public static AsyncOperation DEMO_TERRAIN_ASYNC_SCENE;
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
    public static List<string> tips = new List<string>();
    public static List<PhysicalElement> physical_elements = new List<PhysicalElement>();
}
