using Common;

namespace CreateAndUpdateKmlLib
{
    public interface IKmlGenerator
    {
        KmlModel.Kml? GenerateKml();
    }
}