using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignLanguage.APIs.Errors;
using SignLanguage.Application;
using SignLanguage.Core.Service.Contract;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SignLanguage.APIs.Controllers
{
    public class UploadPhotoController : BaseApiController
    {
        private readonly IAttachmentService _attachmentService;
        private readonly HttpClient _httpClient;

        public UploadPhotoController(IAttachmentService attachmentService,HttpClient httpClient)
        {
            _attachmentService = attachmentService;
            _httpClient = httpClient;
        }

        [HttpPost("predict")]
        public async Task<ActionResult> Predict(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new ApiResponse(400));

            // رفع الملف إلى السيرفر
            string? filePath = _attachmentService.Upload(file, "Uploads");
            if (string.IsNullOrEmpty(filePath))
                return StatusCode(500, "File upload failed.");

            // إرسال الملف إلى FastAPI عبر HTTP
            var response = await SendToPythonApi(filePath);
            if (string.IsNullOrEmpty(response))
                return StatusCode(500, "Error calling the Python API.");

            // إرسال الاستجابة من FastAPI مرة أخرى إلى العميل
            return Ok(response);
        }

        private async Task<string> SendToPythonApi(string filePath)
        {
            try
            {
                using var form = new MultipartFormDataContent();
                using var fileStream = new FileStream(filePath, FileMode.Open);
                using var streamContent = new StreamContent(fileStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                form.Add(streamContent, "file", Path.GetFileName(filePath));

                // إرسال الطلب إلى FastAPI
                var response = await _httpClient.PostAsync("https://7b93-196-136-188-62.ngrok-free.app/predict" , form);
                if (response.IsSuccessStatusCode)
                {
                    // قراءة الاستجابة من FastAPI
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                // التعامل مع أي أخطاء
                return $"Error: {ex.Message}";
            }
        }
    }
}
