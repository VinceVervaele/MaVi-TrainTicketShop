using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Trains_FSD.Models.Entities;
using Trains_FSD.Services;
using Trains_FSD.Services.Interfaces;
using Trains_FSD.ViewModels;

namespace Trains_FSD.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : Controller
    {
        private readonly IMapper _mapper;
        private IServiceTraject<Traject> _trajectService;

        public TrainController(IMapper mapper, IServiceTraject<Traject> trajectService)
        {
            _mapper = mapper;
            _trajectService = trajectService;
        }

        [HttpGet("GetTrainsByDepartureAndArrival")] // /api/train/gettrainsbydepartureandarrival?departurecityid=X&arrivalcityid=X
        public async Task<IEnumerable<TrainVM>> GetTrainsByDepartureAndArrival(int departureCityId, int arrivalCityId)
        {          
            var traject = await _trajectService.FindByMultipleIds(departureCityId, arrivalCityId);
            
            List<TrajectLineVM> trajectLineVMs = _mapper.Map<List<TrajectLineVM>>(traject.TrajectLines);

            List<TrainVM> trainVMs = new List<TrainVM>();

            foreach(TrajectLineVM trajectLineVM in trajectLineVMs)
            {
                trainVMs.Add(trajectLineVM.Line.Train);
            }
            return trainVMs;
        }
    }
}
