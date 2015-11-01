using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using LINE.Geometry;
using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;

namespace Elk.Common
{
    public class ElkLib
    {
        static double earthRadm = 6371000;
        public static string[] featureKeys = { "aerialway", "aeroway", "amenity", "barrier", "boundary", "building", "craft", "emergency", "geological", "highway",
                                      "historic", "landuse", "leisure", "man_made", "military", "natural", "office", "place", "power", "public_transport",
                                      "railway", "route", "shop", "sport", "tourism", "waterway" };

        public enum FeatureType
        {
            Aerialway, Aeroway, Amenity, Barrier, Boundary, Building, Craft, Emergency, Geological, Highway, Historic, Landuse, Leisure, ManMade, Military,
            Natural, Office, Place, Power, PublicTransport, Railway, Route, Shop, Sport, Tourism, Waterway
        };

        #region SubTypes

        public static List<string> Aerialways = new List<string> { "cable_car", "chair_lift", "drag_lift", "gondola", "goods", "j-bar", "magic_carpet", "mixed_lift",
                                                            "platter", "pylon", "rope_tow", "station", "t-bar", "zip_line" };

        public static List<string> Aeroways = new List<string> { "aerodrome", "apron", "gate", "helipad", "hangar", "navigationaid", "runway", "taxiway", "terminal", "windsock" };

        public static List<string> Amenities = new List<string> { "bar", "bbq", "biergarten", "cafe", "drinking_water", "fast_food", "food_court", "ice_cream", "pub", "restaurant",
                                                           "college", "kindergarten", "library", "public_bookcase", "school", "university",
                                                           "bicycle_parking", "bicycle_repair_station", "bicycle_rental", "boat_sharing", "bus_station", "car_rental",
                                                           "car_sharing", "car_wash", "charging_station", "ferry_terminal", "fuel", "grit_bin", "motorcycle_parking",
                                                           "parking", "parking_entrance", "parking_space", "taxi",
                                                           "atm", "bank", "bureau_de_change",
                                                           "baby_hatch", "clinic", "dentist", "doctors", "hospital", "nursing_home", "pharmacy", "social_facility", "veterinary", "blood_donation",
                                                           "arts_centre", "brothel", "casino", "cinema", "community_centre", "fountain", "gambling", "nightclub", "planetarium", "social_centre",
                                                           "strip_club", "studio", "swingerclub", "theatre",
                                                           "animal_boarding", "animal_shelter", "bench", "clock", "courthouse", "coworking_space", "crematorium", "crypt", "dojo", "embassy",
                                                           "fire_station", "game_feeding", "grave_yard", "gym", "hunting_stand", "kneipp_water_cure", "marketplace", "photo_booth", "place_of_worship",
                                                           "police", "post_box", "post_office", "prison", "ranger_station", "register_office", "recycling", "rescue_station", "sauna", 
                                                           "shelter", "shower", "telephone", "toilets", "townhall", "vending_machine", "waste_basket", "waste_disposal", "watering_place", "water_point" };

        public static List<string> Barrier = new List<string> { "cable_barrier", "city_wall", "ditch", "fence", "guard_rail", "handrail", "hedge", "kerb", "retaining_wall", "wall",
                                                         "block", "bollard", "border_control", "bump_gate", "bus_trap", "cattle_grid", "chain", "cycle_barrier", "debris", "entrance",
                                                         "full-height_turnstile", "gate", "hampsire_gate", "height_restrictor", "horse_stile", "jersey_barrier", "kent_carriage_gap",
                                                         "kissing_gate", "lift_gate", "log", "motorcycle_barrier", "rope", "salley_port", "spikes", "stile", "sump_buster", 
                                                         "swing_gate", "toll_booth", "turnstile", "yes" };

        public static List<string> Boundary = new List<string> { "administrative", "maritime", "national_park", "political", "postal_code", "religious_administration", "protected_area" };

        public static List<string> Building = new List<string> { "apartments", "farm", "hotel", "house", "detached", "residential", "dormitory", "terrace", "houseboat", "bungalow", "static_caravan",
                                                          "commercial", "office", "industrial", "retail", "warehouse",
                                                          "cathedral", "chapel", "church", "mosque", "temple", "synagogue", "shrine", "civic", "hospital", "school", "stadium", 
                                                          "train_station", "transportation", "university", "public", 
                                                          "barn", "bridge", "bunker", "cabin", "construction", "cowshed", "farm_auxilliary", "garage" ,"garages", "greenhouse", "hangar", "hut", 
                                                          "roof", "shed", "stable", "sty", "transformer_tower", "service", "kiosk", "ruins", "yes" };

        public static List<string> Craft = new List<string> { "agricultural_engines", "basket_maker", "beekeeper", "blacksmith", "brewery", "boatbuilder", "bookbinder", "carpenter", "carpet_layer", 
                                                                "caterer", "clockmaker", "confectionery", "dressmaker", "electrician", "floorer", "gardener", "glaziery", "handicraft", "hvac", "insulation",
                                                                "jeweller", "key_cutter", "locksmith", "metal_construction", "optician", "painter", "parquet_layer", "photographer", "photogramic_laboratory",
                                                                "plasterer", "plumber", "potter", "rigger", "roofer", "saddler", "sailmaker", "sawmill", "scaffolder", "sculptor", "shoemaker", "stand_builder",
                                                                "stonemason", "sun_protection", "chimney_sweeper", "tailor", "tiler", "tinsmith", "upholsterer", "watchmaker", "window_construction", "winery" };

