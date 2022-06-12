using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bogus;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;
using Gyldendal.Porter.Infrastructure.Repository;
using Builder = Gyldendal.Porter.Console.LoadTester.ProductContainerBuilder;
using Build = Bogus.Faker<Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers.ProductContainer>;

namespace Gyldendal.Porter.Console.LoadTester
{
    public class ProductContainerBuilder
    {
        private Faker<GpmNode> _mediaMaterialTypeFaker;
        private Faker<GpmNode> _supplyAvailabilityCodeFaker;
        private Faker<ProductCollectionTitleContainer> _productCollectionTitleFaker;
        private Faker<ProfileContributorAuthorContainer> _contributorAuthorFaker;
        private Faker<GpmNode> _imprintFaker;
        private List<List<GpmNode>> _internetSubjects;
        private Faker<GpmNode> _subjectFaker;
        private Faker<GpmNode> _educationSubjectLevelFaker;
        private Faker<GpmNode> _subjectCodeMainFaker;
        private Faker<GpmNode> _subjectCodeFaker;
        private Faker<GpmNode> _webShopFaker;
        private Faker<MarketingAttachment> _marketingAttachmentFaker;
        private readonly PorterContext _porterContext;

        public ProductContainerBuilder(PorterContext porterContext)
        {
            _porterContext = porterContext;
        }

        public Builder WithFakeSubjectCodeMain()
        {
            _subjectCodeMainFaker = new Faker<GpmNode>()
                .RuleFor(gn => gn.NodeId, x => x.Random.Int(101))
                .RuleFor(gn => gn.Name, x => x.Random.String2(4, "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"));

            return this;
        }

        public Builder WithFakeSubjectCode()
        {
            var repository = new SubjectCodeRepository(_porterContext, null);
            var task = Task.Run(async () => await repository.GetSubjectCodesAsync());
            var subjectCodes = task.Result;

            _subjectCodeFaker = new Faker<GpmNode>()
                .RuleFor(gn => gn.NodeId, x => Convert.ToInt32(x.PickRandom(subjectCodes).Id))
                .RuleFor(gn => gn.Name, x => x.PickRandom(subjectCodes).Name);

            return this;
        }

        public Builder WithFakeMediaMaterialType()
        {
            _mediaMaterialTypeFaker = new Faker<GpmNode>()
                .StrictMode(false)
                .RuleFor(mmt => mmt.NodeId, f => (int)f.PickRandom<FakeMediaMaterialType>())
                .RuleFor(mmt => mmt.Name, (f, fmmt) => ((FakeMediaMaterialType)fmmt.NodeId).ToString());

            return this;
        }

        public Builder WithFakeSupplyAvailabilityCode()
        {
            _supplyAvailabilityCodeFaker = new Faker<GpmNode>()
                .RuleFor(gn => gn.NodeId, x => x.Random.Int(1))
                .RuleFor(gn => gn.Name, x => x.Random.Int(1).ToString());

            return this;
        }

        public Builder WithFakeProductCollectionTitle()
        {
            _productCollectionTitleFaker = new Faker<ProductCollectionTitleContainer>()
                .RuleFor(pct => pct.ContainerInstanceId, f => f.Random.Int(1))
                .RuleFor(pct => pct.Phase, f => "Published")
                .RuleFor(pct => pct.PhaseState, f => "Published")
                .RuleFor(pct => pct.SeriesTitle, f => f.Commerce.ProductName())
                .RuleFor(pct => pct.SeriesInternetSubject, f => _internetSubjects)
                .RuleFor(pct => pct.SeriesDescription, f => f.Lorem.Paragraphs(3, "\r\n\r\n"))
                .RuleFor(pct => pct.SeriesSerieSystemFlag, f => f.Random.Bool())
                .RuleFor(pct => pct.SeriesSubseries, f => new List<ProductCollectionTitleContainer>
                {
                    new ProductCollectionTitleContainer
                    {
                        ContainerInstanceId = f.Random.Int(100000),
                        Phase = "Published",
                        PhaseState = "Published",
                        SeriesTitle = $"Child Serie - {f.Commerce.ProductName()}",
                        SeriesInternetSubject = _internetSubjects,
                        SeriesDescription = f.Lorem.Paragraphs(3, "\r\n\r\n"),
                        SeriesSerieSystemFlag = f.Random.Bool(),
                        SeriesSubseries = null
                    }
                });

            return this;
        }

