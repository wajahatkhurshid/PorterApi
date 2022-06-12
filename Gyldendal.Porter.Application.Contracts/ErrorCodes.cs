using System.ComponentModel;

namespace Gyldendal.Porter.Application.Contracts
{
    public enum ErrorCodes : ulong
    {
        [Description("Something went wrong while processing the request")]
        InternalServerError = 0001,

        [Description("Provided container type: {0} is not supported")]
        UnsupportedContainerType = 0002,

        [Description("Provided container Id: {0} is not supported")]
        UnsupportedContainerId = 0003,

        [Description("Provided EntityUpdateType: {0} is not supported")]
        UnsupportedEntityUpdateType = 0004,

        [Description("Failed to fetch stock for ISBN {0}. Received status code {1}")]
        GetStockFailure = 0005,

        [Description("Received a null or empty response for taxonomy with ID {0}")]
        GetTaxonomyFailure = 0006
    }
}
