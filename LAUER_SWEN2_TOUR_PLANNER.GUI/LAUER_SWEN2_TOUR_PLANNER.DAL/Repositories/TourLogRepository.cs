using LAUER_SWEN2_TOUR_PLANNER.DAL;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using Npgsql;

namespace LAUER_SWEN2_TOUR_PLANNER.DAL.Repositories
{
    public class TourLogRepository : IRepository<TourLog>
    {
        NpgsqlConnection npgsqlConnection;
        public TourLogRepository(NpgsqlConnection npgsqlConnection)
        {
            this.npgsqlConnection = npgsqlConnection;
        }
        public bool Delete(TourLog obj)
        {
            using var cmd = new NpgsqlCommand("DELETE FROM tourLogs WHERE tl_id=@tl_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("tl_id", obj.Id.ToString());

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public bool Delete(Guid id)
        {
            using var cmd = new NpgsqlCommand("DELETE FROM tourLogs WHERE tl_id=@tl_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("tl_id", id.ToString());

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public bool DeleteAll()
        {
            using var cmd = new NpgsqlCommand("TRUNCATE tourLogs", npgsqlConnection);

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public TourLog? GetById(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM tourLogs WHERE tl_id=@tl_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("tl_id", id.ToString());

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return new TourLog(
                        Guid.Parse(reader.GetString(reader.GetOrdinal("tl_id"))),
                        Guid.Parse(reader.GetString(reader.GetOrdinal("t_id"))),
                        reader.GetDateTime(reader.GetOrdinal("tl_creationtime")),
                        reader.GetString(reader.GetOrdinal("tl_comment")),
                        reader.GetFieldValue<Difficulty>(reader.GetOrdinal("tl_difficulty")),
                        reader.GetInt32(reader.GetOrdinal("tl_time")),
                        reader.GetFieldValue<Rating>(reader.GetOrdinal("tl_rating"))
                        );
                }
                return null;
            }
        }


        public List<TourLog> GetByUserId(Guid user_id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM tourLogs WHERE u_id=@u_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("u_id", user_id.ToString());

            List<TourLog> tourLogs = new();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    tourLogs.Add(new TourLog(
                        Guid.Parse(reader.GetString(reader.GetOrdinal("tl_id"))),
                        Guid.Parse(reader.GetString(reader.GetOrdinal("t_id"))),
                        reader.GetDateTime(reader.GetOrdinal("tl_creationDate")),
                        reader.GetString(reader.GetOrdinal("tl_comment")),
                        reader.GetFieldValue<Difficulty>(reader.GetOrdinal("tl_difficulty")),
                        reader.GetInt32(reader.GetOrdinal("tl_totalTime")),
                        reader.GetFieldValue<Rating>(reader.GetOrdinal("tl_rating"))
                        ));
                }
                return tourLogs;
            }
        }

        public List<TourLog> GetByTourId(Guid tour_id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM tourLogs WHERE t_id=@t_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("t_id", tour_id.ToString());

            List<TourLog> tourLogs = new();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    tourLogs.Add(new TourLog(
                        Guid.Parse(reader.GetString(reader.GetOrdinal("tl_id"))),
                        Guid.Parse(reader.GetString(reader.GetOrdinal("t_id"))),
                        reader.GetDateTime(reader.GetOrdinal("tl_creationTime")),
                        reader.GetString(reader.GetOrdinal("tl_comment")),
                        reader.GetFieldValue<Difficulty>(reader.GetOrdinal("tl_difficulty")),
                        reader.GetInt32(reader.GetOrdinal("tl_time")),
                        reader.GetFieldValue<Rating>(reader.GetOrdinal("tl_rating"))
                        )) ;
                }
                return tourLogs;
            }
        }

        public TourLog? Add(TourLog obj)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO tourLogs (tl_id, tl_comment, tl_creationTime, tl_time, tl_difficulty, tl_rating, t_id) VALUES ((@tl_id), (@tl_comment), (@tl_creationTime), (@tl_time), (@tl_difficulty), (@tl_rating), (@t_id))", npgsqlConnection);

            cmd.Parameters.AddWithValue("tl_id", obj.Id.ToString());
            cmd.Parameters.AddWithValue("tl_comment", obj.Comment);
            cmd.Parameters.AddWithValue("tl_creationTime", obj.CreationDate);
            cmd.Parameters.AddWithValue("tl_time", obj.TotalTime);
            cmd.Parameters.AddWithValue("tl_difficulty", obj.Difficulty);
            cmd.Parameters.AddWithValue("tl_rating", obj.TourRating);
            cmd.Parameters.AddWithValue("t_id", obj.TourId.ToString());
            cmd.Parameters.AddWithValue("tl_creationTime", obj.CreationDate);


            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();
            if (res != 0)
            {
                return obj;
            }
            else
            {
                return null;
            }
        }

        public TourLog? Update(TourLog obj)
        {
            throw new NotImplementedException();
        }

        public List<TourLog> GetAll()
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM tourLogs", npgsqlConnection);

            List<TourLog> tourLogs = new();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    tourLogs.Add(new TourLog(
                        Guid.Parse(reader.GetString(reader.GetOrdinal("tl_id"))),
                        Guid.Parse(reader.GetString(reader.GetOrdinal("t_id"))),
                        reader.GetDateTime(reader.GetOrdinal("tl_creationDate")),
                        reader.GetString(reader.GetOrdinal("tl_comment")),
                        reader.GetFieldValue<Difficulty>(reader.GetOrdinal("tl_difficulty")),
                        reader.GetInt32(reader.GetOrdinal("tl_totalTime")),
                        reader.GetFieldValue<Rating>(reader.GetOrdinal("tl_rating"))
                        ));
                }
                return tourLogs;
            }
        }
    }
}
