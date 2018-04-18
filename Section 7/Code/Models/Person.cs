using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PacktCourse.AppDemo.Models
{
    public class Person : TableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public bool Gender { get; set; }
    }
}
