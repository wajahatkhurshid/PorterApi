using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Common.Exceptions;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Repositories;

namespace Gyldendal.Porter.Application.Services.WorkReview
{
    public class WorkReviewCookingService : IWorkReviewCookingService
    {
        private readonly IWorkRepository _workRepository;
        private readonly ICookedWorkReviewRepository _cookedWorkReviewRepository;
        private readonly IMapper _mapper;

        public WorkReviewCookingService(IWorkRepository workRepository,
            ICookedWorkReviewRepository cookedWorkReviewRepository, IMapper mapper)
        {
            _workRepository = workRepository;
            _cookedWorkReviewRepository = cookedWorkReviewRepository;
            _mapper = mapper;
        }

        public async Task Cook(int workReviewId, CancellationToken cancellationToken)
        {
            var work = await _workRepository.GetWorkByWorkReviewIdAsync(workReviewId);
            if (work == null)
            {
                throw new CookingException(
                    $"{Environment.MachineName} - No WorkReview found to be cooked in Work base collection against WorkReviewId: {workReviewId}");
            }

            var review = work.WorkReviews.First(wr => wr.ContainerInstanceId == workReviewId);

            var cookedReview = _mapper.Map<CookedWorkReview>(review);
            cookedReview.IsDeleted = false; // TODO: Missing. GPM will implement this eventually.
            cookedReview.UpdatedTimestamp = work.UpdatedTimestamp;
            cookedReview.WorkId = work.ContainerInstanceId;
            cookedReview.WebShops = new List<WebShop>
            {
                WebShop.GU, WebShop.GyldendalPlus
            }; //allWorkShopsForReview TODO: Missing. Extract from Products when GPM will implement product ids under work

            await _cookedWorkReviewRepository.UpsertAsync(cookedReview);
        }
    }
}