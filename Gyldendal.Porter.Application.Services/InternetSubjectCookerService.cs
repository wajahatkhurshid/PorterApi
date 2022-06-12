using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Application.Services
{
    public class InternetSubjectCookerService : IInternetSubjectCookerService
    {
        private static ISubjectCodeRepository _subjectCodeRepository;

        public InternetSubjectCookerService(ISubjectCodeRepository subjectCodeRepository)
        {
            _subjectCodeRepository = subjectCodeRepository;
        }

        public async Task<CookedInternetSubjectWithCode> Cook(List<List<GpmNode>> internetSubjects, List<List<GpmNode>> subjectCodesTaxonomy)
        {
            var result = new CookedInternetSubjectWithCode();
            
            var internetSubjectDetails = GetIsolatedInternetSubjectDetails(internetSubjects);
            result.WebShops = internetSubjectDetails.webShops;
            result.Areas = internetSubjectDetails.areas;
            result.Subjects = internetSubjectDetails.subjects;
            result.SubAreas = internetSubjectDetails.subAreas;

            if (subjectCodesTaxonomy != null)
            {
                var cookedSubjectCodes = await GetCookedSubjectCodes(subjectCodesTaxonomy);
                result.SubjectCodes = cookedSubjectCodes;
            }

            return result;
        }

        private static (List<WebShop> webShops, List<Area> areas, List<Subject> subjects, List<SubArea> subAreas) GetIsolatedInternetSubjectDetails(IEnumerable<List<GpmNode>> seriesInternetSubjects)
        {
            (List<WebShop> webShops, List<Area> areas, List<Subject> subjects, List<SubArea> subAreas) result = (null, null, null, null);

            var webShops = new List<WebShop>();
            var areas = new List<Area>();
            var subjects = new List<Subject>();
            var subAreas = new List<SubArea>();

            foreach (var results in seriesInternetSubjects.Select(ExtractInternetSubjectDetails))
            {
                if (results.webShop != WebShop.None)
                    webShops.Add(results.webShop);

                if (results.area != null)
                    areas.Add(results.area);

                if (results.subject != null)
                    subjects.Add(results.subject);

                if (results.subArea != null)
                    subAreas.Add(results.subArea);
            }

            if (webShops.Count > 0)
                result.webShops = webShops;

            if (areas.Count > 0)
                result.areas = areas;

            if (subjects.Count > 0)
                result.subjects = subjects;

            if (subAreas.Count > 0)
                result.subAreas = subAreas;

            return result;
        }

        private static (WebShop webShop, Area area, Subject subject, SubArea subArea) ExtractInternetSubjectDetails(List<GpmNode> seriesInternetSubject)
        {
            (WebShop webShop, Area area, Subject subject, SubArea subArea) result = (WebShop.None, null, null, null);
            var webShop = WebShop.None;
            var areaId = 0;
            var subjectId = 0;
            GpmNode node;

            if (seriesInternetSubject.Count > 0)
            {
                node = seriesInternetSubject[0];
                webShop = (WebShop)Enum.Parse(typeof(WebShop), node.Name);

                result.webShop = webShop;
            }

            if (seriesInternetSubject.Count > 1)
            {
                node = seriesInternetSubject[1];
                areaId = node.NodeId;

                result.area = new Area { Id = areaId, Name = node.Name, WebShop = webShop };
            }

            if (seriesInternetSubject.Count > 2)
            {
                node = seriesInternetSubject[2];
                subjectId = node.NodeId;

                result.subject = new Subject { Id = subjectId, Name = node.Name, AreaId = areaId, WebShop = webShop };
            }

            if (seriesInternetSubject.Count > 3)
            {
                node = seriesInternetSubject[3];

                result.subArea = new SubArea { Id = node.NodeId, Name = node.Name, SubjectId = subjectId, WebShop = webShop };
            }

            return result;
        }

        private static async Task<List<Domain.Contracts.Entities.MasterData.SubjectCode>> GetCookedSubjectCodes(List<List<GpmNode>> subjectCodesTaxonomy)
        {
            var subjectCodes = subjectCodesTaxonomy.SelectMany(sc => sc);
            var cookedSubjectCodes = new List<Domain.Contracts.Entities.MasterData.SubjectCode>();
            foreach (var subjectCode in subjectCodes)
            {
                var cookedSubjectCode = await _subjectCodeRepository.GetSubjectCodeAsync(subjectCode.Name);
                cookedSubjectCodes.Add(cookedSubjectCode);
            }

            return cookedSubjectCodes;
        }
    }
}
