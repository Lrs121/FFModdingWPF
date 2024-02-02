﻿using Bartz24.Data;
using Bartz24.Docs;
using Bartz24.FF13;
using Bartz24.RandoWPF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FF13Rando;

public partial class TreasureRando : Randomizer
{
    public DataStoreWDB<DataStoreTreasurebox> treasuresOrig = new();
    public DataStoreWDB<DataStoreTreasurebox> treasures = new();
    private readonly Dictionary<string, HintData> hintData = new();

    public Dictionary<string, FF13ItemLocation> ItemLocations = new();
    private readonly Dictionary<string, List<string>> hintsMain = new();
    private readonly Dictionary<string, int> hintsNotesUniqueCount = new();
    private readonly Dictionary<string, int> hintsNotesSharedCount = new();
    //private AssumedItemPlacementAlgorithm<FF13ItemLocation> placementAlgoNormal;
    //private ItemPlacementAlgorithm<FF13ItemLocation> placementAlgoBackup;
    private bool usingBackup = false;

    //public ItemPlacementAlgorithm<FF13ItemLocation> PlacementAlgo => usingBackup ? placementAlgoBackup : placementAlgoNormal;

    public TreasureRando(SeedGenerator randomizers) : base(randomizers) { }

    public override void Load()
    {
        RandoUI.SetUIProgressIndeterminate("Loading Treasure Data...");
        treasuresOrig.LoadWDB(Generator, "13", @"\db\resident\treasurebox.wdb");
        treasures.LoadWDB(Generator, "13", @"\db\resident\treasurebox.wdb");

        ItemLocations.Clear();

        FileHelpers.ReadCSVFile(@"data\treasures.csv", row =>
        {
            TreasureData t = new(Generator, row);
            ItemLocations.Add(t.ID, t);
        }, FileHelpers.CSVFileHeader.HasHeader);

        FileHelpers.ReadCSVFile(@"data\battledrops.csv", row =>
        {
            BattleDropData b = new(Generator, row);
            ItemLocations.Add(b.ID, b);
        }, FileHelpers.CSVFileHeader.HasHeader);

        FileHelpers.ReadCSVFile(@"data\enemydrops.csv", row =>
        {
            EnemyDropData e = new(Generator, row);
            ItemLocations.Add(e.ID, e);
        }, FileHelpers.CSVFileHeader.HasHeader);

        hintData.Clear();

        string[] chars = { "lig", "fan", "hop", "saz", "sno", "van" };
        string[] roles = { "com", "rav", "sen", "syn", "sab", "med" };
        int[] hp = { 200, 390, 170, 220, 350, 350 };
        int[] str = { 15, 28, 8, 12, 17, 23 };
        int[] mag = { 15, 22, 20, 12, 7, 35 };

        foreach (string c in chars)
        {
            foreach (string r in roles)
            {
                AddTreasure($"z_ran_{c}_{r}", $"rol_{c}_{r}", 1);
            }

            AddTreasure($"z_ini_{c}_hp", "", hp[chars.ToList().IndexOf(c)]);
            AddTreasure($"z_ini_{c}_str", "", str[chars.ToList().IndexOf(c)]);
            AddTreasure($"z_ini_{c}_mag", "", mag[chars.ToList().IndexOf(c)]);
            AddTreasure($"z_ini_{c}_wea", $"wea_{c}_001", 1);
        }

        AddTreasure("z_ini_potion", "it_potion", 5);
        AddTreasure("z_ini_gil", "", 200);
        AddTreasure("z_ini_shop", "key_shop_00", 1);

        AddTreasure("z_ran_stg_03", "cry_stage", 1);
        AddTreasure("z_ran_stg_04", "cry_stage", 1);
        AddTreasure("z_ran_stg_05", "cry_stage", 1);
        AddTreasure("z_ran_stg_06", "cry_stage", 1);
        AddTreasure("z_ran_stg_07", "cry_stage", 1);
        AddTreasure("z_ran_stg_09", "cry_stage", 1);
        AddTreasure("z_ran_stg_10", "cry_stage", 1);
        AddTreasure("z_ran_stg_11", "cry_stage", 1);
        AddTreasure("z_ran_stg_13", "cry_stage", 1);

        AddTreasure("z_shp_03_06", "key_shop_06", 1);
        AddTreasure("z_shp_04_06", "key_shop_06", 1);
        AddTreasure("z_shp_05_00", "key_shop_00", 1);
        AddTreasure("z_shp_05_06", "key_shop_06", 1);
        AddTreasure("z_shp_06_02", "key_shop_02", 1);
        AddTreasure("z_shp_06_07", "key_shop_07", 1);
        AddTreasure("z_shp_07_00", "key_shop_00", 1);
        AddTreasure("z_shp_07_06", "key_shop_06", 1);
        AddTreasure("z_shp_07_12", "key_shop_12", 1);
        AddTreasure("z_shp_08_07", "key_shop_07", 1);
        AddTreasure("z_shp_08_10", "key_shop_10", 1);
        AddTreasure("z_shp_09_00", "key_shop_00", 1);
        AddTreasure("z_shp_09_06", "key_shop_06", 1);
        AddTreasure("z_shp_09_12", "key_shop_12", 1);
        AddTreasure("z_shp_10_00", "key_shop_00", 1);
        AddTreasure("z_shp_10_03", "key_shop_03", 1);
        AddTreasure("z_shp_10_06", "key_shop_06", 1);
        AddTreasure("z_shp_10_07", "key_shop_07", 1);
        AddTreasure("z_shp_10_10", "key_shop_10", 1);
        AddTreasure("z_shp_10_11", "key_shop_11", 1);
        AddTreasure("z_shp_11_00", "key_shop_00", 1);
        AddTreasure("z_shp_11_06", "key_shop_06", 1);
        AddTreasure("z_shp_11_07", "key_shop_07", 1);
        AddTreasure("z_shp_11_08", "key_shop_08", 1);
        AddTreasure("z_shp_11_10", "key_shop_10", 1);
        AddTreasure("z_shp_11_11", "key_shop_11", 1);
        AddTreasure("z_shp_11_02b", "key_shop_02", 1);
        AddTreasure("z_shp_11_03b", "key_shop_03", 1);
        AddTreasure("z_shp_11_06b", "key_shop_06", 1);
        AddTreasure("z_shp_11_12b", "key_shop_12", 1);
        AddTreasure("z_shp_12_03", "key_shop_03", 1);
        AddTreasure("z_shp_12_06", "key_shop_06", 1);
        AddTreasure("z_shp_13_03", "key_shop_03", 1);

        List<string> hintsNotesLocations = hintData.Values.SelectMany(h => h.Areas).ToList();
        List<string> locations = ItemLocations.Values.SelectMany(t => t.Areas).Distinct().ToList();

        //placementAlgoNormal = new AssumedItemPlacementAlgorithm<FF13ItemLocation>(ItemLocations, locations, Generator, 3);
        //placementAlgoNormal.Logic = new FF13ItemPlacementLogic(placementAlgoNormal, Generator);

        //placementAlgoBackup = new ItemPlacementAlgorithm<FF13ItemLocation>(ItemLocations, locations, Generator, -1);
        //placementAlgoBackup.Logic = new FF13ItemPlacementLogic(placementAlgoBackup, Generator);

    }

