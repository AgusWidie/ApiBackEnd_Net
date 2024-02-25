namespace APIClinic.Repository.IRepository
{
    public interface IQueueNo
    {
        Task<string> CreateQueueDoctor(long SpecialistDoctorId, CancellationToken cancellationToken = default);
        Task<string> PrintQueueDoctor(string QueueNo, CancellationToken cancellationToken = default);
        Task<string> CreateQueueLab(long LaboratoriumId, CancellationToken cancellationToken = default);
        Task<string> PrintQueueLab(string QueueNo, CancellationToken cancellationToken = default);
    }
}
