using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;
using Builder = Gyldendal.Porter.Console.LoadTester.WorkContainerBuilder;
using Build = Bogus.Faker<Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers.WorkContainer>;

namespace Gyldendal.Porter.Console.LoadTester
{
    public class WorkContainerBuilder
    {
        private Faker<GpmNode> _subjectCodeMainFaker;
        private Faker<GpmNode> _subjectCodeFakerWith2Chars;
        private Faker<GpmNode> _subjectCodeFakerWith3Chars;
        private Faker<GpmNode> _webShopFaker;
        private Faker<GpmNode> _subjectFaker;
        private Faker<GpmNode> _educationSubjectLevelFaker;
        private Faker<WorkReviewContainer> _reviewsFaker;
        private List<List<GpmNode>> _internetSubjects;
        private List<int> _fakeGeneratedProductIds;

        public Builder WithFakeSubjectCodeMain()
        {
            _subjectCodeMainFaker = new Faker<GpmNode>()
                .RuleFor(gn => gn.NodeId, x => x.Random.Int(101))
                .RuleFor(gn => gn.Name, x => x.Random.String2(4, "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"));

            return this;
        }

        public Builder WithFakeSubjectCode()
        {
            _subjectCodeFakerWith2Chars = new Faker<GpmNode>()
                .RuleFor(gn => gn.NodeId, x => x.Random.Int(1001))
                .RuleFor(gn => gn.Name, x => x.Random.String2(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"));

            _subjectCodeFakerWith3Chars = new Faker<GpmNode>()
                .RuleFor(gn => gn.NodeId, x => x.Random.Int(1001))
                .RuleFor(gn => gn.Name, x => x.Random.String2(3, "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"));

            return this;
        }

        public Builder WithFakeWebShop()
        {
            _webShopFaker = new Faker<GpmNode>()
                .StrictMode(false)
                .RuleFor(gn => gn.NodeId, x => (int)x.PickRandom<FakeWebShop>())
                .RuleFor(gn => gn.Name, (x, webShop) => ((FakeWebShop)webShop.NodeId).ToString());

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

        public Builder WithFakeWorkReviews()
        {
            _reviewsFaker = new Faker<WorkReviewContainer>()
                .RuleFor(wr => wr.ContainerInstanceId, x => x.Random.Int(1001))
                .RuleFor(wr => wr.ReviewMedia, x => x.Lorem.Word())
                .RuleFor(wr => wr.ReviewRating, x => x.Random.Int(1, 5))
                .RuleFor(wr => wr.ReviewWrittenBy, x => x.Name.FullName(x.PickRandom<Name.Gender>()))
                .RuleFor(wr => wr.ReviewContent, x => x.Rant.Review(x.Commerce.ProductName()));

            return this;
        }

        public Builder WithFakeInternetSubject()
        {
            _internetSubjects = new List<List<GpmNode>>
            {
                new List<GpmNode>
                {
                    _subjectCodeFakerWith2Chars.Generate(count: 1)[0],
                    _subjectCodeFakerWith3Chars.Generate(count: 1)[0],
                    _subjectCodeFakerWith3Chars.Generate(count: 1)[0],
                    _subjectCodeFakerWith3Chars.Generate(count: 1)[0]
                }
            };

            return this;
        }

        public Builder WithFakeProductIdsAs(IEnumerable<int> productIds)
        {
            _fakeGeneratedProductIds = productIds.ToList();
            return this;
        }

        public Build Build()
        {
            var build = new Faker<WorkContainer>()
                .StrictMode(false)
                .RuleFor(wc => wc.Id, f => Guid.NewGuid())
                .RuleFor(wc => wc.ContainerInstanceId, x => x.IndexFaker)
                .RuleFor(wc => wc.ProductIds, (x,node) => (x.Make(1, () => _fakeGeneratedProductIds[node.ContainerInstanceId])))
                .RuleFor(wc => wc.WorkIdentificationNumber, x => x.IndexFaker.ToString())
                .RuleFor(wc => wc.WorkTitle, x => x.Commerce.ProductName())
                .RuleFor(wc => wc.WorkSubtitle, x => x.Commerce.ProductAdjective())
                .RuleFor(wc => wc.WorkGyldendalShopText, x => x.Commerce.ProductDescription())
                .RuleFor(wc => wc.WorkInternalText, x => x.Commerce.ProductDescription())
                .RuleFor(wc => wc.WorkWebText, x => x.Commerce.ProductDescription())
                .RuleFor(wc => wc.WorkSubjectCodeMain, x => new List<List<GpmNode>> { _subjectCodeMainFaker.Generate(count: 1) })
                .RuleFor(wc => wc.WorkSubjectCode, x => _internetSubjects)
                .RuleFor(wc => wc.WorkGUInternetSubject, x => new List<List<GpmNode>>
                {
                   _webShopFaker.Generate(count: 1),
                   _subjectFaker.Generate(count: 1),
                   _subjectCodeFakerWith3Chars.Generate(count: 1),
                   _educationSubjectLevelFaker.Generate(count: 1)
                })
                .RuleFor(wc => wc.WorkEducationSubjectLevel, x => new List<List<GpmNode>>
                {
                    new List<GpmNode> { _subjectFaker.Generate(count: 1)[0], _educationSubjectLevelFaker.Generate(count:1)[0] },
                    new List<GpmNode> { _subjectFaker.Generate(count: 1)[0], _educationSubjectLevelFaker.Generate(count:1)[0] },
                    new List<GpmNode> { _subjectFaker.Generate(count: 1)[0], _educationSubjectLevelFaker.Generate(count:1)[0] },
                    new List<GpmNode> { _subjectFaker.Generate(count: 1)[0], _educationSubjectLevelFaker.Generate(count:1)[0] }
                })
                .RuleFor(wc => wc.WorkReviews, f => _reviewsFaker.Generate(10));

            return build;
        }
    }
}
