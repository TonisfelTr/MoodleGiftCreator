using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiftQuestionCreator
{
    public class FileUploader
    {
        private readonly string _serverUrl;

        public FileUploader(string serverUrl)
        {
            _serverUrl = serverUrl;
        }

        public async Task UploadFileAsync(string filePath)
        {
            using (HttpClient client = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                    // Добавляем файл в форму
                    formData.Add(fileContent, "file", Path.GetFileName(filePath));

                    // Отправляем запрос
                    HttpResponseMessage response = await client.PostAsync(_serverUrl, formData);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Ошибка сервера: {response.ReasonPhrase}");
                    }
                }
            }
        }
    }
}