        public Builder WithFakeContributorAuthor()
        {
            var profileContributorMemberFaker = new Faker<ProfileContributorMember>()
                .StrictMode(false)
                .RuleFor(pcm => pcm.Phase, f => "Published")
                .RuleFor(pcm => pcm.PhaseState, f => "Published")
                .RuleFor(pct => pct.ProfileMemberId, f => f.Random.Int(1))
                .RuleFor(pct => pct.ProfileMemberFirstName, f => f.Person.FirstName)
                .RuleFor(pct => pct.ProfileMemberSecondName, f => f.Person.LastName)
                .RuleFor(pct => pct.ProfileMemberDisplayName, (f, member) => $"{member.ProfileMemberFirstName} {member.ProfileMemberSecondName}");

            _contributorAuthorFaker = new Faker<ProfileContributorAuthorContainer>()
                .RuleFor(pct => pct.ContainerInstanceId, f => f.Random.Int(1))
                .RuleFor(pct => pct.Phase, f => "Published")
                .RuleFor(pct => pct.PhaseState, f => "Published")
                .RuleFor(pct => pct.ProfileID, f => f.Random.Int(1))
                .RuleFor(pct => pct.ProfileName, f => f.Person.FullName)
                .RuleFor(pct => pct.ProfileImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(pct => pct.ProfileContributorMembers, f => profileContributorMemberFaker.Generate(count: 2));

            return this;
        }

        public Builder WithFakeProductImprint()
        {
            _imprintFaker = new Faker<GpmNode>()
                .StrictMode(false)
                .RuleFor(i => i.NodeId, f => (int)f.PickRandom<FakeImprint>())
                .RuleFor(i => i.Name, (f, imprint) => ((FakeImprint)imprint.NodeId).ToString());

            return this;
        }

        public Builder WithFakeInternetSubject()
        {
            _internetSubjects = new List<List<GpmNode>>
            {
                new List<GpmNode>
                {
                    _webShopFaker.Generate(1)[0],
                _subjectCodeFaker.Generate(count: 1)[0],
                _subjectCodeFaker.Generate(count: 1)[0],
                _subjectCodeFaker.Generate(count: 1)[0]
                }
            };

            return this;
        }
        public Builder WithFakeSubject()
        {
            _subjectFaker = new Faker<GpmNode>()
                .StrictMode(false)
                .RuleFor(gn => gn.NodeId, x => (int)x.PickRandom<FakeSubject>())
                .RuleFor(gn => gn.Name, (x, sub) => ((FakeSubject)sub.NodeId).ToString());

            return this;
        }

        public Builder WithFakeSubjectLevel()
        {
            _educationSubjectLevelFaker = new Faker<GpmNode>()
                .RuleFor(gn => gn.NodeId, x => x.Random.Int(101))
                .RuleFor(gn => gn.Name, x => $"{x.Random.Int(1, 10)}. klasse");

            return this;
        }

        public Builder WithFakeMarketingAttachment()
        {
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".docs", ".xls", ".xlsx", ".txt" };
            _marketingAttachmentFaker = new Faker<MarketingAttachment>()
                    .StrictMode(false)
                    .RuleFor(ma => ma.ContainerInstanceId, x => x.Random.Int(101))
                    .RuleFor(ma => ma.AttachmentFileType, x => x.PickRandom(allowedExtensions))
                    .RuleFor(ma => ma.AttachmentTitle, (x, att) => x.Internet.UrlWithPath("http", "rap.gyldendal.dk", att.AttachmentFileType))
                    .RuleFor(ma => ma.AttachmentDescription, x => x.Commerce.ProductDescription());

            return this;
        }

        public Builder WithFakeWebShop()
        {
            _webShopFaker = new Faker<GpmNode>()
                .StrictMode(false)
                .RuleFor(gn => gn.NodeId, x => (int)FakeWebShop.Harmoney)
                .RuleFor(gn => gn.Name, (x, webShop) => ((FakeWebShop)webShop.NodeId).ToString());

            return this;
        }

