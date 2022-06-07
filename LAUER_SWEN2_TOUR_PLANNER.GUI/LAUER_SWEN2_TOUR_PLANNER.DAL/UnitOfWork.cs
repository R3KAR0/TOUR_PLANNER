using LAUER_SWEN2_TOUR_PLANNER.DAL.Repositories;
using Npgsql;

namespace LAUER_SWEN2_TOUR_PLANNER.DAL
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {

        private readonly string connString;
        private readonly NpgsqlConnection npgsqlConnection;
        private bool disposedValue;
        private NpgsqlTransaction? sqlTran;



        #region Repository Declarations

        private TourRepository? tourRepository = null;
        private TourLogRepository? logRepository = null;


        #endregion

        #region RepositoryGetters
        public TourRepository TourRepository()
        {
            if (tourRepository == null)
            {
                tourRepository = new TourRepository(npgsqlConnection);
            }
            return tourRepository;
        }

        public TourLogRepository TourLogRepository()
        {
            if (logRepository == null)
            {
                logRepository = new TourLogRepository(npgsqlConnection);
            }
            return logRepository;
        }

        #endregion


        public UnitOfWork()
        {
            var mapper = ConfigMapper.GetConfigMapper();
            if (mapper == null) throw new NullReferenceException();

            connString = mapper.ConnectionString;

            npgsqlConnection = new NpgsqlConnection(connString);
            npgsqlConnection.Open();
            CreateTransaction();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (sqlTran != null)
                    {
                        Commit();
                    }
                    npgsqlConnection?.Close();
                    npgsqlConnection?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void CreateTransaction()
        {
            //Log.Information("Transaction started");
            sqlTran = npgsqlConnection.BeginTransaction();
        }

        public void Commit()
        {
            //Log.Information("Commited changes");
            sqlTran?.Commit();
        }

        public void Rollback()
        {
            //Log.Information("Rollback started");
            sqlTran?.Rollback();
            sqlTran = null;
        }
    }
}
