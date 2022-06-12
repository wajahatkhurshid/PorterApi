using System.ComponentModel;

namespace Gyldendal.Porter.Console.LoadTester
{
    public enum FakeWebShop
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
        Pressesite
    }
}
