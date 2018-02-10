//  Environment: Visual Studio 2017
//  Download NuGet Package AWSSDK.S3﻿
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
                case "paris":
                    return "eu-west-3";
                case "sao paulo":
                    return "sa-east-1";
            }
            return "";
        }
        static void Main(string[] args)
        {
            string AccessKey = " *** Enter Access key ***";
            string SecretKey = " *** Enter Sceret Key *** ";
            string NameOfTheBucket = " *** Name Of The Bucket *** ";
            string status = " *** enable OR disable *** ";
            string RegionOfTheBucket = " *** Region Of The Bucket ***";
            status = status.ToLower();
            try
            {
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOfTheBucket)));
                PutBucketVersioningRequest BucketVersioning = new PutBucketVersioningRequest();
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