    public void AddTreasure(string newName, string item, int count)
    {
        AddTreasure(treasuresOrig, newName, item, count);
        AddTreasure(treasures, newName, item, count);
    }

    private void AddTreasure(DataStoreWDB<DataStoreTreasurebox> database, string newName, string item, int count)
    {
        database.Copy(database.Keys[0], newName);
        database[newName].sItemResourceId_string = item;
        database[newName].iItemCount = (uint)count;
    }

    public override void Randomize()
    {
        RandoUI.SetUIProgressIndeterminate("Randomizing Treasure Data...");
        if (FF13Flags.Items.Treasures.FlagEnabled)
        {
            FF13Flags.Items.Treasures.SetRand();

            Dictionary<string, double> areaMults = ItemLocations.Values.SelectMany(t => t.Areas).Distinct().ToDictionary(s => s, _ => RandomNum.RandInt(10, 200) * 0.01d);
            //if (!placementAlgoNormal.Randomize(new List<string>(), areaMults))
            {
                usingBackup = true;
                //placementAlgoBackup.Randomize(placementAlgoBackup.Logic.GetNewAreasAvailable(new Dictionary<string, int>(), new List<string>()), areaMults);
            }

            RandoUI.SetUIProgressDeterminate("Randomizing Treasure Data...", 60, 100);

            // Update hints again to reflect actual numbers
            //PlacementAlgo.HintsByLocation.ForEach(l =>
            //{
                //int uniqueCount = ItemLocations.Keys.Where(t => PlacementAlgo.Placement.ContainsKey(t) && ItemLocations[t].Areas.Count == 1 && ItemLocations[t].Areas[0] == l && PlacementAlgo.Logic.IsHintable(PlacementAlgo.Placement[t])).Count();
                //hintsNotesUniqueCount.Add(l, uniqueCount);

                //int sharedCount = ItemLocations.Keys.Where(t => PlacementAlgo.Placement.ContainsKey(t) && ItemLocations[t].Areas.Count > 1 && ItemLocations[t].Areas.Contains(l) && PlacementAlgo.Logic.IsHintable(PlacementAlgo.Placement[t])).Count();
                //hintsNotesSharedCount.Add(l, sharedCount);
            //});

            RandomNum.ClearRand();
        }

        RandoUI.SetUIProgressDeterminate("Randomizing Treasure Data...", 70, 100);
        if (FF13Flags.Items.ShuffleRoles.FlagEnabled)
        {
            FF13Flags.Items.ShuffleRoles.SetRand();
            string[] chars = { "lig", "fan", "hop", "saz", "sno", "van" };
            string[] roles = { "com", "rav", "sen", "syn", "sab", "med" };

            foreach (string c in chars)
            {
                string first = c == "saz" ? "rav" : RandomNum.SelectRandomWeighted(new List<string>() { "com", "rav" }, _ => 1);
                List<string> rolesRemaining = roles.Where(r => r != first).Shuffle();
                ItemLocations.Values.First(t => t.ID.StartsWith("z_ran_" + c) && ItemLocations[t.ID].Traits.Contains("Same"))
                    .SetItem($"rol_{c}_{first}", 1);

                ItemLocations.Values.Where(t => ItemLocations[t.ID].GetItem(false).Value.Item1.StartsWith("rol_" + c) && !ItemLocations[t.ID].Traits.Contains("Same")).ForEach(t =>
                {
                    ItemLocations[t.ID].SetItem($"rol_{c}_{rolesRemaining[0]}", 1);
                    rolesRemaining.RemoveAt(0);
                });
            }

            RandomNum.ClearRand();
        }

        RandoUI.SetUIProgressDeterminate("Randomizing Treasure Data...", 80, 100);
        if (FF13Flags.Items.ShuffleShops.FlagEnabled)
        {
            FF13Flags.Items.ShuffleShops.SetRand();

            ItemLocations.Values.Where(t => IsShop(t.ID, false)).ToList().Shuffle((l1, l2) =>
            {
                (string, int) temp = ItemLocations[l1.ID].GetItem(false).Value;
                l1.SetItem(ItemLocations[l2.ID].GetItem(false).Value.Item1, 1);
                l2.SetItem(temp.Item1, 1);
            });

            RandomNum.ClearRand();
        }

        RandoUI.SetUIProgressDeterminate("Randomizing Treasure Data...", 90, 100);
        if (FF13Flags.Items.Treasures.FlagEnabled)
        {
            FF13Flags.Items.Treasures.SetRand();

            EquipRando equipRando = Generator.Get<EquipRando>();

            List<string> remainingWeapons = equipRando.itemData.Values.Where(i => i.Category == "Weapon").Select(i => i.ID).ToList();

            ItemLocations.Values.Where(t => (!t.ID.EndsWith("_wea") || FF13Flags.Items.StartingEquip.FlagEnabled) && equipRando.itemData.ContainsKey(ItemLocations[t.ID].GetItem(false).Value.Item1)).ForEach(t =>
            {
                (string, int) orig = ItemLocations[t.ID].GetItem(false).Value;
                string repItem = null;
                do
                {
                    string category = equipRando.itemData[orig.Item1].Category;
                    if (FF13Flags.Items.ReplaceAny.Enabled)
                    {
                        category = equipRando.itemData.Values.Select(i => i.Category).Distinct().Shuffle().First();
                    }

                    int rankRange = FF13Flags.Items.ReplaceRank.Value;
                    IEnumerable<ItemData> possible = equipRando.itemData.Values.Where(i =>
                        i.Category == category &&
                        i.Rank >= equipRando.itemData[orig.Item1].Rank - rankRange &&
                        i.Rank <= equipRando.itemData[orig.Item1].Rank + rankRange &&
                        (i.Category != "Weapon" || remainingWeapons.Contains(i.ID)));

                    if (t.ID.EndsWith("_wea"))
                    {
                        string prefix = orig.Item1.Substring(0, "wea_xxx_".Length);
                        possible = possible.Where(i => i.ID.StartsWith(prefix));
                    }

                    if (possible.Count() == 0)
                    {
                        continue;
                    }

                    repItem = possible.Shuffle().Select(i => i.ID).First();
                } while (repItem == null);

                if (equipRando.itemData[repItem].Category == "Weapon")
                {
                    remainingWeapons.Remove(repItem);
                }

                int repCount = equipRando.itemData[repItem].Category == "Weapon" ? 1 : orig.Item2;
                ItemLocations[t.ID].SetItem(repItem, repCount);
            });

            ItemLocations.Values.Where(t => ItemLocations[t.ID].GetItem(false).Value.Item1 == "" || equipRando.itemData.ContainsKey(ItemLocations[t.ID].GetItem(false).Value.Item1)).ForEach(t =>
            {
                (string, int) orig = ItemLocations[t.ID].GetItem(false).Value;
                if (orig.Item2 > 0)
                {
                    ItemLocations[t.ID].SetItem(orig.Item1, RandomNum.RandInt((int)Math.Round(Math.Max(1, orig.Item2 * 0.75)), (int)Math.Round(orig.Item2 * 1.25)));
                }
            });

            RandomNum.ClearRand();
        }
    }

