using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class Grocery
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        [Ignore]
        public List<ToDoItem> Items { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Status { get; set; }
        [Ignore]
        public string StatusText { get { return ((Status)Status).ToString(); } }
    }
}
