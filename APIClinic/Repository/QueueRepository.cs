using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class QueueRepository : IQueueNo
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public QueueRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<string> CreateQueueDoctor(long SpecialistDoctorId, CancellationToken cancellationToken)
        {

            try
            {
                var QueueNo = "";
                long iQueueNo = 0;
                int lengthQueue = 0;
                var checkQueue = await _context.QueueDoctor.Where(x => x.SpecialistDoctorId == SpecialistDoctorId && x.QueueDate.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).AsNoTracking().OrderByDescending(x => x.QueueNo).FirstOrDefaultAsync();
                if (checkQueue != null)
                {
                    lengthQueue = checkQueue.QueueNo.Count();
                    iQueueNo = Convert.ToInt64(checkQueue.QueueNo.Substring(1, lengthQueue - 1)) + 1;
                    QueueNo = "D" + iQueueNo.ToString();

                    checkQueue.QueueNo = QueueNo;
                    checkQueue.QueueDate = DateTime.Now;
                    _context.QueueDoctor.Update(checkQueue);
                    _context.SaveChanges();
                }
                else
                {


                    QueueNo = "D" + DateTime.Now.ToString("yyyyMM") + SpecialistDoctorId.ToString() + "0001";
                    var dataNewQueueDoctor = new QueueDoctor
                    {
                        SpecialistDoctorId = SpecialistDoctorId,
                        QueueNo = QueueNo,
                        QueueDate = DateTime.Now,
                        CreateBy = "System",
                        CreateDate = DateTime.Now
                    };
                    _context.QueueDoctor.Add(dataNewQueueDoctor);
                    _context.SaveChanges();

                }

                return QueueNo;
            }
            catch (Exception ex)
            {
                return "D" + DateTime.Now.ToString("yyyyMM") + SpecialistDoctorId.ToString() + "0001";
            }

        }

        public async Task<string> PrintQueueDoctor(string QueueNo, CancellationToken cancellationToken)
        {

            try
            {

                var checkQueue = await _context.QueueDoctor.Where(x => x.QueueNo == QueueNo).AsNoTracking().FirstOrDefaultAsync();
                if (checkQueue != null)
                {

                    checkQueue.IsPrint = checkQueue.IsPrint + 1;
                    checkQueue.UpdateBy = "System";
                    checkQueue.UpdateDate = DateTime.Now;
                    _context.QueueDoctor.Update(checkQueue);
                    _context.SaveChanges();
                }
                else
                {
                    return "Queue : " + QueueNo + " Not Found.";
                }

                return "Successfully Print Queue Doctor";
            }
            catch (Exception ex)
            {
                return "Error : Print Queue Doctor --> " + ex.Message;
            }

        }

        public async Task<string> CreateQueueLab(long LaboratoriumId, CancellationToken cancellationToken)
        {

            try
            {
                var QueueNo = "";
                long iQueueNo = 0;
                int lengthQueue = 0;
                var checkQueue = await _context.QueueLab.Where(x => x.LaboratoriumId == LaboratoriumId && x.QueueDate.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")).AsNoTracking().OrderByDescending(x => x.QueueNo).FirstOrDefaultAsync();
                if (checkQueue != null)
                {
                    lengthQueue = checkQueue.QueueNo.Count();
                    iQueueNo = Convert.ToInt64(checkQueue.QueueNo.Substring(1, lengthQueue - 1)) + 1;
                    QueueNo = "L" + iQueueNo.ToString();

                    checkQueue.QueueNo = QueueNo;
                    checkQueue.QueueDate = DateTime.Now;
                    _context.QueueLab.Update(checkQueue);
                    _context.SaveChanges();
                }
                else
                {

                    QueueNo = "L" + DateTime.Now.ToString("yyyyMM") + LaboratoriumId.ToString() + "0001";
                    var dataNewQueueLab = new QueueLab
                    {
                        LaboratoriumId = LaboratoriumId,
                        QueueNo = QueueNo,
                        QueueDate = DateTime.Now,
                        CreateBy = "System",
                        CreateDate = DateTime.Now
                    };
                    _context.QueueLab.Add(dataNewQueueLab);
                    _context.SaveChanges();
                }


                return QueueNo;
            }
            catch (Exception ex)
            {
                return "L" + DateTime.Now.ToString("yyyyMM") + LaboratoriumId.ToString() + "0001";
            }

        }

        public async Task<string> PrintQueueLab(string QueueNo, CancellationToken cancellationToken)
        {

            try
            {

                var checkQueue = await _context.QueueLab.Where(x => x.QueueNo == QueueNo).AsNoTracking().FirstOrDefaultAsync();
                if (checkQueue != null)
                {

                    checkQueue.IsPrint = checkQueue.IsPrint + 1;
                    checkQueue.UpdateBy = "System";
                    checkQueue.UpdateDate = DateTime.Now;
                    _context.QueueLab.Update(checkQueue);
                    _context.SaveChanges();

                }
                else
                {
                    return "Queue : " + QueueNo + " Not Found.";
                }

                return "Successfully Print Queue Lab";
            }
            catch (Exception ex)
            {
                return "Error : Print Queue Lab --> " + ex.Message;
            }

        }
    }
}
