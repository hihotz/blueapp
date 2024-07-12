using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blueapp.Models
{
    public class AppLog
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string? UserName { get; set; } = string.Empty;

        [NotNull]
        public string? Message { get; set; } = string.Empty;

        [NotNull]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string? Success { get; set; }
    }
}