    public bool IsRepeatableAllowed(string location)
    {
        return IsInitRole(location) || IsOtherRole(location) || IsEidolon(location) || ItemLocations[location].GetItem(true).Value.Item1 == "key_ctool" || IsGysahlReins(location);
    }

    public bool IsImportantKeyItem(string location)
    {
        return IsInitRole(location) || IsOtherRole(location) || IsStage(location) || IsEidolon(location) || IsShop(location) || IsGysahlReins(location);
    }
    public bool IsInitRole(string t)
    {
        return ItemLocations[t].GetItem(true).Value.Item1.StartsWith("rol") && ItemLocations[t].Areas.Contains("Initial");
    }

    public bool IsOtherRole(string t)
    {
        return ItemLocations[t].GetItem(true).Value.Item1.StartsWith("rol") && !ItemLocations[t].Areas.Contains("Initial");
    }

    public bool IsStage(string t, bool orig = true)
    {
        return ItemLocations[t].GetItem(orig).Value.Item1 == "cry_stage";
    }

    public bool IsEidolon(string t, bool orig = true)
    {
        return ItemLocations[t].Traits.Contains("Eidolon");
    }
    public bool IsShop(string t, bool orig = true)
    {
        return ItemLocations[t].GetItem(orig).Value.Item1.StartsWith("key_shop") || ItemLocations[t].GetItem(orig).Value.Item1 == "key_ctool";
    }
    public bool IsGysahlReins(string t, bool orig = true)
    {
        return ItemLocations[t].GetItem(orig).Value.Item1 == "key_field_00";
    }

