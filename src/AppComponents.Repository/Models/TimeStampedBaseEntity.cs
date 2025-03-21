namespace AppComponents.Repository.Models
{
    public abstract class TimeStampedBaseEntity
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime ModifiedAt { get; private set; }

        public TimeStampedBaseEntity()
        {
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.MinValue;
        }

        public void MarkModified()
        {
            ModifiedAt = DateTime.Now;
        }
    }
}
