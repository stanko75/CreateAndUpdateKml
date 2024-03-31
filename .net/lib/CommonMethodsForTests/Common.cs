using System.Xml;
using Common;
using CreateAndUpdateKmlLib;

namespace CommonMethodsForTests;

public static class Common
{
    public static void CreateKml(CreateKml createKml)
    {
        createKml.Style = new KmlModel.Style
        {
            Id = "red",
            LineStyle = new KmlModel.LineStyle
            {
                Color = "black",
                Width = "4"
            },
            PolyStyle = new KmlModel.PolyStyle
            {
                Color = "white"
            }
        };

        createKml.PlaceMarks = new[]
        {
            new KmlModel.Placemark
            {
                Name = "unit test place mark name"
                , Description = new XmlDocument().CreateCDataSection("unit test place mark description")
                , StyleUrl= "styleUrl test"
                , LineString = new KmlModel.LineString
                {
                    Extrude = "1"
                    , Tessellate = "1"
                    , AltitudeMode = "absolute"
                    , Coordinates = "12.8977216, 48.8408876, 2357, 12.8965296, 48.8417986, 2357, 12.8953309, 48.8426691, 2357"
                    //, Coordinates = "12.8977216, 48.8408876, 12.8965296, 48.8417986, 12.8953309, 48.8426691"
                }
            }
        };
    }

    public static string ChangeFileExtension(string fileName, string extension)
    {
        if (!Path.GetExtension(fileName).Equals(extension, StringComparison.OrdinalIgnoreCase))
        {
            fileName = Path.ChangeExtension(fileName, extension);
        }

        return fileName;
    }
}