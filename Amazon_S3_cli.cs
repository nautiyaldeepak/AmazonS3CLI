//  Environment: Visual Studio 2017
//  Download NuGet Package AWSSDK.S3

using System;
using Amazon.S3;
using Amazon.S3.Model;
using System.Collections.Generic;
using Amazon.S3.Transfer;
using System.IO;
using System.Threading;

namespace AmazonS3CommandLine
{
    class Program
    {
        public static string AmazonRegionForListingPurpose = "error";
        public static string AccessKey = null;
        public static string SecretKey = null;
        public static List<string> AllAmazonRegions = new List<string>();
        public static List<string> AllCommands = new List<string>();
        public static int VerifyCredentialsValue = 0;

        //  Initials -> executed once the user is authorized
        public static void InitialProvisioning()
        {
            AllAmazonRegions.Add("ireland");
            AllAmazonRegions.Add("mumbai");
            AllAmazonRegions.Add("frankfurt");
            AllAmazonRegions.Add("london");
            AllAmazonRegions.Add("sydney");
            AllAmazonRegions.Add("ohio");
            AllAmazonRegions.Add("seoul");
            AllAmazonRegions.Add("california");
            AllAmazonRegions.Add("oregon");
            AllAmazonRegions.Add("singapore");
            AllAmazonRegions.Add("tokyo");
            AllAmazonRegions.Add("canada");
            AllAmazonRegions.Add("virginia");
            AllAmazonRegions.Add("paris");
            AllAmazonRegions.Add("sao paulo");

            AllCommands.Add("ls");
            AllCommands.Add("cb");
            AllCommands.Add("rmb");
            AllCommands.Add("url");
            AllCommands.Add("incopy");
            AllCommands.Add("outcopy");
            AllCommands.Add("en");
            AllCommands.Add("disable");
            AllCommands.Add("acl");

        }
        
        //  Credentials verification Function
        public static int VerifyCredentials()
        {
            if (AccessKey == null || SecretKey == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("AccessKey ID & SecretKey ID not configured");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Use Command");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\t config accesskey \"EnterAccessKeyHere\" secretkey \"EnterSecretKeyHere\"");
                Console.WriteLine("\t Example: ");
                Console.WriteLine("\t config accesskey AKIAJK4FKM5Y47J26YWER secretkey vnJHGC4RvLCJwZe1ajGV/NoJw+KM4j3RyGMAhqWT");
                Console.ForegroundColor = ConsoleColor.White;
                return 0;
            }
            return 1;
        }
        
        //  Establish a connection
        public static string Configuration(string UserAccessKey, string UserSecretKey)
        {
            AccessKey = UserAccessKey;
            SecretKey = UserSecretKey;
            try
            {
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.APSouth1);
                {
                    ListBucketsResponse response = client.ListBuckets();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
                AccessKey = null;
                SecretKey = null;
                return "Code 404: Not Authorised";
            }
            InitialProvisioning();
            VerifyCredentialsValue = 1;
            return "Code 271: Authorised";
        }
        
        //  Regions associated with AWS
        //  Future Regions will be included
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
        
        //  Create Amazon S3 bucket
        
