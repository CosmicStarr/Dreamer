using System;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;


namespace Data.ClassesForInterfaces
{
    public class PhotoService:IPhotoServices
    {
        private readonly Cloudinary _cloud;
        public PhotoService(IOptions<CloudinaryPhotos> config)
        {
            var CloudAccount = new Account
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloud = new Cloudinary(CloudAccount);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var UploadedImage = new ImageUploadResult();
            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var UploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName,stream),
                    Transformation = new Transformation().Height(500).Width(700).Crop("fill").Gravity("face")
                };
                UploadedImage = await _cloud.UploadAsync(UploadParams);
            }
            return UploadedImage;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string PublicId)
        {
            var deletePic = new DeletionParams(PublicId);
            var results = await _cloud.DestroyAsync(deletePic);
            return results;
        }
    }
}