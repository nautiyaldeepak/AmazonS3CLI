//  Environment: Visual Studio 2017
//  Download NuGet Package AWSSDK.S3﻿
using System;
using Amazon.S3.Model;
using Amazon.S3;

namespace CrossRegionReplication
{
    class Program
    {
        public static string ClientRegion(string RegionOfClient)
        {
            //  AWS Region Codes

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
                case "osaka-local":
                    return "ap-northeast-3";
                case "beijing":
                    return "cn-north-1";
                case "ningxia":
                    return "cn-northwest-1";
            }
            return "";
        }

        static void Main(string[] args)
        {
            string AccessKey = " *** Enter Access Key *** ";
            string SecretKey = " *** Enter Secret Key *** ";
            string Source_Bucket = " *** Source Bucket Here *** ";
            string Destination_Bucket = " *** Destination Bucket Here *** ";
            string Source_ObjectKey = " *** Source Object Key *** ";
            string Destination_ObjectKey = " *** Destination Object Key *** ";
            string RegionOfTheBucket = " *** Enter the region of the bucket *** "
            status = status.ToLower();
            
            AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOfTheBucket)));
            try
            {
                CopyObjectRequest Request = new CopyObjectRequest
                {
                    SourceBucket = Source_Bucket,
                    DestinationBucket = Destination_Bucket,
                    SourceKey = Source_ObjectKey,
                    DestinationKey = Destination_ObjectKey
                };
                client.CopyObject(Request);
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR MESSAGE" + e.Message);
            }
            Console.ReadLine();

        }
    }
}