        public Build Build()
        {
            var isbns = File.ReadAllLines("./TestData/gdk_isbns.txt");

            var containerFaker = new Faker<ProductContainer>()
                .RuleFor(pc => pc.Id, f => Guid.NewGuid())
                .RuleFor(pc => pc.ContainerInstanceId, x => x.IndexFaker)
                .RuleFor(pc => pc.ProductISBN13, x => isbns[x.IndexFaker].Trim())
                //.RuleFor(pc => pc.ProductISBN13, x => x.Commerce.Ean13())
                .RuleFor(pc => pc.ProductTitle, x => x.Commerce.ProductName())
                .RuleFor(pc => pc.ProductSubtitle, x => x.Commerce.ProductAdjective())
                .RuleFor(wc => wc.ProductGyldendalShopText, x => x.Commerce.ProductDescription())
                .RuleFor(wc => wc.ProductWebText, x => x.Commerce.ProductDescription())
                .RuleFor(wc => wc.ProductMediaMaterialeType, x => new List<List<GpmNode>> { _mediaMaterialTypeFaker.Generate(count: 1) })
                .RuleFor(wc => wc.RelatedProductFirstEditionProductPublishedDate, x => x.Date.Past().Year.ToString())
                .RuleFor(wc => wc.ProductPublishedDate, x => x.Date.Recent())
                .RuleFor(wc => wc.ProductGUSampleURL, x => x.Internet.Url())
                .RuleFor(wc => wc.ProductEdition, x => x.Random.Int(1, 20))
                .RuleFor(wc => wc.ProductExtentTotalPageCount, x => x.Random.Int(1))
                .RuleFor(wc => wc.ProductSupplyAvailabilityCode, f => new List<List<GpmNode>> { _supplyAvailabilityCodeFaker.Generate(count: 1) })
                .RuleFor(wc => wc.ProductEducationSubjectLevel, f => new List<List<GpmNode>>
                {
                    new List<GpmNode> { _subjectFaker.Generate(count: 1)[0], _educationSubjectLevelFaker.Generate(count:1)[0] },
                    new List<GpmNode> { _subjectFaker.Generate(count: 1)[0], _educationSubjectLevelFaker.Generate(count:1)[0] },
                    new List<GpmNode> { _subjectFaker.Generate(count: 1)[0], _educationSubjectLevelFaker.Generate(count:1)[0] },
                    new List<GpmNode> { _subjectFaker.Generate(count: 1)[0], _educationSubjectLevelFaker.Generate(count:1)[0] }
                })
                .RuleFor(wc => wc.ProductGUInternetSubject, f => _internetSubjects)
                .RuleFor(wc => wc.ProductSubjectCodeMain, x => new List<List<GpmNode>> { _subjectCodeMainFaker.Generate(count: 1) })
                .RuleFor(wc => wc.ProductSubjectCode, x => new List<List<GpmNode>> { _subjectCodeFaker.Generate(count: 2) })
                .RuleFor(wc => wc.ProductEditorialDivision, f => new List<List<GpmNode>> { new List<GpmNode> { new GpmNode { NodeId = 1, Name = f.Commerce.Department() } } })
                .RuleFor(wc => wc.RelatedProductOriginalProductPublisher, f => f.Person.FullName)
                .RuleFor(wc => wc.ProductExtentDuration, f => f.Random.Int(1001).ToString())
                .RuleFor(wc => wc.Stock, f => f.Random.Int(0))
                //.RuleFor(wc => wc.ProductCollectionTitle, f => _productCollectionTitleFaker.Generate(count: 2))
                .RuleFor(wc => wc.MarketingAttachment, f => _marketingAttachmentFaker.Generate(count: 2))
                //.RuleFor(wc => wc.ProductContributorAuthor, f => _contributorAuthorFaker.Generate(count: 2))
                .RuleFor(wc => wc.IsNextPrintRunPlanned, f => f.Random.Bool())
                .RuleFor(wc => wc.ProductGUProductURL, f => f.Internet.Url())
                .RuleFor(wc => wc.MaterialTypeRank, x => x.Random.Int(1, 10))
                .RuleFor(wc => wc.MediaTypeRank, x => x.Random.Int(1, 10))
                .RuleFor(wc => wc.ProductMeasurementHeight, x => $"{Math.Round(x.Random.Double(20.0, 100.0), 2)}")
                .RuleFor(wc => wc.ProductMeasurementWidth, x => $"{Math.Round(x.Random.Double(20.0, 100.0), 2)}")
                .RuleFor(wc => wc.ProductMeasurementThickness, x => $"{Math.Round(x.Random.Double(20.0, 100.0), 2)}")
                .RuleFor(wc => wc.ProductGUEnableInspectionCopy, x => x.Random.Bool())
                .RuleFor(wc => wc.ProductDisplayOnShops, f => new List<List<GpmNode>> { _webShopFaker.Generate(count: 5) })
                .RuleFor(wc => wc.ProductImprint, x => new List<List<GpmNode>> { _imprintFaker.Generate(count: 1) })
                .RuleFor(wc => wc.ProductSupplyPriceWithoutVAT, f => Convert.ToDecimal(f.Commerce.Price()))
                .RuleFor(wc => wc.PriceWithoutVat, f => Convert.ToDecimal(f.Commerce.Price()))
                .RuleFor(wc => wc.ProductSupplyPriceWithVAT, f => Convert.ToDecimal(f.Commerce.Price()));

            return containerFaker;
        }
    }
}
