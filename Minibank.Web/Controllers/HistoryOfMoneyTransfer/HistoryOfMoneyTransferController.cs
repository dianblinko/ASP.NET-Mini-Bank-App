using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.HistoryOfMoneyTransfers.Services;
using Minibank.Web.Controllers.HistoryOfMoneyTransfer.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Minibank.Web.Controllers.HistoryOfMoneyTransfer
{
    [ApiController]
    [Route("[controller]")]
    public class HistoryOfMoneyTransferController
    {
        private readonly IHistoryOfMoneyTransferService _historyOfMoneyTransferService;

        public HistoryOfMoneyTransferController(IHistoryOfMoneyTransferService historyOfMoneyTransferService)
        {
            _historyOfMoneyTransferService = historyOfMoneyTransferService;
        }

        [HttpGet("{id}")]
        public HistoryOfMoneyTransferDto Get(string id)
        {
            var model = _historyOfMoneyTransferService.GetHistoryOfMoneyTransfer(id);

            return new HistoryOfMoneyTransferDto
            {
                Id = model.Id,
                Amount = model.Amount,
                Currency = model.Currency,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId
            };
        }

        [HttpGet]
        public IEnumerable<HistoryOfMoneyTransferDto> GetAll()
        {
            return _historyOfMoneyTransferService.GetAll()
                .Select(it => new HistoryOfMoneyTransferDto
                {
                    Id = it.Id,
                    Amount = it.Amount,
                    Currency = it.Currency,
                    FromAccountId = it.FromAccountId,
                    ToAccountId = it.ToAccountId
                });
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _historyOfMoneyTransferService.Delete(id);
        }

    }
}
