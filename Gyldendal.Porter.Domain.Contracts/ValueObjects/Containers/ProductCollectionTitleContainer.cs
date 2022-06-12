using System.Collections.Generic;

namespace Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers
{
    public class ProductCollectionTitleContainer : ContainerInstanceBase
    {
        public string Phase { get; set; }

        public string PhaseState { get; set; }

        public string SeriesTitle { get; set; }

        public List<List<GpmNode>> SeriesInternetSubject { get; set; }

        public string SeriesDescription { get; set; }

        public bool SeriesSerieSystemFlag { get; set; }

        public List<ProductCollectionTitleContainer> SeriesSubseries { get; set; }
    }
}
