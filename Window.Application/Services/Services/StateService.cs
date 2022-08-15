using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Services.Interfaces;
using Window.Domain.Entities.Location;
using Window.Domain.ViewModels.Admin.State;
using Window.Domain.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using Window.Data.Context;
using Window.Domain.ViewModels.Site.API;

namespace Window.Application.Services.Implementation
{
    public class StateService : IStateService
    {
        #region Ctor

        private WindowDbContext _context;

        public StateService(WindowDbContext context)
        {
            _context = context;
        }

        #endregion

        //Get Location By Unique Name
        public async Task<State> GetLocationByUniqueName(string uniqueName)
        {
            return await _context.States.FirstOrDefaultAsync(p => !p.IsDelete && p.UniqueName == uniqueName);
        }

        //Get List Of States 
        public async Task<List<State>> GetListOfState()
        {
            return await _context.States.Where(p => !p.IsDelete && p.ParentId == 1).ToListAsync();
        }

        //Get List Of City By City Name
        public async Task<List<StateAPIViewModel>> GetListOfCityByCityName(string cityName)
        {
            //Get Parent By Id 
            var state = await _context.States.FirstOrDefaultAsync(p => !p.IsDelete && p.UniqueName == cityName);
            if (state == null) return null;

            //Get City By State Id
            var model = await _context.States.Where(p => !p.IsDelete && p.ParentId == state.Id)
                .Select(p => new StateAPIViewModel()
                {
                    CityName = p.UniqueName
                }).ToListAsync();

            if (model == null) return null;

            return model;
        }

        public Task<State> GetStateById(ulong stateId)
        {
            return _context.States.FirstOrDefaultAsync(s => !s.IsDelete && s.Id == stateId);
        }

        public async Task<List<SelectListViewModel>> GetStateChildren(ulong stateId)
        {
            var select = await _context.States.Where(s => s.ParentId.HasValue && s.ParentId.Value == stateId && !s.IsDelete)
                .Select(s => new SelectListViewModel
                {
                    Id = s.Id,
                    Title = s.Title
                }).ToListAsync();

            return select;
        }

        public async Task<List<SelectListViewModel>> GetAllCountries()
        {
            return await _context.States.Where(s => s.ParentId == null && !s.IsDelete)
                .Select(s => new SelectListViewModel
                {
                    Id = s.Id,
                    Title = s.Title
                }).ToListAsync();
        }

        public async Task<bool> IsExistsStateById(ulong stateId)
        {
            return await _context.States.AnyAsync(s => s.Id == stateId && !s.IsDelete);
        }

        public async Task<CreateStateResult> CreateState(CreateStateViewModel stateViewModel)
        {
            if (await _context.States.AnyAsync(a => a.UniqueName == stateViewModel.UniqueName && !a.IsDelete))
            {
                return CreateStateResult.UniqNameIsExist;
            }

            if (stateViewModel.ParentId.HasValue && stateViewModel.ParentId.Value != 0)
            {
                var parent = await _context.States.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == stateViewModel.ParentId);

                if (parent == null) return CreateStateResult.Fail;
            }

            var state = new State()
            {
                Title = stateViewModel.Title,
                UniqueName = stateViewModel.UniqueName,
            };

            if (stateViewModel.ParentId != null && stateViewModel.ParentId != 0)
            {
                state.ParentId = stateViewModel.ParentId;
            }

            await _context.States.AddAsync(state);
            await _context.SaveChangesAsync();


            return CreateStateResult.Success;
        }

        public async Task<FilterStateViewModel> FilterState(FilterStateViewModel filter)
        {
            var query = _context.States
                .Include(a => a.Parent)
                .Where(a => !a.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region State

            switch (filter.StateStatus)
            {
                case StateStatus.All:
                    break;
                case StateStatus.NotDeleted:
                    query = query.Where(a => !a.IsDelete);
                    break;
                case StateStatus.Deleted:
                    query = query.Where(a => a.IsDelete);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            #endregion

            #region Filter

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title}%"));
            }

            if (!string.IsNullOrEmpty(filter.UniqeName))
            {
                query = query.Where(s => EF.Functions.Like(s.UniqueName, $"%{filter.UniqeName}%"));
            }

            if (filter.ParentId != null)
            {
                query = query.Where(a => a.ParentId == filter.ParentId);
                filter.ParentState = await _context.States.FirstOrDefaultAsync(a => a.Id == filter.ParentId);
            }
            else
            {
                query = query.Where(a => a.ParentId == null);
            }

            #endregion

            await filter.Paging(query);

            return filter;
        }

        public async Task<EditStateViewModel> FillEditStateViewModel(ulong stateId)
        {
            var state = await _context.States.SingleOrDefaultAsync(a => a.Id == stateId);

            if (state == null) return null;

            var result = new EditStateViewModel()
            {
                Title = state.Title,
                UniqueName = state.UniqueName,
                StateId = state.Id,
                ParentId = state.ParentId
            };

            return result;
        }

        public async Task<EditStateResult> EditState(EditStateViewModel stateViewModel)
        {
            var state = await _context.States.SingleOrDefaultAsync(a => a.Id == stateViewModel.StateId);

            if (state == null)
            {
                return EditStateResult.Fail;
            }

            if (stateViewModel.ParentId.HasValue && stateViewModel.ParentId.Value != 0)
            {
                var parent = await _context.States.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == stateViewModel.ParentId);

                if (parent == null) return EditStateResult.Fail;
            }

            if (await _context.States.AnyAsync(a => a.UniqueName == stateViewModel.UniqueName && a.Id != state.Id))
            {
                return EditStateResult.UniqNameIsExist;
            }

            state.Title = stateViewModel.Title;
            state.UniqueName = stateViewModel.UniqueName;

            _context.States.Update(state);
            await _context.SaveChangesAsync();

            return EditStateResult.Success;
        }

        public async Task<bool> DeleteState(ulong stateId)
        {
            var state = await _context.States.SingleOrDefaultAsync(a => a.Id == stateId);

            if (state == null)
            {
                return false;
            }

            state.IsDelete = true;

            _context.States.Update(state);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
