using LAUER_SWEN2_TOUR_PLANNER.BL.Tours;
using LAUER_SWEN2_TOUR_PLANNER.DAL;
using LAUER_SWEN2_TOUR_PLANNER.MAPQUEST.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAUER_SWEN2_TOUR_PLANNER.TEST
{
    public class UtilityTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void MapQuestTest()
        {
            var res = RequestRoute.Request("Krems an der Donau", "Vienna", MODEL.ETransportType.CAR);
            while (!res.IsCompleted)
            {

            }
            if (res != null)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void MapQuestPictureTest()
        {
            var res = RequestRoute.Request("Krems an der Donau", "Vienna", MODEL.ETransportType.CAR);

            while (!res.IsCompleted)
            {

            }

            if (res.Result != null)
            {
                var picture = RequestRoute.GetPicture(res.Result);
                while (!picture.IsCompleted)
                {

                }
                if (picture.Result != null)
                {
                    Assert.Pass();
                }
                Assert.Fail();
            }
            Assert.Fail();
        }

        [Test]
        public void MapQuestDifferentTransportsTest()
        {
            var res = RequestRoute.Request("Krems an der Donau", "Vienna", MODEL.ETransportType.CAR);
            var res2 = RequestRoute.Request("Krems an der Donau", "Vienna", MODEL.ETransportType.FOOT);

            while (!res.IsCompleted && !res2.IsCompleted)
            {

            }

            if (res.Result != null && res2.Result!=null)
            {
                if (res.Result.route.time != res2.Result.route.time)
                {
                    Assert.Pass();
                }
                Assert.Fail();
            }
            Assert.Fail();
        }

        [Test]
        public void OneTourReportTest()
        {
            ReportCreator rc = new();
            var res = RequestRoute.Request("Krems an der Donau", "Vienna", MODEL.ETransportType.CAR);
            var picture = RequestRoute.GetPicture(res.Result);
            rc.GenerateReportForOneTour(new MODEL.Tour("test", "test", "Krems an der Donau", "Vienna", MODEL.ETransportType.CAR, res.Result.route.distance, res.Result.route.time, DateTime.Now, picture.Result));
        }

        [Test]
        public void TourSummeryTest()
        {
            ReportCreator rc = new();
            var res = RequestRoute.Request("Krems an der Donau", "Vienna", MODEL.ETransportType.CAR);
            var picture = RequestRoute.GetPicture(res.Result);
            List<MODEL.Tour> tours = new();
            tours.Add(new MODEL.Tour("test", "test", "Krems an der Donau", "Vienna", MODEL.ETransportType.CAR, res.Result.route.distance, res.Result.route.time, DateTime.Now, picture.Result));
            rc.CreateSummaryReport();
        }

    }
}
