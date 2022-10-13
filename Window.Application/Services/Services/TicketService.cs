using Window.Application.Convertors;
using Window.Application.Extensions;
using Window.Application.Interfaces;
using Window.Application.Security;
using Window.Domain.Entities.Contact;
using Window.Domain.Enums.Contact;
using Window.Domain.Interfaces;
using Window.Domain.ViewModels.Admin.Ticket;
using Window.Domain.ViewModels.User.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Generators;
using Window.Application.StticTools;

namespace Window.Application.Services
{
    public class TicketService : ITicketService
    {
        #region Ctor

        public ITicketRepository _ticket;

        public IUserService _userService;

        public TicketService(ITicketRepository ticket, IUserService userService)
        {
            _ticket = ticket;
            _userService = userService;
        }

        #endregion

        #region Admin 

        public async Task<List<Ticket>> GetLastestTicketForAdminDashboard()
        {
            return await _ticket.GetLastestTicketForAdminDashboard();
        }

        public async Task<bool> CreateAnswerTicketAdmin(AnswerTicketAdminViewModel answer, ulong userId)
        {
            #region Ticket Validation

            var ticket = await GetTicketById(answer.TicketId);

            if (ticket == null) return false;

            #endregion

            #region Create Message Answer

            #region Fill Entity 

            var message = new TicketMessage
            {
                Message = answer.Message,
                SenderId = userId,
                TicketId = ticket.Id,
                CreateDate = DateTime.Now,
            };

            #region Add File

            if (answer.MessageFile != null)
            {
                if (Path.GetExtension(answer.MessageFile.FileName).ToLower() == ".jpg"
                        && Path.GetExtension(answer.MessageFile.FileName).ToLower() == ".png"
                        && Path.GetExtension(answer.MessageFile.FileName).ToLower() == ".jpeg")
                {
                    var res = answer.MessageFile.IsImage();
                    if (!res)
                    {
                        return false;
                    }
                }

                var imageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(answer.MessageFile.FileName);

                if (!Directory.Exists(FilePaths.TicketFilePathServer))
                    Directory.CreateDirectory(FilePaths.TicketFilePathServer);

                string OriginPath = FilePaths.TicketFilePathServer + imageName;

                using (var stream = new FileStream(OriginPath, FileMode.Create))
                {
                    if (!Directory.Exists(OriginPath)) answer.MessageFile.CopyTo(stream);
                }

                message.MessageFile = imageName;
            }

            #endregion


            #endregion

            #region Add Ticket Message Method

            await _ticket.AddMessage(message);

            #endregion

            #endregion

            #region Change Ticket State

            ticket.TicketStatus = TicketStatus.Answered;

            await _ticket.UpdateTicket(ticket);

            #endregion

            #region Read Ticket

            ticket.IsReadByOwner = false;
            ticket.IsReadByAdmin = true;

            await _ticket.UpdateTicket(ticket);

            #endregion

            return true;
        }

        public async Task<List<TicketMessage>> GetTicketMessages(ulong ticketId)
        {
            var ticket = await GetTicketById(ticketId);

            if (ticket == null) return null;

            var messages = await _ticket.GetTicketMessages(ticketId);

            return messages;
        }

        public async Task ReadTicketByAdmin(Ticket ticket)
        {
            ticket.IsReadByAdmin = true;

            await _ticket.ReadTicketByAdmin(ticket);
        }

        public async Task<Ticket?> GetTicketById(ulong ticketId)
        {
            return await _ticket.GetTicketById(ticketId);
        }

        public async Task<AdminFilterTicketViewModel> FilterAdminTicketViewModel(AdminFilterTicketViewModel filter)
        {
            return await _ticket.FilterAdminTicketViewModel(filter);
        }

        public async Task<bool> AddTicketFromAdminPanel(AddTicketViewModel addTicket, ulong adminId)
        {
            #region Check Is Exist User

            if (await _userService.GetUserById(addTicket.userId.Value) == null)
            {
                return false;
            }

            #endregion

            #region Fill Ticket Entity

            var ticket = new Ticket
            {
                Title = addTicket.Title.SanitizeText(),
                IsReadByAdmin = true,
                IsReadByOwner = false,
                OnWorking = false,
                TicketStatus = TicketStatus.Answered,
                OwnerId = addTicket.userId.Value,
                CreateDate = DateTime.Now
            };


            #endregion

            #region Add Ticket Method

            await _ticket.AddTicket(ticket);

            #endregion

            #region Fill Message Entity

            var message = new TicketMessage
            {
                Message = addTicket.Message.SanitizeText(),
                SenderId = adminId,
                TicketId = ticket.Id,
                CreateDate = DateTime.Now
            };

            #region Add File

            if (addTicket.MessageFile != null)
            {
                if (Path.GetExtension(addTicket.MessageFile.FileName).ToLower() == ".jpg"
                        && Path.GetExtension(addTicket.MessageFile.FileName).ToLower() == ".png"
                        && Path.GetExtension(addTicket.MessageFile.FileName).ToLower() == ".jpeg")
                {
                    var res = addTicket.MessageFile.IsImage();
                    if (!res)
                    {
                        return false;
                    }
                }

                var imageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(addTicket.MessageFile.FileName);

                if (!Directory.Exists(FilePaths.TicketFilePathServer))
                    Directory.CreateDirectory(FilePaths.TicketFilePathServer);

                string OriginPath = FilePaths.TicketFilePathServer + imageName;

                using (var stream = new FileStream(OriginPath, FileMode.Create))
                {
                    if (!Directory.Exists(OriginPath)) addTicket.MessageFile.CopyTo(stream);
                }

                message.MessageFile = imageName;
            }

            #endregion

            #endregion

            #region Add Message Method

            await _ticket.AddMessage(message);

            #endregion

            return true;
        }

