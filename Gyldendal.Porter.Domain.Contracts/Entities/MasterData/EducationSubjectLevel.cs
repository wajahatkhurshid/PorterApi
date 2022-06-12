namespace Gyldendal.Porter.Domain.Contracts.Entities.MasterData
{
    public class EducationSubjectLevel : DomainEntityBase
    {
        public string Name { get; set; }

        /// <summary>
        /// ID of area that matched the name of the subject level
        /// </summary>
        public int? AreaId { get; set; }

        /// <summary>
        /// Parent webshop to the area ID
        /// </summary>
        public string Webshop { get; set; }

        public int Level { get; set; }

        public EducationSubjectLevel Parent { get; set; }
    }
}
