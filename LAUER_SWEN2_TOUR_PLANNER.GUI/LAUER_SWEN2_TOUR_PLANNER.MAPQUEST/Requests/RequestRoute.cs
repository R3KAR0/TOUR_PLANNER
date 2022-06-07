using LAUER_SWEN2_TOUR_PLANNER.DAL;
using LAUER_SWEN2_TOUR_PLANNER.DAL.Exceptions;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAUER_SWEN2_TOUR_PLANNER.MAPQUEST.Requests
{
    public static class RequestRoute
    {
        private static readonly HttpClient client = new HttpClient();
        public static async Task<Root?> Request(string from, string to, ETransportType transport)
        {
            Log.Information($"Starting request for: from {from} to: {to} transport type: {transport}");
            try
            {
                string routeType="fastest";
                switch(transport)
                {
                    case ETransportType.CAR:
                    case ETransportType.TRAIN:
                    case ETransportType.SHIP:
                    case ETransportType.UNKNOWN:
                        routeType = "fastest";
                        break;
                    case ETransportType.BICYCLE:
                        routeType = "bicycle";
                        break;
                    case ETransportType.FOOT:
                        routeType = "pedestrian";
                        break;
                }
                var mapper = ConfigMapper.GetConfigMapper();
                if (mapper != null)
                {
                    Log.Debug($"Retreiving MapQuest-Object for from: {from} to: {to} transport type: {transport}");
                    var requestString = $"http://www.mapquestapi.com/directions/v2/route?key={mapper.MapQuestKey}&from={from}&to={to}&routeType={routeType}";

                    var res = await client.GetStringAsync(requestString);

                    if (res == null) throw new NullReferenceException();

                    return System.Text.Json.JsonSerializer.Deserialize<Root>(res);

                }
                else
                {
                    Log.Error("Failed to initialize ConfigMapper for RouteRequest");
                    return null;
                }
            }
            catch (NoValidConfigException)
            {
                Log.Error("No valid config found for mapper!");
                return null;
            }
            catch (NullReferenceException)
            {
                Log.Error("Encountered Nullreference!");
                return null;
            }
        }

        public static async Task<byte[]?> GetPicture(Root? mapObject) 
        {
            try
            {
                Log.Debug($"Started to retrieve picture for {mapObject.route.sessionId}");
                if (mapObject == null) return null;
                var mapper = ConfigMapper.GetConfigMapper();
                if (mapper != null)
                {
                    var picutreRequest = $"http://www.mapquestapi.com/staticmap/v5/map?key={mapper.MapQuestKey}&session={mapObject.route.sessionId}";
                    Log.Debug($"Awaiting ByteArray for {mapObject.route.sessionId}");
                    var picture = await client.GetByteArrayAsync(picutreRequest);
                    return picture;
                }
                return null;
            }
            catch (Exception)
            {
                Log.Error($"Failed to receive picture for {mapObject.route.sessionId}!");
                return null;
            }
        }
    }
}
