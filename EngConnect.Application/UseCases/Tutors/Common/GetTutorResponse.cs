using EngConnect.Application.UseCases.Users.Common;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.Tutors.Common
{
    public class GetTutorResponse : AuditableEntity<Guid>
    {
        public Guid UserId { get; set; }

        public string? Headline { get; set; }

        public string? Bio { get; set; }

        public string? IntroVideoUrl { get; set; }

        public int? MonthExperience { get; set; }

        public string? CvUrl { get; set; }

        public List<string>? Tags { get; set; }
        public string? Avatar { get; set; }

        public int? SlotsCount { get; set; }

        public string? Status { get; set; }

        public string? VerifiedStatus { get; set; }

        public decimal? RatingAverage { get; set; }

        public int? RatingCount { get; set; }
        
        public UserInfo? User { get; set; }
    }
}


