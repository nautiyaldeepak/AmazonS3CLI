using System;
using Amazon.S3;
using Amazon.S3.Model;

namespace EnableVersioning
{
    class Program
    {
        public static string ClientRegion(string RegionOfClient)
        {
            switch (RegionOfClient)
            {
                case "ireland":
                    return "eu-west-1";
                case "mumbai":
                    return "ap-south-1";
                case "frankfurt":
                    return "eu-central-1";
                case "london":
                    return "eu-west-2";
                case "sydney":
                    return "ap-southeast-2";
                case "ohio":
                    return "us-east-2";
                case "virginia":
                    return "us-east-1";
                case "california":
                    return "us-west-1";
                case "oregon":
                    return "us-west-2";
                case "singapore":
                    return "ap-southeast-1";
                case "tokyo":
                    return "ap-northeast-1";
                case "canada":
                    return "ca-central-1";
                case "seoul":
                    return "ap-northeast-2";
                case "sao paulo":
                    return "sa-east-1";
            }
            return "";
        }
        static void Main(string[] args)
        {
            string AccessKey = "AKIAJK4FKM5Y47J27OKQ";
            string SecretKey = "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";
            string NameOfTheBucket = "test-name-mumbai-bucket-region";
            string status = "disable";
            string RegionOfTheBucket = "mumbai";
            status = status.ToLower();
            AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOfTheBucket)));
            PutBucketVersioningRequest BucketVersioning = new PutBucketVersioningRequest();
            try
            {
                BucketVersioning.BucketName = NameOfTheBucket;
                if (status == "enable")
                {
                    BucketVersioning.VersioningConfig = new S3BucketVersioningConfig() { Status = VersionStatus.Enabled };
                    client.PutBucketVersioning(BucketVersioning);
                    Console.WriteLine("Versioning Enabled");
                }
                else if (status == "disable")
                {
                    BucketVersioning.VersioningConfig = new S3BucketVersioningConfig() { Status = VersionStatus.Suspended };
                    client.PutBucketVersioning(BucketVersioning);
                    Console.WriteLine("Versioning Disabled");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
            Console.ReadLine();
        }
    }
}