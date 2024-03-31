using System.Text;
using Newtonsoft.Json.Linq;

namespace KmlCoordinatesHandling;

public class ReadCoordinatesFromFiles : ICoordinatesHandling
{
    public string GenerateCoordinates(string path)
    {
        return AppendLocationsFromPath(path).ToString();
    }

    public StringBuilder AppendLocationsFromPath(string path)
    {
        StringBuilder sb = new StringBuilder();
        if (Directory.Exists(path))
        {
            foreach (string file in Directory.GetFiles(path))
            {
                JObject myJObject = JObject.Parse(File.ReadAllText(file));
                sb.Append($"{myJObject["lng"]}, {myJObject["lat"]}, 2357 {Environment.NewLine}");
            }
        }

        return sb;
    }
}