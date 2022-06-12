using System.ComponentModel.DataAnnotations;

namespace Gyldendal.Porter.Console.LoadTester
{
    public enum FakeSubject
    {
        [Display(Name = "Dansk for voksne")]
        DanskForVoksne,
        Erhvervsuddannelser,
        Grundskole,
        Gymnasie,
        Ordboger,
        Voksenuddannelser
    }
}