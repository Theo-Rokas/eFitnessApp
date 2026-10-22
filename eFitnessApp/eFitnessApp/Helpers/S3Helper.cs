using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static SQLite.SQLite3;

namespace eFitnessApp.Helpers
{
    /// <summary>
    /// This is a helper class for using AWS S3
    /// </summary>
    public class S3Helper
    {
        private static readonly string _bucketName = "Put bucket name here";
        private static readonly string _accessKey = "Put access key here";
        private static readonly string _secretKey = "Put secret key here";
        private static readonly RegionEndpoint _bucketRegion = RegionEndpoint.EUCentral1;



        private static readonly string _exerciseDefaultImageUrl = "Put exercise default image url here";
        public static string GetExerciseDefaultImageUrl() { return _exerciseDefaultImageUrl; }

        private static readonly string _mealDefaultImageUrl = "Put meal default image url here";
        public static string GetMealDefaultImageUrl() { return _mealDefaultImageUrl; }




        /// <summary>
        /// This is a method to upload an image file to AWS S3 bucket
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        public static async Task<string> UploadFile(FileResult imageFile)
        {
            // Create an Amazon S3 Client
            var client = new AmazonS3Client(_accessKey, _secretKey, _bucketRegion);

            // Read the file as stream
            using (var stream = imageFile.OpenReadAsync())
            {
                // The name is current ticks plus the image file name
                var keyName = DateTime.UtcNow.Ticks + Path.GetFileName(imageFile.FileName);

                // Prepare the put request
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = keyName,
                    InputStream = await stream
                };

                // Put object to AWS S3
                await client.PutObjectAsync(request);

                // Create the image file url
                var imageUrl = $"https://{_bucketName}.s3.{_bucketRegion.SystemName}.amazonaws.com/{keyName}";
                return imageUrl;
            }
        }
    }
}