        public static List<string> Emergency = new List<string> { "ambulance_station", "defibrillator", "fire_extinguisher", "fire_flapper", "fire_hose", "fire_hydrant", "water_tank",
                                                                    "lifeguard_base", "lifeguard_tower", "lifeguard_platform", "lifeguard_place", "life_ring", 
                                                                    "assembly_point", "access_point", "phone", "ses_station", "siren" };

        public static List<string> Geological = new List<string> { "moraine", "outcrop", "palaentological_site" };

        public static List<string> Highway = new List<string> { "motorway", "trunk", "primary", "secondary", "tertiary", "unclassified", "residential", "service",
                                                                  "motorway_link", "trunk_link", "primary_link", "secondary_link", "tertiary_link", 
                                                                  "living_street", "pedestrian", "track", "bus_guideway", "raceway", "road",
                                                                  "footway", "bridleway", "steps", "path","cycleway", "busway",
                                                                  "proposed", "custruction", 
                                                                  "bus_stop", "crossing", "elevator", "emergency_access_point", "escape", "give_way", "phone", "mini_roundabout", "motorway_junction",
                                                                  "passing_place", "rest_area", "speed_camera", "street_lamp", "services", "stop", "traffic_signal", "turning_circle" };

        public static List<string> Historic = new List<string> { "archaelological_site", "aircraft", "battlefield", "boundary_stone", "building", "castle", "cannon", "city_gate", "citywalls", "farm",
                                                                 "fort", "manor", "memorial", "monument", "optical_telegraph", "ruins", "rune_stone", "ship", "tomb", "wayside_cross", "wayside_shrine", "wreck", "yes" };

        public static List<string> Landuse = new List<string> { "allotments", "basin", "brownfield", "cementary", "commerical", "construction", "farmland", "farmyard", "forest", "garages", "grass",
                                                                "greenfield", "greenhouse_horticulture", "industrial", "landfill", "meadow", "military", "orchard", "peat_cutting", "plant_nursery",
                                                                "port", "quarry", "railway", "recreation_ground", "resevoir", "residential", "retail", "salt_pond", "village_green", "vineyard" };

        public static List<string> Leisure = new List<string> { "adult_gaming_centre", "amusement_arcade", "beach_resort", "bandstand", "bird_hide", "dance", "dog_park", "firepit", "fishing", "garden",
                                                                "golf_course", "hackerspace", "ice_rink", "marina", "miniature_golf", "nature_reserve", "park", "pitch", "playground", "slipway", "sports_centre",
                                                                "stadium", "summer_camp", "swimming_pool", "swimming_area", "track", "water_park", "wildlife_hide" };

        public static List<string> ManMade = new List<string> { "adit", "beacon", "breakwater", "bridge", "bunker_silo", "campanile", "chimney", "communications_tower", "crane", "cross", "cutline",
                                                                "clearcut", "embankment", "dyke", "flagpole", "gasometer", "groyne", "kiln", "lighthouse", "mast", "mineshaft", "monitoring_station",
                                                                "offshore_platform", "petroleum_well", "pier", "pipeline", "resevoir_covered", "silo", "snow_fence", "snow_net", "storage_tank", 
                                                                "street_cabinet", "surveillance", "survey_point", "tower", "wastewater_plant", "watermill", "water_tower", "water_well", "water_tap", 
                                                                "water_works", "windmill", "works" };

        public static List<string> Military = new List<string> { "airfield", "ammunition", "bunker", "barracks", "checkpoint", "danger_area", "naval_base", "nuclear_explosion_site", "obstacle_course",
                                                                 "office", "range", "training_area" };

        public static List<string> Natural = new List<string> { "wood", "tree_row", "tree", "scrub", "heath", "moor", "grassland", "fell", "bare_rock", "scree", "shingle", "sand", "mud",
                                                                "water", "wetland", "glacier", "bay", "beach", "coastline", "spring",
                                                                "peak", "volcano", "valley", "river_terrace", "ridge", "arete", "cliff", "saddle", "rock", "stone", "sinkhole", "cave_entrance" };

        public static List<string> Office = new List<string> { "accountant", "administrative", "advertising_agency", "architect", "association", "company", "educational_institution", "employment_agency",
                                                               "estate_agent", "forestry", "foundation", "government", "guide" ,"insurance", "it", "lawyer", "newspaper", "ngo", "notary", "political_party",
                                                               "private_investigator", "qunago", "realtor", "real_estate_agent", "register", "religion", "research", "tax", "tax_advisor", "telecommunication", 
                                                               "travel_agent", "water_utility" };

        public static List<string> Places = new List<string> { "country", "state", "region", "province", "district", "county", "municipality",
                                                               "city", "borough", "suburb", "quarter", "neighborhood", "city_block", "plot",
                                                               "town", "village", "hamlet", "isolated_dwelling", "farm", "allotments",
                                                               "continent", "archipelago", "island", "islet", "locality" };

