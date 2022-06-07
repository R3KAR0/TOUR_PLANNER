namespace LAUER_SWEN2_TOUR_PLANNER.DAL
{
    public interface IRepository<TEntity> where TEntity : class
    {
        bool Delete(TEntity obj);
        bool Delete(Guid id);
        TEntity? GetById(Guid id);
        TEntity? Add(TEntity obj);
        TEntity? Update(TEntity obj);
        List<TEntity> GetAll();
    }
}
