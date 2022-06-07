using LAUER_SWEN2_TOUR_PLANNER.DAL;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using Npgsql;

namespace LAUER_SWEN2_TOUR_PLANNER.DAL.Repositories
{
    public class TourRepository : IRepository<Tour>
    {
        NpgsqlConnection npgsqlConnection;
        public TourRepository(NpgsqlConnection npgsqlConnection)
        {
            this.npgsqlConnection = npgsqlConnection;
        }

        public bool Delete(Tour obj)
        {
            using var cmd = new NpgsqlCommand("DELETE FROM tours WHERE t_id=@t_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("t_id", obj.Id.ToString());

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
            using var cmd = new NpgsqlCommand("DELETE FROM tours WHERE t_id=@t_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("t_id", id);

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public Tour? GetById(Guid id)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM tours WHERE t_id=@t_id", npgsqlConnection);

            cmd.Parameters.AddWithValue("t_id", id.ToString());

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();

                    byte[] pictureBuffer = new byte[809600];
                    var picture = reader.GetBytes(reader.GetOrdinal("t_picture"), 0, pictureBuffer, 0, pictureBuffer.Length);

                    return new Tour(
                        Guid.Parse(reader.GetString(reader.GetOrdinal("t_id"))),
                        reader.GetString(reader.GetOrdinal("t_name")),
                        reader.GetString(reader.GetOrdinal("t_description")),
                        reader.GetString(reader.GetOrdinal("t_from")),
                        reader.GetString(reader.GetOrdinal("t_to")),
                        reader.GetFieldValue<ETransportType>(reader.GetOrdinal("t_transport")),
                        reader.GetDouble(reader.GetOrdinal("t_distance")),
                        reader.GetInt32(reader.GetOrdinal("t_estimatedTime")),
                        reader.GetDateTime(reader.GetOrdinal("t_creationTime")),
                        pictureBuffer
                        );
                }
                return null;
            }
        }


        public Tour? GetByName(string name)
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM tours WHERE t_name=@t_name", npgsqlConnection);

            cmd.Parameters.AddWithValue("t_name", name);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();

                    byte[] pictureBuffer = new byte[809600];
                    var picture = reader.GetBytes(reader.GetOrdinal("t_picutre"), 0, pictureBuffer, 0, pictureBuffer.Length);

                    return new Tour(
                        Guid.Parse(reader.GetString(reader.GetOrdinal("t_id"))),
                        reader.GetString(reader.GetOrdinal("t_name")),
                        reader.GetString(reader.GetOrdinal("t_description")),
                        reader.GetString(reader.GetOrdinal("t_from")),
                        reader.GetString(reader.GetOrdinal("t_to")),
                        reader.GetFieldValue<ETransportType>(reader.GetOrdinal("t_transport")),
                        reader.GetDouble(reader.GetOrdinal("t_distance")),
                        reader.GetInt32(reader.GetOrdinal("t_estimatedTime")),
                        reader.GetDateTime(reader.GetOrdinal("t_creationTime")),
                        pictureBuffer
                        );
                }
                return null;
            }
        }

        public Tour? Add(Tour obj)
        {
            using var cmd = new NpgsqlCommand("INSERT INTO tours (t_id, t_name, t_description, t_distance, t_creationTime, t_picture, t_estimatedTime, t_from, t_to, t_transport) VALUES ((@t_id), (@t_name), (@t_description), (@t_distance), (@t_creationTime), (@t_picture) ,(@t_estimatedTime), (@t_from), (@t_to), (@t_transport))", npgsqlConnection);

            cmd.Parameters.AddWithValue("t_id", obj.Id.ToString());
            cmd.Parameters.AddWithValue("t_name", obj.Name);
            cmd.Parameters.AddWithValue("t_description", obj.Description);
            cmd.Parameters.AddWithValue("t_distance", obj.Distance);
            cmd.Parameters.AddWithValue("t_creationTime", obj.CreationDate);
            cmd.Parameters.AddWithValue("t_picture", obj.PictureBytes);
            cmd.Parameters.AddWithValue("t_estimatedTime", obj.EstimatedTime);
            cmd.Parameters.AddWithValue("t_from", obj.From);
            cmd.Parameters.AddWithValue("t_to", obj.To);
            cmd.Parameters.AddWithValue("t_transport", obj.TransportType);

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

        public bool DeleteAll()
        {
            using var cmd = new NpgsqlCommand("TRUNCATE tours CASCADE", npgsqlConnection);

            cmd.Prepare();
            int res = cmd.ExecuteNonQuery();

            if (res != 0)
            {
                return true;
            }
            return false;
        }

        public Tour? Update(Tour obj)
        {
            throw new NotImplementedException();
        }

        public List<Tour> GetAll()
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM tours", npgsqlConnection);

            List<Tour> tours = new();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    byte[] pictureBuffer = new byte[809600];
                    var picture = reader.GetBytes(reader.GetOrdinal("t_picture"),0, pictureBuffer, 0, pictureBuffer.Length);
                    tours.Add(new Tour(
                        Guid.Parse(reader.GetString(reader.GetOrdinal("t_id"))),
                        reader.GetString(reader.GetOrdinal("t_name")),
                        reader.GetString(reader.GetOrdinal("t_description")),
                        reader.GetString(reader.GetOrdinal("t_from")),
                        reader.GetString(reader.GetOrdinal("t_to")),
                        reader.GetFieldValue<ETransportType>(reader.GetOrdinal("t_transport")),
                        reader.GetDouble(reader.GetOrdinal("t_distance")),
                        reader.GetInt32(reader.GetOrdinal("t_estimatedTime")),
                        reader.GetDateTime(reader.GetOrdinal("t_creationTime")),
                        pictureBuffer
                        ));
                }
                return tours;
            }
        }
    }
}
