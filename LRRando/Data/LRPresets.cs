﻿using Bartz24.RandoWPF;

namespace LRRando
{
    public class LRPresets
    {
        public static Preset Custom;
        public static void Init()
        {
            Presets.PresetsList.Clear();

            Presets.LoadPresets();

            Custom = new Preset()
            {
                Name = "Custom (Modified)",
                CustomModified = true
            }.Register();

            Presets.Selected = Presets.PresetsList[0];
        }
    }
}