    private void SaveHints()
    {
    }

    public override void Save()
    {
        RandoUI.SetUIProgressIndeterminate("Saving Treasure Data...");
        SaveHints();
        treasures.SaveWDB(Generator, @"\db\resident\treasurebox.wdb");
    }

    public override Dictionary<string, HTMLPage> GetDocumentation()
    {
        Dictionary<string, HTMLPage> pages = base.GetDocumentation();
        HTMLPage page = new("Item Locations", "template/documentation.html");

        page.HTMLElements.Add(new Table("Item Locations", (new string[] { "Name", "New Contents", "Difficulty" }).ToList(), (new int[] { 45, 45, 10 }).ToList(), ItemLocations.Values.Select(t =>
        {
            string itemID = ItemLocations[t.ID].GetItem(false).Value.Item1;
            string name = GetItemName(itemID);
            string reqsDisplay = t.Requirements.GetDisplay(GetItemName);
            if (reqsDisplay.StartsWith("(") && reqsDisplay.EndsWith(")"))
            {
                reqsDisplay = reqsDisplay.Substring(1, reqsDisplay.Length - 2);
            }

            string location = $"{ItemLocations[t.ID].Name}";

            TableCellMultiple nameCell = new(new List<string>());
            nameCell.Elements.Add($"<div style=\"margin-right: auto\">{t.Areas[0]} - {location}</div>");
            if (reqsDisplay != ItemReq.TRUE.GetDisplay())
            {
                nameCell.Elements.Add(new IconTooltip("common/images/lock_white_48dp.svg", "Requires: " + reqsDisplay).ToString());
            }

            return (new object[] { nameCell, $"{name} x {ItemLocations[t.ID].GetItem(false).Value.Item2}", t.BaseDifficulty.ToString() }).ToList();
        }).ToList(), "itemlocations"));

        pages.Add("item_locations", page);
        return pages;
    }

    private string GetItemName(string itemID)
    {
        EquipRando equipRando = Generator.Get<EquipRando>();
        TextRando textRando = Generator.Get<TextRando>();
        string name;
        if (itemID == "")
        {
            name = "Gil";
        }
        else
        {
            name = textRando.mainSysUS[equipRando.items[itemID].sItemNameStringId_string];
            if (name.Contains("{End}"))
            {
                name = name.Substring(0, name.IndexOf("{End}"));
            }
        }

        return name;
    }
}
