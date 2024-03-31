using System.Text.Json;

namespace PreparePicturesAndHtmlAndUploadToWebsite;

public class UpdateJsonIfExistsOrCreateNewIfNot
{
    public void Execute(string jsonFileName, LatLngFileNameModel latLngFileNameModel)
    {
        string latLngFileName;

        if (File.Exists(jsonFileName))
        {
            string jsonString = File.ReadAllText(jsonFileName);
            List<LatLngFileNameModel>? latLngFileNameModels = new List<LatLngFileNameModel>();
            try
            {
                latLngFileNameModels =
                    JsonSerializer.Deserialize<List<LatLngFileNameModel>>(jsonString);
                latLngFileNameModels?.Add(latLngFileNameModel);
                latLngFileName = JsonSerializer.Serialize(latLngFileNameModels);
            }
            catch
            {
                LatLngFileNameModel? latLngFileNameModelToSave =
                    JsonSerializer.Deserialize<LatLngFileNameModel>(jsonString);
                if (latLngFileNameModelToSave is not null) latLngFileNameModels?.Add(latLngFileNameModelToSave);
                latLngFileNameModels?.Add(latLngFileNameModel);
                latLngFileName = JsonSerializer.Serialize(latLngFileNameModels);
            }
        }
        else
        {
            latLngFileName = JsonSerializer.Serialize(latLngFileNameModel);
        }

        if (!string.IsNullOrEmpty(latLngFileName))
            File.WriteAllText(jsonFileName, latLngFileName);
    }
}