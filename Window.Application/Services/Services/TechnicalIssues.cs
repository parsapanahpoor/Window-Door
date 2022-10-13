using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.TechnicalIssues;
using Window.Domain.ViewModels.Admin.TechnicalIssues;
using Window.Domain.ViewModels.Article;

namespace Window.Application.Services.Services
{
    public class TechnicalIssuesService : ITechnicalIssues
    {
        #region Ctor

        private readonly WindowDbContext _context;

        public TechnicalIssuesService(WindowDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Admin Side 

        //Filter Technical Issues Admin Side View Model
        public async Task<FilterTechnicalIssuesAdminSideViewModel> FilterTechnicalIssuesAdminSideViewModel(FilterTechnicalIssuesAdminSideViewModel filter)
        {
            var query = _context.TechnicalIssues
                .Where(a => !a.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region State 

            #region Base On Ordering CreateDate

            switch (filter.filterTechnicalIssuesAdminSideOrder)
            {
                case FilterTechnicalIssuesAdminSideOrder.CreateDate_Des:
                    break;
                case FilterTechnicalIssuesAdminSideOrder.CreateDate_Asc:
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


        //Create Technical Issues
        public async Task CreateTechnicalIssues(TechnicalIssues technicalIssues)
        {
            await _context.TechnicalIssues.AddAsync(technicalIssues);
            await _context.SaveChangesAsync();
        }

        //Get Technical Issues By Id
        public async Task<TechnicalIssues?> GetTechnicalIssuesById(ulong id)
        {
            return await _context.TechnicalIssues.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == id);
        }

        //Update Technical Issues 
        public async Task UpdateTechnicalIssues(TechnicalIssues technical)
        {
            var technicalIssues = await GetTechnicalIssuesById(technical.Id);

            technicalIssues.Title = technical.Title;
            technicalIssues.Description = technical.Description;

            _context.TechnicalIssues.Update(technicalIssues);
            await _context.SaveChangesAsync();
        }

        //Delete Technical Issues 
        public async Task<bool> DeleteTechnicalIssues(ulong id)
        {
            var technicalIssues = await GetTechnicalIssuesById(id);
            if (technicalIssues == null) return false;

            technicalIssues.IsDelete = true;

            _context.TechnicalIssues.Update(technicalIssues);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion
    }
}
