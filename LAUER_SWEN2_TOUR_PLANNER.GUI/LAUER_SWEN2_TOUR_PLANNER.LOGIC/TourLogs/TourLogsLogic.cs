using LAUER_SWEN2_TOUR_PLANNER.BL.CustomExceptions;
using LAUER_SWEN2_TOUR_PLANNER.DAL;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LAUER_SWEN2_TOUR_PLANNER.BL.TourLogs
{
    public class TourLogsLogic
    {
        public static bool DeleteTourLog(TourLog log)
        {
            try
            {
                using (UnitOfWork unit = new())
                {
                    var logRepo = unit.TourLogRepository().Delete(log);
                    Log.Information($"Successfully deleted TourLog with Id {log.Id}");
                    return true;
                }

            }
            catch (Exception)
            {
                Log.Error($"Failed to delete TourLog with Id {log.Id}");
                throw;
            }
        }

        public static void UpdateTourLog(TourLog tl)
        {
            try
            {
                Log.Information($"Try to update Tour with id: {tl.Id}");
                using (UnitOfWork unit = new())
                {
                    var repo = unit.TourLogRepository();
                    var tourlog = repo.GetById(tl.Id);
                    if (tourlog == null)
                    {
                        throw new UpdateFailedException();
                    }
                    Regex stringRegex = new(@"^[a-zA-Z0-9\x20\-]+$");
                    //Regex numberRegex = new("^[0-9]+$"); ensured in ViewModel
                    if (tl.Comment.Length == 0 || tl.TotalTime == 0)
                    {
                        throw new MissingMemberException();
                    }
                    if (stringRegex.IsMatch(tl.Comment))
                    {
                        tl.Comment = tl.Comment.Trim();
                        repo.Delete(tl);
                        unit.Commit();
                        unit.CreateTransaction();
                        repo.Add(tl);
                        Log.Information($"Updated Tour with id: {tl.Id} successfully");
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
                Log.Error($"Encountered exception {e} while updating Tour with id: {tl.Id}");
                throw;
            }
        }
    }
}
