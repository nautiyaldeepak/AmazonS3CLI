using System;
using Amazon.S3;
using Amazon.S3.Model;

namespace NewBucket
{
    class Program
    {
        static void Main(string[] args)
        {
            string accessKey = "*** Enter Access Key Here ***";
            string secretKey = "*** Enter Secret Key Here ***";
            string NameOFTheBucket = "*** Name of the Bucket Here ***";
            AmazonS3Client client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.APSouth1);
            PutBucketRequest request = new PutBucketRequest
            {
                BucketName = NameOFTheBucket,
                UseClientRegion = true
            };
            client.PutBucket(request);
            Console.WriteLine("Bucket Created");
            Console.ReadLine();
        }
    }
}
