﻿using Newtonsoft.Json.Linq;

namespace CreateAndUpdateKmlWebApi.Models;

public class LiveConfigModel
{
    public string? KmlFileName { get; set; }
    public string? CurrentLocation { get; set; }
    public string? LiveImageMarkersJsonUrl { get; set; }
}