        public static void CreateBucketFunctionality(string NameOfTheBucket, string RegionOfTheBucket)
        {
            RegionOfTheBucket = RegionOfTheBucket.ToLower();
            try
            {
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOfTheBucket)));
                PutBucketRequest request = new PutBucketRequest
                {
                    BucketName = NameOfTheBucket,
                    UseClientRegion = true
                };
                client.PutBucket(request);
                Console.WriteLine("Bucket Created");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
        }
        
        //  Delete bucket functionality
        public static void DeleteBucketFunctionality(string NameOfTheBucket, string RegionOfTheBucket)
        {
            RegionOfTheBucket = RegionOfTheBucket.ToLower();
            try
            {
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOfTheBucket)));
                ListObjectsRequest ObjectRequest = new ListObjectsRequest
                {
                    BucketName = NameOfTheBucket
                };
                ListObjectsResponse ListResponse;
                do
                {
                    ListResponse = client.ListObjects(ObjectRequest);
                    foreach (S3Object obj in ListResponse.S3Objects)
                    {
                        DeleteObjectRequest DeleteObject = new DeleteObjectRequest
                        {
                            BucketName = NameOfTheBucket,
                            Key = obj.Key
                        };
                        client.DeleteObject(DeleteObject);
                    }
                    ObjectRequest.Marker = ListResponse.NextMarker;
                } while (ListResponse.IsTruncated);

                DeleteBucketRequest DeleteRequest = new DeleteBucketRequest
                {
                    BucketName = NameOfTheBucket,
                    UseClientRegion = true
                };
                client.DeleteBucket(DeleteRequest);
                Console.WriteLine("Bucket Deleted");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
        }
        public static void ClientCredentials()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Access Key : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(AccessKey);

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Secret Key : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(SecretKey);
        }
        
        //  Enable/Disable Transfer Accleration on Bucket/Object
        public static void EnableDisableTransferAcclerationFunctionality(string NameOfTheBucket, string RegionOfTheBucket, string WhatToDo)
        {
            RegionOfTheBucket = RegionOfTheBucket.ToLower();
            try
            {
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOfTheBucket)));
                if (WhatToDo == "enable")
                {
                    PutBucketAccelerateConfigurationRequest Request = new PutBucketAccelerateConfigurationRequest
                    {
                        BucketName = NameOfTheBucket,
                        AccelerateConfiguration = new AccelerateConfiguration { Status = BucketAccelerateStatus.Enabled }
                    };
                    client.PutBucketAccelerateConfiguration(Request);
                    Console.WriteLine("Transfer Accleration Enabled");
                }
                else if (WhatToDo == "disable")
                {
                    PutBucketAccelerateConfigurationRequest Request = new PutBucketAccelerateConfigurationRequest
                    {
                        BucketName = NameOfTheBucket,
                        AccelerateConfiguration = new AccelerateConfiguration { Status = BucketAccelerateStatus.Suspended }
                    };
                    client.PutBucketAccelerateConfiguration(Request);
                    Console.WriteLine("Transfer Accleration Disabled");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
        }
        
        //  Enable Disable Versioning
        public static void EnableDisableVersioningFunctionality(string NameOfTheBucket, string RegionOfTheBucket, string WhatToDo)
        {
            RegionOfTheBucket = RegionOfTheBucket.ToLower();
            try
            {
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOfTheBucket)));
                PutBucketVersioningRequest BucketVersioning = new PutBucketVersioningRequest();
                BucketVersioning.BucketName = NameOfTheBucket;
                if (WhatToDo == "enable")
                {
                    BucketVersioning.VersioningConfig = new S3BucketVersioningConfig() { Status = VersionStatus.Enabled };
                    client.PutBucketVersioning(BucketVersioning);
                    Console.WriteLine("Versioning Enabled");
                }
                else if (WhatToDo == "disable")
                {
                    BucketVersioning.VersioningConfig = new S3BucketVersioningConfig() { Status = VersionStatus.Suspended };
                    client.PutBucketVersioning(BucketVersioning);
                    Console.WriteLine("Versioning Disabled");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
        }
        public static void ListBucket(string UseCase)
        {
            try
            {
                using (AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.APSouth1))
                {
                    ListBucketsResponse response = client.ListBuckets();
                    if (UseCase == "detail")
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\tCreation Date \t\t" + "Total Objects\t\t"+"Region \t\t" + "Bucket Name");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine();
                        foreach (S3Bucket b in response.Buckets)
                        {
                            Console.WriteLine((b.CreationDate) + " \t\t" + CountObjectsInBucket(b.BucketName) + " \t\t\t" + RegionOfTheBucketForDetailListing(b.BucketName, 1) + "\t\t" + b.BucketName);
                        }
                    }
                    else if (UseCase == "normal")
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\t Bucket Name");
                        Console.ForegroundColor = ConsoleColor.White;
                        foreach (S3Bucket b in response.Buckets)
                        {
                            Console.WriteLine(b.BucketName);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
        }
        public static string RegionOfTheBucketForDetailListing(string CurrentAmazonRegion, int UseCaseValue)
        {
            if(UseCaseValue == 0)
            {
                AmazonRegionForListingPurpose = CurrentAmazonRegion;
                return "";
            }
            else
            {
                return AmazonRegionForListingPurpose;
            }
        }
        public static void ListObjectsInBucket(string NameOfTheBucket, string RegionOfTheBucket)
        {
            RegionOfTheBucket = RegionOfTheBucket.ToLower();
            try
            {
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOfTheBucket)));
                ListObjectsV2Request Request = new ListObjectsV2Request();
                Request.BucketName = NameOfTheBucket;
                ListObjectsV2Response Response;
                do
                {
                    Response = client.ListObjectsV2(Request);
                    foreach (S3Object entry in Response.S3Objects)
                    {
                        Console.WriteLine(entry.Key);
                    }
                    Request.ContinuationToken = Response.NextContinuationToken;
                } while (Response.IsTruncated == true);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
        }
        public static int CountObjectsInBucket(string NameOfTheBucket)
        {
            int count = 0;
            for(int i = 0; i < 13; i++)
            {
                string AmazonRegion = AllAmazonRegions[i];
                try
                {
                    AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(AmazonRegion)));
                    ListObjectsV2Request Request = new ListObjectsV2Request();
                    Request.BucketName = NameOfTheBucket;
                    ListObjectsV2Response Response;
                    do
                    {
                        Response = client.ListObjectsV2(Request);
                        foreach (S3Object entry in Response.S3Objects)
                        {
                            count++;
                        }
                        RegionOfTheBucketForDetailListing(AmazonRegion, 0);
                        Request.ContinuationToken = Response.NextContinuationToken;
                    } while (Response.IsTruncated == true);
                    return count;
                }
                catch (Exception e)
                {
                }
            }
            return count;
        }
        
        //  Generating URL for bucket object
        
        public static void GenerateObjectURL(string NameOfTheBucket, string NameOfTheObject, string RegionOfTheBucket)
        {
            RegionOfTheBucket = RegionOfTheBucket.ToLower();
            try
            {
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOfTheBucket)));
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
            catch (Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
        }
        public static int UploadDataToS3(string NameOfThebucket, string DirectoryPath, string RegionOftheBucket)
        {
            if (DirectoryPath.Contains(".") == true)
            {
                RegionOftheBucket = RegionOftheBucket.ToLower();
                try
                {
                    TransferUtility directoryTransferUtility = new TransferUtility(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOftheBucket)));
                    AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOftheBucket)));
                    int len = DirectoryPath.Length;
                    int temp = DirectoryPath.LastIndexOf("\\");
                    string KeyName = DirectoryPath.Substring(temp + 1, len - temp -1);
                    PutObjectRequest Request = new PutObjectRequest
                    {
                        BucketName = NameOfThebucket,
                        Key = KeyName,
                        FilePath = DirectoryPath,
                        ContentType = "text/plain"
                    };
                    client.PutObject(Request);
                    Console.WriteLine("Transfer Complete");
                    return 0;
                }
                catch(Exception e)
                {
                    Console.WriteLine("ERROR MESSAGE : " + e.Message);
                }
                
            }
            try
            {
                TransferUtility directoryTransferUtility = new TransferUtility(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOftheBucket)));
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOftheBucket)));
                directoryTransferUtility.UploadDirectory(DirectoryPath, NameOfThebucket);
                directoryTransferUtility.UploadDirectory(DirectoryPath, NameOfThebucket, "*.*", SearchOption.AllDirectories);
                TransferUtilityUploadDirectoryRequest request = new TransferUtilityUploadDirectoryRequest
                {
                    BucketName = NameOfThebucket,
                    Directory = DirectoryPath,
                    SearchOption = SearchOption.AllDirectories,
                    SearchPattern = "*.*"
                };
                directoryTransferUtility.UploadDirectory(request);
                Console.WriteLine("Transfer Complete");
                return 0;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("There is Some Problem");
                Console.WriteLine(e.Message, e.InnerException);
            }
            return 0;
        }
        public static void DownloadFromS3(string NameOfTheBucket, string NameOfTheObject, string @DirectoryPath, string RegionOfTheBucket)
        {
            string KeyNameOfTheObject;
            if(NameOfTheObject.Contains("/"))
            {
                int len = NameOfTheObject.Length;
                int temp = NameOfTheObject.LastIndexOf("/");
                KeyNameOfTheObject = NameOfTheObject.Substring(temp + 1, len - temp - 1);
            }
            else
            {
                KeyNameOfTheObject = NameOfTheObject;
            }
            RegionOfTheBucket = RegionOfTheBucket.ToLower();
            try
            {
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOfTheBucket)));
                GetObjectRequest Request = new GetObjectRequest
                {
                    BucketName = NameOfTheBucket,
                    Key = NameOfTheObject
                };
                GetObjectResponse Response = client.GetObject(Request);
                Response.WriteResponseStreamToFile(DirectoryPath + KeyNameOfTheObject);
                Console.WriteLine("File Downloaded");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
        }
        
        //  Access Control on bucket/object functionality
        
        public static void AccessControlListFunctionality(string NameOfTheBucket, string NameOfTheObject, string MakeStatus, string RegionOfTheBucket)
        { 
            
            MakeStatus = MakeStatus.ToLower();
            RegionOfTheBucket = RegionOfTheBucket.ToLower();
            try
            {
                AmazonS3Client client = new AmazonS3Client(AccessKey, SecretKey, Amazon.RegionEndpoint.GetBySystemName(ClientRegion(RegionOfTheBucket)));
                if(NameOfTheObject == null)
                {
                    if (MakeStatus == "publicread")
                    {
                        PutACLRequest Request = new PutACLRequest
                        {
                            BucketName = NameOfTheBucket,
                            CannedACL = S3CannedACL.PublicRead
                        };
                        client.PutACL(Request);
                    }
                    else if (MakeStatus == "publicreadwrite")
                    {
                        PutACLRequest Request = new PutACLRequest
                        {
                            BucketName = NameOfTheBucket,
                            CannedACL = S3CannedACL.PublicReadWrite
                        };
                        client.PutACL(Request);
                    }
                    else if (MakeStatus == "private")
                    {
                        PutACLRequest Request = new PutACLRequest
                        {
                            BucketName = NameOfTheBucket,
                            CannedACL = S3CannedACL.Private
                        };
                        client.PutACL(Request);
                    }
                }
                else
                {
                    if (MakeStatus == "publicread")
                    {
                        PutACLRequest Request = new PutACLRequest
                        {
                            BucketName = NameOfTheBucket,
                            Key = NameOfTheObject,
                            CannedACL = S3CannedACL.PublicRead
                        };
                        client.PutACL(Request);
                    }
                    else if (MakeStatus == "publicreadwrite")
                    {
                        PutACLRequest Request = new PutACLRequest
                        {
                            BucketName = NameOfTheBucket,
                            Key = NameOfTheObject,
                            CannedACL = S3CannedACL.PublicReadWrite
                        };
                        client.PutACL(Request);
                    }
                    else if (MakeStatus == "private")
                    {
                        PutACLRequest Request = new PutACLRequest
                        {
                            BucketName = NameOfTheBucket,
                            Key = NameOfTheObject,
                            CannedACL = S3CannedACL.Private
                        };
                        client.PutACL(Request);
                    }
                }
                Console.WriteLine("Access type changed");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR MESSAGE : " + e.Message);
            }
        }

         
        //  The main program stars here:

        static void Main(string[] args)
        {
            InitialProvisioning();
            string loop = "active";
            string[] SplitTerm;
            do
            {
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Cloud-Computing ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("@AmazonS3 cli ~/" + "\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("$ ");
                string InputForTheConsole = Console.ReadLine();
                SplitTerm = InputForTheConsole.Split();
                if(VerifyCredentialsValue == 0)
                {
                    foreach(var temp in AllCommands)
                    {
                        if(SplitTerm[0] == temp)
                        {
                            if (VerifyCredentials() == 0)
                                goto Scape;
                        }
                    }
                }
                switch (SplitTerm[0])
                {
                    case "clear":
                        Console.Clear();
                        break;
                    case "ls":
                        if (SplitTerm.Length == 2)
                            if (SplitTerm[1] == "detail")
                                ListBucket(SplitTerm[1]);
                        if (SplitTerm.Length == 3)
                            ListObjectsInBucket(SplitTerm[1], SplitTerm[2]);
                        if(SplitTerm.Length == 1)
                            ListBucket("normal");
                        break;
                    case "outcopy":
                        if (SplitTerm.Length == 5)
                            DownloadFromS3(SplitTerm[1], SplitTerm[2], SplitTerm[3], SplitTerm[4]);
                        if (SplitTerm.Length == 4)
                        {
                            int len = SplitTerm[1].Length;
                            int temp = SplitTerm[1].IndexOf("/");
                            string KeyName = SplitTerm[1].Substring(temp + 1, len - temp - 1);
                            DownloadFromS3(SplitTerm[1].Substring(0, temp), SplitTerm[1].Substring(temp + 1, len - temp - 1), SplitTerm[2], SplitTerm[3]);
                        }
                        break;
                    case "credentials":
                        ClientCredentials();
                        break;
                    case "incopy":
                        UploadDataToS3(SplitTerm[1], @SplitTerm[2], SplitTerm[3]);
                        break;
                    case "url":
                        if (SplitTerm.Length == 4)
                            GenerateObjectURL(SplitTerm[1], SplitTerm[2], SplitTerm[3]);
                        else if(SplitTerm.Length == 3)
                        {
                            int temp = SplitTerm[1].IndexOf("/");
                            int len = SplitTerm[1].Length - temp;
                            GenerateObjectURL(SplitTerm[1].Substring(0, temp), SplitTerm[1].Substring(temp + 1, len - 1), SplitTerm[2]);
                        }
                        break;
                    case "en":
                        int tempLen = SplitTerm.Length;
                        for(int i = 2; i < tempLen; i++)
                        {
                            if (SplitTerm[i] == "versioning")
                                EnableDisableVersioningFunctionality(SplitTerm[1], SplitTerm[SplitTerm.Length - 1], "enable");
                            else if (SplitTerm[i] == "transferaccleration")
                                EnableDisableTransferAcclerationFunctionality(SplitTerm[1], SplitTerm[SplitTerm.Length - 1], "enable");
                        }
                        break;
                    case "disable":
                        int Len = SplitTerm.Length;
                        for (int i = 2; i < Len; i++)
                        {
                            if (SplitTerm[i] == "versioning")
                                EnableDisableVersioningFunctionality(SplitTerm[1], SplitTerm[SplitTerm.Length - 1], "disable");
                            else if (SplitTerm[i] == "transferaccleration")
                                EnableDisableTransferAcclerationFunctionality(SplitTerm[1], SplitTerm[SplitTerm.Length - 1], "disable");
                        }
                        break;
                    case "acl":
                        int temporary = 0;
                        temporary = SplitTerm[1].IndexOf("/");
                        Console.WriteLine("temp" + temporary);
                        int ACLlen = SplitTerm[1].Length - temporary;
                        if (SplitTerm.Length == 4)
                            if (temporary == -1)
                                AccessControlListFunctionality(SplitTerm[1], null, SplitTerm[2], SplitTerm[3]);
                            else
                                AccessControlListFunctionality(SplitTerm[1].Substring(0, temporary), SplitTerm[1].Substring(temporary + 1, ACLlen - 1), SplitTerm[2], SplitTerm[3]);
                        else if (SplitTerm.Length == 5)
                            AccessControlListFunctionality(SplitTerm[1], SplitTerm[2], SplitTerm[3], SplitTerm[4]);
                        break;
                    case "config":
                        if (SplitTerm[1] == "accesskey" && SplitTerm[3] == "secretkey")
                        {
                            string code = Configuration(SplitTerm[2], SplitTerm[4]);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine(code);
                            Console.ForegroundColor = ConsoleColor.White;
                            VerifyCredentialsValue = 1;
                        }
                        else if (SplitTerm[3] == "accesskey" && SplitTerm[1] == "secretkey")
                        {
                            string code = Configuration(SplitTerm[4], SplitTerm[2]);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine(code);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                            Console.WriteLine("Wrong Syntax");
                        break;
                    case "cb":
                        if (SplitTerm.Length == 3)
                            CreateBucketFunctionality(SplitTerm[1], SplitTerm[2]);
                        else
                            Console.WriteLine("cli : Command not found");
                        break;
                    case "rmb":
                        if (SplitTerm.Length == 3)
                            DeleteBucketFunctionality(SplitTerm[1], SplitTerm[2]);
                        else
                            Console.WriteLine("cli : Command not found");
                        break;
                    case "exit":
                        loop = "kill";
                        AccessKey = null;
                        SecretKey = null;
                        break;
                    case "kill":
                        AccessKey = null;
                        SecretKey = null;
                        VerifyCredentialsValue = 0;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Command not found");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
                SplitTerm = null;
                Scape:
                Console.Write("");
            } while (loop == "active");
            
            
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Logged out");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(100);
        }
    }
}
