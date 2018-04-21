//  Environment: Visual Studio 2017
//  Download NuGet Package AWSSDK.S3﻿
using System;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.IO;

namespace TransferDataToS3
{
    // adding regions in progress
    class Program
    {
        public static string ClientRegion(string RegionOfClient)
        {
            switch(RegionOfClient)
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
        
        public static int UploadToS3()
        {
            string AccessKey = "*** Enter Access Key ***";
            string SecretKey = "*** Enter Secret Key ***";
            string existingBucketName = "*** Bucket Name ***";
            string directoryPath = @"*** Location of the File ***";
            string RegionOfTheBucket = "*** Region Name ***"
            try
            {
                
                //  Here Region of the bucket is Mumbai. Change the region as per your bucket
                //  Amazon Client provides access to S3
                
                TransferUtility directoryTransferUtility = new TransferUtility();
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.APSouth1);
                directoryTransferUtility.UploadDirectory(directoryPath, existingBucketName);
                directoryTransferUtility.UploadDirectory(directoryPath, existingBucketName, "*.*", SearchOption.AllDirectories);
                TransferUtilityUploadDirectoryRequest request = new TransferUtilityUploadDirectoryRequest
                {
                    BucketName = existingBucketName,
                    Directory = directoryPath,
                    SearchOption = SearchOption.AllDirectories,
                    SearchPattern = "*.*"
                };
                directoryTransferUtility.UploadDirectory(request);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("There is Some Problem");
                Console.WriteLine(e.Message, e.InnerException);
            }
            Directory.Delete(directoryPath, true);
            Directory.CreateDirectory(directoryPath);
            Console.ReadLine();
        }
        
        //  Main Function
        
        static void Main(string[] args)
        {
            UploadToS3();
        }
    }
}
