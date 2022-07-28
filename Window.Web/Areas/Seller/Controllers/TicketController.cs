using Window.Application.Extensions;
using Window.Application.Interfaces;
using Window.Domain.ViewModels.User.Ticket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Window.Web.Areas.Seller.Controllers;

namespace Window.Web.Areas.Seller.Controllers
{
    public class TicketController : SellerBaseController
    {
        #region Ctor

        public ITicketService _ticketService;

        public IUserService _userService;

        public IStringLocalizer<TicketController> _localizer;

        public TicketController(ITicketService ticketService, IUserService userService, IStringLocalizer<TicketController> localizer)
        {
            _ticketService = ticketService;
            _userService = userService;
            _localizer = localizer;
        }

        #endregion

        #region Filter Tickets

        public async Task<IActionResult> FilterTickets(FilterSiteTicketViewModel filter)
        {
            var result = await _ticketService.FilterSiteTicket(filter, User.GetUserId());

            return View(result);
        }

        #endregion

        #region Create Ticket

        [HttpGet]
        public async Task<IActionResult> CreateTicket()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTicket(CreateTicketViewModel create)
        {
            if (string.IsNullOrEmpty(create.Message))
            {
                TempData[ErrorMessage] = _localizer["Text entry required"].Value;
                return View(create);
            }

            var result = await _ticketService.CreateTicket(create, User.GetUserId());

            if (result != 0)
            {
                TempData[SuccessMessage] = _localizer["mission accomplished"].Value;
                return RedirectToAction("TicketDetail", "Ticket", new { area = "Seller", id = result });
            }

            TempData[ErrorMessage] = _localizer["An error occurred Please try again"].Value;

            return View(create);
        }

        #endregion

        #region Ticket Detail

        public async Task<IActionResult> TicketDetail(ulong id)
        {
            #region Fill View Model

            var result = await _ticketService.GetUserPanelTicketDetailViewModel(id, User.GetUserId());

            if (result == null) return NotFound();

            #endregion

            ViewData["TicketDetailViewModel"] = result;

            return View(new AnswerTicketViewModel
            {
                TicketId = id
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TicketDetail(AnswerTicketViewModel answer)
        {
            if (ModelState.IsValid)
            {
                var result = await _ticketService.AnswerTicketByUser(answer, User.GetUserId());

                if (result)
                {
                    TempData[SuccessMessage] = _localizer["mission accomplished"].Value;
                    return RedirectToAction("TicketDetail", "Ticket", new { area = "Seller", id = answer.TicketId });
                }
            }

            var viewModel = await _ticketService.GetUserPanelTicketDetailViewModel(answer.TicketId, User.GetUserId());

            if (viewModel == null)
            {
                TempData[ErrorMessage] = _localizer["An error occurred Please try again"].Value;
                return RedirectToAction("FilterTickets", "Ticket");
            }

            ViewData["TicketDetailViewModel"] = viewModel;

            TempData[ErrorMessage] = _localizer["Text entry required"].Value;

            return View(answer);
        }

        #endregion

    }
}
