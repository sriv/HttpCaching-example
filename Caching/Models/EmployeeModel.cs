using System;

namespace Caching.Models
{
    public class EmployeeModel : IAmModelOfEntity, IVersionable
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public long Version { get; set; }
    }
}