using System;
using System.Linq;
using AutoMapper;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Models;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;
using Contributor = Gyldendal.Porter.Domain.Contracts.Entities.Contributor;
using MediaMaterialType = Gyldendal.Porter.Domain.Contracts.Entities.MasterData.MediaMaterialType;
using Product = Gyldendal.Porter.Domain.Contracts.Entities.Product;
using Series = Gyldendal.Porter.Domain.Contracts.Entities.Series;
using SubjectCode = Gyldendal.Porter.Domain.Contracts.Entities.MasterData.SubjectCode;
using Work = Gyldendal.Porter.Application.Contracts.Models.Work;
using WorkReview = Gyldendal.Porter.Application.Contracts.Models.WorkReview;

namespace Gyldendal.Porter.Application.Configuration.AutoMapper
{
    /// <summary>
    /// Mapping of Contracts
    /// </summary>
    public class PorterMapper : Profile
    {
        /// <summary>
        /// Mapping
        /// </summary>
        public PorterMapper()
        {
            ConfigureAttachmentMappings();
            ConfigureContributorMappings();
            ConfigureProductMappings();
            ConfigureSeriesMappings();
            ConfigureTaxonomyMappings();
            ConfigureWorkMappings();
            ConfigureWorkReviewMappings();
            ConfigureMerchandiseProductMappings();
        }

        private void ConfigureMerchandiseProductMappings()
        {
            CreateMap<MerchandiseProductContainer, MerchandiseProduct>()
                .ForMember(cm => cm.MerchandiseContributorAuthor, m => m.MapFrom(mc => mc.MerchandiseContributorAuthor))
                .ForMember(cm => cm.MerchandiseCollectionTitle, m => m.MapFrom(mc => mc.MerchandiseCollectionTitle))
                .ReverseMap();
            CreateMap<CookedProduct, MerchandiseProduct>().ReverseMap()
                .ForMember(cm => cm.Isbn, m => m.MapFrom(o => o.MerchandiseEan))
                .ForMember(cm => cm.Description, m => m.MapFrom(o => o.MerchandiseDescription))
                .ForMember(cm => cm.Title, m => m.MapFrom(o => o.MerchandiseTitle))
                .ForMember(cm => cm.Subtitle, m => m.MapFrom(o => o.MerchandiseSubtitle))
                .ForMember(cm => cm.FirstPrintPublishDate, m => m.MapFrom(o => o.MerchandisePublishedDate))
                .ForMember(cm => cm.WebText, m => m.MapFrom(o => o.MerchandiseWebText))
                .ForMember(cm => cm.SeoText, m => m.MapFrom(o => o.MerchandiseWebText))
                //.ForMember(cm => cm.Edition, m => m.MapFrom(o => o.Edi)) TODO: Needs mapping
                .ForMember(cm => cm.CurrentPrintRunPublishDate, m => m.MapFrom(o => o.MerchandisePublishedDate))
                //.ForMember(cm => cm.EditorialStaff, m => m.MapFrom(o => o.Edi)) TODO: Needs mapping
                //.ForMember(cm => cm.Attachments, m => m.MapFrom(o => o.Mark)) TODO: Needs mapping
                .ForMember(cm => cm.Height, m => m.MapFrom(o => o.MerchandiseMeasurementHeight))
                .ForMember(cm => cm.Width, m => m.MapFrom(o => o.MerchandiseMeasurementWidth))
                .ForMember(cm => cm.PriceWithVat, m => m.MapFrom(o => o.MerchandiseSupplyPriceWithVAT))
                .ForMember(cm => cm.PriceWithoutVat, m => m.MapFrom(o => o.MerchandiseSupplyPriceWithoutVAT))
                .ForMember(cm => cm.Series, m => m.MapFrom(o => o.MerchandiseCollectionTitle))
                .ForMember(x => x.WebShops,
                    o => o.MapFrom(y =>
                        y.MerchandiseDisplayOnShops.SelectMany(website =>
                            website.Select(node => Enum.Parse(typeof(WebShop), node.Name)))));
        }

