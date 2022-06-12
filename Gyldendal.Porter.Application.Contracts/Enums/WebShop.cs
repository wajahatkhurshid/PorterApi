using System.ComponentModel;

namespace Gyldendal.Porter.Application.Contracts.Enums
{
    public enum WebShop
    {
        [Description("None")]
        None,

        [Description("Gyldendals Bogklub")]
        GyldendalPlus,

        [Description("Harmoney")]
        Harmoney,
        
        [Description("Gyldendal Uddannelse")]
        GU,

        [Description("Hans Reitzel")]
        HansReitzel,

        [Description("Munksgaard")]
        MunksGaard,

        [Description("Pressesite")]
        Pressesite,

        [Description("All")]
        All=-1
    }
}
