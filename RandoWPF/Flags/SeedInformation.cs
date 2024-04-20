﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bartz24.RandoWPF;
public class SeedInformation
{
    public string Seed { get; set; }
    public RandoFlags.SeedMode SeedMode { get; set; }
    public string SeedModeDisplay => SeedMode.ToString();
    public DateTime Created { get; set; }
    public string Version { get; set; }
    public string FlagString { get; set; }    
    public string PresetUsed { get; set; }
    public ArchipelagoData ArchipelagoData { get; set; }
}
