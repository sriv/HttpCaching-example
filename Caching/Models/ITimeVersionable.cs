using System;

namespace Caching.Models
{
    public interface ITimeVersionable
    {
        DateTime LastModified { get; set; }     
    }
}