using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace CretBucket
{
    class Program
    {
        static void Main(string[] args)
        {
            string accessKey = "*** Enter Access Key Here ***";
            string secretKey = "*** Enter Secret Key Here ***";
            using (AmazonS3Client client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.APSouth1))
            {
                ListBucketsResponse response = client.ListBuckets();
                foreach (S3Bucket b in response.Buckets)
                {
                    Console.WriteLine(b.BucketName);
                }
            }
            Console.ReadLine();
            
        }
    }
}
