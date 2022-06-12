using System.ComponentModel.DataAnnotations;

namespace Gyldendal.Porter.Console.LoadTester
{
    public enum FakeMediaMaterialType
    {
        Hæftet = 2,

        [Display(Name = "Fold ud bog")]
        FoldUdBog = 10,
        
        Spiralryg = 9,
        
        Hardback = 4,
        
        Pocket = 8,
        
        Kolli = 11,
        
        Indbundet = 3,
        
        Papbog = 7,
        
        Lærredsindbundet = 5,
        
        Android = 35,
        
        IOS = 34,

        [Display(Name = "Andet operativsystem")]
        AndetOperativsystem = 36,

        [Display(Name = "Musik DVD")]
        MusikDVD = 41,

        [Display(Name = "e-pub 3 fixed-layout")]
        EPub3FixedLayout = 27,

        Paperback = 6,

        [Display(Name = "Systime iBog")]
        SystimeIBog = 16,

        [Display(Name = "e-pub 2")]
        EPub2 = 28,

        [Display(Name = "Kindle fixed-layout")]
        KindleFixedLayout = 31,

        [Display(Name = "e-bub 2 fixed-layout")]
        EBub2FixedLayout = 29,

        PDF = 32
    }
}