        private void ConfigureAttachmentMappings()
        {
            CreateMap<MarketingAttachment, Attachment>().ReverseMap();
        }

        private void ConfigureTaxonomyMappings()
        {
            CreateMap<Area, Domain.Contracts.Entities.MasterData.Area>().ReverseMap();
            CreateMap<SubArea, Domain.Contracts.Entities.MasterData.SubArea>().ReverseMap();
            CreateMap<EducationLevel, Domain.Contracts.Entities.MasterData.EducationLevel>().ReverseMap();
            CreateMap<Subject, Domain.Contracts.Entities.MasterData.Subject>().ReverseMap();
            CreateMap<Application.Contracts.Models.MediaMaterialType, MediaMaterialType>().ReverseMap();
            CreateMap<Application.Contracts.Models.SubjectCode, SubjectCode>().ReverseMap()
                .ForMember(x => x.Id, f => f.MapFrom(y => y.Id))
                .ForMember(x => x.Code, f => f.MapFrom(y => y.Name))
                ;
        }

        private void ConfigureContributorMappings()
        {
            CreateMap<Contributor, Application.Contracts.Models.Contributor>();
            CreateMap<CookedContributor, Application.Contracts.Models.Contributor>().ReverseMap();
            CreateMap<CookedContributor, ProfileContributorAuthorContainer>()
                .ReverseMap()
                .ForMember(x => x.Id, m => m.MapFrom(o => o.ProfileID))
                .ForMember(x => x.FirstName,
                    m => m.MapFrom(o =>
                        string.Join(" ", GetParts(o.ProfileName).Take(GetParts(o.ProfileName).Length - 1))))
                .ForMember(x => x.LastName, m => m.MapFrom(o => GetParts(o.ProfileName).LastOrDefault()))
                .ForMember(x => x.Name, m => m.MapFrom(o => o.ProfileName))
                .ForMember(x => x.PhotoUrl, m => m.MapFrom(o => o.ProfileImageUrl));
            CreateMap<Contributor, ProfileContributorAuthorContainer>()
                .ReverseMap()
                .ForMember(x => x.Id, m => m.MapFrom(o => o.ProfileID))
                .ForMember(x => x.FirstName,
                    m => m.MapFrom(o =>
                        string.Join(" ", GetParts(o.ProfileName).Take(GetParts(o.ProfileName).Length - 1))))
                .ForMember(x => x.LastName, m => m.MapFrom(o => GetParts(o.ProfileName).LastOrDefault()))
                .ForMember(x => x.PhotoUrl, m => m.MapFrom(o => o.ProfileImageUrl));
        }

        private void ConfigureSeriesMappings()
        {
            CreateMap<Series, Application.Contracts.Models.Series>()
                .ReverseMap(); // Missing: WebShop ids List to Enum mapping, Child Series ids to List, Parent Series id as well as Parent Series   
            CreateMap<Series, CookedSeries>().ReverseMap();
            CreateMap<Application.Contracts.Models.Series, CookedSeries>().ReverseMap();
            CreateMap<ProductCollectionTitleContainer, CookedSeries>()
                .ForMember(x => x.Id, v => v.MapFrom(m => m.ContainerInstanceId))
                .ForMember(x => x.Name, v => v.MapFrom(m => m.SeriesTitle))
                .ForMember(x => x.Description, v => v.MapFrom(m => m.SeriesDescription))
                .ForMember(x => x.IsSystemSeries, v => v.MapFrom(m => m.SeriesSerieSystemFlag))
                .ForMember(x => x.ChildSeries, v => v.MapFrom(m => m.SeriesSubseries))
                .ReverseMap();
            CreateMap<ProductCollectionTitleContainer, Series>()
                .ForMember(x => x.Id, v => v.MapFrom(m => m.ContainerInstanceId))
                .ForMember(x => x.SeriesTitle, v => v.MapFrom(m => m.SeriesTitle))
                .ForMember(x => x.SeriesDescription, v => v.MapFrom(m => m.SeriesDescription))
                .ForMember(x => x.SeriesSerieSystemFlag, v => v.MapFrom(m => m.SeriesSerieSystemFlag))
                .ForMember(x => x.SeriesSubseries, v => v.MapFrom(m => m.SeriesSubseries))
                .ReverseMap();
        }

