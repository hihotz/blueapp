using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blueapp.Models
{
    public class OperationRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [NotNull]
        public int Rate { get; set; }
    }
}
