using APIClinic.CacheList.ProcessDataList.Interface;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace APIClinic.CacheList.ProcessDataList
{
    public class MasterDataList : IMasterDataList
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public MasterDataList(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public void GetMasterClinicList()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<ClinicResponse>? clinicList = null;
                    clinicList = (from clinic in _context.Clinic
                                  select new ClinicResponse
                                  {
                                      Id = clinic.Id,
                                      Name = clinic.Name,
                                      Address = clinic.Address,
                                      Telp = clinic.Telp,
                                      Fax = clinic.Fax,
                                      CreateBy = clinic.CreateBy,
                                      CreateDate = clinic.CreateDate,
                                      UpdateBy = clinic.UpdateBy,
                                      UpdateDate = clinic.UpdateDate
                                  }).OrderBy(x => x.Name).AsNoTracking();

                    if (clinicList.Count() > 0)
                    {
                        if (GeneralList._listClinic.Count() <= 0 || GeneralList._listClinic.Count() < clinicList.Count())
                        {
                            GeneralList._listClinic.Clear();
                            GeneralList._listClinic.AddRange(clinicList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();
                }
                catch (Exception ex)
                {

                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetMasterClinicList";
                    logDataError.ErrorDeskripsi = "Error Job MasterDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);

                }
            }

        }

        public void GetMasterBranchList()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<BranchResponse>? branchList = null;
                    branchList = (from branch in _context.Branch
                                  join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                  select new BranchResponse
                                  {
                                      Id = branch.Id,
                                      Name = branch.Name,
                                      ClinicId = clinic.Id,
                                      ClinicName = clinic.Name,
                                      Address = branch.Address,
                                      Telp = branch.Telp,
                                      Fax = branch.Fax,
                                      CreateBy = branch.CreateBy,
                                      CreateDate = branch.CreateDate,
                                      UpdateBy = branch.UpdateBy,
                                      UpdateDate = branch.UpdateDate
                                  }).OrderBy(x => x.Name).AsNoTracking();

                    if (branchList.Count() > 0)
                    {
                        if (GeneralList._listBranch.Count() <= 0 || GeneralList._listBranch.Count() < branchList.Count())
                        {
                            GeneralList._listBranch.Clear();
                            GeneralList._listBranch.AddRange(branchList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();

                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetMasterBranchList";
                    logDataError.ErrorDeskripsi = "Error Job MasterDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }

        }

        public void GetMasterDrugList()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<DrugResponse>? drugList = null;
                    drugList = (from branch in _context.Branch
                                join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                join drug in _context.Drug on branch.Id equals drug.BranchId
                                select new DrugResponse
                                {
                                    Id = drug.Id,
                                    ClinicId = clinic.Id,
                                    ClinicName = clinic.Name,
                                    BranchId = branch.Id,
                                    BranchName = branch.Name,
                                    DrugName = drug.DrugName,
                                    UnitType = drug.UnitType,
                                    Price = drug.Price,
                                    Stock = drug.Stock,
                                    Active = drug.Active,
                                    CreateBy = drug.CreateBy,
                                    CreateDate = drug.CreateDate,
                                    UpdateBy = drug.UpdateBy,
                                    UpdateDate = drug.UpdateDate
                                }).OrderBy(x => x.DrugName).AsNoTracking();

                    if (drugList.Count() > 0)
                    {
                        if (GeneralList._listDrug.Count() <= 0 || GeneralList._listDrug.Count() < drugList.Count())
                        {
                            GeneralList._listDrug.Clear();
                            GeneralList._listDrug.AddRange(drugList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();

                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetMasterDrugList";
                    logDataError.ErrorDeskripsi = "Error Job MasterDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }

        }

        public void GetMasterDoctorList()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<DoctorResponse>? doctorList = null;
                    doctorList = (from branch in _context.Branch
                                  join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                  join doc in _context.Doctor on branch.Id equals doc.BranchId
                                  select new DoctorResponse
                                  {
                                      Id = doc.Id,
                                      ClinicId = clinic.Id,
                                      ClinicName = clinic.Name,
                                      BranchId = branch.Id,
                                      BranchName = branch.Name,
                                      DoctorName = doc.DoctorName,
                                      DateOfBirth = doc.DateOfBirth,
                                      Address = doc.Address,
                                      NoTelephone = doc.NoTelephone,
                                      MobilePhone = doc.MobilePhone,
                                      Active = doc.Active,
                                      CreateBy = doc.CreateBy,
                                      CreateDate = doc.CreateDate,
                                      UpdateBy = doc.UpdateBy,
                                      UpdateDate = doc.UpdateDate
                                  }).OrderBy(x => x.DoctorName).AsNoTracking();

                    if (doctorList.Count() > 0)
                    {
                        if (GeneralList._listDoctor.Count() <= 0 || GeneralList._listDoctor.Count() < doctorList.Count())
                        {
                            GeneralList._listDoctor.Clear();
                            GeneralList._listDoctor.AddRange(doctorList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();

                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetMasterDoctorList";
                    logDataError.ErrorDeskripsi = "Error Job MasterDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }

        }

        public void GetMasterSpecialList()
        {

            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<SpecialistResponse>? specialistList = null;
                    specialistList = (from spe in _context.Specialist
                                      orderby spe.Name
                                      select new SpecialistResponse
                                      {
                                          Id = spe.Id,
                                          Name = spe.Name,
                                          Active = spe.Active,
                                          CreateBy = spe.CreateBy,
                                          CreateDate = spe.CreateDate,
                                          UpdateBy = spe.UpdateBy,
                                          UpdateDate = spe.UpdateDate
                                      }).OrderBy(x => x.Name).AsNoTracking();


                    if (specialistList.Count() > 0)
                    {
                        if (GeneralList._listSpeciallist.Count() <= 0 || GeneralList._listSpeciallist.Count() < specialistList.Count())
                        {
                            GeneralList._listSpeciallist.Clear();
                            GeneralList._listSpeciallist.AddRange(specialistList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();

                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetMasterSpecialList";
                    logDataError.ErrorDeskripsi = "Error Job MasterDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }

        }

        public void GetMasterSpecialListDoctor()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<SpecialistDoctorResponse>? specialDoctorList = null;
                    specialDoctorList = (from branch in _context.Branch
                                         join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                         join spe in _context.SpecialistDoctor on branch.Id equals spe.BranchId
                                         join doc in _context.Doctor on spe.DoctorId equals doc.Id
                                         join specialist in _context.Specialist on spe.SpecialistId equals specialist.Id
                                         select new SpecialistDoctorResponse
                                         {
                                             Id = spe.Id,
                                             ClinicId = clinic.Id,
                                             ClinicName = clinic.Name,
                                             BranchId = branch.Id,
                                             BranchName = branch.Name,
                                             DoctorId = spe.DoctorId,
                                             DoctorName = doc.DoctorName,
                                             SpecialistId = specialist.Id,
                                             SpecialistName = specialist.Name,
                                             Description = spe.Description,
                                             Active = spe.Active,
                                             CreateBy = spe.CreateBy,
                                             CreateDate = spe.CreateDate,
                                             UpdateBy = spe.UpdateBy,
                                             UpdateDate = spe.UpdateDate
                                         }).OrderBy(x => x.SpecialistName).AsNoTracking();


                    if (specialDoctorList.Count() > 0)
                    {
                        if (GeneralList._listSpecialDoctor.Count() <= 0 || GeneralList._listSpecialDoctor.Count() < specialDoctorList.Count())
                        {
                            GeneralList._listSpecialDoctor.Clear();
                            GeneralList._listSpecialDoctor.AddRange(specialDoctorList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();
                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetMasterSpecialList";
                    logDataError.ErrorDeskripsi = "Error Job MasterDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }

            }


        }

        public void GetMasterProfil()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<ProfilResponse>? profilList = null;
                    profilList = (from pro in _context.Profil
                                  orderby pro.Name
                                  select new ProfilResponse
                                  {
                                      Id = pro.Id,
                                      Name = pro.Name,
                                      Description = pro.Description,
                                      Active = pro.Active,
                                      CreateBy = pro.CreateBy,
                                      CreateDate = pro.CreateDate,
                                      UpdateBy = pro.UpdateBy,
                                      UpdateDate = pro.UpdateDate
                                  }).OrderBy(x => x.Name).AsNoTracking();

                    if (profilList.Count() > 0)
                    {
                        if (GeneralList._listProfil.Count() <= 0 || GeneralList._listProfil.Count() < profilList.Count())
                        {
                            GeneralList._listProfil.Clear();
                            GeneralList._listProfil.AddRange(profilList);
                            GarbageCollector.GarbageCollection();
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetMasterProfil";
                    logDataError.ErrorDeskripsi = "Error Job MasterDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }



        }

        public void GetMasterMenu()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<MenuResponse>? menuList = null;
                    menuList = (from menu in _context.Menu
                                select new MenuResponse
                                {
                                    Id = menu.Id,
                                    Name = menu.Name,
                                    ControllerName = menu.Name,
                                    ActionName = menu.ActionName,
                                    Description = menu.Description,
                                    IsHeader = menu.IsHeader,
                                    Active = menu.Active,
                                    Sort = menu.Sort,
                                    CreateBy = menu.CreateBy,
                                    CreateDate = menu.CreateDate,
                                    UpdateBy = menu.UpdateBy,
                                    UpdateDate = menu.UpdateDate
                                }).OrderBy(x => x.Sort).AsNoTracking();


                    if (menuList.Count() > 0)
                    {
                        if (GeneralList._listMenu.Count() <= 0 || GeneralList._listMenu.Count() < menuList.Count())
                        {
                            GeneralList._listMenu.Clear();
                            GeneralList._listMenu.AddRange(menuList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();
                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetMasterMenu";
                    logDataError.ErrorDeskripsi = "Error Job MasterDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }

            }

        }

        public void GetMasterProfilMenu()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<ProfilMenuResponse>? profilMenuList = null;
                    profilMenuList = (from proMenu in _context.ProfilMenu
                                      join pro in _context.Profil on proMenu.ProfilId equals pro.Id
                                      join menuParent in _context.Menu on proMenu.ParentMenuId equals menuParent.Id
                                      join menu in _context.Menu on proMenu.MenuId equals menu.Id
                                      select new ProfilMenuResponse
                                      {
                                          Id = proMenu.Id,
                                          ProfilId = proMenu.ProfilId,
                                          ProfilName = pro.Name,
                                          ParentMenuId = proMenu.ParentMenuId,
                                          ParentMenuName = menuParent.Name,
                                          MenuId = menu.Id,
                                          MenuName = menu.Name,
                                          CreateBy = proMenu.CreateBy,
                                          CreateDate = proMenu.CreateDate,
                                          UpdateBy = proMenu.UpdateBy,
                                          UpdateDate = proMenu.UpdateDate
                                      }).OrderBy(x => x.ProfilName).AsNoTracking();


                    if (profilMenuList.Count() > 0)
                    {
                        if (GeneralList._listProfilMenu.Count() <= 0 || GeneralList._listProfilMenu.Count() < profilMenuList.Count())
                        {
                            GeneralList._listProfilMenu.Clear();
                            GeneralList._listProfilMenu.AddRange(profilMenuList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();
                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetMasterProfilMenu";
                    logDataError.ErrorDeskripsi = "Error Job MasterDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }

        }

        public void GetMasterProfilUser()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<ProfilUserResponse>? profilUserList = null;
                    profilUserList = (from proUser in _context.ProfilUser
                                      join user in _context.Users on proUser.UserId equals user.Id
                                      join branch in _context.Branch on user.BranchId equals branch.Id
                                      join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                      join pro in _context.Profil on proUser.ProfilId equals pro.Id
                                      select new ProfilUserResponse
                                      {
                                          Id = proUser.Id,
                                          ProfilId = proUser.ProfilId,
                                          ProfilName = pro.Name,
                                          UserId = user.Id,
                                          UserName = user.UserName,
                                          CreateBy = user.CreateBy,
                                          CreateDate = user.CreateDate,
                                          UpdateBy = user.UpdateBy,
                                          UpdateDate = user.UpdateDate
                                      }).OrderBy(x => x.UserName).AsNoTracking();

                    if (profilUserList.Count() > 0)
                    {
                        if (GeneralList._listProfilUser.Count() <= 0 || GeneralList._listProfilMenu.Count() < profilUserList.Count())
                        {
                            GeneralList._listProfilUser.Clear();
                            GeneralList._listProfilUser.AddRange(profilUserList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();

                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetMasterProfilUser";
                    logDataError.ErrorDeskripsi = "Error Job MasterDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }

        }

        public void GetMasterLaboratorium()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<LaboratoriumResponse>? laboratoriumList = null;
                    laboratoriumList = (from branch in _context.Branch
                                        join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                        join lab in _context.Laboratorium on branch.Id equals lab.BranchId
                                        select new LaboratoriumResponse
                                        {
                                            Id = lab.Id,
                                            ClinicId = clinic.Id,
                                            ClinicName = clinic.Name,
                                            BranchId = branch.Id,
                                            BranchName = branch.Name,
                                            LaboratoriumName = lab.LaboratoriumName,
                                            Description = lab.Description,
                                            Price = lab.Price,
                                            Active = lab.Active,
                                            CreateBy = lab.CreateBy,
                                            CreateDate = lab.CreateDate,
                                            UpdateBy = lab.UpdateBy,
                                            UpdateDate = lab.UpdateDate
                                        }).OrderBy(x => x.LaboratoriumName).AsNoTracking();


                    if (laboratoriumList.Count() > 0)
                    {
                        if (GeneralList._listLaboratorium.Count() <= 0 || GeneralList._listLaboratorium.Count() < laboratoriumList.Count())
                        {
                            GeneralList._listLaboratorium.Clear();
                            GeneralList._listLaboratorium.AddRange(laboratoriumList);
                            GarbageCollector.GarbageCollection();
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetMasterLaboratorium";
                    logDataError.ErrorDeskripsi = "Error Job MasterDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }

        }

        public void GetMasterScheduleDoctor()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<ScheduleDoctorResponse>? scheduleDoctorList = null;
                    scheduleDoctorList = (from branch in _context.Branch
                                          join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                          join spe in _context.ScheduleDoctor on branch.Id equals spe.BranchId
                                          join doc in _context.Doctor on spe.DoctorId equals doc.Id
                                          join spec in _context.SpecialistDoctor on spe.SpecialistDoctorId equals spec.Id
                                          join specialist in _context.Specialist on spec.SpecialistId equals specialist.Id
                                          select new ScheduleDoctorResponse
                                          {
                                              Id = spe.Id,
                                              ClinicId = clinic.Id,
                                              ClinicName = clinic.Name,
                                              BranchId = branch.Id,
                                              BranchName = branch.Name,
                                              SpecialistDoctorId = spec.Id,
                                              DoctorId = spe.DoctorId,
                                              DoctorName = doc.DoctorName,
                                              SpecialistId = specialist.Id,
                                              SpecialistName = specialist.Name,
                                              Active = spe.Active,
                                              CreateBy = spe.CreateBy,
                                              CreateDate = spe.CreateDate,
                                              UpdateBy = spe.UpdateBy,
                                              UpdateDate = spe.UpdateDate
                                          }).OrderBy(x => x.DoctorName).AsNoTracking();

                    if (scheduleDoctorList.Count() > 0)
                    {
                        if (GeneralList._listScheduleDoctor.Count() <= 0 || GeneralList._listScheduleDoctor.Count() < scheduleDoctorList.Count())
                        {
                            GeneralList._listScheduleDoctor.Clear();
                            GeneralList._listScheduleDoctor.AddRange(scheduleDoctorList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();
                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetMasterScheduleDoctor";
                    logDataError.ErrorDeskripsi = "Error Job MasterDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }

        }
    }
}