        public static List<string> Power = new List<string> { "plant", "cable", "converter", "generator", "heliostat", "insulator", "line", "minor_line", "pole", "portal", "substation", "switch", "tower", "transformer" };

        public static List<string> PublicTransport = new List<string> { "stop_position", "platform", "station", "stop_area" };

        public static List<string> Railway = new List<string> { "abandoned", "construction", "disused", "funicular", "light_rail", "miniature", "monorail", "narrow_gauge", "preserved", "rail", "subway",
                                                                "tram", "halt", "station", "subway_entrance", "tram_stop", "platform", 
                                                                "buffer_stop", "derail", "crossing", "level_crossing", "signal", "switch", "railway_crossing", "turntable", "roundhouse", "railway_crossing" };

        public static List<string> Route = new List<string> { "bicycle", "bus", "inline_skates", "canoe", "detour", "ferry", "hiking", "horse", "light_rail", "mtb", "nordic_walking", "pipeline", "piste",
                                                              "power", "railway", "road", "running", "ski", "train", "tram" };

        public static List<string> Shop = new List<string> { "alcohol", "bakery", "beverages", "butcher", "cheese", "chocolate", "coffee", "confectionery", "convenience", "deli", "dairy", "farm", "greengrocer",
                                                             "organic", "pasta", "pastry", "seafood", "tea", "wine",
                                                             "department_store", "general", "kiosk", "mall", "supermarket",
                                                             "baby_goods", "bag", "boutique", "clothes", "fabric", "fashion", "jewelry", "leather", "shoes", "tailor", "watches",
                                                             "charity", "second_hand", "variety_store",
                                                             "beauty", "chemist", "cosmetics", "drugstore", "erotic", "hairdresser", "hearing_aids", "herbalist", "massage", "medical_supply", "optician",
                                                             "perfumery", "tattoo",
                                                             "bathroom_furnishing", "doityourself", "electrical", "energy", "florist", "furnace", "garden_centre", "garden_furniture", "gas", "glaziery",
                                                             "hardware", "houseware", "locksmith", "paint", "trade",
                                                             "antiques", "bed", "candles", "carpet", "curtain", "furniture", "interior_decoration", "kitchen", "lamps", "window_blind",
                                                             "computer", "electronics", "hifi", "mobile_phone", "radiotechnics", "vacuum_cleaner",
                                                             "bicycle", "carpet", "car_repair", "car_parts", "fishing", "free_flying", "hunting", "motorcycle", "outdoor", "scuba_diving", "sports", "tyres", "swimming_pool",
                                                             "art", "craft", "frame", "games", "model", "music", "musical_instrument", "photo", "trophy", "video", "video_games",
                                                             "anime", "books", "gift", "newsagent", "stationery", "ticket",
                                                             "copyshop", "dry_cleaning", "e-cigarette", "funeral_directors", "laundry", "money_lender", "pawnbroker", "pet", "pyrotechnics", "religion",
                                                             "storage_rental", "tobacco", "toys", "travel_agency", "vacant", "weapons" };

        public static List<string> Sport = new List<string> { "9pin", "10pin", "american_football", "aikido", "archery", "athletics", "australian_football", "base", "badminton", "bandy", "baseball",
                                                              "basketball", "beachvolleyball", "billiards", "bmx", "bobsleigh", "boules", "bowls", "boxing", "canadian_football", "canoe", "chess", "cliff_diving",
                                                              "climbing", "climbing_adventure", "cockfighting", "cricket", "croquet", "curling", "cycling", "darts", "dog_racing", "equestrian", "fencing",
                                                              "field_hockey", "free_flying", "gaelic_games", "golf", "gymnastics", "handball", "hapkido", "horseshoes", "horse_racing", "ice_hockey", "ice_skating",
                                                              "ice_stock", "judo", "karting", "kitesurfing", "korfball", "model_aerodrome", "motocross", "motor", "multi", "obstacle_course", "orienteering",
                                                              "paddle_tennis", "paragliding", "pelota", "racquet", "rc_car", "roller_skating", "rowing", "rugby_league", "rugby_union", "running", "safety_training",
                                                              "sailing", "scuba_diving", "shooting", "skateboarding", "skiing", "soccer", "surfing", "swimming", "table_tennis", "table_soccer", "taekwondo",
                                                              "tennis", "toboggan", "volleyball", "water_polo", "water_ski", "weightlifting", "wrestling" };

        public static List<string> Toursim = new List<string> { "alpine_hut", "apartment", "attraction", "artwork", "camp_site", "caravan_site", "chalet", "gallery", "guest_house", "hostel", "hotel",
                                                                "information", "motel", "museum", "picnic_site", "theme_park", "viewpoint", "wilderness_hut", "zoo", "yes" };

        public static List<string> Waterway = new List<string> { "river", "riverbank", "stream", "canal", "drain", "ditch", "dock", "boatyard", "dam", "weir", "waterfall", "lock_gate", "turning_point", "water_point" };

        #endregion

