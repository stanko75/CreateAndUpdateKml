using Common;
using System.Xml;

namespace CreateAndUpdateKmlLib;

public class UpdateKmlIfExistsOrCreateNewIfNot : IUpdateKmlIfExistsOrCreateNewIfNot
{
    private readonly IUpdateKml _updateKml;
    private readonly ICreateKml _createKml;
    private readonly IKmlSerializer _kmlSerializer;

    public UpdateKmlIfExistsOrCreateNewIfNot(
        IUpdateKml updateKml
        , ICreateKml createKml
        , IKmlSerializer kmlSerializer)
    {
        _updateKml = updateKml;
        _createKml = createKml;
        _kmlSerializer = kmlSerializer;
    }

    public void Execute(string fileName, string coordinates)
    {
        if (string.IsNullOrWhiteSpace(coordinates))
            throw new Exception("Coordinates cannot be empty!");

        SaveKml saveKml;

        if (File.Exists(fileName))
        {
            KmlModel.Kml? kmlToUpdate = _kmlSerializer.DoDeserialization(fileName);

            _updateKml.OldKml = kmlToUpdate;
            _updateKml.Placemark = new KmlModel.Placemark
            {
                LineString = new KmlModel.LineString
                {
                    Coordinates = coordinates
                }
            };
            saveKml = new SaveKml(_updateKml, _kmlSerializer);
        }
        else
        {
            _createKml.PlaceMarks = new[]
            {
                new KmlModel.Placemark
                {
                    Name = "test"
                    , Description = new XmlDocument().CreateCDataSection("test")
                    , StyleUrl= "styleUrl test"
                    , LineString = new KmlModel.LineString
                    {
                        Extrude = "1"
                        , Tessellate = "1"
                        , AltitudeMode = "absolute"
                        , Coordinates = coordinates
                    }
                }
            };
            saveKml = new SaveKml(_createKml, _kmlSerializer);
        }

        saveKml.Execute(fileName);
    }

}