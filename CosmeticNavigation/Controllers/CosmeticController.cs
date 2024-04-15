using Microsoft.AspNetCore.Mvc;
using Tesseract;
using System.Drawing;
using System.Drawing.Imaging;
using CosmeticsStore.Models;
using CosmeticsStore.Core.Abstractions;
using ComponentsStore.Application.Service;
using CosmeticNavigation.Api.Contracts;
using static System.Net.Mime.MediaTypeNames;

namespace CosmeticNavigation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CosmeticController : ControllerBase
    {
        private readonly IComponentsService _componentsService;
        public CosmeticController(IComponentsService componentsService)
        {
            _componentsService = componentsService;
        }
        [HttpPost("Upload")]
        public async Task<ActionResult<List<Components>>> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("File is not selected or empty");
            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var fileName = $"{Guid.NewGuid()}.png";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);//копирование цветной картинки в проект
                }
                var fileName2 = $"{Guid.NewGuid()}.png";                //новый имя новой чб картинки
                var filePath2 = Path.Combine(uploadsFolder, fileName2); //новый путь новой чб картинки
                var SourceImage = Bitmap.FromFile(filePath);//создание картинки в формате Bitmap 
                NewImage(SourceImage, fileName2);//вызов функции создающей чб картинку
                var textArray = GetImageText(filePath2);
                var answer = await _componentsService.GetAnswerComponents(textArray);
                return Ok(answer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        private void NewImage(System.Drawing.Image SourceImage, string fileName)
        {
            using Graphics gr = Graphics.FromImage(SourceImage);// SourceImage is a Bitmap object
            var gray_matrix = new float[][] {
                    new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                    new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                    new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                    new float[] { 0,      0,      0,      1, 0 },
                    new float[] { 0,      0,      0,      0, 1 }
                    };

            var ia = new ImageAttributes();
            ia.SetColorMatrix(new ColorMatrix(gray_matrix));

            ia.SetThreshold((float)0.7); // Change this threshold as needed
            var rc = new Rectangle(0, 0, SourceImage.Width, SourceImage.Height);

            gr.DrawImage(SourceImage, rc, 0, 0, SourceImage.Width, SourceImage.Height, GraphicsUnit.Pixel, ia);
            SourceImage.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }
        private IEnumerable<string> GetImageText(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return Array.Empty<string>();
            }
            using var engine = new TesseractEngine(Path.Combine(Directory.GetCurrentDirectory(), "tessdata"), "rus+eng", EngineMode.Default);
            using Pix pix = Pix.LoadFromFile(imagePath);
            using Tesseract.Page page = engine.Process(pix);
            string text = page.GetText();
            MainText t1 = new MainText { text = text };
            return t1.GetArray();
        }
        [HttpGet]
        public async Task<ActionResult<List<ComponentsResponse>>> GetComponents()
        {
            var components = await _componentsService.GetAllComponents();
            var response = components.Select(b => new ComponentsResponse(b.id, b.name, b.description));
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<List<ComponentsResponse>>> CreateComponents([FromBody] ComponentsResponse request)
        {
            var (components, error) = Components.Create(
                Guid.NewGuid(),
                request.name,
                request.description
                );
            if (!string.IsNullOrEmpty(error) || request.name == String.Empty)
            {
                return BadRequest(error);
            }
            var appId = await _componentsService.CreateComponents(components);
            var response = new ComponentsResponse(appId,request.name, request.description);
            return Ok(response);

        }

    }
}
