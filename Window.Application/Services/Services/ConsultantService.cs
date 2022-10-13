using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Counseling;
using Window.Domain.Entities.TechnicalIssues;
using Window.Domain.ViewModels.Admin.Cosultant;
using Window.Domain.ViewModels.Admin.TechnicalIssues;

namespace Window.Application.Services.Services
{
    public class ConsultantService : IConsulantService
    {
        #region Ctor

        private readonly WindowDbContext _context;

        public ConsultantService(WindowDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Admin Side 

        //Filter Consultant Admin Side View Model
        public async Task<FilterConsultantAdminSideViewModel> FilterConsultantAdminSideViewModel(FilterConsultantAdminSideViewModel filter)
        {
            var query = _context.Counselings
                .Where(a => !a.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region State 

            #region Base On Ordering CreateDate

            switch (filter.FilterConcultantAdminSideOrder)
            {
                case FilterConcultantAdminSideOrder.CreateDate_Des:
                    break;
                case FilterConcultantAdminSideOrder.CreateDate_Asc:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
            }

            #endregion

            #endregion

            #region Filter By Properties

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title}%"));

            #endregion

            await filter.Paging(query);

            return filter;
        }


        //Create Consultant
        public async Task CreateConsultant(Counseling counseling)
        {
            await _context.Counselings.AddAsync(counseling);
            await _context.SaveChangesAsync();
        }

        //Get Consultant By Id
        public async Task<Counseling?> GetConsultantById(ulong id)
        {
            return await _context.Counselings.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == id);
        }

        //Update Consultant 
        public async Task UpdateConsultant(Counseling counseling)
        {
            var consultant = await GetConsultantById(counseling.Id);

            consultant.Title = counseling.Title;
            consultant.Description = counseling.Description;

            _context.Counselings.Update(consultant);
            await _context.SaveChangesAsync();
        }

        //Delete Consultant 
        public async Task<bool> DeleteConsultant(ulong id)
        {
            var consultant = await GetConsultantById(id);
            if (consultant == null) return false;

            consultant.IsDelete = true;

            _context.Counselings.Update(consultant);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion

    }
}
