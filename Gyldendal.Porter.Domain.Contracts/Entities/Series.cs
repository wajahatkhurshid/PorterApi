using System;
using System.Collections.Generic;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Domain.Contracts.Entities
{
    public class Series : DomainEntityBase
    {
        public string Phase { get; set; }

        public string PhaseState { get; set; }

        public string SeriesTitle { get; set; }

        public List<List<GpmNode>> SeriesInternetSubject { get; set; }

        public string SeriesDescription { get; set; }

        public bool SeriesSerieSystemFlag { get; set; }

        public List<Series> SeriesSubseries { get; set; }
    }
}