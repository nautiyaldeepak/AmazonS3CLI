//  Environment: Visual Studio 2017
//  Download NuGet Package AWSSDK.S3
using System;
using Amazon.S3;
using Amazon.S3.Model;

namespace GenerateObjectURL
{
    class Program
    {
        static void Main(string[] args)
        {
            string AccessKey = " *** Enter Access Key Here *** ";
            string SecretKey = " *** Enter Secret Key Here *** ";
            string NameOfTheBucket = " *** Name Of The Bucket *** ";
            string NameOfTheObject = " *** Name Of The Object *** ";
            string RegionOfTheBucket = " *** Enter The Region Of The Bucket (Eg: mumbai) ***";
            RegionOfTheBucket = RegionOfTheBucket.ToLower();
            try
            {
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.APSouth1);
                GetPreSignedUrlRequest Request = new GetPreSignedUrlRequest
                {
                    BucketName = NameOfTheBucket,
                    Key = NameOfTheObject,
                    Expires = DateTime.Now.AddHours(1),
                    Protocol = Protocol.HTTP
                };
                string url = client.GetPreSignedURL(Request);
                Console.WriteLine(url);
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
            Console.ReadLine();
        }
    }
}