        public static void NodePreProcess(string xmlFilePath, double scale, out string xmlStr, out List<OSMPoint> osmPoints, out Interval2d geoDomain)
        {
            // Read the XML Document
            if (System.IO.File.Exists(xmlFilePath))
            {
                double minLat = 0;
                double maxLat = 0;
                double minLon = 0;
                double maxLon = 0;

                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlTextReader reader = new XmlTextReader(xmlFilePath);
                    xmlDoc.Load(reader);
                    
                    if (xmlDoc != null)
                    {
                        // get the boundary domains.
                        XmlNodeList bounds = xmlDoc.SelectNodes("osm/bounds");
                        XmlNode boundary = bounds[0];
                        minLat = Convert.ToDouble(boundary.Attributes["minlat"].Value);
                        maxLat = Convert.ToDouble(boundary.Attributes["maxlat"].Value);
                        minLon = Convert.ToDouble(boundary.Attributes["minlon"].Value);
                        maxLon = Convert.ToDouble(boundary.Attributes["maxlon"].Value);
                        geoDomain = new Interval2d(minLon, maxLon, minLat, maxLat);

                        // Get the overal size of the domains
                        double deltaLat = maxLat - minLat;
                        double deltaLon = maxLon - minLon;

                        // Get the median latitude value for evaluating the earth's radius at the location
                        double averageLat = minLat + (deltaLat / 2);
                        double yLenPerDegree = ((Math.PI * earthRadm) / 180) * scale;
                        double xLenPerDegree = ((Math.PI * (Math.Cos((averageLat * Math.PI) / 180) * earthRadm)) / 180) * scale;
                        
                        // Find all of the points in the XML file
                        XmlNodeList filePoints = xmlDoc.SelectNodes("osm/node");
                        osmPoints = new List<OSMPoint>();
                        foreach(XmlNode nodePt in filePoints)
                        {
                            try
                            {
                                // Get the information from the node
                                string ptId = nodePt.Attributes["id"].Value;
                                double ptLon = Convert.ToDouble(nodePt.Attributes["lon"].Value);
                                double ptLat = Convert.ToDouble(nodePt.Attributes["lat"].Value);

                                // Assign it to the osmPoint
                                OSMPoint osmPoint = new OSMPoint();
                                Point3d pt = Point3d.Origin;
                                pt.X = (ptLon - minLon) * xLenPerDegree;
                                pt.Y = (ptLat - minLat) * yLenPerDegree;

                                string k = "unknown";
                                string v = "unknown";

                                Dictionary<string, string> tags = new Dictionary<string, string>();

                                XmlNodeList pointTags = nodePt.SelectNodes("tag");
                                if(pointTags.Count > 0)
                                {
                                    foreach (XmlNode tag in pointTags)
                                    {
                                        string tempK = string.Empty;
                                        try
                                        {
                                            tempK = tag.Attributes["k"].Value;
                                            string tempV = tag.Attributes["v"].Value;
                                            tags.Add(tempK, tempV);
                                            if (featureKeys.Contains(tempK))
                                            {
                                                k = tempK;
                                                v = tag.Attributes["v"].Value;
                                            }
                                        }
                                        catch { }
                                    }
                                }

                                osmPoint.ID = ptId;
                                osmPoint.Point = pt;
                                osmPoint.Latitude = ptLat;
                                osmPoint.Longitude = ptLon;
                                osmPoint.Key = k + ":" + v;
                                osmPoint.OSMKey = k;
                                osmPoint.OSMValue = v;
                                osmPoint.Tags = tags;
                                osmPoints.Add(osmPoint);
                            }
                            catch { }
                        }

                        // Get the XML file contents as a string
                        try
                        {
                            System.IO.StringWriter sw = new System.IO.StringWriter();
                            XmlTextWriter tw = new XmlTextWriter(sw);
                            xmlDoc.WriteTo(tw);
                            xmlStr = sw.ToString();
                        }
                        catch
                        {
                            xmlStr = null;
                        }
                    }
                    else
                    {
                        xmlStr = null;
                        osmPoints = null;
                        geoDomain = null;
                    }

                    reader.Close();
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Problem reading OSM file.", "Error");
                    xmlStr = null;
                    osmPoints = null;
                    geoDomain = null;
                }
            }
            else
            {
                xmlStr = null;
                osmPoints = null;
                geoDomain = null;
            }
        }

        public static List<OSMWay> GatherWays(string featureType, string[] featureSubType, string xmlString, List<OSMPoint> allPoints)
        {
            List<OSMWay> ways = new List<OSMWay>();
            List<OSMWay> unorderedWays = new List<OSMWay>();
            if (featureSubType.Count() == 0 || featureSubType == null)
            {
                featureSubType = new string[] { "*" };
            }
            
            // Load the XML string into an XmlDocument
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                XmlReader reader = new XmlTextReader(xmlString);
                xmlDoc.Load(reader);
            }
            catch
            {
                xmlDoc = null;
            }

