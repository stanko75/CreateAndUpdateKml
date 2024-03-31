// See https://aka.ms/new-console-template for more information

using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

Console.WriteLine("Hello, World!");
string imageUrl = @"C:\Users\Pera Haker\Pictures\mallorca.jpg"; // Replace with the path to your image file

// Convert image to Base64 string
string base64Image = ConvertImageToBase64(imageUrl);

// Create JSON object with image data
var jsonData = new JObject
{
    ["image"] = base64Image
    , ["testKey"] = "test"
};


// Convert JSON object to string
string jsonContent = jsonData.ToString();

// Send JSON data to the server
using (var client = new HttpClient())
{
    StringContent content = new StringContent(jsonContent);
    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

    var response = await client.PostAsync("https://localhost:7293/api/UpdateCoordinates/UploadImage", content);

    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("Image uploaded successfully!");
    }
    else
    {
        Console.WriteLine("Failed to upload image. Status code: " + response.StatusCode);
    }
}

static string ConvertImageToBase64(string imagePath)
{
    byte[] imageBytes = File.ReadAllBytes(imagePath);
    return Convert.ToBase64String(imageBytes);
}