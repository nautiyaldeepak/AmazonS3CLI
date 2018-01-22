//  Environment: Visual Studio 2017
//  Download NuGet Package AWSSDK.S3﻿
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
            string AccessKey = " *** Enter Access Key Here *** ";
            string SecretKey = " *** Enter Secret Key Here *** ";
            string NameOFTheBucket = "*** Name Of The Bucket *** ";
            
            // The region of the client is Mumbai -> APSouth1
            //  Amazon Client provides access to S3
            
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
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
            Console.ReadLine();
        }
    }
}