        private static string[] GetParts(string str)
        {
            var strWithoutDash = str.Split('-')[0].Trim();
            var parts = strWithoutDash.Split(' ');
            return parts;
        }

        private void ConfigureWorkMappings()
        {
            CreateMap<Work, Domain.Contracts.Entities.Work>().ReverseMap();
        }

        private void ConfigureWorkReviewMappings()
        {
            CreateMap<WorkReview, CookedWorkReview>().ReverseMap();
            CreateMap<WorkReviewContainer, CookedWorkReview>()
                .ForMember(x => x.Id, o => o.MapFrom(y => y.ContainerInstanceId))
                .ForMember(x => x.AuthorInfo, o => o.MapFrom(y => y.ReviewWrittenBy))
                .ForMember(x => x.Rating, o => o.MapFrom(y => y.ReviewRating))
                .ForMember(x => x.Review, o => o.MapFrom(y => y.ReviewContent))
                .ForMember(x => x.ReviewMedia, o => o.MapFrom(y => y.ReviewMedia))
                ;
        }

        private void ConfigureProductMappings()
        {
            // todo
            //Some field are ignore because they will be mapped from cooked collection 
            CreateMap<Product, Application.Contracts.Models.Product>()
                .ForMember(x => x.Attachments, o => o.Ignore())
                .ForMember(x => x.Series, o => o.Ignore())
                .ForMember(x => x.Contributors, o => o.Ignore())
                .ForMember(x => x.WebShops, o => o.Ignore())
                .ForMember(x => x.MaterialType, o => o.Ignore())
                .ForMember(x => x.MediaType, o => o.Ignore())
                .ForMember(x => x.ExcuseCode, o => o.Ignore())
                .ForMember(x => x.Imprint, o => o.Ignore())
                .ForMember(x => x.Height, o => o.Ignore())
                .ForMember(x => x.Width, o => o.Ignore())
                .ForMember(x => x.ThicknessDepth, o => o.Ignore());
            CreateMap<CookedProduct, Application.Contracts.Models.Product>()
                .ForMember(p => p.Height, o => o.Ignore())
                .ForMember(p => p.ThicknessDepth, o => o.Ignore())
                .ForMember(p => p.Width, o => o.Ignore())
                .ForMember(p => p.FirstPrintPublishDate, o => o.Ignore())
                .ForMember(p => p.SubjectCodes, o => o.Ignore())
                .ReverseMap();
            CreateMap<Product, ProductBasicDetail>().ReverseMap();
            CreateMap<CookedProduct, ProductBasicDetail>().ReverseMap();

            CreateMap<CookedProduct, Product>().ReverseMap()
                .ForMember(p => p.Height, o => o.Ignore())
                .ForMember(p => p.ThicknessDepth, o => o.Ignore())
                .ForMember(p => p.Width, o => o.Ignore())
                .ForMember(p => p.FirstPrintPublishDate, o => o.Ignore())
                .ForMember(p => p.UpdatedTimestamp, o => o.MapFrom(y => y.UpdatedTimestamp))
                .ForMember(p => p.SubjectCodes, o => o.Ignore())
                .ForMember(x => x.ProductEducationSubjectLevels, f => f.Ignore())
                .ForMember(x => x.ExcuseCode,
                    o => o.MapFrom(y => y.ExcuseCode.SelectMany(w => w.Select(x => x.Name)).FirstOrDefault()))
                .ForMember(x => x.Imprint,
                    o => o.MapFrom(y => y.Imprint.SelectMany(w => w.Select(x => x.Name)).FirstOrDefault()))
                .ForMember(x => x.EditorialStaff,
                    o => o.MapFrom(y => y.EditorialStaff.SelectMany(w => w.Select(x => x.Name)).FirstOrDefault()))
                ;

            CreateMap<Scope, Domain.Contracts.Entities.Subscription.Scope>().ReverseMap();
            CreateMap<Subscription, Domain.Contracts.Entities.Subscription.Subscription>().ReverseMap();
        }
    }
}