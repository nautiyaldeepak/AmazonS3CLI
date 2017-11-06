using System;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace ListVersions
{
    class Program
    {
        static void Main(string[] args)
        {
            string AccessKey = "";
            string SecretKey = "";
            string NameOfTheBucket = "";
            try
            {
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.APSouth1);
                ListObjectsV2Request Request = new ListObjectsV2Request();
                Request.BucketName = NameOfTheBucket;
                ListObjectsV2Response Response;
                do
                {
                    Response = client.ListObjectsV2(Request);

                    // Process response.
                    foreach (S3Object entry in Response.S3Objects)
                    {
                        Console.WriteLine(entry.Key);
                    }
                    Request.ContinuationToken = Response.NextContinuationToken;
                } while (Response.IsTruncated == true);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}