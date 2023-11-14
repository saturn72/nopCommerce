﻿using System;
using Nop.Core;

namespace Nop.Plugin.Misc.KM.Catalog.Domain
{
    public partial class MediaItemInfo : BaseEntity
    {
        public string Uri { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public byte[] BinaryData { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string Type { get; set; }
        public string Storage { get; set; }
        public string StorageIdentifier { get; set; }
    }
}
