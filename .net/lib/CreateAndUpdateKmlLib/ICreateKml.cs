using Common;

namespace CreateAndUpdateKmlLib;

public interface ICreateKml: IKmlGenerator
{
    public KmlModel.Style? Style { get; set; }
    public KmlModel.Placemark[]? PlaceMarks { get; set; }
}