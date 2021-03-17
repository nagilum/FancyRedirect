using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FancyRedirect.DataHandlers
{
    [Table("UrlEntries")]
    public class UrlEntry
    {
        #region ORM

        [Key]
        [Column]
        [JsonIgnore]
        public int Id { get; set; }

        [Column]
        public string Created { get; set; }

        [Column]
        public string LastUsed { get; set; }

        [Column]
        public int Hits { get; set; }

        [Column]
        public string Code { get; set; }

        [Column]
        public string Url { get; set; }

        #endregion
    }
}