            if (xmlDoc != null)
            {
                // Get all of the single point ways from the labeled nodes
                foreach (OSMPoint pt in allPoints)
                {
                    string key = pt.OSMKey;
                    Point3d point = pt.Point;

                    if (key == featureType)
                    {
                        string wayType = pt.OSMValue;
                        OSMWay currentWay = new OSMWay();
                        currentWay.Name = "POINT";
                        currentWay.Type = key;
                        currentWay.Name = wayType;
                        currentWay.ID = pt.ID;
                        List<Point3d> points = new List<Point3d>();
                        points.Add(pt.Point);
                        List<List<Point3d>> wayPoints = new List<List<Point3d>>();
                        wayPoints.Add(points);
                        
                        currentWay.Points = wayPoints;
                        currentWay.Tags = pt.Tags;
                        unorderedWays.Add(currentWay);
                    }
                }
                
                // Get all of the typical Way objects
                XmlNodeList wayNodes = xmlDoc.SelectNodes("osm/way");
                // Create a temporary Way list that contains all Ways.
                // This will be used in the next section to find the relations.
                List<OSMWay> allWays = new List<OSMWay>();
                foreach (XmlNode way in wayNodes)
                {
                    bool matchKey = false;
                    OSMWay currentWay = new OSMWay();
                    string wayType = "Unknown";

                    // See if the current way matches the featureType
                    
                    XmlNodeList tagNodes = way.SelectNodes("tag");
                    foreach (XmlNode tag in tagNodes)
                    {
                        string k = tag.Attributes["k"].Value;
                        if (featureType.Contains(k))
                        {
                            matchKey = true;
                            wayType = tag.Attributes["v"].Value;
                            break;
                        }
                    }

                    
                    Dictionary<string, string> pointIds = new Dictionary<string, string>();
                    // If the waytype matches, continue finding it's information
                    if (matchKey)
                    {
                        Dictionary<string, string> tags = new Dictionary<string, string>();
                        string wayName = "Unknown";
                        if (featureSubType.Contains(wayType) || featureSubType[0] == "*")
                        {
                            // Find the feature's name, if it is known.
                            foreach (XmlNode tag in tagNodes)
                            {
                                string k = tag.Attributes["k"].Value;
                                string v = tag.Attributes["v"].Value;
                                tags.Add(k, v);
                                if (k == "name")
                                {
                                    wayName = tag.Attributes["v"].Value;
                                }
                            }
                            
                            // Collect all of the related points
                            List<Point3d> points = new List<Point3d>();
                            XmlNodeList refPoints = way.SelectNodes("nd");
                            foreach (XmlNode refPoint in refPoints)
                            {
                                try
                                {
                                    string reference = refPoint.Attributes["ref"].Value;
                                    OSMPoint point = allPoints.Find(delegate(OSMPoint pt)
                                    {
                                        return pt.ID == reference;
                                    });

                                    points.Add(point.Point);
                                    pointIds.Add(reference, string.Empty);
                                }
                                catch { } // Failed to find a point, ignore.
                            }

                            currentWay.Type = wayType;
                            currentWay.Name = wayName;
                            
                            List<List<Point3d>> wayPoints = new List<List<Point3d>> { points };
                            //System.Windows.Forms.MessageBox.Show("Lists: " + wayPoints.Count.ToString() + "\nPoints: " + wayPoints[0].Count.ToString());
                            currentWay.Points = wayPoints;
                            currentWay.Tags = tags;

                            unorderedWays.Add(currentWay);
                        }
                    }

                    // Add the ways to the temp list for the relations
                    string wayId = way.Attributes["id"].Value;
                    
                    if (pointIds.Count == 0)
                    {
                        XmlNodeList refPoints = way.SelectNodes("nd");
                        foreach (XmlNode refPoint in refPoints)
                        {
                            try
                            {
                                string reference = refPoint.Attributes["ref"].Value;
                                pointIds.Add(reference, string.Empty);
                            }
                            catch { } // Failed to find a point, ignore.
                        }
                    }

                    OSMWay allWayNode = new OSMWay();
                    allWayNode.ID = wayId;
                    allWayNode.Tags = pointIds;
                    allWays.Add(allWayNode);
                }

                int test = 0;
                // Collect all the Relations related to the selected Feature and Sub-Feature(s).
                XmlNodeList relationNodes = xmlDoc.SelectNodes("osm/relation");
                foreach (XmlNode relation in relationNodes)
                {
                    bool matchKey = false;
                    OSMWay currentWay = new OSMWay();
                    string wayType = "Unknown";

                    // See if the current way matches the featureType

                    XmlNodeList tagNodes = relation.SelectNodes("tag");
                    foreach (XmlNode tag in tagNodes)
                    {
                        string k = tag.Attributes["k"].Value;
                        if (featureType.Contains(k))
                        {
                            matchKey = true;
                            wayType = tag.Attributes["v"].Value;
                            break;
                        }
                    }

                    // If the waytype matches, continue finding it's information
                    if (matchKey)
                    {
                        Dictionary<string, string> tags = new Dictionary<string, string>();
                        string wayName = "Unknown";
                        if (featureSubType.Contains(wayType) || featureSubType[0] == "*")
                        {
                            // Find the feature's name, if it is known.
                            foreach (XmlNode tag in tagNodes)
                            {
                                string k = tag.Attributes["k"].Value;
                                string v = tag.Attributes["v"].Value;
                                tags.Add(k, v);
                                if (k == "name")
                                {
                                    wayName = tag.Attributes["v"].Value;
                                }
                            }

                            // Collect all of the related points
                            List<List<Point3d>> wayPoints = new List<List<Point3d>>();
                            XmlNodeList refWays = relation.SelectNodes("member");
                            foreach (XmlNode refWay in refWays)
                            {
                                List<Point3d> points = new List<Point3d>();
                                try
                                {
                                    string reference = refWay.Attributes["ref"].Value;
                                    
                                    OSMWay way = allWays.Find(delegate(OSMWay oWay)
                                    {
                                        return oWay.ID == reference;
                                    });
                                    foreach (KeyValuePair<string, string> kvp in way.Tags)
                                    {

                                        OSMPoint point = allPoints.Find(delegate(OSMPoint pt)
                                        {
                                            return pt.ID == kvp.Key;
                                        });
                                        points.Add(point.Point);
                                    }
                                }
                                catch { } // Failed to find a point, ignore.
                                wayPoints.Add(points);
                            }
                            currentWay.Type = wayType;
                            currentWay.Name = wayName;
                            currentWay.Points = wayPoints;
                            currentWay.Tags = tags;

                            unorderedWays.Add(currentWay);
                            test++;
                        }
                    }
                }
                
            }
            
