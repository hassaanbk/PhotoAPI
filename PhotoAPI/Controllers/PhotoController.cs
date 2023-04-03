using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoAPI.Models;
using PhotoAPI.Services;
using Amazon.S3;

namespace PhotoAPI.Controllers
{
    [Route("api/photos")]
    public class PhotoController : BaseController<PhotoController>
    {
        private readonly IPhotoService _photoService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPhotoUploadService _photoUploadService;

        public PhotoController(IPhotoService photoService, ILogger<PhotoController> logger, IWebHostEnvironment webHostEnvironment, IPhotoUploadService photoUploadService) : base(logger)
        {
            _photoService = photoService;
            _webHostEnvironment = webHostEnvironment;
            _photoUploadService = photoUploadService;
        }

        // GET: api/photo/{name}
        [HttpGet("{name}")]
        public async Task<IActionResult> FindByName(string name)
        {
            var photo = await _photoService.FindByName(name);
            if(photo == null)
            {
                return NotFound();
            }
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto([FromForm] string name, [FromForm] IFormFile image)
        {
            var newPhoto = new Photo();
            newPhoto.Name = name;

            if (image == null || image.Length == 0)
                return BadRequest("File is Empty");

            string webRoot = _webHostEnvironment.WebRootPath;

            string extension = Path.GetExtension(image.FileName);

            string photoName = Path.Combine(webRoot + Guid.NewGuid().ToString() + extension);

            try
            {
                string uploadPhotoUrl = await _photoUploadService.UploadPhoto(image, photoName);

                newPhoto.Url = uploadPhotoUrl;

                newPhoto = await _photoService.Add(newPhoto);
            }
            catch (DbUpdateException e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch(AmazonS3Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return Ok(newPhoto);
        }

        // PUT: api/photo
        [HttpPut]
        public async Task<IActionResult> UpdatePhoto([FromForm] string name, [FromForm] IFormFile image)
        {
            var newPhoto = new Photo();
            newPhoto.Name = name;

            if (image == null || image.Length == 0)
                return BadRequest("File is Empty");

            string webRoot = _webHostEnvironment.WebRootPath;
            string extension = Path.GetExtension(image.FileName);
            string photoName = Path.Combine(webRoot + Guid.NewGuid().ToString() + extension);

            try
            {
                await _photoUploadService.DeletePhoto(name);

                string uploadPhotoUrl = await _photoUploadService.UploadPhoto(image, photoName);

                newPhoto.Url = uploadPhotoUrl;

                await _photoService.Update(newPhoto);
            }
            catch (DbUpdateException e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return Ok(newPhoto);

        }


        // DELETE: api/Photo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(Guid id)
        {
            var photo = await _photoService.FindById(id);

            if(photo == null)
                return NotFound();

            try
            {
                await _photoService.Delete(photo);
            }
            catch(Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return NoContent();
        }

        
    }
}
