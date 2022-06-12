using System.ComponentModel.DataAnnotations;

namespace Gyldendal.Porter.Console.LoadTester
{
    public enum FakeImprint
    {
        [Display(Name = "Høst og Søn")]
        HøstogSøn = 1,

        [Display(Name = "Det Blå Hus")]
        DetBlåHus,

        [Display(Name = "Gyldendal Stereo")]
        GyldendalStereo,

        Samleren,

        Rosinante,

        [Display(Name = "Tiderne Skifter")]
        TiderneSkifter,

        Flamingo,

        [Display(Name = "Default imprint")]
        DefaultImprint,

        [Display(Name = "Imprit Hans Reitzel")]
        ImpritHansReitzel,

        [Display(Name = "Imprint Munksgaard")]
        ImprintMunksgaard
    }
}
