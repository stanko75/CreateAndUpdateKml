using static Common.KmlModel;

namespace CreateAndUpdateKmlLib;

public interface IUpdateKml : IKmlGenerator
{
    public Style? Style { get; set; }
    public Placemark? Placemark { get; set; }

    public Kml? OldKml { get; set; }
}