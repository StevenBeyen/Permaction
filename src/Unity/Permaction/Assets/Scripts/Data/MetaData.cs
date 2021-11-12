using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject[] demoLives = new GameObject[DEMO_MAX_NB_ELEMENTS];
    public int currentActiveDemoLives = DEMO_MAX_NB_ELEMENTS;
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
    // Locale
    public Dictionary<string, int> id_locale_mapping;
    // Terrain
    public List<string> prefab_grass;
    public List<string> prefab_rocks;
    public const string ARC_LINK_CONTAINER = "ArcLinkContainer";

    public MetaData()
    {
        // 1. ID locale
        id_locale_mapping = new Dictionary<string, int>();
        id_locale_mapping.Add("en", 1);
        id_locale_mapping.Add("fr", 2);

        // 2. Prefabs
        // Prefab grass & rocks
        prefab_grass = new List<string>();
        prefab_grass.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Grass_Patch_01");
        //prefab_grass.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Grass_Patch_02"); // Unpretty grass
        prefab_grass.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Grass_Patch_03");
        prefab_rocks = new List<string>();
        prefab_rocks.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Small_Rocks_01");
        prefab_rocks.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Small_Rocks_02");
        prefab_rocks.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Small_Rocks_03");
        prefab_rocks.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Small_Rocks_04");
        prefab_rocks.Add(FIXED_SIZE_TERRAIN_OBJECTS + "SM_Generic_Small_Rocks_05");
        // Mapping of prefab objects to their ID.
        prefab_mapping = new Dictionary<int, List<string>>();
        // English
        prefab_mapping_add(02, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Aromatic_Plants_01");
        prefab_mapping_add(03, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Barn_01");
        prefab_mapping_add(04, FIXED_SIZE_TERRAIN_OBJECTS + "Beehive_01");
        prefab_mapping_add(07, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Composter_01");
        prefab_mapping_add(08, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Composting_Toilet_01");
        prefab_mapping_add(14, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Greenhouse_01");
        prefab_mapping_add(15, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Sunroom_01");
        prefab_mapping_add(19, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Henhouse_01");
        prefab_mapping_add(20, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Farmhouse_01");
        prefab_mapping_add(25, FIXED_SIZE_TERRAIN_OBJECTS + "Path_Straight_01");
        prefab_mapping_add(25, FIXED_SIZE_TERRAIN_OBJECTS + "Path_Straight_02");
        prefab_mapping_add(25, FIXED_SIZE_TERRAIN_OBJECTS + "Path_Straight_03");
        prefab_mapping_add(25, FIXED_SIZE_TERRAIN_OBJECTS + "Path_Straight_04");
        prefab_mapping_add(28, FIXED_SIZE_TERRAIN_OBJECTS + "Road_Straight_01");
        prefab_mapping_add(32, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Tool_Shed_01");
        prefab_mapping_add(37, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Pond_01");
        // French
        prefab_mapping_add(42, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Aromatic_Plants_01");
        prefab_mapping_add(43, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Barn_01");
        prefab_mapping_add(44, FIXED_SIZE_TERRAIN_OBJECTS + "Beehive_01");
        prefab_mapping_add(47, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Composter_01");
        prefab_mapping_add(48, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Composting_Toilet_01");
        prefab_mapping_add(54, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Greenhouse_01");
        prefab_mapping_add(55, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Sunroom_01");
        prefab_mapping_add(59, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Henhouse_01");
        prefab_mapping_add(60, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Farmhouse_01");
        prefab_mapping_add(65, FIXED_SIZE_TERRAIN_OBJECTS + "Path_Straight_01");
        prefab_mapping_add(65, FIXED_SIZE_TERRAIN_OBJECTS + "Path_Straight_02");
        prefab_mapping_add(65, FIXED_SIZE_TERRAIN_OBJECTS + "Path_Straight_03");
        prefab_mapping_add(65, FIXED_SIZE_TERRAIN_OBJECTS + "Path_Straight_04");
        prefab_mapping_add(68, FIXED_SIZE_TERRAIN_OBJECTS + "Road_Straight_01");
        prefab_mapping_add(72, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Tool_Shed_01");
        prefab_mapping_add(77, STRETCHABLE_ONE_METER_PREFAB_RESOURCES_PATH + "Pond_01");

        // TODO Add overlay for path and road re-generation

        // Adding sizes of fixed objects since they have to be copied to fill given size, and not stretched.
        prefab_fixed_size_widths = new Dictionary<int, int>();
        prefab_fixed_size_widths.Add(04, 1);
        prefab_fixed_size_widths.Add(25, 1);
        prefab_fixed_size_widths.Add(28, 3);
        prefab_fixed_size_widths.Add(44, 1);
        prefab_fixed_size_widths.Add(65, 1);
        prefab_fixed_size_widths.Add(68, 3);

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

    public void prefab_mapping_add(int id, string value)
    {
        List<string> values;

        if (!prefab_mapping.TryGetValue(id, out values))
        {
            values = new List<string>();
            prefab_mapping.Add(id, values);
        }
        values.Add(value);
    }
}
