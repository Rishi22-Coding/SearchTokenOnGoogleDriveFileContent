using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

//Globally declared variables
List<string> listName = new List<string>();
List<string> listId = new List<string>();

//scope to search authenticate user and search for files and folders listed in the parent directory.
{
    const string pathToCredentials = @"E:\PROJECTS\SearchOnGoogleDriveAPI\credentials.json";
    var tokenStorage = new FileDataStore("UserCredentialStoragePath", true);
    UserCredential credential;
    await using (var stream = new FileStream(pathToCredentials, FileMode.Open, FileAccess.Read))
    {
        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                new[] { DriveService.ScopeConstants.DriveReadonly },
                "userName",
                CancellationToken.None,
                tokenStorage)
            .Result;
    }
    
    //create new drive service
    var service = new DriveService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = credential
    });
    
    var request = service.Files.List();
    var results = await request.ExecuteAsync();
    Console.WriteLine("Searching for Files and Folders....");
    foreach (var driveFile in results.Files)
    {
        Console.WriteLine($"{driveFile.Name} {driveFile.MimeType} {driveFile.Id} {driveFile.WebViewLink}");
        listName.Add(driveFile.Name);
        listId.Add(driveFile.Id);
    }
}

//Method to get the content of a .txt files.
string GetContent(string url, string authToken)
{
    string resultContent = "";
    var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);

    using (HttpResponseMessage response = httpClient.GetAsync(url).Result)
    {
        using (HttpContent content = response.Content)
        {
            var json = content.ReadAsStringAsync().Result;
            Console.WriteLine(json);
            resultContent = json;
        }
    }
    return resultContent;
}

//Method to search from the token on the content passed by API. (For now I have hardcoded the Token)
List<KeyValuePair<string, string>> SearchToken(List<KeyValuePair<string, string>> contentList, string token)
{
    var fileDetails = new List<KeyValuePair<string, string>>();
    int numberIteration = 0;
    foreach (var checkIterator in contentList)
    {
        var content = checkIterator.Value;
        if (content.Contains(token))
        {
            fileDetails.Add(new KeyValuePair <string, string>(checkIterator.Key, listId[numberIteration]));
            /*Console.WriteLine(checkIterator.Key);*/
        }

        numberIteration++;
    }
    return fileDetails;
}

//Implemented functionalities
void Main()
{
    Console.WriteLine("Fetching Contents...");
    var contentList = new List<KeyValuePair<string, string>>();
    var authToken = "ya29.A0AVA9y1sA00g58YKngmPA5PANl54Mm8PTp0OLZUmhcs1Bcv8tLvDbZCh135msrBLoIlPG2qCrxanU1VmLVuHdP88DwLYVapJOpbC3TgVaKeWEubp_lu7VxGzwzWHAm2ypkajlETjgZ8FF70w4s57btSIC4OW5YUNnWUtBVEFTQVRBU0ZRRTY1ZHI4d0xmU1kzbU5tY19JSzdDTWk5RndSZw0163";
    var content1 = GetContent(
            "https://www.googleapis.com/drive/v3/files/1fRzgazg9Jovp8oKDJene6UjcV8WNaval?alt=media&key=AIzaSyDCTZpRY-YG3i1KO1cryafvUd-K3DV-aJg HTTP/1.1",
            authToken);
    var content2 = GetContent(
        "https://www.googleapis.com/drive/v3/files/1lrG0bg4WwkVJCk_ABK88ZnJMBwUeoGJZ?alt=media&key=AIzaSyDCTZpRY-YG3i1KO1cryafvUd-K3DV-aJg HTTP/1.1",
        authToken);
    contentList.Add(new KeyValuePair <string, string>(listName[0], content1));
    contentList.Add(new KeyValuePair <string, string>(listName[1], content2));
    List<KeyValuePair<string, string>> fileDetails = SearchToken(contentList, "test2");
    Console.WriteLine("Searching Started");
    if (fileDetails.Count > 0)
    {
        foreach (var itr in fileDetails)
        {
            Console.WriteLine("Found Token: ");
            Console.WriteLine(itr);
        }
    }
    else
    {
        Console.WriteLine("Token is Invalid");
    }
}
Main(); //Calling the Main function.