            // Sort the ways
            unorderedWays.Sort((x, y) => (x.Type + " : " + x.Name).CompareTo(y.Type + " : " + y.Name));

            return unorderedWays;
        }

        public static List<string> TopoFileInfo(string filePath)
        {
            List<string> fileInfo = new List<string>();
            fileInfo.Add("File Path: " + filePath);
            Interval2d latLon = new Interval2d();

            // Determine the File Type, SRTM Height file or USGS Img file.
            System.IO.FileInfo fi = new System.IO.FileInfo(filePath);
            if (fi.Extension.ToLower() == ".hgt")
            {
                // Try to get teh domain from the file name

                string file = fi.Name.Replace(fi.Extension, string.Empty);

                if (file.Contains("N") && file.Contains("W"))
                {
                    // North/West Sector
                    // remove the 'N' and split at the 'W'

                    string[] localArr = file.Replace("N", string.Empty).Split(new char[] { 'W' });
                    double west = 0.0;
                    double south = 0.0;

                    double.TryParse(localArr[0], out south);
                    double.TryParse(localArr[1], out west);


                    if (south != 0 && west != 0)
                    {
                        latLon.UMin = -west;
                        latLon.UMax = -west + 1.0;
                        latLon.VMin = south;
                        latLon.VMax = south + 1.0;
                    }
                }
                else if (file.Contains("N") && file.Contains("E"))
                {
                    // North/East Sector
                    // remove the 'N' and split at the 'E'

                    string[] localArr = file.Replace("N", string.Empty).Split(new char[] { 'E' });
                    double west = 0.0;
                    double south = 0.0;

                    double.TryParse(localArr[0], out south);
                    double.TryParse(localArr[1], out west);

                    if (south != 0 && west != 0)
                    {
                        latLon.UMin = west;
                        latLon.UMax = west + 1.0;
                        latLon.VMin = south;
                        latLon.VMax = south + 1.0;
                    }
                }
                else if (file.Contains("S") && file.Contains("W"))
                {
                    // South/West Sector
                    // remove the 'S' and split at the 'W'

                    string[] localArr = file.Replace("S", string.Empty).Split(new char[] { 'W' });
                    double west = 0.0;
                    double south = 0.0;
                    
                    double.TryParse(localArr[0], out south);
                    double.TryParse(localArr[1], out west);

                    if (south != 0 && west != 0)
                    {
                        latLon.UMin = -west;
                        latLon.UMax = -west + 1.0;
                        latLon.VMin = -south;
                        latLon.VMax = -south + 1.0;
                    }
                }
                else if (file.Contains("S") && file.Contains("E"))
                {
                    // South/East sector
                    // remove the 'S' and split at the 'E'

                    string[] localArr = file.Replace("S", string.Empty).Split(new char[] { 'E' });
                    double west = 0.0;
                    double south = 0.0;

                    double.TryParse(localArr[0], out south);
                    double.TryParse(localArr[1], out west);

                    if (south != 0 && west != 0)
                    {
                        latLon.UMin = west;
                        latLon.UMax = west + 1.0;
                        latLon.VMin = -south;
                        latLon.VMax = -south + 1.0;
                    }
                }
                fileInfo.Add("File Domain: " + latLon.ToString());
            }

            else
            {  
                // TODO: GeoTransform[0] gives the western longitude (lonMin)
                // TODO: GeoTransform[3] Gives the Northern Latitude (latMax)
                // TODO: GeoTransform[1] gives the longitude pixel increment (should be identical except for sign to GeoTransform[5])
                // TODO: GeoTransform[5] gives the latitude pixel increment (should be identical except for sign to GeoTransform[1])
                // TODO: DataSet RasterXSize gives the number of pixels in the X (Longitude) direction.
                // TODO: DataSet RasterYSize gives the number of pixels in the Y (Latitude) direction.
                // TODO: multiply the appropriate pixel count and pixel increment and add it to the given lat/lon to determine
                //       the actual bounding box for the geotiff.

                // TODO: Consider a different component that just reads out GeoTIFF data as a list of strings/dictionary
                //       instead of adding the domain to the component.

                // USGS Raster File
                Gdal.AllRegister();
                Dataset ds = Gdal.Open(filePath, Access.GA_ReadOnly);

                double[] geoTransform = new double[6];
                ds.GetGeoTransform(geoTransform);

                Band band = ds.GetRasterBand(1);

                
                double longPixel = geoTransform[1];
                double latPixel = geoTransform[5];
                int longPixelCt = ds.RasterXSize;
                int latPixelCt = ds.RasterYSize;

                double west = geoTransform[0];
                double east = west + (longPixel * longPixelCt);
                double north = geoTransform[3];
                double south = north + (latPixel * latPixelCt);
                latLon.UMin = west;
                latLon.UMax = east;
                latLon.VMin = south;
                latLon.VMax = north;

                fileInfo.Add("File Domain: " + latLon.ToString());
                fileInfo.Add("Pixel Size - Longitude: " + longPixel.ToString());
                fileInfo.Add("Pixel Size - Latitude: " + latPixel.ToString());
                fileInfo.Add("Pixel Count - Longitude: " + longPixelCt.ToString());
                fileInfo.Add("Pixel Count - Latitude: " + latPixelCt.ToString());
            }

            return fileInfo;
        }

        public static List<List<Point3d>> ProcessTopoFile(string filePath, double scale, Interval2d latlon)
        {
            

            Interval latDomain = new Interval(latlon.VMin, latlon.VMax);
            Interval lonDomain = new Interval(latlon.UMin, latlon.UMax);

            List<List<Point3d>> initialPoints = new List<List<Point3d>>();

            // Determine the File Type, SRTM Height file or USGS Img file.
            System.IO.FileInfo fi = new System.IO.FileInfo(filePath);
            if (fi.Extension.ToLower() == ".hgt")
            {
                // Basic SRTM Data
                // Read the contents of the linked HGT file
                byte[] data = System.IO.File.ReadAllBytes(filePath);
                int formatSize = 0;

                // SRTM Data
                if (data.Length >= 25934402)
                {
                    formatSize = 3601;
                }
                else if (data.Length >= 2884802)
                {
                    formatSize = 1201;
                }
                else
                {
                    formatSize = 0;
                }
                if (formatSize != 0)
                {

                    // Get the lat and lon deltas
                    double latDelta = latDomain.Max - latDomain.Min;
                    double lonDelta = lonDomain.Max - lonDomain.Min;

                    // Get the basic longitude and latitude data
                    double minLatY = latDomain.Min - Math.Truncate(latDomain.Min); // percentage of 1°
                    double maxLatY = minLatY + latDelta; // percentage of 1°
                    double minLonX = lonDomain.Min - Math.Truncate(lonDomain.Min); // percentage of 1°
                    double maxLonX = minLonX + lonDelta; // percentage of 1°

                    double mLatY = minLatY * formatSize;
                    double MLatY = maxLatY * formatSize;
                    double mLonX = minLonX * formatSize;
                    double MLonX = maxLonX * formatSize;

                    int srtmXStart = Convert.ToInt32(mLonX); //0-3600: row/column count
                    int srtmXEnd = Convert.ToInt32(MLonX);   //0-3600: row/column count
                    int srtmYStart = 3600 - Convert.ToInt32(MLatY); //0-3600: row/column count
                    int srtmYEnd = 3600 - Convert.ToInt32(mLatY);   //0-3600: row/column count

                    int xTravel = srtmXEnd - srtmXStart - 1;
                    int yTravel = srtmYEnd - srtmYStart;

                    double theta = (Math.PI / 180) * latDomain.Mid;
                    double yFtPerDegree = 111131.745 * scale;
                    double xFtPerDegree = Math.Cos(latDomain.Mid * (Math.PI / 180)) * yFtPerDegree;

                    //System.Windows.Forms.MessageBox.Show("XStart: " + srtmXStart.ToString() + "\nXEnd: " + srtmXEnd.ToString() +
                    //    "\nYStart: " + srtmYStart.ToString() + "\nYEnd: " + srtmYEnd.ToString() + "\nxTravel: " + xTravel.ToString() + "\nyTravel: " + yTravel.ToString());

                    // Convert the binary HGT file to a list of signed integers
                    List<short> elevations = new List<short>();
                    int i = 0;
                    if (System.IO.Path.GetExtension(filePath) == ".dt2")
                    {
                        i = 3436;
                    } 
                    int incr = i;

                    while (i < formatSize * formatSize)
                    {
                        try
                        {
                            Array.Reverse(data, incr, 2);
                            elevations.Add(BitConverter.ToInt16(data, incr));
                        }
                        catch
                        {
                        }
                        i++;
                        incr += 2;
                    }
                    
                    // While working within the domain subsets, generate the points for the topo
                    int counter = 0;
                    for (int y = 0; y < yTravel; y++)
                    {

                        List<Point3d> rowPoints = new List<Point3d>();
                        int yPos = (srtmYEnd - y) * formatSize;
                        double yFt = ((float)y / (float)formatSize) * yFtPerDegree;

                        for (int x = 0; x < xTravel; x++)
                        {
                            int pos = yPos + srtmXStart + x;
                            double xFt = ((float)x / (float)formatSize) * xFtPerDegree;
                            Point3d tempPoint;
                            try
                            {
                                tempPoint = new Point3d(xFt, yFt, elevations[pos] * scale);
                                rowPoints.Add(tempPoint);
                            }
                            catch
                            {
                                // MessageBox.Show("Error at (X,Y): (" + x.ToString() + ", " + yPos.ToString() + ")." + "\n\nsrtmYEnd = " + srtmYEnd.ToString());
                            }
                            counter++;
                        }
                        initialPoints.Add(rowPoints);
                    }
                }

            }
            else if (fi.Extension.ToLower() == ".img" || fi.Extension.ToLower() == ".tif")
            {
                // USGS Raster File
                Gdal.AllRegister();
                Dataset ds = Gdal.Open(filePath, Access.GA_ReadOnly);

                double[] geoTransform = new double[6];
                ds.GetGeoTransform(geoTransform);

                Band band = ds.GetRasterBand(1);

                double west = lonDomain.Min;
                double east = lonDomain.Max;
                double north = latDomain.Max;
                double south = latDomain.Min;

                double lower = geoTransform[3] - north;
                double upper = geoTransform[3] - south;
                double left = west - geoTransform[0];
                double right = east - geoTransform[0];

                int startRow = band.XSize - Convert.ToInt32(upper * band.XSize);
                int endRow = band.XSize - Convert.ToInt32(lower * band.XSize);
                int startColumn = Convert.ToInt32(left * band.YSize);
                int endColumn = Convert.ToInt32(right * band.YSize);
                //System.Windows.Forms.MessageBox.Show("lower: " + lower.ToString() + "\nupper: " + upper.ToString() + "\nleft: " + left.ToString() +
                //    "\nright: " + right.ToString() + "\nstartRow: " + startRow.ToString() + "\nendRow" + endRow.ToString() + "\nstartColumn: " + startColumn.ToString() +
                //    "\nendColumn: " + endColumn.ToString());
                byte[] bytes = new byte[band.XSize * band.YSize];
                short[] buffer = new short[band.XSize * band.YSize];
                band.ReadRaster(0, 0, band.XSize, band.YSize, buffer, band.XSize, band.YSize, 0, 0);

                List<List<float>> elevations = new List<List<float>>();
                float high = -100000;
                float low = 100000;
                
                for (int i = 0; i < band.XSize; i++)
                {
                    List<float> currentRow = new List<float>();
                    for (int j = 0; j < band.YSize; j++)
                    {
                        try
                        {
                            float curVal = Convert.ToSingle(buffer[i * band.XSize + j]);
                            currentRow.Add(curVal);
                            if (curVal > high)
                                high = curVal;
                            if (curVal < low)
                                low = curVal;
                        }
                        catch { }
                    }
                    elevations.Add(currentRow);
                }
                elevations.Reverse();
                //System.Windows.Forms.MessageBox.Show("elevations: " + elevations.Count.ToString());

                double theta = (Math.PI / 180) * latDomain.Mid;
                double yFtPerDegree = (111131.745 * scale) / band.XSize;
                double xFtPerDegree = Math.Cos(latDomain.Mid * (Math.PI / 180)) * yFtPerDegree;
                
                for (int i = startRow; i < endRow; i++)
                {
                    List<Point3d> rowPts = new List<Point3d>();
                    double yCoord = (i - startRow) * yFtPerDegree;
                    for (int j = startColumn; j < endColumn; j++)
                    {
                        try
                        {
                            double xCoord = (j - startColumn) * xFtPerDegree;
                            double zCoord = Convert.ToDouble(elevations[i][j]) * scale;
                            Point3d pt = new Point3d(xCoord, yCoord, zCoord);
                            //points.Add(pt, new GH_Path(i - startRow));
                            rowPts.Add(pt);
                        }
                        catch { }
                    }
                    initialPoints.Add(rowPts);
                }
            }
            
            // Flip the matrix of the list to make sure the UV will match up naturally.
            List<List<Point3d>> outPoints = new List<List<Point3d>>();
            for (int i = 0; i < initialPoints[0].Count; i++)
            {
                List<Point3d> tempPoints = new List<Point3d>();
                for (int j = 0; j < initialPoints.Count; j++)
                {
                    try
                    {
                        tempPoints.Add(initialPoints[j][i]);
                    }
                    catch { }
                }
                outPoints.Add(tempPoints);
            }
            return outPoints;
        }

        public static FeatureType ByName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("FeatureType");
            }
            else
            {
                string tempName = string.Empty;
                System.Globalization.TextInfo ti = new System.Globalization.CultureInfo("en-US", false).TextInfo;
                if (name.Contains("_"))
                {
                    string[] split = name.Split(new char[] { '_' });
                    tempName = ti.ToTitleCase(split[0]);
                    tempName += ti.ToTitleCase(split[1]);
                }
                else
                {
                    tempName = ti.ToTitleCase(name);
                }
                FeatureType ft;
                Enum.TryParse(tempName, out ft);
                return ft;
            }
        }
    }
}
