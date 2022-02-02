using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using API;

public class MetaData
{
    // Server
    public const string API_URI = "permaction.com:13731";
    public const string BINARY_INTERACTIONS_URI = API_URI + "/binary_interactions";
    public const string USER_LOGIN_URI = API_URI + "/login";
    public const string PHYSICAL_ELEMENTS_URI = API_URI + "/physical_elements";
    public const string PLACEMENT_REQUEST_URI = API_URI + "/placement_request";
    public const string USER_SIGNUP_URI = API_URI + "/signup";
    // Demo
    public const int DEMO_MAX_NB_ELEMENTS = 10;
    public const string DEMO_TERRAIN_SCENE = "DemoTerrain";
    public GameObject[] demo_lives = new GameObject[DEMO_MAX_NB_ELEMENTS];
    public int current_active_demo_lives = DEMO_MAX_NB_ELEMENTS;
    // Prefabs
    private const string STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH = "Prefabs/StretchableOneMeterTerrainObjects/";
    private const string FIXED_SIZE_TERRAIN_OBJECTS = "Prefabs/FixedSizeTerrainObjects/";
    public Dictionary<int, List<string>> prefab_mapping;
    public Dictionary<int, int> prefab_fixed_size_widths;
    public Dictionary<int, int> prefab_fixed_size_lengths;
    public const int PREFAB_FIXED_SIZE_DEFAULT_LENGTH = 1;
    // Menu
    public const string BG_TAG = "Background";
    public const string ICON_TAG = "Icon";
    public const string COUNTER_TAG = "Counter";
    public const string SECOND_COUNTER_TAG = "Counter2";
    private const string ICONS_RESOURCES_PATH = "Icons/";
    public Dictionary<string, string> icon_mapping;
    public Dictionary<int, string> element_name_data;
    public Dictionary<int, bool> terrain_flattening_data;
    public Dictionary<int, bool> show_interactions_data;
    // Locale
    public Dictionary<string, int> id_locale_mapping;
    // Terrain
    public static Terrain terrain;
    public List<string> prefab_grass;
    public List<string> prefab_rocks;
    public List<string> prefab_trees;
    public const string ARC_LINK_CONTAINER = "ArcLinkContainer";
    public const float NON_FLATTENING_HEIGHT_MARGIN = 0.0035f;
    public const float EPSILON = 0.001f;
    public const float EMPTY_COLLIDER_HEIGHT_CORRECTION = 10.0f;
    public const float BOX_COLLIDER_ADDITIONAL_CORRECTION = 0.5f;

