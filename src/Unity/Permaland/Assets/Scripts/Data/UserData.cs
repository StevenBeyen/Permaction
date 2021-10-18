using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using API;
using Graphical;

public static class UserData
{
    public static MetaData meta_data = new MetaData();
    public static User user;
    public static List<int> selectedElementIds;
    public static List<Element> selectedElements = new List<Element>();
    public static string[][] terrain_heightmap;
    public static bool terrain_loaded = false;
    public static PlacementReply reply;
    public static bool elements_loaded = false;
    public static GraphicalElement selectedElement = null;
}