        public async Task<string> ChangeTicketStatus(int state, ulong ticketId)
        {
            var ticket = await GetTicketById(ticketId);

            if (ticket == null) return string.Empty;

            ticket.TicketStatus = (TicketStatus)state;

            await _ticket.UpdateTicket(ticket);

            return ticket.TicketStatus.GetEnumName();
        }

        public async Task<bool> ChangeOnWorkingTicketStatus(ulong ticketId)
        {
            var ticket = await GetTicketById(ticketId);

            if (ticket == null) return false;

            ticket.OnWorking = !ticket.OnWorking;

            await _ticket.UpdateTicket(ticket);

            return true;
        }

        public async Task<bool> DeleteTicketMessage(ulong messageId)
        {
            var message = await _ticket.GetTicketMessageById(messageId);

            if (message == null) return false;

            message.IsDelete = true;

            await _ticket.UpdateTicketMessage(message);

            return true;
        }

        #endregion

        #region User Side

        public async Task<FilterSiteTicketViewModel> FilterSiteTicket(FilterSiteTicketViewModel filter, ulong userId)
        {
            return await _ticket.FilterSiteTicket(filter, userId);
        }

        public async Task<ulong> CreateTicket(CreateTicketViewModel create, ulong userId)
        {
            #region Get user By Id

            var user = await _userService.GetUserById(userId);

            if (user == null) return 0;

            #endregion

            #region Fill Ticket Entity

            var ticket = new Ticket
            {
                Title = create.Title.SanitizeText(),
                IsReadByAdmin = false,
                IsReadByOwner = true,
                OnWorking = false,
                TicketStatus = TicketStatus.Pending,
                OwnerId = userId,
                CreateDate = DateTime.Now,
            };

            #endregion

            #region Add Ticket Method

            await _ticket.AddTicket(ticket);

            #endregion

            #region Fill Ticket Message Entity

            var message = new TicketMessage
            {
                Message = create.Message.SanitizeText(),
                SenderId = userId,
                TicketId = ticket.Id,
                CreateDate = DateTime.Now
            };

            #region Add File

            if (create.MessageFile != null)
            {
                if (Path.GetExtension(create.MessageFile.FileName).ToLower() == ".jpg"
                        && Path.GetExtension(create.MessageFile.FileName).ToLower() == ".png"
                        && Path.GetExtension(create.MessageFile.FileName).ToLower() == ".jpeg")
                {
                    var res = create.MessageFile.IsImage();
                    if (!res)
                    {
                        return 0;
                    }
                }

                var imageName = CodeGenerator.GenerateUniqCode() + Path.GetExtension(create.MessageFile.FileName);

                if (!Directory.Exists(FilePaths.TicketFilePathServer))
                    Directory.CreateDirectory(FilePaths.TicketFilePathServer);

                string OriginPath = FilePaths.TicketFilePathServer + imageName;

                using (var stream = new FileStream(OriginPath, FileMode.Create))
                {
                    if (!Directory.Exists(OriginPath)) create.MessageFile.CopyTo(stream);
                }

                message.MessageFile = imageName;
            }

            #endregion

            #endregion

            #region Add Ticket Message Method 

            await _ticket.AddMessage(message);

            #endregion

            return ticket.Id;
        }


        public async Task<UserPanelTicketDetailViewModel> GetUserPanelTicketDetailViewModel(ulong ticketId, ulong userId)
        {
            #region Get Ticket

            var ticket = await _ticket.GetTicketById(ticketId);

            if (ticket == null || ticket.OwnerId != userId) return null;

            #endregion

            #region Get Ticket Messages

            var messages = await _ticket.GetTicketMessages(ticketId);

            #endregion

            #region Read Ticket

            if (!ticket.IsReadByOwner)
            {
                ticket.IsReadByOwner = true;
                
                await _ticket.UpdateTicket(ticket);
            }

            #endregion

            #region Fill View Model

            var result = new UserPanelTicketDetailViewModel
            {
                CreateDate = ticket.CreateDate.ToShamsi(),
                Status = ticket.TicketStatus.GetEnumName(),
                MessagesCount = messages.Count(),
                TicketTitle = ticket.Title,
                TicketMessages = messages,
                OwnerId = ticket.OwnerId,
                TicketStatus = ticket.TicketStatus
            };

            #endregion

            return result;
        }

        public async Task<bool> AnswerTicketByUser(AnswerTicketViewModel answer, ulong userId)
        {
            #region Get Ticket

            var ticket = await _ticket.GetTicketById(answer.TicketId);

            if (ticket == null || ticket.OwnerId != userId || ticket.TicketStatus != TicketStatus.Answered) return false;

            #endregion

            #region Fill Ticket Message Entity

            var message = new TicketMessage
            {
                Message = answer.Message.SanitizeText(),
                SenderId = userId,
                TicketId = ticket.Id,
                CreateDate = DateTime.Now
            };

            #endregion

            #region Add Ticket Message Method

            await _ticket.UpdateTicketMessage(message);

            #endregion

            #region Read Ticket & Update Ticket

            ticket.IsReadByOwner = true;
            ticket.IsReadByAdmin = false;
            ticket.TicketStatus = TicketStatus.Pending;

            await _ticket.UpdateTicket(ticket);

            #endregion

            return true;
        }

        #endregion
    }
}