    public MetaData()
    {
        // 1. ID locale
        id_locale_mapping = new Dictionary<string, int>();
        id_locale_mapping.Add("en", 1);
        id_locale_mapping.Add("fr", 2);

        // 2. Prefabs
        // Prefab grass, rocks & trees
        prefab_grass = new List<string>();
        prefab_grass.Add(FIXED_SIZE_TERRAIN_OBJECTS + "LPW_Grass_A1_40cm_01");
        prefab_grass.Add(FIXED_SIZE_TERRAIN_OBJECTS + "LPW_Grass_A1_50cm_01");
        prefab_grass.Add(FIXED_SIZE_TERRAIN_OBJECTS + "PP_Grass_11");
        prefab_grass.Add(FIXED_SIZE_TERRAIN_OBJECTS + "PP_Grass_12");
        prefab_grass.Add(FIXED_SIZE_TERRAIN_OBJECTS + "PP_Grass_15");
        prefab_grass.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Grass_Patch_01");
        prefab_grass.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Grass_Patch_03");
        prefab_rocks = new List<string>();
        prefab_rocks.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Small_Rocks_01");
        prefab_rocks.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Small_Rocks_02");
        prefab_rocks.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Small_Rocks_03");
        prefab_rocks.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Small_Rocks_04");
        prefab_rocks.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Small_Rocks_05");
        prefab_trees = new List<string>();
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "LPW_Tree_A1_6.5m_01");
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "LPW_Tree_B1_9m_01");
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "LPW_Tree_B1_9m_02");
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "PP_Birch_Tree_05");
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "PP_Birch_Tree_06");
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "PP_Tree_02");
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "PP_Tree_10");
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Env_Tree_Apple_01");
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Env_Tree_Apricot_01");
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Env_Tree_Cherry_01");
        //prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Env_Tree_Large_01"); // Big tree with swing, should be unique
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Env_Tree_Lemon_01");
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Env_Tree_Orange_01");
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Env_Tree_Peach_01");
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Env_Tree_Pear_01");
        prefab_trees.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Env_Tree_Plum_01");
        // Mapping of prefab objects to their ID.
        prefab_mapping = new Dictionary<int, List<string>>();
        // English
        PrefabMappingAdd(02, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Aromatic_Plants_01");
        PrefabMappingAdd(03, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Barn_01");
        PrefabMappingAdd(04, FIXED_SIZE_TERRAIN_OBJECTS + "Beehive_01");
        PrefabMappingAdd(07, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Composter_01");
        PrefabMappingAdd(08, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Composting_Toilet_01");
        PrefabMappingAdd(09, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Poultry_Feed_Forest_01");
        PrefabMappingAdd(11, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Flower_Meadow_01");
        PrefabMappingAdd(12, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Forest_01");
        PrefabMappingAdd(13, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Garden_01");
        PrefabMappingAdd(14, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Greenhouse_01");
        PrefabMappingAdd(15, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Sunroom_01");
        PrefabMappingAdd(17, FIXED_SIZE_TERRAIN_OBJECTS + "Hedge_01");
        PrefabMappingAdd(19, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Henhouse_01");
        PrefabMappingAdd(20, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Farmhouse_01");
        PrefabMappingAdd(22, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Medicinal_Plants_01");
        PrefabMappingAdd(24, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Orchard_01");
        //PrefabMappingAdd(25, FIXED_SIZE_TERRAIN_OBJECTS + "Path_Straight_01"); // Paths should not be straight
        PrefabMappingAdd(25, FIXED_SIZE_TERRAIN_OBJECTS + "Path_Straight_02");
        PrefabMappingAdd(25, FIXED_SIZE_TERRAIN_OBJECTS + "Path_Straight_03");
        PrefabMappingAdd(26, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Phytopurification_01");
        PrefabMappingAdd(27, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Pine_Trees_01");
        PrefabMappingAdd(28, FIXED_SIZE_TERRAIN_OBJECTS + "Road_Straight_01");
        PrefabMappingAdd(28, FIXED_SIZE_TERRAIN_OBJECTS + "Road_Straight_02");
        PrefabMappingAdd(28, FIXED_SIZE_TERRAIN_OBJECTS + "Road_Straight_03");
        PrefabMappingAdd(32, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Tool_Shed_01");
        PrefabMappingAdd(33, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Main_Square_01");
        PrefabMappingAdd(35, FIXED_SIZE_TERRAIN_OBJECTS + "Vegetable_Garden_01");
        PrefabMappingAdd(37, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Pond_01");
        // French
        PrefabMappingAdd(42, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Aromatic_Plants_01");
        PrefabMappingAdd(43, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Barn_01");
        PrefabMappingAdd(44, FIXED_SIZE_TERRAIN_OBJECTS + "Beehive_01");
        PrefabMappingAdd(47, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Composter_01");
        PrefabMappingAdd(48, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Composting_Toilet_01");
        PrefabMappingAdd(49, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Poultry_Feed_Forest_01");
        PrefabMappingAdd(51, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Flower_Meadow_01");
        PrefabMappingAdd(52, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Forest_01");
        PrefabMappingAdd(53, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Garden_01");
        PrefabMappingAdd(54, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Greenhouse_01");
        PrefabMappingAdd(55, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Sunroom_01");
        PrefabMappingAdd(57, FIXED_SIZE_TERRAIN_OBJECTS + "Hedge_01");
        PrefabMappingAdd(59, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Henhouse_01");
        PrefabMappingAdd(60, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Farmhouse_01");
        PrefabMappingAdd(62, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Medicinal_Plants_01");
        PrefabMappingAdd(64, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Orchard_01");
        //PrefabMappingAdd(65, FIXED_SIZE_TERRAIN_OBJECTS + "Path_Straight_01"); // Paths should not be straight
        PrefabMappingAdd(65, FIXED_SIZE_TERRAIN_OBJECTS + "Path_Straight_02");
        PrefabMappingAdd(65, FIXED_SIZE_TERRAIN_OBJECTS + "Path_Straight_03");
        PrefabMappingAdd(66, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Phytopurification_01");
        PrefabMappingAdd(67, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Pine_Trees_01");
        PrefabMappingAdd(68, FIXED_SIZE_TERRAIN_OBJECTS + "Road_Straight_01");
        PrefabMappingAdd(68, FIXED_SIZE_TERRAIN_OBJECTS + "Road_Straight_02");
        PrefabMappingAdd(68, FIXED_SIZE_TERRAIN_OBJECTS + "Road_Straight_03");
        PrefabMappingAdd(72, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Tool_Shed_01");
        PrefabMappingAdd(73, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Main_Square_01");
        PrefabMappingAdd(75, FIXED_SIZE_TERRAIN_OBJECTS + "Vegetable_Garden_01");
        PrefabMappingAdd(77, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Pond_01");

        // Adding sizes of fixed objects since they have to be copied to fill given size, and not stretched.
        prefab_fixed_size_widths = new Dictionary<int, int>();
        prefab_fixed_size_widths.Add(04, 1);
        prefab_fixed_size_widths.Add(17, 5);
        prefab_fixed_size_widths.Add(25, 1);
        prefab_fixed_size_widths.Add(28, 3);
        prefab_fixed_size_widths.Add(35, 2);
        prefab_fixed_size_widths.Add(44, 1);
        prefab_fixed_size_widths.Add(57, 5);
        prefab_fixed_size_widths.Add(65, 1);
        prefab_fixed_size_widths.Add(68, 3);
        prefab_fixed_size_widths.Add(75, 2);
        // Only necessary if != 1
        prefab_fixed_size_lengths = new Dictionary<int, int>();
        prefab_fixed_size_lengths.Add(25, 4);
        prefab_fixed_size_lengths.Add(28, 4);
        prefab_fixed_size_lengths.Add(65, 4);
        prefab_fixed_size_lengths.Add(68, 4);

        // 3. Icons
        // Mapping category names to the icons
        icon_mapping = new Dictionary<string, string>();
        // Categories EN
        icon_mapping.Add("Animal shelters", ICONS_RESOURCES_PATH + "dog-house");
        icon_mapping.Add("Buildings", ICONS_RESOURCES_PATH + "barn");
        icon_mapping.Add("Paths", ICONS_RESOURCES_PATH + "road");
        icon_mapping.Add("Plants", ICONS_RESOURCES_PATH + "seedling");
        icon_mapping.Add("Trees & shrubs", ICONS_RESOURCES_PATH + "trees");
        icon_mapping.Add("Water points", ICONS_RESOURCES_PATH + "lake");
        // Elements EN
        icon_mapping.Add("Beehive", ICONS_RESOURCES_PATH + "beehive");
        icon_mapping.Add("Hedgehog shelter", ICONS_RESOURCES_PATH + "hedgehog");
        icon_mapping.Add("Henhouse", ICONS_RESOURCES_PATH + "chick");
        icon_mapping.Add("Barn", ICONS_RESOURCES_PATH + "barn");
        icon_mapping.Add("Composting toilet", ICONS_RESOURCES_PATH + "toilet");
        icon_mapping.Add("Greenhouse", ICONS_RESOURCES_PATH + "greenhouse");
        icon_mapping.Add("House", ICONS_RESOURCES_PATH + "house");
        icon_mapping.Add("Main square", ICONS_RESOURCES_PATH + "weathercock");
        icon_mapping.Add("Sunroom", ICONS_RESOURCES_PATH + "sunroom");
        icon_mapping.Add("Terrace", ICONS_RESOURCES_PATH + "terrace");
        icon_mapping.Add("Tool shed", ICONS_RESOURCES_PATH + "gardening-tools");
        icon_mapping.Add("Workshop", ICONS_RESOURCES_PATH + "tools");
        icon_mapping.Add("Tree", ICONS_RESOURCES_PATH + "tree");
        icon_mapping.Add("Feed forest for the poultry", ICONS_RESOURCES_PATH + "forest");
        icon_mapping.Add("Forest", ICONS_RESOURCES_PATH + "trees");
        icon_mapping.Add("Hedge", ICONS_RESOURCES_PATH + "hedge");
        icon_mapping.Add("Orchard", ICONS_RESOURCES_PATH + "fruits");
        icon_mapping.Add("Pine trees", ICONS_RESOURCES_PATH + "pine-forest");
        icon_mapping.Add("Wild corridor", ICONS_RESOURCES_PATH + "nature");
        icon_mapping.Add("Path", ICONS_RESOURCES_PATH + "walking");
        icon_mapping.Add("Road", ICONS_RESOURCES_PATH + "road");
        icon_mapping.Add("Aromatic plants", ICONS_RESOURCES_PATH + "plant");
        icon_mapping.Add("Composter", ICONS_RESOURCES_PATH + "ground");
        icon_mapping.Add("Flower meadow", ICONS_RESOURCES_PATH + "flowers");
        icon_mapping.Add("Garden", ICONS_RESOURCES_PATH + "field");
        icon_mapping.Add("Medicinal plants", ICONS_RESOURCES_PATH + "leaves");
        icon_mapping.Add("Vegetable garden", ICONS_RESOURCES_PATH + "sow");
        icon_mapping.Add("Phytopurification", ICONS_RESOURCES_PATH + "water-cycle");
        icon_mapping.Add("Pond", ICONS_RESOURCES_PATH + "park");
        // Categories FR
        icon_mapping.Add("Abris pour animaux", ICONS_RESOURCES_PATH + "dog-house");
        icon_mapping.Add("Batiments", ICONS_RESOURCES_PATH + "barn");
        icon_mapping.Add("Chemins", ICONS_RESOURCES_PATH + "road");
        icon_mapping.Add("Plantes", ICONS_RESOURCES_PATH + "seedling");
        icon_mapping.Add("Arbres & arbustes", ICONS_RESOURCES_PATH + "trees");
        icon_mapping.Add("Points d’eau", ICONS_RESOURCES_PATH + "lake");
        // Elements FR
        icon_mapping.Add("Ruche", ICONS_RESOURCES_PATH + "beehive");
        icon_mapping.Add("Abri herisson", ICONS_RESOURCES_PATH + "hedgehog");
        icon_mapping.Add("Poulailler", ICONS_RESOURCES_PATH + "chick");
        icon_mapping.Add("Grange", ICONS_RESOURCES_PATH + "barn");
        icon_mapping.Add("Toilettes seches", ICONS_RESOURCES_PATH + "toilet");
        icon_mapping.Add("Serre", ICONS_RESOURCES_PATH + "greenhouse");
        icon_mapping.Add("Habitation", ICONS_RESOURCES_PATH + "house");
        icon_mapping.Add("Place principale", ICONS_RESOURCES_PATH + "weathercock");
        icon_mapping.Add("Veranda", ICONS_RESOURCES_PATH + "sunroom");
        icon_mapping.Add("Terrasse", ICONS_RESOURCES_PATH + "terrace");
        icon_mapping.Add("Remise a outils", ICONS_RESOURCES_PATH + "gardening-tools");
        icon_mapping.Add("Atelier", ICONS_RESOURCES_PATH + "tools");
        icon_mapping.Add("Arbre", ICONS_RESOURCES_PATH + "tree");
        icon_mapping.Add("Foret nourriciere pour la volaille", ICONS_RESOURCES_PATH + "forest");
        icon_mapping.Add("Foret", ICONS_RESOURCES_PATH + "trees");
        icon_mapping.Add("Haie", ICONS_RESOURCES_PATH + "hedge");
        icon_mapping.Add("Verger", ICONS_RESOURCES_PATH + "fruits");
        icon_mapping.Add("Pins", ICONS_RESOURCES_PATH + "pine-forest");
        icon_mapping.Add("Couloir sauvage", ICONS_RESOURCES_PATH + "nature");
        icon_mapping.Add("Sentier", ICONS_RESOURCES_PATH + "walking");
        icon_mapping.Add("Route", ICONS_RESOURCES_PATH + "road");
        icon_mapping.Add("Plantes aromatiques", ICONS_RESOURCES_PATH + "plant");
        icon_mapping.Add("Composteur", ICONS_RESOURCES_PATH + "ground");
        icon_mapping.Add("Prairie fleurie", ICONS_RESOURCES_PATH + "flowers");
        icon_mapping.Add("Jardin", ICONS_RESOURCES_PATH + "field");
        icon_mapping.Add("Plantes medicinales", ICONS_RESOURCES_PATH + "leaves");
        icon_mapping.Add("Potager", ICONS_RESOURCES_PATH + "sow");
        icon_mapping.Add("Phytoepuration", ICONS_RESOURCES_PATH + "water-cycle");
        icon_mapping.Add("Etang", ICONS_RESOURCES_PATH + "park");
    }

    private void PrefabMappingAdd(int id, string value)
    {
        List<string> values;

        if (!prefab_mapping.TryGetValue(id, out values))
        {
            values = new List<string>();
            prefab_mapping.Add(id, values);
        }
        values.Add(value);
    }

    public void ExtractData(Element[] elements)
    {
        ExtractElementName(elements);
        ExtractTerrainFlattening(elements);
        ExtractShowInteractions(elements);
    }

    private void ExtractElementName(Element[] elements)
    {
        element_name_data = new Dictionary<int, string>();
        foreach(Element element in elements)
            element_name_data.Add(element.id, element.name);
    }

    private void ExtractTerrainFlattening(Element[] elements)
    {
        terrain_flattening_data = new Dictionary<int, bool>();
        foreach(Element element in elements)
            terrain_flattening_data.Add(element.id, element.terrain_flattening);
    }

    private void ExtractShowInteractions(Element[] elements)
    {
        show_interactions_data = new Dictionary<int, bool>();
        foreach (Element element in elements)
            show_interactions_data.Add(element.id, element.show_interactions);
    }
}
