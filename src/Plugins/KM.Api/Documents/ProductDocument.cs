﻿
namespace KM.Api.Documents
{
    [FirestoreData]
    public class ProductDocument 
    {
        [FirestoreProperty]
        public string id { get; set; }
        [FirestoreProperty]
        public string name { get; set; }
        [FirestoreProperty]
        public string description { get; set; }
    }
}
