//  Environment: Visual Studio 2017
//  Download NuGet Package AWSSDK.S3﻿
using System;
using Amazon.S3.Model;
using Amazon.S3;

namespace CrossRegionReplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string AccessKey = " *** Enter Access Key *** ";
            string SecretKey = " *** Enter Secret Key *** ";
            string Source_Bucket = " *** Source Bucket Here *** ";
            string Destination_Bucket = " *** Destination Bucket Here *** ";
            string Source_ObjectKey = " *** Source Object Key *** ";
            string Destination_ObjectKey = " *** Destination Object Key *** ";
            AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.APSouth1);
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
