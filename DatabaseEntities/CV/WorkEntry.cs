using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DatabaseEntities.CV
{
    public class WorkEntry : PersistentEntity<int>
    {
        [Required]
        public int CVId { get; set; }
        [ForeignKey("CVId")]
        public virtual CV CV { get; set; }
        [Required]
        public int WorkplaceId { get; set; }
        [ForeignKey("WorkplaceId")]
        public Workplace Workplace { get; set; }
        [Required]
        [DisplayName("From Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime FromDate { get; set; }
        [Required]
        [DisplayName("To Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ToDate { get; set; }
        [Required,
         MaxLength(50, ErrorMessage = "Position must be less than 50 characters")]
        public string Position { get; set; }
        [Required]
        public string Responsibilities { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is WorkEntry)
            {
                WorkEntry other = (WorkEntry)obj;
                return Id == other.Id &&
                    CVId == other.CVId &&
                    WorkplaceId == other.WorkplaceId &&
                    DateTime.Equals(FromDate, other.FromDate) &&
                    DateTime.Equals(ToDate, other.ToDate) &&
                    string.Equals(Position, other.Position) &&
                    string.Equals(Responsibilities, other.Responsibilities);
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = 1;
            hashCode = (Id ^ hashCode) * 31;
            hashCode = (CVId ^ hashCode) * 31;
            hashCode = (WorkplaceId ^ hashCode) * 31;
            if (FromDate != null)
            {
                hashCode = (FromDate.GetHashCode() ^ hashCode) * 31;
            }
            if (ToDate != null)
            {
                hashCode = (ToDate.GetHashCode() ^ hashCode) * 31;
            }
            if (Position != null)
            {
                hashCode = (Position.GetHashCode() ^ hashCode) * 31;
            }
            if (Responsibilities != null)
            {
                hashCode = (Responsibilities.GetHashCode() ^ hashCode) * 31;
            }
            return hashCode;
        }
    }
}
