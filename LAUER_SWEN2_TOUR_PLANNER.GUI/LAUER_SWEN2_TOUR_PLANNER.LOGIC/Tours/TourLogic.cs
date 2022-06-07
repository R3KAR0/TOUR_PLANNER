using LAUER_SWEN2_TOUR_PLANNER.BL.CustomExceptions;
using LAUER_SWEN2_TOUR_PLANNER.DAL;
using LAUER_SWEN2_TOUR_PLANNER.MAPQUEST.Requests;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LAUER_SWEN2_TOUR_PLANNER.BL.Tours
{
    public class TourLogic
    {
        public static List<Tour> GetAllToursWithTourLogs()
        {
            try
            {
                Log.Information("Started to receive Tours with TourLogs");
                using (UnitOfWork unit = new())
                {
                    var tours = unit.TourRepository().GetAll();

                    foreach (var t in tours)
                    {
                        var tourlogs = unit.TourLogRepository().GetByTourId(t.Id);
                        if (tourlogs != null)
                        {
                            t.Logs = tourlogs;
                        }
                    }
                    return tours;
                }
            }
            catch (Exception)
            {
                Log.Error("Encountered exception while retreiving Tours with TourLogs from DB!");
                throw;
            }
        }
        public async void CreateTour(string Name, string From, string To, string Description, ETransportType _selectedTransportType)
        {
            using (var unit = new UnitOfWork())
            {
                var _repo = unit.TourRepository();
                var duplicationCheck = _repo.GetByName(Name);
                if (duplicationCheck != null)
                {
                    throw new AlreadyExistsException();
                }
                Regex regex = new(@"^[a-zA-Z0-9\x20\-]+$");
                if (Name.Length == 0 || From.Length == 0 || To.Length == 0)
                {
                    throw new MissingMemberException();
                }
                if (regex.IsMatch(Name) && regex.IsMatch(From) && regex.IsMatch(To) && (regex.IsMatch(Description) || Description.Length == 0))
                {
                    var res = await RequestRoute.Request(From, To, _selectedTransportType);
                    if (res == null)
                    {
                        throw new NoResultException();
                    }
                    var picture = await RequestRoute.GetPicture(res);

                    var result = _repo.Add(new Tour(Name, Description, From, To, _selectedTransportType, res.route.distance, res.route.time, DateTime.Now, picture));
                    if (result == null)
                    {
                        throw new CreationFailedException();
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                    throw new InvalidCharactersException();
                }
            }
        }
        public static void DeleteTour(Tour t)
        {
            try
            {
                Log.Information($"Try to delete Tour with id: {t.Id}");
                using (UnitOfWork unit = new())
                {
                    var logRepo = unit.TourLogRepository();
                    t.Logs.ForEach(tl => logRepo.Delete(tl));
                    unit.TourRepository().Delete(t);
                    Log.Information($"Deleted Tour with id: {t.Id} successfully");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Encountered exception {e} while deleting Tour with id: {t.Id}");
                throw;
            }
        }

        
        public static async Task UpdateTour(Tour t)
        {
            try
            {
                Log.Information($"Try to update Tour with id: {t.Id}");
                using (UnitOfWork unit = new())
                {
                    var repo = unit.TourRepository();
                    var tour = repo.GetById(t.Id);
                    t.Description = t.Description.Trim();
                    if(tour == null)
                    {
                        throw new UpdateFailedException();
                    }
                    Regex regex = new(@"[a-zA-Z0-9\x20\-]+$");
                    if (t.Name.Length == 0 || t.From.Length == 0 || t.To.Length == 0)
                    {
                        throw new MissingMemberException();
                    }
                    if (regex.IsMatch(t.Name) && regex.IsMatch(t.From) && regex.IsMatch(t.To) && (regex.IsMatch(t.Description) || t.Description.Length == 0))
                    {
                        var mapquest = await RequestRoute.Request(t.From.Trim(), t.To.Trim(), t.TransportType);
                        if (mapquest == null) throw new NullReferenceException();
                        var pic = await RequestRoute.GetPicture(mapquest);
                        if (pic == null) throw new PictureNullException();

                        t.Distance = mapquest.route.distance;
                        t.EstimatedTime = mapquest.route.time;
                        t.PictureBytes = pic;

                        unit.TourRepository().Delete(t);
                        unit.Commit();
                        unit.CreateTransaction();
                        unit.TourRepository().Add(t);
                        Log.Information($"Updated Tour with id: {t.Id} successfully");
                    }
                    else
                    {
                        throw new InvalidCharactersException();
                    }
                }
            }
            catch (PictureNullException)
            {
                Log.Error($"No picture received!");
                throw;
            }
            catch (NullReferenceException)
            {
                Log.Error($"Mapquest response was null!");
                throw;
            }
            catch (Exception e)
            {
                Log.Error($"Encountered exception {e} while updating Tour with id: {t.Id}");
                throw;
            }
        }
    }
}
