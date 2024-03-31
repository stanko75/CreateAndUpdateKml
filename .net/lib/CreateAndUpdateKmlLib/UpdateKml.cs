using static Common.KmlModel;

namespace CreateAndUpdateKmlLib;

public class UpdateKml : IUpdateKml
{
    public Kml? GenerateKml()
    {
        if (OldKml is not null
                && OldKml.Document is not null
           )
        {
            if (OldKml.Document.Style is not null
                    && Style is not null
                )
            {
                if (!string.Equals(OldKml.Document.Style.Id, Style.Id))
                {
                    OldKml.Document.Style.Id = Style.Id;
                }

                if (OldKml.Document.Style.PolyStyle is not null
                        && Style.PolyStyle is not null
                        && !string.Equals(OldKml.Document.Style.PolyStyle.Color, Style.PolyStyle.Color)
                    )
                {
                    OldKml.Document.Style.PolyStyle.Color = Style.PolyStyle.Color;
                }

                if (OldKml.Document.Style.LineStyle is not null
                        && Style.LineStyle is not null
                        && !string.Equals(OldKml.Document.Style.LineStyle.Color, Style.LineStyle.Color)
                    )
                {
                    OldKml.Document.Style.LineStyle.Color = Style.LineStyle.Color;
                }
            }

            if (OldKml.Document.Placemarks is not null
                    && Placemark is not null
                )
            {
                LineString? lineString = OldKml.Document.Placemarks[0].LineString;
                if (lineString is not null)
                {
                    lineString.Coordinates = lineString.Coordinates + ", " + Placemark.LineString?.Coordinates;
                }
            }
            else if (OldKml.Document.Placemarks is null
                     && Placemark is not null)
            {
                OldKml.Document.Placemarks = new Placemark[1];
                OldKml.Document.Placemarks[0] = new Placemark
                {
                    LineString = new LineString
                    {
                        Coordinates = Placemark.LineString?.Coordinates
                    }
                };
            }
        }

        return OldKml;
    }

    public Style? Style { get; set; }
    public Placemark? Placemark { get; set; }

    public Kml? OldKml { get; set; }
}