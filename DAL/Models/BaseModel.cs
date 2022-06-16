namespace Data.Models
{
    /// <summary>
    /// Base model for all domain models.
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Unique identifier of model in DB table.
        /// </summary>
        public int Id { get; set; }
    }
}