using LAUER_SWEN2_TOUR_PLANNER.BL.CustomExceptions;
using LAUER_SWEN2_TOUR_PLANNER.DAL;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAUER_SWEN2_TOUR_PLANNER.BL.Tours
{
    public class IOLogic
    {
        public static async Task<bool> ExportTours()
        {
            try
            {
                Log.Information("Start to export tours!");
                using (UnitOfWork unit = new())
                {
                    var tours = unit.TourRepository().GetAll();
                    foreach (var tour in tours)
                    {
                        tour.Logs = unit.TourLogRepository().GetByTourId(tour.Id);
                    }
                    var mapper = ConfigMapper.GetConfigMapper();
                    if (mapper != null)
                    {
                        await File.WriteAllTextAsync(ConfigMapper.GetConfigMapper().Export, System.Text.Json.JsonSerializer.Serialize(tours));
                        Log.Information("Successfully exported tours!");
                        return true;
                    }
                    Log.Error("Failed to export tours!");
                    return false;

                }
            }
            catch (Exception e)
            {
                Log.Error("Failed to export tours!");
                throw;
            }
        }

        public static bool ImportTours()
        {
            try
            {
                Log.Information("Start to import tours!");
                using (UnitOfWork unit = new())
                {
                    unit.TourRepository().DeleteAll();

                    var mapper = ConfigMapper.GetConfigMapper();
                    if (mapper != null)
                    {
                        var json = File.ReadAllText(ConfigMapper.GetConfigMapper().Import);
                        var tours = System.Text.Json.JsonSerializer.Deserialize<List<Tour>>(json);
                        if (tours != null)
                        {
                            foreach (var t in tours)
                            {
                                unit.TourRepository().Add(t);
                                foreach (var tl in t.Logs)
                                {
                                    unit.TourLogRepository().Add(tl);
                                }

                            }
                            Log.Information("Imported tours successfully!");
                            return true;
                            
                        }
                        else
                        {
                            Log.Error("No tours to import!");
                            throw new NoToursToImportException();
                        }
                    }
                    else
                    {
                        Log.Error("Config mapper exception encountered while importing tours!");
                        throw new ConfigMapperException();
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("exception encountered while importing tours!");
                throw;
            }
        }
    }
}
