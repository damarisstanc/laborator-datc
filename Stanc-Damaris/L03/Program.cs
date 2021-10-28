using System;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Google.Apis.Util.Store;

namespace L03
{
    class Program
    {
        private static DriveService _service;
        private static string _token1;
        static void Main(string[] args)
        {
            Initialize();
        }

        static void Initialize()
        {
            string[] scopes = new string[]{
                DriveService.Scope.Drive,
                DriveService.Scope.DriveFile
            };
            var clientId = "979291146571-v5qqcc9oaq3nt97h499o2upvfcsck8lo.apps.googleusercontent.com";
            var clientSecret = "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";

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                scopes,
                Environment.UserName,
                CancellationToken.None,

                new FileDataStore("Daimto.GoogleDrive.Auth.Store2")
            ).Result;
            _service = new DriveService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            _token1 = credential.Token.AccessToken;
            Console.Write("Token: " + credential.Token.AccessToken);
            GetMyFiles();
            UploadFile("C:\\Users\\Damaris\\Documents\\laborator-datc\\Stanc-Damaris\\L03\\fisier.txt", _service);
        }

    static void GetMyFiles()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/drive/v3/files?q='root'%20in%20parents");
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _token1);

            using (var response = request.GetResponse())
            {
                using (Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();
                    var myData = JObject.Parse(text);
                    foreach (var file in myData["files"])
                    {
                        if (file["mimeType"].ToString() != "application/vnd.google-apps.folder")
                        {
                            Console.WriteLine("File name: " + file["name"]);
                        }
                    }
                }
            }
        }

        static void UploadFile(string path, DriveService _service)
        {
            var driveFile = new Google.Apis.Drive.v3.Data.File();
            driveFile.Name = Path.GetFileName(path);
            driveFile.MimeType = "text/plain";

            var stream = new System.IO.FileStream(path, System.IO.FileMode.Open);
            var request = _service.Files.Create(driveFile, stream, "text/plain");
            request.Fields = "id";
            request.Upload();

            var file = request.ResponseBody;
            Console.WriteLine("successfully uploaded: File id: " + file.Id);
        }

    }

}
